using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;


namespace DroneWebConsole.Controllers
{
    [Route("api")]
    public class apiController : Controller
    {

        HostingEnvironment host;

        public apiController(HostingEnvironment e)
        {
            host = e;
        }


        [Route("upload_image_from_pi")]
        public string upload_image_from_pi(IFormCollection file)
        {
            try
            {
                //upload the image from the raspberry pi then update the webconsole to read the image
                var image = new mGalleryItem();
                var filename = Path.Combine(host.WebRootPath, "uploads/" + image._id + Path.GetExtension(file.Files[0].FileName));
                using (var stream = new FileStream(filename, FileMode.Create))
                {
                    file.Files[0].CopyTo(stream);
                    stream.Close();
                    stream.Dispose();
                }
                image.url = "/uploads/" + Path.GetFileName(filename);
                image.drone_id = "drone_1";
                image.area_name = "web_console_image";
                var gcol = globals.getDB().GetCollection<mGalleryItem>("mGalleryItem");
                gcol.InsertOne(image);
                //tell the mqtt listener to pic up the image
                dynamic notification = new JObject();
                notification.type = "drone_image_snap";
                notification.img_url = image.url;
                MQTT.MQTT.mqtt.Publish("drone_web_console_server", Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(notification)), 0, false);
                return "ok";
            }
            catch (Exception ex)
            {
                return "err";
            }
        }//.upload_image_from_pi




        //send mavlink command to the drone
        [Route("send_mav_link_command_to_drone")]
        public static string send_mav_link_command_to_drone(string command)
        {
            try
            {
                MQTT.MQTT.mqtt.Publish("mavlink_drone_commands", Encoding.ASCII.GetBytes(command), 1, false);
                dynamic r = new JObject();
                r.res = "ok";
                r.msg = "command sent";
                return JsonConvert.SerializeObject(r);
            }
            catch (Exception ex)
            {
                dynamic r = new JObject();
                r.res = "err";
                r.msg = ex.Message;
                return JsonConvert.SerializeObject(r);
            }

        }//.send_mav_link_command_to_drone








    }//class controller
}//namespace
