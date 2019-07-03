using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DroneWebConsole.Controllers
{
    [AdminRequestFilter]
    [Route("Admin")]
    [Route("")]
    public class AdminController : Controller
    {

        HostingEnvironment host;

        public AdminController(HostingEnvironment e)
        {
            host = e;
        }

        [Route("Dashboard")]
        [Route("")]
        public IActionResult Dashboard()
        {
            ViewBag.title = "Dashboard";
            return View();
        }

        [Route("Gallery")]
        public IActionResult Gallery(string search = "", int page = 1)
        {
            ViewBag.title = "Gallery";
            int itemsPerPage = 12;
            var gcol = globals.getDB().GetCollection<mGalleryItem>("mGalleryItem");

            if (search != null && search != "")
            {
                var gitems = gcol.Find(x => x.area_name.Contains(search)).ToList();//.GroupBy(x => x.area_name).ToList();
                var items_ = (from i in gitems select i).AsQueryable().ToPagedList(page, itemsPerPage);
                ViewBag.items = items_;

            }
            else
            {
                var gitems = gcol.Find(x => x._id != null).ToList();//.GroupBy(x => x.area_name).ToList();
                var items_ = (from i in gitems select i).AsQueryable().ToPagedList(page, itemsPerPage);
                ViewBag.items = items_;
            }
            ViewBag.search = search;

            return View();
        }

        [Route("Controller")]
        public IActionResult Controller()
        {
            ViewBag.title = "Controller";
            return View();
        }

        [Route("Analytics")]
        public IActionResult Analytics()
        {
            ViewBag.title = "Analytics";
            return View();
        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();//clear all sessions
            return RedirectToAction("Login", "Auth");
        }

        /// <summary>
        /// execute the commmand from the mqtt controller
        /// </summary>
        [Route("execCommand")]
        public JsonResult execCommand(string command, string drone)
        {
            //execute the commands but dont store in databse

            //send the command to the drone async, remember to send the command to the correct drone when we get more drones
            Task.Run(() =>
            {
                if (MQTT.MQTT.mqtt == null || !MQTT.MQTT.mqtt.IsConnected) MQTT.MQTT.initMqttClient();//check init
                MQTT.MQTT.mqtt.Publish("droneCommands", Encoding.ASCII.GetBytes(command));
            });


            //giving the mavlink drone some aditional commands
            MQTT.MQTT.mqtt.Publish("mavlink_drone_commands", Encoding.ASCII.GetBytes(command), 1, false);


            var nc = new mCommands();
            nc.drone_id = drone;
            nc.command = command;
            nc.response = "ok";
            return Json(nc);//return the commmand to the front end

        }



       



    }
}
