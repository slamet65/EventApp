using EventApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using System.Net.Http.Headers;

namespace EventApp.Controllers
{
    public class OrganizerController : Controller
    {
        private readonly AppSettings _appSettings;
        public OrganizerController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        // GET: OrganizerController
        public async Task<ActionResult> Index(int? page)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var uriBuilder = new UriBuilder(_appSettings.ApiUrl);
                uriBuilder.Path = "api/v1/organizers";
                if (page != null && page > 1)
                {
                    uriBuilder.Query = "page=" + page.ToString();
                }
                client.BaseAddress = uriBuilder.Uri;
                var resp = await client.GetAsync("");
                var response = await resp.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<IndexOrganizerViewModel>(response);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "User");
            }
        }

        // GET: OrganizerController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.GetFromJsonAsync("organizers/" + id.ToString(), typeof(Organizer));
                return View(response);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "User");
            }
        }

        // GET: OrganizerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrganizerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateOrganizerViewModel model)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.PostAsJsonAsync("organizers", model);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Organizer");
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

        // GET: OrganizerController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.GetFromJsonAsync("organizers/" + id.ToString(), typeof(Organizer));
                return View(response);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "User");
            }
        }

        // POST: OrganizerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Organizer model)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.PutAsJsonAsync("organizers/" + model.Id.ToString(), model);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Organizer");
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

        // GET: OrganizerController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.GetFromJsonAsync("organizers/" + id.ToString(), typeof(Organizer));
                return View(response);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "User");
            }
        }

        // POST: OrganizerController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteOrg(int id)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.DeleteAsync("organizers/" + id.ToString());
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Organizer");
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var detail = JsonConvert.DeserializeObject<ApiError>(body);
                    TempData["error"] = detail;
                    return View(0);
                }
            }
            catch
            {
                return RedirectToAction("Login", "User");
            }
        }
    }
}
