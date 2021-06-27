using ETicket.Domain.DomainModels;
using ETicket.Domain.DTO;
using ETicket.Domain.Identity;
using ETicket.Service.Implementation;
using ETicket.Service.Interface;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ETicket.Web.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ETicketAppUser> _userManager;
        private readonly SignInManager<ETicketAppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;

        public AccountController(UserManager<ETicketAppUser> userManager, SignInManager<ETicketAppUser> signInManager, RoleManager<IdentityRole> roleManager, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userService = userService;
        }
        [Authorize(Roles = "Admin, Standard User")]
        public IActionResult AddUserToRole()
        {
            AddUserToRole model = new AddUserToRole();

            List<ETicketAppUser> users = _userService.GetAllUsers().ToList();

            foreach(var user in users)
            {
                model.UserEmails.Add(user);
            }

            model.Roles.Add("Standard User");
            model.Roles.Add("Admin");

            return View(model);
        }
        [HttpPost, Authorize(Roles = "Admin, Standard User")]
        public async Task<IActionResult> AddUserToRole(AddUserToRole model)
        {
            ETicketAppUser user = await _userManager.FindByIdAsync(model.SelectedUserId);
            List<string> roles = new List<string>
            {
                "Standard User",
                "Admin"
            };
            foreach(var i in roles)
            {
                if (await _userManager.IsInRoleAsync(user, i))
                {
                    await _userManager.RemoveFromRoleAsync(user, i);
                }
            }

            var res = await _userManager.AddToRoleAsync(user, model.SelectedRole);

            if (res.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            UserRegisterDto model = new UserRegisterDto();
            return View(model);
        }
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var userCheck = await _userManager.FindByEmailAsync(model.Email);

                if (userCheck == null)
                {
                    var user = new ETicketAppUser
                    {
                        Email = model.Email,
                        NormalizedUserName = model.Email,
                        UserName = model.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        ShoppingCart = new ETicket.Domain.DomainModels.ShoppingCart()
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    
                    if (result.Succeeded)
                    {
                        bool roleExists = await _roleManager.RoleExistsAsync("Standard User");

                        if (!roleExists)
                        {
                            await _roleManager.CreateAsync(new IdentityRole("Standard User"));
                            
                        }
                        roleExists = await _roleManager.RoleExistsAsync("Admin");

                        if (!roleExists)
                        {
                            await _roleManager.CreateAsync(new IdentityRole("Admin"));
                        }

                        var res = await _userManager.AddToRoleAsync(user, "Standard User");
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        if (result.Errors.Count() > 0)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("message", error.Description);
                            }
                        }
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("message", "Email already exists");
                    return View(model);
                }
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            UserLoginDto model = new UserLoginDto();
            return View(model);
        }
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError("message", "Email not confirmed yet!");
                    return View(model);
                }
                if (await _userManager.CheckPasswordAsync(user, model.Password) == false)
                {
                    ModelState.AddModelError("message", "Invalid Password");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);

                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim("UserRole", "StandardUser"));
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                else
                {
                    ModelState.AddModelError("message", "Invalid login attempt!");
                    return View(model);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ImportUsers()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult ImportUsers(IFormFile file)
        {
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\Files\\{file.FileName}";
            using (FileStream fs = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fs);
                fs.Flush();
            }

            List<UserImportDto> users = getAllUsersFromFile(file.FileName);

            bool status = true;

            foreach (var user in users)
            {
                var check = _userManager.FindByEmailAsync(user.Email).Result;
                if (check == null)
                {
                    var res = _userManager.CreateAsync(new ETicketAppUser
                    {
                        UserName = user.Email,
                        NormalizedUserName = user.Email.ToUpper(),
                        Email = user.Email,
                        NormalizedEmail = user.Email.ToUpper(),
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        ShoppingCart = new ShoppingCart()
                    }, user.Password).Result;

                    if (res.Succeeded)
                    {
                        var usr = _userManager.FindByEmailAsync(user.Email).Result;
                        res = _userManager.AddToRoleAsync(usr, user.Role).Result;
                    }

                    status = status && res.Succeeded;
                }
                else
                {
                    continue;
                }
            }

            if (status)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        private List<UserImportDto> getAllUsersFromFile(string fileName)
        {
            List<UserImportDto> users = new List<UserImportDto>();

            string filePath = $"{Directory.GetCurrentDirectory()}\\Files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (FileStream fs = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(fs))
                {
                    while (reader.Read())
                    {
                        users.Add(new UserImportDto
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            Role = reader.GetValue(2).ToString()
                        });
                    }
                }
            }
            return users;
        }

    }
}
