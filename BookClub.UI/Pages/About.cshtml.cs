using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace BookClub.UI.Pages
{
    public class AboutModel : PageModel
    {
        public void OnGet()
        {
            throw new Exception("Users should not see this!");
        }
    }
}