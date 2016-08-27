using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;


//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////


//implemented CreateRapid based on logic from the
//Dynamo_ABB utility function with contributions from Matt Jezyk and Mike Dewberry
//converted to DynamoToRobot utility function at Autodesk, Inc. Waltham

//implemented PlaneToQuaternian based on logic from the 
//Design Robotics Group @ Harvard Gsd with contributions from Sola Grantham, Anthony Kane, Nathan King, Jonathan Grinham, and others. 
//converted to Dynamo_ABB utility function at the Virginia Tech Robot Summit


//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////


namespace Dynamo_TORO
{
    internal class RobotUtils
    {

        public static List<double> PlaneToQuaternian(Plane plane)
        {

            //Point origin = plane.Origin;
            Vector xVect = plane.XAxis;
            Vector yVect = plane.YAxis;
            Vector zVect = plane.Normal;

            double s, trace;
            double x1, x2, x3, y1, y2, y3, z1, z2, z3;
            double q1, q2, q3, q4;

            x1 = xVect.X;
            x2 = xVect.Y;
            x3 = xVect.Z;
            y1 = yVect.X;
            y2 = yVect.Y;
            y3 = yVect.Z;
            z1 = zVect.X;
            z2 = zVect.Y;
            z3 = zVect.Z;

            trace = x1 + y2 + z3 + 1;

            if (trace > 0.00001)
            {
                // s = (trace) ^ (1 / 2) * 2
                s = Math.Sqrt(trace) * 2;
                q1 = s / 4;
                q2 = (-z2 + y3) / s;
                q3 = (-x3 + z1) / s;
                q4 = (-y1 + x2) / s;

            }
            else if (x1 > y2 && x1 > z3)
            {
                //s = (x1 - y2 - z3 + 1) ^ (1 / 2) * 2
                s = Math.Sqrt(x1 - y2 - z3 + 1) * 2;
                q1 = (z2 - y3) / s;
                q2 = s / 4;
                q3 = (y1 + x2) / s;
                q4 = (x3 + z1) / s;

            }
            else if (y2 > z3)
            {
                //s = (-x1 + y2 - z3 + 1) ^ (1 / 2) * 2
                s = Math.Sqrt(-x1 + y2 - z3 + 1) * 2;
                q1 = (x3 - z1) / s;
                q2 = (y1 + x2) / s;
                q3 = s / 4;
                q4 = (z2 + y3) / s;
            }

            else
            {
                //s = (-x1 - y2 + z3 + 1) ^ (1 / 2) * 2
                s = Math.Sqrt(-x1 - y2 + z3 + 1) * 2;
                q1 = (y1 - x2) / s;
                q2 = (x3 + z1) / s;
                q3 = (z2 + y3) / s;
                q4 = s / 4;

            }
            List<double> quatDoubles = new List<double>();
            quatDoubles.Add(q1);
            quatDoubles.Add(q2);
            quatDoubles.Add(q3);
            quatDoubles.Add(q4);
            return quatDoubles;
        }





        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////





        public static int closestSpeed(double searchValue)
        {
            double[] s = { 5, 10, 20, 30, 40, 50, 60, 80, 100, 150, 200, 300, 400, 500, 600, 800, 1000, 1500, 2000, 2500, 3000, 4000, 5000, 6000, 7000 };
            double currentNearest = s[0];
            double currentDifference = Math.Abs(currentNearest - searchValue);

            for (int i = 1; i < s.Length; i++)
            {
                double diff = Math.Abs(s[i] - searchValue);
                if (diff < currentDifference)
                {
                    currentDifference = diff;
                    currentNearest = s[i];
                }
            }

            return (int)currentNearest;
        }



        public static int closestZone(double searchValue)
        {
            double[] z = { 0, 1, 5, 10, 15, 20, 30, 40, 50, 60, 80, 100, 150, 200 };
            double currentNearest = z[0];
            double currentDifference = Math.Abs(currentNearest - searchValue);

            for (int i = 1; i < z.Length; i++)
            {
                double diff = Math.Abs(z[i] - searchValue);
                if (diff < currentDifference)
                {
                    currentDifference = diff;
                    currentNearest = z[i];
                }
            }

            return (int)currentNearest;
        }

        




    }

    /// <summary>
    /// Functions for the UI controls
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class ToroUIfunctions
    {
        [IsVisibleInDynamoLibrary(false)]
        ToroUIfunctions()
        {
            
        }

        public static string processUIdata(string filePath, List<string> cnstList, List<string> instList, List<string> tool, List<string> wobj)
        {

            Dictionary<string, string> writeData = null;
            Task writeFile = new TaskFactory().StartNew(() => writeData= Write.createRapidCode(filePath, cnstList, instList, tool, wobj));
            writeFile.Wait();
            //var writeData = Write.createRapid0(filePath, cnstList, instList, tool, wobj);
            return writeData["robotCode"];



        }
    }
}
