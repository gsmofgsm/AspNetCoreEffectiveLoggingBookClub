using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookClub.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookClub.UI.Pages
{
    public class CreateModel : PageModel
    {
        public Book Book { get; set; }
        public void OnGet()
        {
            Book = new Book();
        }
    }
}
