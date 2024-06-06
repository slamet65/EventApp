using EventApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace EventApp.Controllers
{
    public class SportController : Controller
    {
        private readonly AppSettings _appSettings;
        public SportController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        private async Task<IEnumerable<Organizer>> getOrgReff()
        {
            var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var uriBuilder = new UriBuilder(_appSettings.ApiUrl);
            uriBuilder.Path = "api/v1/organizers";

            uriBuilder.Query = "perPage=" + 2500;

            client.BaseAddress = uriBuilder.Uri;
            var resp = client.GetAsync("").Result;
            var response = resp.Content.ReadAsStringAsync().Result;
            var model = JsonConvert.DeserializeObject<IndexOrganizerViewModel>(response);
            return model.Data;
        }
        // GET: SportController
        public async Task<IActionResult> Index(int? page, int? organizerId)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var uriBuilder = new UriBuilder(_appSettings.ApiUrl);
                uriBuilder.Path = "api/v1/sport-events";
                string query = "";
                if (page != null && page > 1)
                {
                    query += string.Format("page={0}&", page.ToString());
                }
                if (organizerId != null && organizerId != 0)
                {
                    query += string.Format("organizerId={0}&", organizerId);
                }
                uriBuilder.Query = query;
                client.BaseAddress = uriBuilder.Uri;
                var resp = await client.GetAsync("");
                var response = await resp.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<IndexEventViewModel>(response);
                model.selectedOrganizerId = organizerId;
                model.ReffOrganizer = await getOrgReff();

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "User");
            }
        }

        // GET: SportController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.GetFromJsonAsync("sport-events/" + id.ToString(), typeof(Event));
                return View(response);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "User");
            }
        }

        // GET: SportController/Create
        public async Task<IActionResult> Create()
        {
            CreateEvent model = new CreateEvent();
            model.ReffOrganizer = await getOrgReff();
            return View(model);
        }

        // POST: SportController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEvent model)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.PostAsJsonAsync("sport-events", model);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Sport");
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

        // GET: SportController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = (Event)await client.GetFromJsonAsync("sport-events/" + id.ToString(), typeof(Event));
                EditEvent model = new EditEvent()
                {
                    Id = response.Id,
                    EventType = response.EventType,
                    EventName = response.EventName,
                    EventDate = response.EventDate,
                    OrganizerId = response.Organizer.Id,
                    ReffOrganizer = await getOrgReff()
                };

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "User");
            }
        }

        // POST: SportController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditEvent model)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.PutAsJsonAsync("sport-events/" + model.Id.ToString(), model);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Sport");
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

        // GET: SportController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = (Event)await client.GetFromJsonAsync("sport-events/" + id.ToString(), typeof(Event));
                EditEvent model = new EditEvent()
                {
                    Id = response.Id,
                    EventType = response.EventType,
                    EventName = response.EventName,
                    EventDate = response.EventDate,
                    OrganizerId = response.Organizer.Id,
                    ReffOrganizer = await getOrgReff()
                };

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "User");
            }
        }

        // POST: SportController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSport(int id)
        {
            try
            {
                var token = Encoding.UTF8.GetString(HttpContext.Session.Get("token"));
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.BaseAddress = new Uri(_appSettings.ApiUrl);
                var response = await client.DeleteAsync("sport-events/" + id.ToString());
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Sport");
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
