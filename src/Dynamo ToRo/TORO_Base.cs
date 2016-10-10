using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.RapidDomain;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;



namespace Dynamo_TORO
{
    /// <summary>
    /// testing
    /// </summary>
    internal class WIP_Welding
    {

        

        


        /// <summary>
        /// Create a spot-welding routine.
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="planes">Targets as planes</param>
        /// <param name="waitTime">Duration between welds (sec) </param>
        /// <param name="weldTime">Duration of weld (sec)</param>
        /// <param name="retract">Distance offset (Z) from target in tool-space</param>
        /// <returns></returns>
        [MultiReturn(new[] { "filePath", "robotCode" })]
        public static Dictionary<string, string> createWeldRoutine0(string filePath, [DefaultArgumentAttribute("{Plane.ByOriginNormal(Point.ByCoordinates(0,0,0),Vector.ByCoordinates(0,0,-1))}")] List<Plane> planes, double waitTime = 2.9, double weldTime = 0.9, double retract = -10)
        {
            // setup
            var targBuilder = new StringBuilder();
            int ct = planes.Count;
            foreach (Plane plane in planes)
            {
                RobTarget targ = Dynamo_TORO.DataTypes.RobTargetAtPlane(plane);
                string targ2 = "\n\t\t" + "[" + targ.ToString() + "],";
                if (plane == planes[planes.Count - 1]) { targ2 = "\n\t\t" + "[" + targ.ToString() + "]"; }
                targBuilder.Append(targ2);
            }

            // create rapid
            string r = "";
            using (var tw = new StreamWriter(filePath, false))
            {
                r =                         "MODULE MainModule" +
                                            "\n" +

                                            "\n\t" + "! variables" +
                                            "\n\t" + "CONST num wt:=" + waitTime + ";" +
                                            "\n\t" + "CONST num wd:=" + weldTime + ";" +
                                            "\n\t" + "CONST num dz:=" + retract + ";" +
                                            "\n" +

                                            "\n\t" + "! targets" +
                                            "\n\t" + "VAR robtarget targets{" + ct + ",1}:=" +
                                            "\n\t" + "[" + targBuilder.ToString() +
                                            "\n\t" + "];" +
                                            "\n" +

                                            "\n\t" + "! weld routine" +
                                            "\n\t" + "PROC wStart(robtarget target)" +
                                            "\n\t\t" + "MoveL RelTool(target,0,0,dz),v300,z5,t_welder\\WObj:=w_plate;" +
                                            "\n\t\t" + "MoveL target,v300,fine,t_welder\\WObj:=w_plate;" +
                                            "\n\t\t" + "SetDO welder,1;" +
                                            "\n\t\t" + "WaitTime\\InPos,wd;" +
                                            "\n\t\t" + "SetDO welder,0;" +
                                            "\n\t\t" + "MoveL RelTool(target,0,0,dz),v300,z5,t_welder\\WObj:=w_plate;" +
                                            "\n\t\t" + "WaitTime\\InPos,wt;" +
                                            "\n\t" + "ENDPROC\n" +

                                            "\n\t" + "! main routine" +
                                            "\n\t" + "PROC main()" +
                                            "\n\t\t" + "ConfL\\On;" +
                                            "\n\t\t" + "SingArea\\Wrist;" +
                                            "\n\t\t" + "FOR i FROM 1 TO " + ct + " DO" +
                                            "\n\t\t\t" + "TPWrite(valtostr(i))" + " + \" of " + ct + " \" + \"(\" + valtostr(100*i/" + ct + ") + \"%)\";" +
                                            "\n\t\t\t" + "wStart(targets{i,1});" +
                                            "\n\t\t" + "ENDFOR" +
                                            "\n\t\t" + "Stop;" +
                                            "\n\t" + "ENDPROC" +
                                            "\n" +

                                            "\n" + "ENDMODULE"
                                            ;
                tw.Write(r);
                tw.Flush();
            }

            // end step
            return new Dictionary<string, string>
            {
                {"filePath", filePath},
                {"robotCode", r}
            };
        }






        /// <summary>
        /// Create a spot-welding routine (with exit routine).
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="planes">Targets as planes</param>
        /// <param name="waitTime">Duration between welds (sec) </param>
        /// <param name="weldTime">Duration of weld (sec)</param>
        /// <param name="retract_tool">Distance offset (Z) from target in tool-space</param>
        /// <param name="retract_world">Distance offset (Z) from target in world-space</param>
        /// <returns></returns>
        [MultiReturn(new[] { "filePath", "robotCode" })]
        public static Dictionary<string, string> createWeldRoutine1(string filePath, [DefaultArgumentAttribute("{Plane.ByOriginNormal(Point.ByCoordinates(0,0,0),Vector.ByCoordinates(0,0,-1))}")] List<Plane> planes, double waitTime = 2.9, double weldTime = 0.9, double retract_tool = -10, double retract_world = 100)
        {
            // setup
            var targBuilder = new StringBuilder();
            int ct = planes.Count;
            foreach (Plane plane in planes)
            {
                RobTarget targ = Dynamo_TORO.DataTypes.RobTargetAtPlane(plane);
                string targ2 = "\n\t\t" + "[" + targ.ToString() + "],";
                if (plane == planes[planes.Count - 1]) { targ2 = "\n\t\t" + "[" + targ.ToString() + "]"; }
                targBuilder.Append(targ2);
            }

            // create rapid
            string r = "";
            using (var tw = new StreamWriter(filePath, false))
            {
                r = "MODULE MainModule" +
                                            "\n" +

                                            "\n\t" + "! variables" +
                                            "\n\t" + "CONST num wt:=" + waitTime + ";" +
                                            "\n\t" + "CONST num wd:=" + weldTime + ";" +
                                            "\n\t" + "CONST num rt:=" + retract_tool + ";" +
                                            "\n\t" + "CONST num rw:=" + retract_world + ";" +
                                            "\n" +

                                            "\n\t" + "! targets" +
                                            "\n\t" + "VAR robtarget targets{" + ct + ",1}:=" +
                                            "\n\t" + "[" + targBuilder.ToString() +
                                            "\n\t" + "];" +
                                            "\n" +

                                            "\n\t" + "! weld routine" +
                                            "\n\t" + "PROC wStart(robtarget target)" +
                                            "\n\t\t" + "MoveL Offs(RelTool(target,0,0,rt),0,0,rw),v300,z5,t_welder\\WObj:=w_plate;" +
                                            "\n\t\t" + "MoveL RelTool(target,0,0,rt),v300,z5,t_welder\\WObj:=w_plate;" +
                                            "\n\t\t" + "MoveL target,v300,fine,t_welder\\WObj:=w_plate;" +
                                            "\n\t\t" + "SetDO welder,1;" +
                                            "\n\t\t" + "WaitTime\\InPos,wd;" +
                                            "\n\t\t" + "SetDO welder,0;" +
                                            "\n\t\t" + "MoveL RelTool(target,0,0,rt),v300,z5,t_welder\\WObj:=w_plate;" +
                                            "\n\t\t" + "MoveL Offs(RelTool(target,0,0,rt),0,0,rw),v300,z5,t_welder\\WObj:=w_plate;" +
                                            "\n\t\t" + "WaitTime\\InPos,wt;" +
                                            "\n\t" + "ENDPROC\n" +

                                            "\n\t" + "! main routine" +
                                            "\n\t" + "PROC main()" +
                                            "\n\t\t" + "ConfL\\On;" +
                                            "\n\t\t" + "SingArea\\Wrist;" +
                                            "\n\t\t" + "FOR i FROM 1 TO " + ct + " DO" +
                                            "\n\t\t\t" + "TPWrite(valtostr(i))" + " + \" of " + ct + " \" + \"(\" + valtostr(100*i/" + ct + ") + \"%)\";" +
                                            "\n\t\t\t" + "wStart(targets{i,1});" +
                                            "\n\t\t" + "ENDFOR" +
                                            "\n\t\t" + "Stop;" +
                                            "\n\t" + "ENDPROC" +
                                            "\n" +

                                            "\n" + "ENDMODULE"
                                            ;
                tw.Write(r);
                tw.Flush();
            }

            // end step
            return new Dictionary<string, string>
            {
                {"filePath", filePath},
                {"robotCode", r}
            };
        }



        /// <summary>
        /// Create a spot-welding routine (with exit routine, with waittime-per-distance function).
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="planes">Targets as planes</param>
        /// <param name="waitTime">Duration between welds (sec) </param>
        /// <param name="weldTime">Duration of weld (sec)</param>
        /// <param name="retract_tool">Distance offset (Z) from target in tool-space</param>
        /// <param name="retract_world">Distance offset (Z) from target in world-space</param>
        /// <returns></returns>
        [MultiReturn(new[] { "filePath", "robotCode" })]
        public static Dictionary<string, string> createWeldRoutine2(string filePath, [DefaultArgumentAttribute("{Plane.ByOriginNormal(Point.ByCoordinates(0,0,0),Vector.ByCoordinates(0,0,-1))}")] List<Plane> planes, double waitTime = 2.9, double weldTime = 0.9, double retract_tool = -10, double retract_world = 100)
        {
            // setup
            var targBuilder = new StringBuilder();
            int ct = planes.Count;
            foreach (Plane plane in planes)
            {
                RobTarget targ = Dynamo_TORO.DataTypes.RobTargetAtPlane(plane);
                string targ2 = "\n\t\t" + "[" + targ.ToString() + "],";
                if (plane == planes[planes.Count - 1]) { targ2 = "\n\t\t" + "[" + targ.ToString() + "]"; }
                targBuilder.Append(targ2);
            }

            // create rapid
            string r = "";
            using (var tw = new StreamWriter(filePath, false))
            {
                r = "MODULE MainModule" +
                                            "\n" +

                                            "\n\t" + "! variables" +
                                            "\n\t" + "CONST num wt:=" + waitTime + ";" +
                                            "\n\t" + "CONST num wd:=" + weldTime + ";" +
                                            "\n\t" + "CONST num rt:=" + retract_tool + ";" +
                                            "\n\t" + "CONST num rw:=" + retract_world + ";" +
                                            "\n" +

                                            "\n\t" + "! targets" +
                                            "\n\t" + "VAR robtarget targets{" + ct + ",1}:=" +
                                            "\n\t" + "[" + targBuilder.ToString() +
                                            "\n\t" + "];" +
                                            "\n" +

                                            "\n\t" + "! weld routine" +
                                            "\n\t" + "PROC wStart(num i)" +
                                            "\n\t\t" + "IF (i=1) THEN" +
                                            "\n\t\t\t" + "WaitTime\\InPos,wt;" +
                                            "\n\t\t" + "ELSE" +
                                            "\n\t\t\t" + "WaitTime\\InPos,(wt/(Distance(targets{i,1}.trans,targets{i-1,1}.trans)+0.01));" +
                                            "\n\t\t" + "ENDIF" +
                                            "\n\t\t" + "MoveL Offs(RelTool(targets{i,1},0,0,-rt),0,0,rw),v300,z5,t_welder\\WObj:=w_plate;" +
                                            "\n\t\t" + "MoveL RelTool(targets{i,1},0,0,-rt),v300,z5,t_welder\\WObj:=w_plate;" +
                                            "\n\t\t" + "MoveL targets{i,1},v300,fine,t_welder\\WObj:=w_plate;" +
                                            "\n\t\t" + "SetDO welder,1;" +
                                            "\n\t\t" + "WaitTime\\InPos,wd;" +
                                            "\n\t\t" + "SetDO welder,0;" +
                                            "\n\t\t" + "MoveL RelTool(targets{i,1},0,0,-rt),v300,z5,t_welder\\WObj:=w_plate;" +
                                            "\n\t\t" + "MoveL Offs(RelTool(targets{i,1},0,0,-rt),0,0,rw),v300,z5,t_welder\\WObj:=w_plate;" +
                                            "\n\t" + "ENDPROC\n" +

                                            "\n\t" + "! main routine" +
                                            "\n\t" + "PROC main()" +
                                            "\n\t\t" + "ConfL\\On;" +
                                            "\n\t\t" + "SingArea\\Wrist;" +
                                            "\n\t\t" + "FOR i FROM 1 TO " + ct + " DO" +
                                            "\n\t\t\t" + "TPWrite(valtostr(i))" + " + \" of " + ct + " \" + \"(\" + valtostr(100*i/" + ct + ") + \"%)\";" +
                                            "\n\t\t\t" + "wStart(i);" +
                                            "\n\t\t" + "ENDFOR" +
                                            "\n\t\t" + "Stop;" +
                                            "\n\t" + "ENDPROC" +
                                            "\n" +

                                            "\n" + "ENDMODULE"
                                            ;
                tw.Write(r);
                tw.Flush();
            }

            // end step
            return new Dictionary<string, string>
            {
                {"filePath", filePath},
                {"robotCode", r}
            };
        }

        



        /// <summary>
        /// Sort points by Z value with indices; version 2.
        /// </summary>
        /// <param name="points">Point list</param>
        /// <returns></returns>
        [MultiReturn(new[] { "sortedPoints", "sortedIndices" })]
        public static Dictionary<string, List<object>> sortPointsByZ_2(List<Point> points)
        {
            var sorted = points
                .Select((coord, i) => new KeyValuePair<Point, int>(coord, i))
                .OrderBy(coord => coord.Key.Z)
                .ToList();

            List<object> sortedPoints = sorted.Select(coord => (object) coord.Key).ToList();
            List<object> sortedIndices = sorted.Select(i => (object) i.Value).ToList();

            // end step
            return new Dictionary<string, List<object>>
            {
                {"sortedPoints", sortedPoints},
                {"sortedIndices", sortedIndices}
            };
        }


        /// <summary>
        /// Sort planes by Z value with indices; version 2.
        /// </summary>
        /// <param name="planes">Plane list</param>
        /// <returns></returns>
        [MultiReturn(new[] { "sortedPlanes", "sortedIndices" })]
        public static Dictionary<string, List<object>> sortPlanesByZ_2(List<Plane> planes)
        {
            var sorted = planes
                .Select((coord, i) => new KeyValuePair<Point, int>(coord.Origin, i))
                .OrderBy(coord => coord.Key.Z)
                .ToList();

            List<object> sortedPlanes = sorted.Select(coord => (object)coord.Key).ToList();
            List<object> sortedIndices = sorted.Select(i => (object)i.Value).ToList();

            // end step
            return new Dictionary<string, List<object>>
            {
                {"sortedPlanes", sortedPlanes},
                {"sortedIndices", sortedIndices}
            };
        }









    }
}
