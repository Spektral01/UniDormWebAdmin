using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UniDormMVC.Models;

namespace UniDormMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IFirebaseConfig _config;
        private IFirebaseClient _client;

        public UserController()
        {
            _config = new FirebaseConfig
            {
                AuthSecret = "Rc1Je3RMFk9aUZepyOr2hYsM3NbL0n3HTA3Gfcgm",
                BasePath = "https://unidormdb-default-rtdb.europe-west1.firebasedatabase.app/"
            };
            _client = new FireSharp.FirebaseClient(_config);
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            FirebaseResponse response = _client.Get("UserData");
            if (response.Body == "null")
            {
                return NotFound();
            }
            else
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, User>>(response.Body);
                return Ok(data.Values);
            }
        }

        [HttpGet]
        public IActionResult GetUsersByName(string name)
        {
            try
            {
                FirebaseResponse response = _client.Get("UserData");
                if (response.Body == "null")
                {
                    return NotFound();
                }
                else
                {
                    var data = JsonConvert.DeserializeObject<Dictionary<string, User>>(response.Body);
                    var users = new List<User>();
                    foreach (var userData in data.Values)
                    {
                        // Проверяем, содержит ли имя пользователя введенную подстроку
                        if (userData.Name.Contains(name))
                        {
                            users.Add(userData);
                        }
                    }
                    return Ok(users);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Error: {ex.Message}" });
            }
        }


        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            try
            {
                PushResponse response = _client.Push("UserData", user);
                user.UserId = response.Result.name;
                SetResponse setResponse = _client.Set($"UserData/{user.UserId}", user);
                return Ok(new { success = true, message = "User added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpPut]
        public IActionResult UpdateUser(string userId, [FromBody] User user)
        {
            try
            {
                SetResponse response = _client.Set($"UserData/{userId}", user);
                return Ok(new { success = true, message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        [HttpDelete]
        public IActionResult DeleteUser(string userId)
        {
            try
            {
                FirebaseResponse response = _client.Delete($"UserData/{userId}");
                return Ok(new { success = true, message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Error: {ex.Message}" });
            }
        }
    }
}
