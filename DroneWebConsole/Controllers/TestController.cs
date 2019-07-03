using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DroneWebConsole.Controllers
{
    [Route("test")]
    public class TestController : Controller
    {
        [Route("")]
        public string Index()
        {
            var scol = globals.getDB().GetCollection<mSensor>("mSensor");
            Random r = new Random();

            for (int i = 2; i < 3; i++)
            {
                var s = new mSensor();
                s.sensorName = "ss" + i;
                s.lat = "9.0765";
                s.lng = "7.3986";
                s.description = "seismic sensor";
                s.pingTime = 10000;
                for (int p = 0; p < 1200; p++)
                {
                    int rInt = r.Next(0, 250);
                    var pd = new SensorPingData();
                    pd.value = rInt;
                    s.sensorPingData.Add(pd);
                }

                //scol.InsertOne(s);
            }

            for (int i = 2; i < 3; i++)
            {
                var s = new mSensor();
                s.sensorName = "sp" + i;
                s.lat = "9.0765";
                s.lng = "7.3986";
                s.description = "pressure sensor";
                s.pingTime = 10000;
                for (int p = 0; p < 1200; p++)
                {
                    int rInt = r.Next(2000, 4000);
                    var pd = new SensorPingData();
                    pd.value = rInt;
                    s.sensorPingData.Add(pd);
                }

               //scol.InsertOne(s);
            }


            
            return DateTime.Now.ToLongDateString();
        }
    }
}
