// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using DWebProjFinal.Controllers;

using DWebProjFinal.Data;
using DWebProjFinal.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DWebProjFinal.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly UtentesController _utenteController;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
          UtentesController utenteController,
          UserManager<IdentityUser> userManager,
          IUserStore<IdentityUser> userStore,
          SignInManager<IdentityUser> signInManager,
          ILogger<RegisterModel> logger,
          IEmailSender emailSender,
          ApplicationDbContext context)
        {

            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _utenteController = utenteController;
        }

        /// <summary>
        /// objeto a ser utilizado para transportar os dados
        /// entre a interface e o código
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// esta variável irá conter o 'destino' a ser aplicado
        /// pela aplicação, quando após o 'registo' a aplicação
        /// pretender ser reposicionada na página original
        /// </summary>
        public string ReturnUrl { get; set; }

        // /// <summary>
        // /// se se adicionar as chaves de autenticação por 
        // /// 'providers' externos, aqui serão listados
        // /// por esta variável
        // /// Ver: https://go.microsoft.com/fwlink/?LinkID=532715
        // /// </summary>
        // public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        /// 'inner class'
        /// define os atributos a serem enviados/recebidos para/da
        /// interface
        /// </summary>
        public class InputModel
        {
            /// <summary>
            /// Email do utilizador
            /// </summary>
            [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
            [EmailAddress(ErrorMessage = "Escreva um {0} válido, por favor.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            /// Password de acesso ao sistema, pelo utilizador
            /// </summary>
            [Required(ErrorMessage = "A {0} é de preenchimento obrigatório.")]
            [StringLength(20, ErrorMessage = "A {0} tem de ter, pelo menos {2}, e um máximo de {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            /// confirmação da password
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirmar password")]
            [Compare("Password", ErrorMessage = "A password e a sua confirmação não coincidem.")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            /// Recolhe os dados do Utente
            /// </summary>
            public Utentes utente { get; set; }
            public IFormFile IconFile { get; set; }
        }


        /// <summary>
        /// Este método reage ao verbo HTTP GET
        /// </summary>
        /// <param name="returnUrl">o endereço onde 'estávamos'
        ///  quando foi feito o pedido para nos registarmos
        /// </param>
        /// <returns></returns>
        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        /// <summary>
        /// Método para criar um novo Utente e um login para ele
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid) //validacao
            {

                var userLogin = CreateUser();

                await _userStore.SetUserNameAsync(userLogin, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(userLogin, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(userLogin, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Utilizador criado com sucesso!");

                    try
                    {
                        await _utenteController.Create(Input.utente, Input.IconFile); //chamar UtentesContoller para criação de um objeto Utente
                    }
                    catch (Exception ex) 
                    {
                        await _userManager.DeleteAsync(userLogin);
                        await _context.SaveChangesAsync();
                        Url.Content("~/");
                    }

                    var userId = await _userManager.GetUserIdAsync(userLogin);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(userLogin);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Por favor, confirme o seu email <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(userLogin, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }



                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
