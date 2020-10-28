using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ManageProjects.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ManageProjects.Controllers
{
    public class UserController : Controller
    {
        const string Token = "_Token";
        const string UserName = "_Email";

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(LogIn model)
        {
            using HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44306/api/User/")
            };
            string stringData = JsonConvert.SerializeObject(model);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
            var responseTask = client.PostAsync("authenticate", contentData);
            responseTask.Wait();

            var result = responseTask.Result;
            UserModel userDetails = new UserModel();
            if (result.IsSuccessStatusCode)
            {
                var userData = result.Content.ReadAsStringAsync().Result;
                userDetails = JsonConvert.DeserializeObject<UserModel>(userData);
                HttpContext.Session.SetString(Token, userDetails.Token);
                HttpContext.Session.SetString(UserName, userDetails.Username);
            }
            return RedirectToAction("Index","Project");
        }
    }
}