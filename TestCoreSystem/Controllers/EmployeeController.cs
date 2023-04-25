using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using TestCoreSystem.Models;

namespace TestCoreSystem.Controllers
{
    public class EmployeeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Book> employees = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7163/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var responseTask = client.GetAsync("api/Employee/all");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Book>>(readTask.Result);
                    readTask.Wait();
                    employees = deserialized;
                }
                else
                {
                    employees = Enumerable.Empty<Book>();
                    ModelState.AddModelError(string.Empty, "Employees not found.");
                }
            }
            return View(employees);
        }
        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployee(Book employee)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7163/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var postTask = client.PostAsJsonAsync<Book>("api/Employee/add", employee);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Failed Try again.");
            return View(employee);
        }


        [HttpGet]
        public IActionResult GetEmployeeById(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7251/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var postTask = client.GetAsync($"api/Employee/GetEmployeeById/{id}");
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<Book>(readTask.Result);
                    readTask.Wait();

                    return Json(deserialized);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateEmployee(Book employee)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7163/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var postTask = client.PostAsJsonAsync<Book>("api/Employee/edit", employee);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Failed Try again.");
            return View(employee);
        }

        [HttpGet]
        public IActionResult DeleteEmployee()
        {
            return View();
        }

        [HttpPost, ActionName("DeleteEmployee")]
        public async Task<ActionResult> DeletData(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7163/");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));


                    var postTask = await client.PostAsJsonAsync("api/Employee/delete", id);
                    var result = await postTask.Content.ReadAsStringAsync();
                    if (postTask.IsSuccessStatusCode)
                    {
                        return Json(result);
                    }
                }
                return Json("\"Failed Try again.\"");
            }
            catch (Exception ex) { throw ex; }
        }

        /*[HttpGet]
        public IActionResult AddFeedback()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddFeedback(Feedback feedback)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7130/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var postTask = client.PostAsJsonAsync<Feedback>("api/Employee/addFeedback", feedback);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Failed Try again.");
            return View(feedback);
        }

        [HttpGet]
        public ActionResult GetFeedbacks(int id)
        {
            IEnumerable<Feedback> feedback = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7130/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var postTask = client.GetAsync($"api/Employee/feedbacks/{id}");
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Feedback>>(readTask.Result);
                    readTask.Wait();
                    feedback = deserialized;
                }
            }
            return View(feedback);
        }*/
    }
}
