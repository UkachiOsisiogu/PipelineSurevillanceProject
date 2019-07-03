using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for users
/// </summary>
public class mUsers
{
    public string _id { get; set; }=Guid.NewGuid().ToString();
    public string username { get; set; }
    public string password { get; set; }
 }