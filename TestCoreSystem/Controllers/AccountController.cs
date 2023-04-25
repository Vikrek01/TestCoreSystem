using Microsoft.AspNetCore.Mvc;
using TestCoreSystem.Models;

namespace TestCoreSystem.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(Login login)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7163/");
                var postTask = await client.PostAsJsonAsync<Login>("api/Account/Login", login);
                if (postTask.IsSuccessStatusCode)
                {
                    var customerJsonString = postTask.Content.ReadAsStringAsync();
                    var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginModel>(custome‌​rJsonString.Result);

                    HttpContext.Session.SetString("Token", deserialized.Token);
                    HttpContext.Session.SetString("Username", deserialized.Username);

                    HttpContext.Session.SetString("UserRole", deserialized.Role);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Please enter the valid email and password.");
            }

            return View(login);
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(Register register)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7163/");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<Register>("api/Account/SignUp", register);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(register);
        }

        public ActionResult Logout()
        {
            using (var client = new HttpClient())
            {

                HttpContext.Session.Clear();

                return RedirectToAction("Index", "Home");
                //client.BaseAddress = new Uri("https://localhost:7163/");

                ////HTTP POST
                //var postTask = client.GetAsync("api/Account/Logout");

                ////postTask.Wait();

                //var result = postTask.Result;
                //if (result.IsSuccessStatusCode)
                //{

                //    HttpContext.Session.Clear();

                //    return RedirectToAction("About", "Home");
                //}
                //ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
        }
    }
}
