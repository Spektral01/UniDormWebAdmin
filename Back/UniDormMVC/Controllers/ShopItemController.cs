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
    public class ShopItemController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Rc1Je3RMFk9aUZepyOr2hYsM3NbL0n3HTA3Gfcgm",
            BasePath = "https://unidormdb-default-rtdb.europe-west1.firebasedatabase.app/"
        };

        IFirebaseClient client;

        [HttpGet]
        public IActionResult GetShopItems()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("ShopItem");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<ShopItem>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<ShopItem>(((JProperty)item).Value.ToString()));
                }
            }

            // Возвращаем данные в формате JSON
            return Json(list);
        }

        [HttpPost]
        public ActionResult Create([FromBody] ShopItem data)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);

                // Отправка данных на Firebase
                PushResponse response = client.Push("ShopItem/", data);
                data.itemId = response.Result.name;
                SetResponse setResponse = client.Set("ShopItem/" + data.itemId, data);

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
        public ActionResult Edit([FromBody] ShopItem item)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("ShopItem/" + item.itemId, item);
            return Json(new { message = "Data updated successfully" });
        }

        [HttpPost]
        public ActionResult Delete([FromBody] ShopItem item)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("ShopItem/" + item.itemId);
            return Json(new { message = "Deleted successfully" });
        }
    }
}
