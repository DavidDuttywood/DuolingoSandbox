using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DavidWoodward.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Nancy.Json;

namespace DavidWoodward.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly string _jwt;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

            var url = _config.GetValue<string>("Duolingo:LoginUrl");

            //if jwt is empty..... session
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["login"] = _config.GetValue<string>("Duolingo:Username");
                data["password"] = _config.GetValue<string>("Duolingo:Password");

                var response = wb.UploadValues(url, "POST", data);
                _jwt = wb.ResponseHeaders["jwt"];
            }
        }

        public IActionResult Index()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);
            HttpResponseMessage response = httpClient.GetAsync(_config.GetValue<string>("Duolingo:DataUrl")).Result;
            var result = response.Content.ReadAsStringAsync();
            
            RootObject datalist = JsonConvert.DeserializeObject<RootObject>(result.Result);


            var lang = datalist.Courses.Select(course => course.Title).ToList();
            var xp = datalist.Courses.Select(course => course.Xp).ToList();

            ViewBag.Lang = lang;
            ViewBag.xp = xp;

            return View(datalist);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
