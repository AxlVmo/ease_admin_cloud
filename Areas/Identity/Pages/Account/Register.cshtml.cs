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
using AspNetCoreHero.ToastNotification.Abstractions;
using ease_admin_cloud.Areas.Users.Models;
using ease_admin_cloud.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace ease_admin_cloud.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly INotyfService _toastNotification;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context,
            INotyfService toastNotification
        )
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _toastNotification = toastNotification;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        public class InputModel
        {
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Confirm password")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (
                await _signInManager.GetExternalAuthenticationSchemesAsync()
            ).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (
                await _signInManager.GetExternalAuthenticationSchemesAsync()
            ).ToList();

            if (String.IsNullOrEmpty(Input.Email))
            {
                _toastNotification.Warning(
                    "Correo electrónico nulo y/o vacio, favor de llenar el campo",
                    5
                );
            }
            else if (String.IsNullOrEmpty(Input.Password))
            {
                _toastNotification.Warning(
                    "Contraseña nulo y/o vacio, favor de llenar el campo",
                    5
                );
            }
            else if (String.IsNullOrEmpty(Input.ConfirmPassword))
            {
                _toastNotification.Warning(
                    "Confirmación de contraseña nulo y/o vacio, favor de llenar el campo",
                    5
                );
            }
            else
            {
                if (Input.ConfirmPassword != Input.Password)
                {
                    _toastNotification.Warning(
                        "Confirmación de contraseña es diferente a la de contraseña, favor de llenar el campo",
                        5
                    );
                }
                else
                {
                    var user = CreateUser();
                    await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                    var result = await _userManager.CreateAsync(user, Input.Password);
                    if (result.Succeeded)
                    {
                        var user_email = user.Email;
                        var user_username = user.UserName;
                        var user_id = user.Id;
                        var addUsuarios = new usuario_control
                        {
                            id_usuario_control = Guid.Parse(user_id),
                            id_usuario_modifico = Guid.Empty,
                            correo_acceso = user_email,
                            nombre_usuario = user_username,
                            id_area = 1,
                            id_perfil = 1,
                            id_rol = 1,
                            terminos_uso = false,
                            fecha_registro = DateTime.Now,
                            fecha_actualizacion = DateTime.Now,
                            id_estatus_registro = 1
                        };
                        _context.Add(addUsuarios);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("User created a new account with password.");

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new
                            {
                                area = "Identity",
                                userId = userId,
                                code = code,
                                returnUrl = returnUrl
                            },
                            protocol: Request.Scheme
                        );

                        await _emailSender.SendEmailAsync(
                            Input.Email,
                            "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
                        );

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage(
                                "RegisterConfirmation",
                                new { email = Input.Email, returnUrl = returnUrl }
                            );
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }
                    else
                    {
                        var list_err = result.Errors.ToList();
                        string msj_err = null;

                        foreach (var item in list_err)
                        {
                            if (
                                item.Description
                                == "Passwords must have at least one uppercase ('A'-'Z')."
                            )
                            {
                                msj_err =
                                    "Las contraseñas deben tener al menos una letra mayúscula (A-Z)";
                                _toastNotification.Warning(msj_err, 5);
                            }
                            if (
                                item.Description
                                == "Passwords must have at least one non alphanumeric character."
                            )
                            {
                                msj_err =
                                    "Las contraseñas deben tener al menos un carácter no alfanumérico";
                                _toastNotification.Warning(msj_err, 5);
                            }
                            if (item.Code == "DuplicateUserName")
                            {
                                msj_err = "El nombre de usuario ya está en uso";
                                _toastNotification.Warning(msj_err, 5);
                            }
                        }
                    }
                }
            }
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
                throw new InvalidOperationException(
                    $"Can't create an instance of '{nameof(IdentityUser)}'. "
                        + $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively "
                        + $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml"
                );
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException(
                    "The default UI requires a user store with email support."
                );
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
