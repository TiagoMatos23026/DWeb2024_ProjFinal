using DWebProjFinal.Controllers;
using DWebProjFinal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class AccountAPIController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UtentesController _utentesController;

    public AccountAPIController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, UtentesController utentesController)
    {
        _utentesController = utentesController;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterModel model, [FromForm] Utentes utente, [FromForm] IFormFile IconFile)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                utente.UserID = user.Id;
                await _utentesController.Create(utente, IconFile);

                await _signInManager.SignInAsync(user, isPersistent: false);

                // Automatically confirm the email
                var userId = await _userManager.FindByIdAsync(utente.UserID);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(userId);
                var confirmResult = await _userManager.ConfirmEmailAsync(userId, token);

                if (confirmResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Erro ao confirmar o email");
                }
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            
        }
        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Email);
        if (user == null)
        {
            return Unauthorized();
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return Ok(model.Email);
        }

        return Unauthorized();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }
}

public class RegisterModel
{
    [Required]
    public Utentes utente = new Utentes();

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}

public class LoginModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}
