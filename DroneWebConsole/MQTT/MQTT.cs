using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DroneWebConsole.MQTT
{
    public class MQTT
    {
        public static MqttClient mqtt;

        public static void initMqttClient()
        {
           // try
            {
                if (mqtt != null) return;//only init the instance once
                mqtt = new MqttClient("192.168.138.1");
                mqtt.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                string clientId = "123Server" +DateTime.Now;//this is to make the connection unique
                mqtt.Connect(clientId);
                mqtt.Subscribe(new string[] { "drone_web_console_server" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            }
            //catch (Exception ex) { }
        }


        public static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            //recieve message and route it to the appropriate controller
            var msg = Encoding.ASCII.GetString(e.Message);
            dynamic json = JsonConvert.DeserializeObject(msg);
            var type = json.type;

            if(type=="pressure_sensor_reading")
            {
                double value = double.Parse((string)json.value);
                if(value<=70)//pressure drop to 70
                {
                    //activate drone
                    mqtt.Publish("mavlink_drone_commands", Encoding.ASCII.GetBytes("func_arm_and_takeoff"), 1, false);
                }

            }
        }








    }//class







}//namespace
