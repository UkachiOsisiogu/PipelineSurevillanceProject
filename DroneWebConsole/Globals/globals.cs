using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Html;
using uPLibrary.Networking.M2Mqtt;

/// <summary>
/// Summary description for globals
/// </summary>
public class globals
{

    public static MongoClient client;
    public static IMongoDatabase database;
    public static string dconnectionString = "mongodb://localhost";
    public static string supportEmail="porchlyt@gmail.com";
    public static string privacyEmail="privacy@VPM.com";
    public static string customerCareEmail= "consumercare@KVPM.com";
    public static string supportMobile= "+234 812 057 2154";
    public static string chatSupport = "<script type='text/javascript' id='553e47e969b4b7fe2b2999bada2bf72d' src='//porchlyt.com:8080/script.php?id=553e47e969b4b7fe2b2999bada2bf72d' defer></script>";
    public static string chatSupportWithEmail = chatSupport;
    //public static string chatSupportWithEmail = "<script type='text/javascript'>var lz_data = {overwrite:false,112:'<!--replace_me_with_Email-->'};</script><script type='text/javascript' id='553e47e969b4b7fe2b2999bada2bf72d' src='//porchlyt.com:8080/script.php?id=553e47e969b4b7fe2b2999bada2bf72d' defer></script>";
    public static string cloudsms_api = "http://localhost:61506/api/";
    public static string cloudsms_user_id = "12345";


    public static string getId()
    {
        Guid g;
        g = Guid.NewGuid();
        return g.ToString();//DateTime.Now.ToBinary().ToString();
    }

    public static IMongoDatabase getDB()
    {
        if (client == null) client = new MongoClient(dconnectionString);
        if (database == null) database = client.GetDatabase("drone_web_console");
        return database;

    }




    public static string getLocalCurrencyNG(double money=0.00)
    {
        return String.Format(new System.Globalization.CultureInfo("en-NG"), "{0:C}", money);
    }
    //

    public static string trimString(string text,int length )
    {
        if(text.Length>length)
        {
            return text.Substring(0, length) + "...";
        }
        else
        {
            return text;
        }
    }

    // Generate a random password of a given length (optional)  
    public static string RandomPassword(int size = 0)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(RandomString(4, true));
        builder.Append(RandomNumber());
        builder.Append(RandomString(2, false));
        return builder.ToString();
    }

    public static string RandomString(int size, bool lowerCase)
    {
        StringBuilder builder = new StringBuilder();
        Random random = new Random();
        char ch;
        for (int i = 0; i < size; i++)
        {
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            builder.Append(ch);
        }
        if (lowerCase)
            return builder.ToString().ToLower();
        return builder.ToString();
    }

    public static int RandomNumber()
    {
        // Generate a random number  
        Random random = new Random();
        // Any random integer   
        int num = random.Next();
        return num;
    }

    //log data into a text file
    public static void logdata(object data)
    {
        try
        {
            System.IO.File.AppendAllText(@"c:\qbit\logs\vpm.qbit", data + Environment.NewLine);
        }
        catch (Exception ex)
        {
            System.IO.File.AppendAllText(@"c:\qbit\logs\vpm.qbit", data + Environment.NewLine);
        }
    }

    public static string getmd5(string input)
    {


        // step 1, calculate MD5 hash from input

        MD5 md5 = System.Security.Cryptography.MD5.Create();

        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

        byte[] hash = md5.ComputeHash(inputBytes);

        // step 2, convert byte array to hex string

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < hash.Length; i++)

        {

            sb.Append(hash[i].ToString("X2"));

        }

        return sb.ToString();

    }



    public static void emailNotifcation(string to,string from,string message, HostingEnvironment host)
    {
        var email = new globals.emailMessage();
        email.to = to;
        email.subject = "New Messgae Notification";
        var emsg = System.IO.File.ReadAllText(host.WebRootPath + "/emialViews/email_notification.html");
        emsg = emsg.Replace("{{message}}", "Good day, You have a new message from " + from + "<br>: " + message);
        email.message = emsg;
        globals.sendEmail(email);
    }


    public class emailMessage
    {
        private string from = "postmansentinel@gmail.com";
        private string password = "QazWsxEdc@123";
        private string fromName = "Porchlyt";

        public string to { get; set; }
        public string subject { get; set; } = "System notification";
        public string message { get; set; }


        public string getFrom()
        {
            return this.from;
        }
        public string getPassword()
        {
            return this.password;
        }
        public string getFromName()
        {
            return this.fromName;
        }
    }

    public static void sendEmail(List<string> to, string message, string subject = "System Notification")
    {
        message = message.Replace("<br>", "/n");
        try
        {

            var email = new globals.emailMessage();
            Task.Run(() =>
            {
                foreach (var e in to)
                {
                    try
                    {
                        email = new globals.emailMessage();
                        email.to = e;
                        email.message = message;
                        email.subject = subject;
                        globals.sendEmail(email);
                    }
                    catch (Exception ex)
                    {
                        globals.logdata(ex.Message);
                    }
                }


            });
        }
        catch (Exception ex)
        {
            logdata(ex + "");
        }

    }

    public static bool sendEmail(emailMessage e)
    {
        bool sent = false;
        try
        {
            new SmtpClient
            {
                Host = "Smtp.Gmail.com",
                Port = 587,
                EnableSsl = true,
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(e.getFrom(), e.getPassword())
            }.Send(new MailMessage { IsBodyHtml = true, From = new MailAddress(e.getFrom(), e.getFromName()), To = { e.to }, Subject = e.subject, Body = e.message, BodyEncoding = Encoding.UTF8 });

            sent = true;
        }
        catch (Exception ex)
        {
            globals.logdata(ex + "");
        }

        return sent;
    }


















}//class