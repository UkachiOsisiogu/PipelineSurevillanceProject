using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for mGallery
/// </summary>
public class mGalleryItem
{
    public string _id { get; set; } = Guid.NewGuid().ToString();
    public string area_name { get; set; }//get set the area name using geolocation api to get the area name
    public string url { get; set; }//file path
    public string mime_type { get; set; }
    public string drone_id { get; set; }//name of drone that took the pic
    public DateTime date { get; set; } = DateTime.Now;
}




