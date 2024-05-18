using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using UniDormMVC.Models;

namespace UniDormMVC.Controllers
{
    public class DormitoryController : Controller
    {
        IFirebaseConfig _config = new FirebaseConfig
        {
            AuthSecret = "Rc1Je3RMFk9aUZepyOr2hYsM3NbL0n3HTA3Gfcgm",
            BasePath = "https://unidormdb-default-rtdb.europe-west1.firebasedatabase.app/"
        };

        IFirebaseClient _client;

        [HttpGet]
        public IActionResult GetDormitories()
        {
            _client = new FireSharp.FirebaseClient(_config);
            FirebaseResponse response = _client.Get("Dormitory");
            var data = JsonConvert.DeserializeObject<Dictionary<string, Dormitory>>(response.Body);
            var list = new List<Dormitory>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(item.Value);
                }
            }
            return Json(list);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Dormitory dormitory)
        {
            try
            {
                _client = new FireSharp.FirebaseClient(_config);

                PushResponse response = _client.Push("Dormitory/", dormitory);
                dormitory.dormId = response.Result.name;
                SetResponse setResponse = _client.Set("Dormitory/" + dormitory.dormId, dormitory);

                if (setResponse.StatusCode == HttpStatusCode.OK)
                {
                    ModelState.AddModelError(string.Empty, "Added Successfully");
                    return Json(new { success = true, message = "Added Successfully" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong!!");
                    return Json(new { success = false, message = "Something went wrong!!" });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost]
        public IActionResult Edit([FromBody] Dormitory dormitory)
        {
            _client = new FireSharp.FirebaseClient(_config);
            SetResponse response = _client.Set("Dormitory/" + dormitory.dormId, dormitory);
            return Json(new { message = "Data updated successfully" });
        }

        [HttpPost]
        public IActionResult Delete([FromBody] Dormitory dormitory)
        {
            _client = new FireSharp.FirebaseClient(_config);
            FirebaseResponse response = _client.Delete("Dormitory/" + dormitory.dormId);
            return Json(new { message = "Deleted successfully" });
        }
    }
}
