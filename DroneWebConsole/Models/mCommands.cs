using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

/// <summary>
/// Summary description for listinstructions
/// </summary>
public class mCommands
{
    public string _id { get; set; } = Guid.NewGuid().ToString();
    public DateTime time { get; set; } = DateTime.Now;
    public string drone_id { get; set; }
    public string command { get; set; }
    public string response { get; set; }
}