using BookClub.Data;
using BookClub.Entities;
using BookClub.Logic.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BookClub.Logic
{
    public class BookLogic : IBookLogic
    {
        private readonly IBookRepository _repo;
        private readonly ILogger _logger;

        public BookLogic(IBookRepository repo, ILogger logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<List<BookModel>> GetAllBooks()
        {
            var books = _repo.GetAllBooks();

            var bookList = new List<BookModel>();
            foreach (var book in books)
            {
                bookList.Add(await GetBookModelFromBook(book));
            }

            return bookList;
        }

        private async Task<BookModel> GetBookModelFromBook(Book book)
        {
            var bookToReturn = new BookModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Category = book.Category,
                Submitter = GetSubmitterFromId(book.Submitter)
            };

            using (var httpClient = new HttpClient())
            {
                var uri = $"https://www.googleapis.com/books/v1/volumes?q=isbn:{book.Isbn}";

                try
                {
                    var bookResponse = await httpClient.GetFromJsonAsync<GoogleBookResponse>(uri);

                    var thisBook = bookResponse?.Items?.FirstOrDefault();
                    if (thisBook != null)
                    {
                        bookToReturn.Description = thisBook.VolumeInfo?.Description;
                        bookToReturn.PageCount = thisBook.VolumeInfo?.PageCount ?? 0;
                        bookToReturn.InfoLink = thisBook.VolumeInfo?.InfoLink;
                        bookToReturn.Thumbnail = thisBook.VolumeInfo?.ImageLinks?.Thumbnail;
                    }
                    else
                    {
                        _logger.LogWarning("No book information found in Google for ISBN {ISBN}.", book.Isbn);
                    }
                }
                catch (Exception ex)
                {

                    // it's ok if google api call doesn't work
                    _logger.LogError("Api failure in Google API call.", ex);
                }
                return bookToReturn;
            }
        }

        private string GetSubmitterFromId(int submitter)
        {
            switch (submitter)
            {
                case 11:
                    return "Bob";
                case 111:
                    return "Erik";
                default:
                    _logger.LogWarning("Unknown user {UserId} in database.", submitter);
                    return "Alice";
            }
        }
    }
}
