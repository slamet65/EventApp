using EventApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace EventApp.Controllers
{
    public class UserController : MyController
    {
        private readonly AppSettings _appSettings;
        public UserController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                string userId = Encoding.UTF8.GetString(HttpContext.Session.Get("userId"));
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.GetFromJsonAsync("users/" + userId, typeof(User));
                return View(response);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "User");
            }
        }
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.GetFromJsonAsync("users/" + id.ToString(), typeof(User));
                return View(response);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "User");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(User model)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.PutAsJsonAsync("users/" + model.id.ToString(), model);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var detail = JsonConvert.DeserializeObject<ApiError>(body);
                    TempData["error"] = detail;
                    return View(model);
                }
            }
            catch
            {
                return RedirectToAction("Login", "User");
            }
        }
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.GetFromJsonAsync("users/" + id.ToString(), typeof(User));
                return View(response);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "User");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(User model)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.DeleteAsync("users/" + model.id.ToString());
                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var detail = JsonConvert.DeserializeObject<ApiError>(body);
                    TempData["error"] = detail;
                    return View(model);
                }
            }
            catch
            {
                return RedirectToAction("Login", "User");
            }
        }

        public async Task<IActionResult> ChangePassword(int id)
        {
            ChangePassword model = new ChangePassword();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            try
            {
                string userId = Encoding.UTF8.GetString(HttpContext.Session.Get("userId"));
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.PutAsJsonAsync(string.Format("users/{0}/password", userId), model);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var detail = JsonConvert.DeserializeObject<ApiError>(body);
                    TempData["error"] = detail;
                    return View(model);
                }
            }
            catch
            {
                return RedirectToAction("Login", "User");
            }
        }

        public IActionResult Login()
        {
            LoginViewModel vm = new LoginViewModel();
            return View(vm);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }

        public IActionResult Register()
        {
            RegisterViewModel vm = new RegisterViewModel();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_appSettings.ApiUrl);
            var response = await client.PostAsJsonAsync("users", model);
            if (response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(body);
                return RedirectToAction("Login");
            }
            else
            {
                string body = await response.Content.ReadAsStringAsync();
                var detail = JsonConvert.DeserializeObject<ApiError>(body);
                TempData["error"] = detail;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_appSettings.ApiUrl);
            var response = await client.PostAsJsonAsync("users/login", model);
            if (response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(body);
                token = user.token;
                HttpContext.Session.Set("token", Encoding.UTF8.GetBytes(user.token == null ? "" : user.token));
                HttpContext.Session.Set("userId", Encoding.UTF8.GetBytes(user.id.ToString() == null ? "" : user.id.ToString()));
                return RedirectToAction("Index", "User");
            }
            else
            {
                string body = await response.Content.ReadAsStringAsync();
                var detail = JsonConvert.DeserializeObject<ApiError>(body);
                TempData["error"] = detail;
                return View(model);
            }
        }
    }
}
