using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace URLShortener.Pages;

public class IndexModel : PageModel
{
    public void OnGet()
    {
        // Renders the initial form
    }

    public IActionResult OnPost(string FirstName, string LastName, string Department, string Priority)
    {
        // TODO: Add your Entity Framework Core / Database logic here!

        // Return a clean HTML snippet back to HTMX to display success to the user
        return Content($@"
            <article style='background-color: #183c28; border-color: #2d6a4f; color: white; margin-top: 1rem; margin-bottom: 0;'>
                <strong>Success!</strong> Record saved for <em>{FirstName} {LastName}</em> under <strong>{Department}</strong> ({Priority} Priority).
            </article>", "text/html");
    }
}