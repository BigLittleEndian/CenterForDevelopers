using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CenterForDevelopers.Pages
{
    public class IndexPageModel : PageModel
    {
        public string Message { get; set; }

        [BindProperty] // => opt-in to model binding
        public int Year { get; set; }
        [BindProperty] // => opt-in to model binding
        public string Title { get; set; }

        public IndexPageModel()
        {
        }

        public void OnGet()
        {
            (Year, Title) = GetDataFromTheService();

            BuildMessage("Get");
        }

        public PageResult OnPost()
        {
            BuildMessage("Post");

            // Send data to the service

            return Page();
        }

        private void BuildMessage(string action)
        {
            Message = $"{action} for '{Title}' ({Year})";
        }
        
        private (int Year, string Title) GetDataFromTheService()
        {
            return (1976, "Rocky");
        }
    }
}