using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ManageProjects.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ManageProjects.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;

        public HomeController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            List<ManageProject> projectList = null;
            using HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44306")
            };
            var responseTask = client.GetAsync("api/Project");
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var stringData = result.Content.ReadAsStringAsync().Result;
                projectList = JsonConvert.DeserializeObject<List<ManageProject>>(stringData);

            }
            return View(projectList);
        }

        [HttpGet]
        public ActionResult AddProject()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProject(AddUpdateProject model)
        {
            var image = HttpContext.Request.Form.Files[0];
            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(image.FileName);
            if (image != null)
            {
                var uploads = Path.Combine(_appEnvironment.WebRootPath, "images");

                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

            }

            ManageProject project = new ManageProject()
            {
                ProjectName = model.ProjectName,
                ProjectImage = fileName,
                Duration = model.Duration,
                Cost = model.Cost,
                Description = model.Description
            };

            using HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44306")
            };
            string stringData = JsonConvert.SerializeObject(project);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
            var responseTask = client.PostAsync("/api/Project", contentData);
            responseTask.Wait();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditProject(int id)
        {
            ManageProject projectList = null;
            using HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44306")
            };
            var responseTask = client.GetAsync("api/Project/" + id.ToString());
            responseTask.Wait();
            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var stringData = result.Content.ReadAsStringAsync().Result;
                projectList = JsonConvert.DeserializeObject<ManageProject>(stringData);

            }
            AddUpdateProject model = new AddUpdateProject()
            {
                ProjectName = projectList.ProjectName,
                Image = projectList.ProjectImage,
                Cost = projectList.Cost,
                Duration = projectList.Duration,
                Description = projectList.Description
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditProject(int id, AddUpdateProject model)
        {
            ManageProject project = new ManageProject();
            if (model.ProjectImage != null)
            {
                var image = HttpContext.Request.Form.Files[0];

                var uploads = Path.Combine(_appEnvironment.WebRootPath, "images");
                var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(image.FileName);
                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                {
                    image.CopyTo(fileStream);
                    project.ProjectImage = fileName;
                }
            }
            else
            {
                project.ProjectImage = model.Image;
            }

            project.id = id;
            project.ProjectName = model.ProjectName;
            project.Duration = model.Duration;
            project.Cost = model.Cost;
            project.Description = model.Description;

            using HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44306")
            };
            string stringData = JsonConvert.SerializeObject(project);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
            var responseTask = client.PutAsync("/api/Project/" + id.ToString(), contentData);
            responseTask.Wait();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteProject(int id)
        {
            using HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44306")
            };
            var deleteTask = client.DeleteAsync("api/Project/" + id.ToString());
            deleteTask.Wait();
            return RedirectToAction("Index");
        }
    }
}
