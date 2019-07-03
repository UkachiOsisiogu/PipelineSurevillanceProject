using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB;
using MongoDB.Bson;

/// <summary>
/// Summary description for drones
/// </summary>
public class mDrones
{
    public string _id { get; set; } = Guid.NewGuid().ToString();
    public string drone_name { get; set; }
    public string latitude { get; set; }
    public string longitude { get; set; }
    public bool autopilot { get; set; }
}