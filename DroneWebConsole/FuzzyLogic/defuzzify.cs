using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
/// <summary>
/// Summary description for defuzzify
/// </summary>
public class defuzzify
{
    public static double averageSensorReading(string sensorName, double rangeMin, double rangeMax)
    {
        var scol = globals.getDB().GetCollection<mSensor>("mSensor");
        var sensor = scol.Find(x => x.sensorName == sensorName).FirstOrDefault();
        if (sensor == null) return 0;
        var average = sensor
            .sensorPingData
            .Where(x => x.value >= rangeMin && x.value <= rangeMax)
            .ToList()
            .Select(x => x.value)
            .Average();
        return average;
    }

    public static List<string> SeismicSensorsMembership()
    {
        var scol = globals.getDB().GetCollection<mSensor>("mSensor");
        var seismicSensors = scol.Find(x => x.sensorName.Contains("ss")).ToList();//collect all ss sensors
        var proximitySensors = scol.Find(x => x.sensorName.Contains("sp")).ToList();//collect all sp sensors
        List<double> ssAverages = new List<double>();//averages of seismic sensors
        foreach (var s in seismicSensors)
        {
            var avg = averageSensorReading(s.sensorName, 0, 99999999);
            ssAverages.Add(avg);
        }

        //degree of memberdship seismic sensors

        //ss sensor 1
        var dMemLeft_ss_1 = fuzzySet.mTrapezoidalLeft(ssAverages[0], 0, 300);
        var dMemCenter_ss_1 = fuzzySet.mTrapezoidal(ssAverages[0], 250, 300, 500, 600);
        var dMemRight_ss_1 = fuzzySet.mTrapezoidalRight(ssAverages[0], 500, 600);

        //ss sensor 2
        var dMemLeft_ss_2 = fuzzySet.mTrapezoidalLeft(ssAverages[1], 0, 300);
        var dMemCenter_ss_2 = fuzzySet.mTrapezoidal(ssAverages[1], 250, 300, 500, 600);
        var dMemRight_ss_2 = fuzzySet.mTrapezoidalRight(ssAverages[1], 500, 600);

        //ss sensor 3
        var dMemLeft_ss_3 = fuzzySet.mTrapezoidalLeft(ssAverages[2], 0, 300);
        var dMemCenter_ss_3 = fuzzySet.mTrapezoidal(ssAverages[2], 250, 300, 500, 600);
        var dMemRight_ss_3 = fuzzySet.mTrapezoidalRight(ssAverages[2], 500, 600);


        //low medium high degree of membership seismic sensors
        var ss_1_dm = "low";
        var ss_2_dm = "low";
        var ss_3_dm = "low";

        var ss_1_m = Math.Max(Math.Max(dMemLeft_ss_1, dMemCenter_ss_1), dMemRight_ss_1);
        if (ss_1_m == dMemLeft_ss_1) ss_1_dm = "low";
        if (ss_1_m == dMemCenter_ss_1) ss_1_dm = "medium";
        if (ss_1_m == dMemRight_ss_1) ss_1_dm = "high";

        var ss_2_m = Math.Max(Math.Max(dMemLeft_ss_2, dMemCenter_ss_2), dMemRight_ss_2);
        if (ss_2_m == dMemLeft_ss_2) ss_2_dm = "low";
        if (ss_2_m == dMemCenter_ss_2) ss_2_dm = "medium";
        if (ss_2_m == dMemRight_ss_2) ss_2_dm = "high";

        var ss_3_m = Math.Max(Math.Max(dMemLeft_ss_3, dMemCenter_ss_3), dMemRight_ss_3);
        if (ss_3_m == dMemLeft_ss_3) ss_3_dm = "low";
        if (ss_3_m == dMemCenter_ss_3) ss_3_dm = "medium";
        if (ss_3_m == dMemRight_ss_3) ss_3_dm = "high";

        return new List<string> {

            ss_1_dm,
            ss_2_dm,
            ss_3_dm,

        };

    }


    //pressure
    public static List<string> ProximitySensorsMembership()
    {
        var scol = globals.getDB().GetCollection<mSensor>("mSensor");
        var proximitySensors = scol.Find(x => x.sensorName.Contains("sp")).ToList();//collect all sp sensors
        List<double> spAverages = new List<double>();//averages of proxmity sensors
        foreach (var s in proximitySensors)
        {
            var avg = averageSensorReading(s.sensorName, 0, 99999999);
            spAverages.Add(avg);
        }

        //degree of membership

        //ss sensor 1
        List<double> sensor_1 = new List<double>();
        sensor_1.Add(fuzzySet.mTrapezoidalLeft(spAverages[0], 1600, 2000));
        sensor_1.Add(fuzzySet.mTrapezoidal(spAverages[0], 1600, 2000, 4000,4500));
        sensor_1.Add(fuzzySet.mTrapezoidalRight(spAverages[0], 4000,4500));

        //ss sensor 2
        List<double> sensor_2 = new List<double>();
        sensor_2.Add(fuzzySet.mTrapezoidalLeft(spAverages[1], 1600, 2000));
        sensor_2.Add(fuzzySet.mTrapezoidal(spAverages[1], 1600, 2000, 4000, 4500));
        sensor_2.Add(fuzzySet.mTrapezoidalRight(spAverages[1], 4000, 4500));

        //ss sensor 3
        List<double> sensor_3 = new List<double>();
        sensor_3.Add(fuzzySet.mTrapezoidalLeft(spAverages[2], 1600, 2000));
        sensor_3.Add(fuzzySet.mTrapezoidal(spAverages[2], 1600, 2000, 4000, 4500));
        sensor_3.Add(fuzzySet.mTrapezoidalRight(spAverages[2], 4000, 4500));

        //0,1,2,3,4,5 degree of membership proximity sensor
        var ss_1_dm = "";
        var ss_2_dm = "";
        var ss_3_dm = "";

        int index = 0;

        var ss_1_m = sensor_1.Max();//get the max then find its index in the list
        index = sensor_1.IndexOf(ss_1_m);//just the index of the maximum
        if (index == 0) ss_1_dm = "low";
        if (index == 1) ss_1_dm = "medium";
        if (index == 2) ss_1_dm = "high";

        var ss_2_m = sensor_2.Max();
        index = sensor_2.IndexOf(ss_2_m);//just the index of the maximum
        if (index == 0) ss_2_dm = "low";
        if (index == 1) ss_2_dm = "medium";
        if (index == 2) ss_2_dm = "high";

        var ss_3_m = sensor_3.Max();
        index = sensor_3.IndexOf(ss_3_m);//just the index of the maximum
        if (index == 0) ss_3_dm = "low";
        if (index == 1) ss_3_dm = "medium";
        if (index == 2) ss_3_dm = "high";

        //return the list
        return new List<string>
        {
            ss_1_dm
            , ss_2_dm
            , ss_3_dm
        };
    }


    public static string s1Result()
    {
        var dbLevel = SeismicSensorsMembership()[0];
        var mDistance = ProximitySensorsMembership()[0];
        var result = fuzzySet.getResult(dbLevel, mDistance);
        return result;
    }

    public static string s2Result()
    {
        var dbLevel = SeismicSensorsMembership()[1];
        var mDistance = ProximitySensorsMembership()[1];
        var result = fuzzySet.getResult(dbLevel, mDistance);
        return result;
    }

    public static string s3Result()
    {
        var dbLevel = SeismicSensorsMembership()[2];
        var mDistance = ProximitySensorsMembership()[2];
        var result = fuzzySet.getResult(dbLevel, mDistance);
        return result;
    }


    public static string LoadSensorGraphData(string sensorName, int rangeStart = 0, int rangeEnd = 100)
    {

        Random r = new Random();
        int rInt = r.Next(rangeStart, rangeEnd); //for ints
        int range = 100;
        double rDouble = r.NextDouble() * range; //for doubles


        var sdcol = globals.getDB().GetCollection<mSensor>("mSensor");
        var sensor = sdcol.Find(x => x.sensorName == sensorName).FirstOrDefault();
        if (sensor == null) return "";
        var timeStamp = DateTime.Now;
        //collect only data within 200 hours
        //uncomment this line to parameterize
        //var pingList = sensor.sensorPingData.Where(x => x.timestamp >= timeStamp.AddHours(-200)).ToList();
        var pingList = sensor.sensorPingData.Where(x => x._id!=null).ToList();
        string sdata = "";
        foreach (var p in pingList)
        {
            rInt = r.Next(rangeStart, rangeEnd);//random
            //old graph
            //sdata += "[" + rInt + ", " + p.value + ", 1],";
            //new graph
            sdata +=  p.value + ",";
        }
        return sdata;
    }


}