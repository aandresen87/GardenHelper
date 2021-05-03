using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GHv2.Models;
using Firebase.Database.Query;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GHv2.Controllers
{
    public class HomeController : Controller
    {



        public IActionResult Index() { return View(); }

        [HttpPost]
        public async Task<ActionResult> Index(string currentUser)
        {
            //download from firebase
            var client2 = new Firebase.Database.FirebaseClient("https://clddb-a63b0-default-rtdb.firebaseio.com/");

            //Retrieve data from Firebase
            var nameList = new List<string>();
            var locList = new List<string>();
            var inList = new List<string>();
            var outList = new List<string>();
            var UserList = new List<string>();

            if (currentUser == null)
            {
                var dbPlants = await client2
              .Child("Garden")
              .Child("default")
              .OnceAsync<InfoContext>();
                
                //Convert JSON data to original datatype
                foreach (var i in dbPlants)
                {
                    nameList.Add(i.Object.Name);
                    locList.Add(i.Object.Location);
                    inList.Add(i.Object.DateIn);
                    outList.Add(i.Object.DateOut);
                    UserList.Add(i.Object.CurrentUser);
                }
            }
            else
            {
                var dbPlants = await client2
              .Child("Garden")
              .Child(currentUser)
              .OnceAsync<InfoContext>();
                
                //Convert JSON data to original datatype
                foreach (var i in dbPlants)
                {
                    nameList.Add(JsonConvert.ToString(i.Object.Name).ToString().Trim('"'));
                    locList.Add(JsonConvert.ToString(i.Object.Location).ToString().Trim('"'));
                    inList.Add(JsonConvert.ToString(i.Object.DateIn).ToString().Trim('"'));
                    outList.Add(JsonConvert.ToString(i.Object.DateOut).ToString().Trim('"'));
                    UserList.Add(JsonConvert.ToString(i.Object.CurrentUser).ToString().Trim('"'));
                }
            }

            ViewBag.NameList = nameList;
            ViewBag.LocList = locList;
            ViewBag.InList = inList;
            ViewBag.OutList = outList;
            return View();
        }


        public IActionResult About() { return View(); }

        [HttpPost]
        public async Task<ActionResult> About(string name, string location, string dateIn, string dateOut, string currentUser)
        {
            // upload to firebase <----this works
            var currentTestData = new InfoContext()
            {
                Name = name,
                Location = location,
                DateIn = dateIn,
                DateOut = dateOut,
                CurrentUser = currentUser
            };
            var client = new Firebase.Database.FirebaseClient("https://clddb-a63b0-default-rtdb.firebaseio.com/");
            await client
              .Child("Garden")
              .Child(currentUser)
              .PostAsync(currentTestData);
            
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Remove()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Remove(string plantName, string currentUser)
        {
           
            var del = new Firebase.Database.FirebaseClient("https://clddb-a63b0-default-rtdb.firebaseio.com/");
            await del.Child("Garden")
                .Child(currentUser)
                .DeleteAsync();
             
            return View();
        }

        
        public IActionResult Privacy( )
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
