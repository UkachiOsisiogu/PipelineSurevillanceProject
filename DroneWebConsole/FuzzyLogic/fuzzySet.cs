using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for fuzzySet
/// </summary>
public class fuzzySet
{

    //membership functions


    /// <summary>
    /// triangualr membership function
    /// </summary>
    /// <param name="x">inpout</param>
    /// <param name="a">first lower left triangle</param>
    /// <param name="m">triangular midpoint</param>
    /// <param name="b">second lower right triangle</param>
    /// <returns></returns>
    public static double mTriangular(double x, double a, double m, double b)
    {
        if (x <= a)
        {
            return 0;
        }
        else if (a < x && x <= m)
        {
            var output = (x - a) / (m - a);
            return output;
        }
        else if (m < x && x < b)
        {
            var output = (b - x) / (b - m);
            return output;
        }
        else if (x >= b)
        {
            return 0;
        }
        else
            return 0;
    }



    /// <summary>
    /// trapezoidal membership fuction
    /// </summary>
    /// <param name="x">the input</param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static double mTrapezoidal(double x, double a, double b, double c, double d)
    {
        if (x < a || x > d)
        {
            return 0;
        }
        else if (a <= x && x <= b)
        {
            var output = (x - a) / (b - a);
            return output;
        }
        else if (b <= x && x <= c)
        {
            return 1;
        }
        else if (c <= x && x <= d)
        {
            var output = (d - x) / (d - c);
            return output;
        }
        else
            return 0;
    }


    /// <summary>
    /// trapezidal left membership function
    /// </summary>
    /// <param name="x">input</param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <returns></returns>
    public static double mTrapezoidalLeft(double x, double c, double d)
    {
        if (x > d)
        {
            return 0;
        }
        else if (c <= x && x <= d)
        {
            var output = (d - x) / (d - c);
            return output;
        }
        else if (x < c)
        {
            return 1;
        }
        else
            return 0;
    }

    /// <summary>
    /// trapezoidal membership far right
    /// </summary>
    /// <param name="x">input</param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static double mTrapezoidalRight(double x, double a, double b)
    {
        if (x < a)
        {
            return 0;
        }
        else if (a <= x && x <= b)
        {
            var output = (x - a) / (b - a);
            return output;
        }
        else if (x > b)
        {
            return 1;
        }
        else
            return 0;
    }

    /// <summary>
    /// gaussian membership function
    /// </summary>
    /// <param name="x">input</param>
    /// <param name="m">midpoint</param>
    /// <param name="k">standard deviation</param>
    /// <returns></returns>
    public static double mGaussian(double x, double m, double k)
    {
        var pow = ((x - m) * (x - m)) / (2 * k * k);
        pow = pow * (-1);


        var membership = Math.Exp(pow);
        return membership;
    }

    public fuzzySet()
    {
        //
        // TODO: Add constructor logic here
        //
    }



    //rules engine
    /// <summary>
    /// takes parameters of decibals and pressure and returns the classification based on the rules
    /// </summary>
    /// <param name="dbLevel"> decibal level ie frequency</param>
    /// <param name="m_pressure">pressure sensor reading</param>
    /// <returns></returns>
    public static string getResult(string dbLevel, string m_pressure)
    {
        string NV = "No Vandalism";
        string V = "Vandalism";
        string L = "low";
        string M = "medium";
        string H = "high";


        if (dbLevel == L && m_pressure == L) return V;
        if (dbLevel == L && m_pressure == M) return NV;
        if (dbLevel == H && m_pressure == L) return V;
        if (dbLevel == L && m_pressure == M) return NV;
        if (dbLevel == M && m_pressure == M) return NV;
        if (dbLevel == H && m_pressure == M) return V;
        if (dbLevel == L && m_pressure == H) return NV;
        if (dbLevel == M && m_pressure == H) return NV;
        if (dbLevel == H && m_pressure == H) return V;

        return "Err";


    }


}

