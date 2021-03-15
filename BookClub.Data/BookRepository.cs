using System.Collections.Generic;
using System.Data;
using System.Linq;
using BookClub.Entities;
using Dapper;
using Microsoft.Extensions.Logging;

namespace BookClub.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly IDbConnection _db;
        private readonly ILogger _logger;

        public BookRepository(IDbConnection db, ILoggerFactory loggerFactory)
        {
            _db = db;
            _logger = loggerFactory.CreateLogger("Database"); // this allows for logging all database-related entries, even from other classes(?)
        }

        public List<Book> GetAllBooks()
        {
            _logger.LogInformation("Inside the repository about to call GetAllBooks.");
            _logger.LogDebug(DataEvents.GatMany, "Debugging information for stored proc: {ProcName}", "GetAllBooks");
            var books = _db.Query<Book>("GetAllBooks", commandType: CommandType.StoredProcedure)
                .ToList();
            return books;
        }

        public void SubmitNewBook(Book bookToSubmit, int submitter)
        {
            _db.Execute("InsertBook", new
            {
                bookToSubmit.Title,
                bookToSubmit.Author,
                Classification = bookToSubmit.Category,
                bookToSubmit.Genre,
                bookToSubmit.Isbn,
                submitter
            }, commandType: CommandType.StoredProcedure);
        }
    }
}
