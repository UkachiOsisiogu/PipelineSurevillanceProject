using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for mSensor
/// </summary>
public class mSensor
{
    public string _id { get; set; } = Guid.NewGuid().ToString();
    public string sensorName { get; set; }
    public string lat { get; set; }//sensor latitude
    public string lng { get; set; }//sensor longitude
    public string description { get; set; }
    public long pingTime { get; set; } = 10000;//time between each ping
    public List<SensorPingData> sensorPingData { get; set; } = new List<SensorPingData>();

}


public class SensorPingData
{
    public string _id { get; set; } = Guid.NewGuid().ToString();
    public DateTime timestamp { get; set; } = DateTime.Now;//timestamp of the ping
    public double value { get; set; }//ping value
}