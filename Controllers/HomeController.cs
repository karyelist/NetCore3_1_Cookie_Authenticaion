﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCore3_1_Cookie_Authenticaion.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace NetCore3_1_Cookie_Authenticaion.Controllers
{
    public class HomeController : Controller
    { 
            private readonly ILogger<HomeController> _logger;

            private readonly AppDbContext _appDbContext;

            public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
            {
                _logger = logger;
                _appDbContext = appDbContext;
            }

            public IActionResult Index()
            {
                return View();
            }
        [Authorize]
        public IActionResult Kullanici()
        {
            var name = User.Claims.Where(c => c.Type == ClaimTypes.Name)
                  .Select(c => c.Value).SingleOrDefault();
            var claim = User.Identities;
             
            var type = User.Identity.AuthenticationType;

            ViewBag.param ="Login Name : " +  name + " Type : " + type;
            return View("Kullanici");
            }

        [Authorize]
        public IActionResult Privacy()
            {
                return View();
    
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            public IActionResult Login()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> login(LoginModel loginViewModel)

            {
                var user = _appDbContext.users.ToList();

                if (ModelState.IsValid)
                {
                    var isUser = _appDbContext.users.FirstOrDefault(x => x.Email == loginViewModel.Email && x.Password == loginViewModel.Password);

                    if (isUser != null)
                    {
                        List<Claim> userClaims = new List<Claim>();

                        userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.Id.ToString()));
                        userClaims.Add(new Claim(ClaimTypes.Name, isUser.UserName));
                        userClaims.Add(new Claim(ClaimTypes.GivenName, isUser.Name));
                        userClaims.Add(new Claim(ClaimTypes.Surname, isUser.SurName.ToString()));

                        //Veritabanımızdaki role tablosunda kullanıcı hakkında roller varsa onlarıda ekliyoruz
                        //Farzedelim,  fcakiroglu16@outlook.com adlı email admin rolüne sahip,

                        if (isUser.Email == "f-cakiroglu@outlook.com")
                        {
                            userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                        }
                        //Veritabanımızdaki claim tablosunda kullanıcı hakkında claim'ler varsa onlarıda ekliyoruz.

                        var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = loginViewModel.IsRememberMe
                        };

                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
                        return RedirectToAction("Kullanici", "Home");
                        //Sadece üye olan kullanıcıların göreceği sayfaya yönlendirme
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış");
                    }
                }

                return View(loginViewModel);
            }
        }
    }
 