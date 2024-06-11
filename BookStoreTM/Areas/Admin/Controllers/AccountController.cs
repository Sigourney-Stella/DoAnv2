using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookStoreTM.Models;
using BookStoreTM.Models.EF;
using BookStoreTM.Models.Entities;

namespace BookStoreTM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private SignInManager<AppUserModel> _signInManager;

        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _db;


        public object DataLocal { get; private set; }

        public AccountController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public IActionResult Index()
        {

            var items = _db.Users.ToList();
            return View(items);
        }
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginViewModel loginVM, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    if (returnUrl == null || returnUrl == "/")
                    {
                        return RedirectToAction("index", "homeadmin");
                    }

                    return Redirect(returnUrl);

                }
                ModelState.AddModelError("", " Tên đăng nhập hoặc mật khẩu bị sai!");

            }
            return View(loginVM);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                AppUserModel newUser = new AppUserModel { UserName = user.Username, Email = user.Email };
                IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    TempData["success"] = "Tạo user thành công ";
                    return RedirectToAction("Index");

                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "homeAdmin");
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var item = _db.Users.Find(id);
            if (item != null)
            {
                _db.Users.Remove(item);
                _db.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
    }
}
