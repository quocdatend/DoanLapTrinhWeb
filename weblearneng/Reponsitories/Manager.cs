using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using weblearneng.Models;

public class Manager : PageModel
{
    private readonly UserManager<Account> _userManager;

    public Manager(UserManager<Account> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null || !await _userManager.IsInRoleAsync(user, "Admin"))
        {
            return RedirectToPage("/AccessDenied");
        }

        return RedirectToPage("/AccessDenied");
    }
}
