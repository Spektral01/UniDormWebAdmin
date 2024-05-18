using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UniDormMVC.Models;
using System.Net;

namespace UniDormMVC.Controllers
{
    public class NotificationController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Rc1Je3RMFk9aUZepyOr2hYsM3NbL0n3HTA3Gfcgm",
            BasePath = "https://unidormdb-default-rtdb.europe-west1.firebasedatabase.app/"
        };

        IFirebaseClient client;

        [HttpGet]
        public IActionResult GetNotif()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Notification");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Notification>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Notification>(((JProperty)item).Value.ToString()));
                }
            }

            // Возвращаем данные в формате JSON
            return Json(list);
        }

        [HttpPost]
        public ActionResult Create([FromBody]Notification data)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);

                // Отправка данных на Firebase
                PushResponse response = client.Push("Notification/", data);
                data.notifID = response.Result.name;
                SetResponse setResponse = client.Set("Notification/" + data.notifID, data);

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

        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Notification/" + id);
            Notification data = JsonConvert.DeserializeObject<Notification>(response.Body);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit([FromBody] Notification notif)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Notification/" + notif.notifID, notif);
            return Json(new { message = "Data updated successfully" });
        }

        [HttpPost]
        public ActionResult Delete([FromBody] Notification notif)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Notification/" + notif.notifID);
            return Json(new { message = "Deleted successfully" });
        }
    }
}
