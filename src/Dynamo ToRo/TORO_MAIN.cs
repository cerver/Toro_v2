using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using ABB.Robotics;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.RapidDomain;
using ABB.Robotics.Controllers.MotionDomain;
//using ABBMath = ABB.Robotics.Math;
//using ABB.Robotics.RobotStudio;
//using ABB.Robotics.RobotStudio.Stations;
//using Adapters;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

using ABBMesh = Autodesk.DesignScript.Geometry.Mesh;


//using Dynamo_TORO.SimFunctions;


//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////

//authored by Nick Cote, 2015
//authored by Nick Cote, 2015

//implemented CreateRapid based on logic from Dynamo_ABB from Autodesk, Inc. Waltham at Virginia Tech Robotics Summit, 2015
//with contributions from Matt Jezyk and Mike Dewberry

//implemented PlaneToQuaternian based on logic from the Design Robotics Group at Harvard Gsd
//with contributions from Sola Grantham, Anthony Kane, Nathan King, Jonathan Grinham, and others. 

//Edits and UI nodes by Robert Cervellione



//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////



namespace Dynamo_TORO
{


    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    /*
    /// <summary>
    /// simulate robot.
    /// </summary>
    public class RobotSimulation
    {
        [IsVisibleInDynamoLibrary(false)]
        public RobotSimulation()
        { }
        
        public static double[] checkTarget(string station, Plane target)
        {
            VirtualStation vs = new VirtualStation(station);
           var rsTarget = vs.CreateTarget(target);

            return vs.CalulateIK(target).Result;
            

        }

        public static bool CheckTargetReach(string station, Plane target)
        {
            VirtualStation vs = new VirtualStation(station);
            return vs.isTargetReachable(target);
        }


    }
    */
    /// <summary>
    /// Create datatype.
    /// </summary>
    public class DataTypes
    {
        private static object reader;


        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        /*
                public static bool CreateTool(string[] controllerData, string ToolName, ABBMesh ToolGeom, Plane AttachmentPl, Plane ToolTip)
                {


         
                    Station station = Project.ActiveProject as Station;
                    RsTask task = station.ActiveTask;
            
            // create sample geometry
                    Part part = new Part();
                    MeshBody tool = ToroHelpFunc.MeshToMeshBody(ToolGeom);
                    part.Bodies.Add(tool.Body);
                    part.Name = ToolName + "_Mesh";

                    // create new tool
                    MechanismBuilder b = new MechanismBuilder(MechanismType.Tool);
                    b.Name = ToolName;
                    b.ModelName = ToolName+"_Model";

                    string link = "Geometry";
                    b.AddLink(link, part);
                    b.BaseLink = link;

                ABBMath.Matrix4 offset = ABBMath.Matrix4.Identity;
                    offset.TranslateLocal(ToolTip.Origin.X, ToolTip.Origin.Y, ToolTip.Origin.Z);
                    // ...
                    List<double> atQuat = Utilities.QuatListAtPlane(AttachmentPl);
                    b.SetLoadData(1.0,new ABBMath.Vector3(AttachmentPl.Origin.X, AttachmentPl.Origin.Y, AttachmentPl.Origin.Z),new ABBMath.Quaternion(atQuat[0], atQuat[1], atQuat[2],atQuat[3]),new ABBMath.Vector3(AttachmentPl.Normal.X, AttachmentPl.Normal.Y, AttachmentPl.Normal.Z)  );
                    // assign gravity, offset, ...
                    b.AddToolData(ToolName+"_Data", link, offset);

                    Mechanism mechTool = b.CompileMechanism();
                    mechTool.Name = ToolName+"_Mech";

                    station.GraphicComponents.Add(mechTool);

                    // and attach it to the flange:
                    // of course you should check if flange does exist and so on
                    task.Mechanism.GetFlanges()[0].Attach(mechTool, true);

                    return true;
                }
*/
        /// <summary>
        /// Create Tool
        /// </summary>
        /// <param name="attacmentPlane">Coordinate</param>
        /// <param name="toolTip">Coordinate</param>
        /// <param name="toolGeom">Coordinate</param>
      
        /// <returns></returns>
        public static ToolData EoaToolData(Plane attacmentPlane, Plane toolTip, Geometry toolGeom)
        {
            ToolData tool = new ToolData();
            tool.Tframe = RobotUtils.PlaneToPose(toolTip);

            return tool;
        }

        /// <summary>
        /// Create robot target from coordinate and quaternion values.
        /// </summary>
        /// <param name="ptX">Coordinate</param>
        /// <param name="ptY">Coordinate</param>
        /// <param name="ptZ">Coordinate</param>
        /// <param name="q1">Quaternion</param>
        /// <param name="q2">Quaternion</param>
        /// <param name="q3">Quaternion</param>
        /// <param name="q4">Quaternion</param>
        /// <returns></returns>
        public static RobTarget RobTargetAtVals(double ptX = 0, double ptY = 0, double ptZ = 0, double q1 = 1, double q2 = 0, double q3 = 0, double q4 = 0)
        {
            var target = new RobTarget();
            {
                target.FillFromString2(
                    string.Format(
                        "[[{0},{1},{2}],[{3},{4},{5},{6}],[0,0,0,0],[9.999999999E09,9.999999999E09,9.999999999E09,9.999999999E09,9.999999999E09,9.999999999E09]];",
                        ptX, ptY, ptZ, q1, q2, q3, q4));
            }
            return target;
        }

        /// <summary>
        /// Create robot target from point and quaternion values.
        /// </summary>
        /// <param name="point">Point</param>
        /// <param name="q1">Quaternion</param>
        /// <param name="q2">Quaternion</param>
        /// <param name="q3">Quaternion</param>
        /// <param name="q4">Quaternion</param>
        /// <returns></returns>
        public static RobTarget RobTargetAtPoint([DefaultArgumentAttribute("Point.ByCoordinates(0,0,0)")] Point point, double q1 = 1, double q2 = 0, double q3 = 0, double q4 = 0)
        {
            var target = new RobTarget();
            if (point != null)
            {
                target.FillFromString2( $"[[{point.X},{point.Y},{point.Z}],[{q1},{q2},{q3},{q4}],[0,0,0,0],[9.999999999E09,9.999999999E09,9.999999999E09,9.999999999E09,9.999999999E09,9.999999999E09]];" );
            }
            return target;
        }

        /// <summary>
        /// Create robot target from plane.
        /// </summary>
        /// <param name="plane">Plane</param>
        /// <returns></returns>
        public static RobTarget RobTargetAtPlane([DefaultArgumentAttribute("Plane.ByOriginNormal(Point.ByCoordinates(0,0,0),Vector.ByCoordinates(0,0,1))")] Plane plane)
        {
            var target = new RobTarget();
            if (plane != null)
            {
                List<double> quatDoubles = RobotUtils.PlaneToQuaternian(plane);
                target.FillFromString2($"[[{plane.Origin.X},{plane.Origin.Y},{plane.Origin.Z}],[{quatDoubles[0]},{quatDoubles[1]},{quatDoubles[2]},{quatDoubles[3]}],[0,0,0,0],[9.999999999E09,9.999999999E09,9.999999999E09,9.999999999E09,9.999999999E09,9.999999999E09]];");
            }
            return target;
        }

        /// <summary>
        /// Create joint target from rotational values per axis.
        /// </summary>
        /// <param name="j1">Degrees</param>
        /// <param name="j2">Degrees</param>
        /// <param name="j3">Degrees</param>
        /// <param name="j4">Degrees</param>
        /// <param name="j5">Degrees</param>
        /// <param name="j6">Degrees</param>
        /// <returns></returns>
        public static JointTarget JointTargetAtVals(double j1 = 0, double j2 = 0, double j3 = 0, double j4 = 0, double j5 = 0, double j6 = 0)
        {
            var target = new JointTarget();
            target.FillFromString2(
                string.Format("[[{0},{1},{2},{3},{4},{5}],[9.999999999E09,9.999999999E09,9.999999999E09,9.999999999E09,9.999999999E09,9.999999999E09]];",
                j1, j2, j3, j4, j5, j6));
            return target;
        }
    
        /// <summary>
        /// Define speeddata.
        /// </summary>
        /// <param name="varName">Name of speeddata variable</param>
        /// <param name="v_tcp">Speed at tool center point in mm/s</param>
        /// <param name="v_ori">Reorientation speed of the TCP in deg</param>
        /// <param name="v_leax">Linear speed of external axes in mm/s</param>
        /// <param name="v_reax">Rotational speed of external axes in deg</param>
        /// <returns></returns>
        public static string Speeddata(string varName = "speed", double v_tcp = 250, double v_ori = 500, double v_leax = 5000, double v_reax = 1000)
        {
            string speed = string.Format("VAR speeddata {0}:=[{1},{2},{3},{4}];", varName, v_tcp, v_ori, v_leax, v_reax);
            return speed;
        }

        /// <summary>
        /// Define zonedata.
        /// </summary>
        /// <param name="varName">Name of zonedata variable</param>
        /// <param name="pfine">T: stop-point | F: fly-by-point</param>
        /// <param name="pzone_tcp">Radius for TCP</param>
        /// <param name="pzone_ori">Radius for reorientation</param>
        /// <param name="pzone_eax">Radius for external axes</param>
        /// <param name="zone_ori">Radius for tool reorientation in deg</param>
        /// <param name="zone_leax">Radius for linear external axes in mm</param>
        /// <param name="zone_reax">Radius for rotating external axes in deg</param>
        /// <returns></returns>
        public static string Zonedata(string varName = "zone", bool pfine = false, double pzone_tcp = 10, double pzone_ori = 15, double pzone_eax = 15, double zone_ori = 1.5, double zone_leax = 15, double zone_reax = 1.5)
        {
            string zone = string.Format("VAR zonedata {0}:=[{1},{2},{3},{4},{5},{6},{7}];", varName, pfine, pzone_tcp, pzone_ori, pzone_eax, zone_ori, zone_leax, zone_reax);
            return zone;
        }

        /// <summary>
        /// Define loaddata.
        /// </summary>
        /// <param name="varName">Name of loaddata variable</param>
        /// <param name="load">Load in kg</param>
        /// <param name="cog_x">Center of gravity coordinate</param>
        /// <param name="cog_y">Center of gravity coordinate</param>
        /// <param name="cog_z">Center of gravity coordinate</param>
        /// <param name="aom_q1">Axes of moment quaternion</param>
        /// <param name="aom_q2">Axes of moment quaternion</param>
        /// <param name="aom_q3">Axes of moment quaternion</param>
        /// <param name="aom_q4">Axes of moment quaternion</param>
        /// <param name="ix">Inertia in kgm^2</param>
        /// <param name="iy">Inertia in kgm^2</param>
        /// <param name="iz">Inertia in kgm^2</param>
        /// <returns></returns>
        public static string Loaddata(string varName = "load", double load = 1, double cog_x = 0, double cog_y = 0, double cog_z = 0.001, double aom_q1 = 1, double aom_q2 = 0, double aom_q3 = 0, double aom_q4 = 0, double ix = 0, double iy = 0, double iz = 0)
        {
            string loadData = string.Format("PERS loaddata {0}:=[{1},[{2},{3},{4}],[{5},{6},{7},{8}],{9},{10},{11}];", varName, load, cog_x, cog_y, cog_z, aom_q1, aom_q2, aom_q3, aom_q4, ix, iy, iz);
            return loadData;
        }

        /// <summary>
        /// Define confdata.
        /// </summary>
        /// <param name="varName">Name of confdata variable</param>
        /// <param name="cf1">Current quadrant or meter interval of axis 1</param>
        /// <param name="cf4">Current quadrant or meter interval of axis 4</param>
        /// <param name="cf6">Current quadrant or meter interval of axis 6</param>
        /// <param name="cfx">Current quadrant or meter interval of axis 2 | X</param>
        /// <returns></returns>
        public static string Confdata(string varName = "conf", double cf1 = 0, double cf4 = 0, double cf6 = 0, double cfx = 0)
        {
            string conf = string.Format("PERS confdata {0}:=[{1},{2},{3},{4}];", varName, cf1, cf4, cf6, cfx);
            return conf;
        }




        /*
        /// <summary>
        /// Define motion set data.
        /// </summary>
        /// <param name="varName">Name of motsetdata variable</param>
        /// <param name="vel_oride">Velocity as a percentage of programmed velocity.</param>
        /// <param name="vel_max">Maximum velocity in mm/s.</param>
        /// <param name="acc_acc">Acceleration and deceleration as a percentage of the normal values.</param>
        /// <param name="acc_ramp">The rate by which acceleration and deceleration increases as a percentage of the normal values. </param>
        /// <param name="sing_wrist">The orientation of the tool is allowed to deviate somewhat in order to prevent wrist singularity. </param>
        /// <param name="sing_arm">The orientation of the tool is allowed to deviate somewhat in order to prevent arm singularity (not implemented).</param>
        /// <param name="sing_base">The orientation of the tool is not allowed to deviate. </param>
        /// <param name="conf_jsup">Supervision of joint configuration is active during joint movement. </param>
        /// <param name="conf_lsup">Supervision of joint configuration is active during linear and circular movement. </param>
        /// <param name="conf_ax1">Maximum permitted deviation in degrees for axis 1 (not used in this version). </param>
        /// <param name="conf_ax4">Maximum permitted deviation in degrees for axis 4 (not used in this version). </param>
        /// <param name="conf_ax6">Maximum permitted deviation in degrees for axis 6 (not used in this version). </param>
        /// <param name="pathresol">Current override in percentage of the configured path resolution.</param>
        /// <param name="motionsup">Mirror RAPID status (TRUE = On and FALSE = Off) of motion supervision function.</param>
        /// <param name="tunevalue">Current RAPID override as a percentage of the configured tunevalue for the motion supervision function.</param>
        /// <param name="acclim">Limitation of tool acceleration along the path. (TRUE = limitation and FALSE = no limitation).</param>
        /// <param name="accmax">TCP acceleration limitation in m/s2. If acclim is FALSE, the value is always set to -1.</param>
        /// <param name="decellim">Limitation of tool deceleration along the path. (TRUE = limitation and FALSE = no limitation).</param>
        /// <param name="decelmax">TCP deceleration limitation in m/s2. If decellim is FALSE, the value is always set to -1.</param>
        /// <param name="cirpathreori">Tool reorientation during circle path: 0 = Interpolation in path frame; 1 = Interpolation in object frame; 2 = Programmed tool orientation in CirPoint</param>
        /// <param name="worldacclim">Limitation of acceleration in world coordinate system. (TRUE = limitation and FALSE = no limitation).</param>
        /// <param name="worldaccmax">Limitation of acceleration in world coordinate system in m/s2. If worldacclim is FALSE, the value is always set to -1.</param>
        /// <param name="evtbufferact">Event buffer active or not active. (TRUE = event buffer active and FALSE = event buffer not active).</param>
        /// <returns></returns>
        public static string Motsetdata(string varName, double vel_oride, double vel_max, double acc_acc, double acc_ramp, bool sing_wrist, bool sing_arm, bool sing_base, bool conf_jsup, bool conf_lsup, double conf_ax1, double conf_ax4, double conf_ax6, double pathresol, bool motionsup, double tunevalue, bool acclim, double accmax, bool decellim, double decelmax, int cirpathreori, bool worldacclim, double worldaccmax, bool evtbufferact)
        {
            string motset = string.Format("\n\tVAR motsetdata {0}:=" + "[{1},{2}],\n" +
                                                                        "[{3},{4}],\n" +
                                                                        "[{5},{6},{7}],\n" +
                                                                        "[{8},{9},{10},{11},{12},{13}],\n" +
                                                                        "{14},\n" +
                                                                        "{15},\n" +
                                                                        "{16},\n" +
                                                                        "{17},\n" +
                                                                        "{18},\n" +
                                                                        "{19},\n" +
                                                                        "{20},\n" +
                                                                        "{21},\n" +
                                                                        "{22},\n" +
                                                                        "{23},\n" +
                                                                        "{24},\n",
                                                                        varName,
                                                                        vel_oride, vel_max,
                                                                        acc_acc, acc_ramp,
                                                                        sing_wrist, sing_arm, sing_base,
                                                                        conf_jsup, conf_lsup, conf_ax1, conf_ax4, conf_ax6,
                                                                        pathresol,
                                                                        motionsup,
                                                                        tunevalue,
                                                                        acclim,
                                                                        accmax,
                                                                        decellim,
                                                                        decelmax,
                                                                        cirpathreori,
                                                                        worldacclim,
                                                                        worldaccmax,
                                                                        evtbufferact);
            return motset;
        }

        /// <summary>
        /// Define stoppointdata
        /// </summary>
        /// <param name="varName">Name of stoppointdata variable</param>
        /// <param name="progsynch">Sychronization with RAPID program execution</param>
        /// <param name="type">1 = inpos; 2 = stoptime; 3 = followtime</param>
        /// <param name="inpos_position">Position condition for TCP</param>
        /// <param name="inpos_speed">Speed condition for TCP</param>
        /// <param name="inpos_mintime">Minimum wait time</param>
        /// <param name="inpos_maxtime">Maximum wait time</param>
        /// <param name="stoptime">Time stopped</param>
        /// <param name="followtime">Follow time</param>
        /// <returns></returns>
        public static string Stoppointdata(string varName, bool progsynch, string type, int inpos_position, int inpos_speed, double inpos_mintime, double inpos_maxtime, double stoptime, double followtime)
        {
            string stop = string.Format("\n\tVAR stoppointdata {0}:= [{1},{2},[{3},{4},{5},{6}],{7},{8},'',0,0];", varName);
            return stop;
        }

    */





        /// <summary>
        /// Define shapedata and create work-zone instruction for box.
        /// </summary>
        /// <param name="varName">Name of shapedata variable</param>
        /// <param name="Inside_Outside">Define as volume "Inside" | "Outside"</param>
        /// <param name="lo_x">Low point coordinate</param>
        /// <param name="lo_y">Low point coordinate</param>
        /// <param name="lo_z">Low point coordinate</param>
        /// <param name="hi_x">High point coordinate</param>
        /// <param name="hi_y">High point coordinate</param>
        /// <param name="hi_z">High point coordinate</param>
        /// <returns></returns>
        [MultiReturn(new[] { "cnst", "inst" })]
        public static Dictionary<string, string> WZBoxAtVals(string varName, string Inside_Outside, double lo_x, double lo_y, double lo_z, double hi_x, double hi_y, double hi_z)
        {
            string cnst = string.Format("VAR shapedata {0};",
                                        varName);
            string inst = string.Format("WZBoxDef \\{0},{1},[{2},{3},{4}],[{5},{6},{7}];",
                                        Inside_Outside, varName, lo_x, lo_y, lo_z, hi_x, hi_y, hi_z);

            return new Dictionary<string, string>
            {
                {"cnst", cnst},
                {"inst", inst},
             };
        }

        /// <summary>
        /// Define shapedata and create work-zone instruction for box.
        /// </summary>
        /// <param name="varName">Name of shapedata variable</param>
        /// <param name="Inside_Outside">Define as volume "Inside" | "Outside"</param>
        /// <param name="box">Cuboidic work-zone</param>
        /// <returns></returns>
        [MultiReturn(new[] { "cnst", "inst" })]
        public static Dictionary<string, string> WZBoxAtGeometry(string varName, string Inside_Outside, Cuboid box)
        {
            double lo_x = box.Vertices[6].PointGeometry.X;
            double lo_y = box.Vertices[6].PointGeometry.Y;
            double lo_z = box.Vertices[6].PointGeometry.Z;
            double hi_x = box.Vertices[1].PointGeometry.X;
            double hi_y = box.Vertices[1].PointGeometry.Y;
            double hi_z = box.Vertices[1].PointGeometry.Z;
            string cnst = string.Format("VAR shapedata {0};",
                                        varName);
            string inst = string.Format("WZBoxDef \\{0},{1},[{2},{3},{4}],[{5},{6},{7}];",
                                        Inside_Outside, varName, lo_x, lo_y, lo_z, hi_x, hi_y, hi_z);

            return new Dictionary<string, string>
            {
                {"cnst", cnst},
                {"inst", inst},
             };
        }


        /// <summary>
        /// Define shapedata and create work-zone instruction for cylinder.
        /// </summary>
        /// <param name="varName">Name of shapedata variable</param>
        /// <param name="Inside_Outside">Define as volume "Inside" | "Outside"</param>
        /// <param name="center_x">Coordinate</param>
        /// <param name="center_y">Coordinate</param>
        /// <param name="center_z">Coordinate</param>
        /// <param name="radius">Radius</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        [MultiReturn(new[] { "cnst", "inst" })]
        public static Dictionary<string, string> WZCylAtVals(string varName, string Inside_Outside, double center_x, double center_y, double center_z, double radius, double height)
        {
            if (radius < 5)
            { radius = 5; }
            string cnst = string.Format("VAR wzstationary wz{0};" +
                                        "\tVAR shapedata {0};\n",
                                        varName);
            string inst = string.Format("WZCylDef \\{0},{1},[{2},{3},{4}],{5},{6};\n" +
                                        "\t\tWZLimSup \\Stat, wz{1}, {1};\n",
                                       Inside_Outside, varName, center_x, center_y, center_z, radius, height);

            return new Dictionary<string, string>
            {
                {"cnst", cnst},
                {"inst", inst},
             };
        }

        /// <summary>
        /// Define shapedata and create work-zone instruction for cylinder.
        /// </summary>
        /// <param name="varName">Name of shapedata variable</param>
        /// <param name="Inside_Outside">Define as volume "Inside" | "Outside"</param>
        /// <param name="cylinder">Cylindrical work-zone</param>
        /// <returns></returns>
        [MultiReturn(new[] { "cnst", "inst" })]
        public static Dictionary<string, string> WZCylAtGeometry(string varName, string Inside_Outside, Cylinder cylinder)
        {
            double center_x = cylinder.StartPoint.X;
            double center_y = cylinder.StartPoint.Y;
            double center_z = cylinder.StartPoint.Z;
            double radius = cylinder.Radius;
            double height = cylinder.Height;
            if (radius < 5)
            { radius = 5; }
            string cnst = string.Format("VAR wzstationary wz{0};" +
                                        "\tVAR shapedata {0};\n",
                                        varName);
            string inst = string.Format("WZCylDef \\{0},{1},[{2},{3},{4}],{5},{6};\n" +
                                        "\t\tWZLimSup \\Stat, wz{1}, {1};\n",
                                       Inside_Outside, varName, center_x, center_y, center_z, radius, height);

            return new Dictionary<string, string>
            {
                {"cnst", cnst},
                {"inst", inst},
             };
        }

        /// <summary>
        /// Define shapedata and create work-zone instruction for sphere.
        /// </summary>
        /// <param name="varName">Name of shapedata variable</param>
        /// <param name="Inside_Outside">Define as volume "Inside" | "Outside"</param>
        /// <param name="center_x">Coordinate</param>
        /// <param name="center_y">Coordinate</param>
        /// <param name="center_z">Coordinate</param>
        /// <param name="radius">Radius</param>
        /// <returns></returns>
        [MultiReturn(new[] { "cnst", "inst" })]
        public static Dictionary<string, string> WZSphAtVals(string varName, string Inside_Outside, double center_x, double center_y, double center_z, double radius)
        {
            string cnst = string.Format("VAR shapedata {0};",
                                        varName);
            string inst = string.Format("WZBoxDef \\{0},{1},[{2},{3},{4}],{5};",
                                        Inside_Outside, varName, center_x, center_y, center_z, radius);

            return new Dictionary<string, string>
            {
                {"cnst", cnst},
                {"inst", inst},
             };
        }

        /// <summary>
        /// Define shapedata and create work-zone instruction for sphere.
        /// </summary>
        /// <param name="varName">Name of shapedata variable</param>
        /// <param name="Inside_Outside">Define as volume "Inside" | "Outside"</param>
        /// <param name="sphere">Spherical work-zone</param>
        /// <returns></returns>
        [MultiReturn(new[] { "cnst", "inst" })]
        public static Dictionary<string, string> WZSphAtGeometry(string varName, string Inside_Outside, Sphere sphere)
        {
            double center_x = sphere.CenterPoint.X;
            double center_y = sphere.CenterPoint.Y;
            double center_z = sphere.CenterPoint.Z;
            double radius = sphere.Radius;
            string cnst = string.Format("VAR shapedata {0};",
                                        varName);
            string inst = string.Format("WZBoxDef \\{0},{1},[{2},{3},{4}],{5};",
                                        Inside_Outside, varName, center_x, center_y, center_z, radius);

            return new Dictionary<string, string>
            {
                {"cnst", cnst},
                {"inst", inst},
             };
        }


        /// <summary>
        /// Define joint-targets for joint limits.
        /// </summary>
        /// <param name="varName">Name of variables</param>
        /// <param name="Inside_Outside">Define as volume "Inside" | "Outside"</param>
        /// <param name="loJointVal">JointTarget</param>
        /// <param name="hiJointVal">JointTarget</param>
        /// <returns></returns>
        [MultiReturn(new[] { "cnst", "inst" })]
        public static Dictionary<string, string> WZLimJointDef(string varName, string Inside_Outside, JointTarget loJointVal, JointTarget hiJointVal)
        {
            string cnst = string.Format("VAR wzstationary wl{0};" +
                                        "VAR shapedata js{0};" +
                                        "CONST jointtarget lo{0}:={1};" +
                                        "CONST jointtarget hi{0}:={2};",
                                        varName, loJointVal, hiJointVal);
            string inst = string.Format("WZLimJointDef \\{0},js{1},lo{1},hi{1};" +
                                        "WzLimSup \\Stat, wl{1},js{1};",
                                        Inside_Outside, varName);

            return new Dictionary<string, string>
            {
                {"cnst", cnst},
                {"inst", inst},
             };
        }

        /// <summary>
        /// Define tooldata from coordinate, quaternion, and load values.
        /// </summary>
        /// <param name="x">Coordinate</param>
        /// <param name="y">Coordinate</param>
        /// <param name="z">Coordinate</param>
        /// <param name="q1">Quaternion</param>
        /// <param name="q2">Quaternion</param>
        /// <param name="q3">Quaternion</param>
        /// <param name="q4">Quaternion</param>
        /// <param name="load">Load in kg</param>
        /// <param name="name">Name of tooldata variable</param>
        /// <returns></returns>
        public static List<string> ToolAtVals(double x = 0, double y = 0, double z = 0.001, double q1 = 1, double q2 = 0, double q3 = 0, double q4 = 0, double load = 0.001, string name = "t")
        {
            List<string> toolData = new List<string>();
            string tool = string.Format("PERS tooldata {0}:=[TRUE,[[{1},{2},{3}],[{4},{5},{6},{7}]],[{8},[0,0,0.001],[1,0,0,0],0,0,0]];", name, x, y, z, q1, q2, q3, q4, load);
            toolData.Add(tool);
            return toolData;
        }

        /// <summary>
        /// Define tooldata from point, quaternion, and load values.
        /// </summary>
        /// <param name="pt">Point</param>
        /// <param name="q1">Quaternion</param>
        /// <param name="q2">Quaternion</param>
        /// <param name="q3">Quaternion</param>
        /// <param name="q4">Quaternion</param>
        /// <param name="load">Load in kg</param>
        /// <param name="name">Name of tooldata variable</param>
        /// <returns></returns>
        public static List<string> ToolAtPoint([DefaultArgumentAttribute("Point.ByCoordinates(0,0,0.001)")] Point pt, double q1 = 1, double q2 = 0, double q3 = 0, double q4 = 0, double load = 0.001, string name = "t")
        {
            List<string> toolData = new List<string>();
            string tool = string.Format("PERS tooldata {0}:=[TRUE,[[{1},{2},{3}],[{4},{5},{6},{7}]],[{8},[0,0,0.001],[1,0,0,0],0,0,0]];", name, pt.X, pt.Y, pt.Z, q1, q2, q3, q4, load);
            toolData.Add(tool);
            return toolData;
        }

        /// <summary>
        /// Define tooldata from plane and load value.
        /// </summary>
        /// <param name="pl">Plane</param>
        /// <param name="load">Load in kg</param>
        /// <param name="name">Name of tooldata variable</param>
        /// <returns></returns>
        public static List<string> ToolAtPlane([DefaultArgumentAttribute("Plane.ByOriginNormal(Point.ByCoordinates(0,0,0.001),Vector.ByCoordinates(0,0,1))")] Plane pl, double load = 0.001, string name = "t")
        {
            List<string> toolData = new List<string>();
            List<double> quats = RobotUtils.PlaneToQuaternian(pl);
            string tool = string.Format("PERS tooldata {0}:=[TRUE,[[{1},{2},{3}],[{4},{5},{6},{7}]],[{8},[0,0,0.001],[1,0,0,0],0,0,0]];", name, pl.Origin.X, pl.Origin.Y, pl.Origin.Z, quats[0], quats[1], quats[2], quats[3], load);
            toolData.Add(tool);
            return toolData;
        }

        /// <summary>
        /// Define wobjdata from coordinate and quaternion values.
        /// </summary>
        /// <param name="x">Coordinate</param>
        /// <param name="y">Coordinate</param>
        /// <param name="z">Coordinate</param>
        /// <param name="q1">Quaternion</param>
        /// <param name="q2">Quaternion</param>
        /// <param name="q3">Quaternion</param>
        /// <param name="q4">Quaternion</param>
        /// <param name="name">Name of wobjdata variable</param>
        /// <returns></returns>
        public static List<string> WobjAtVals(double x = 0, double y = 0, double z = 0, double q1 = 1, double q2 = 0, double q3 = 0, double q4 = 0, string name = "w")
        {
            List<string> wobjData = new List<string>();
            string wobj = string.Format("TASK PERS wobjdata {0}:=[FALSE,TRUE," + @"""""" + ",[[{1},{2},{3}],[{4},{5},{6},{7}]],[[0,0,0],[1,0,0,0]]];", name, x, y, z, q1, q2, q3, q4);
            wobjData.Add(wobj);
            return wobjData;
        }

        /// <summary>
        /// Define wobjdata from point and quaternion values.
        /// </summary>
        /// <param name="pt">Point</param>
        /// <param name="q1">Quaternion</param>
        /// <param name="q2">Quaternion</param>
        /// <param name="q3">Quaternion</param>
        /// <param name="q4">Quaternion</param>
        /// <param name="name">Name of wobjdata variable</param>
        /// <returns></returns>
        public static List<string> WobjAtPoint([DefaultArgumentAttribute("Point.ByCoordinates(0,0,0)")] Point pt, double q1 = 1, double q2 = 0, double q3 = 0, double q4 = 0, string name = "w")
        {
            List<string> wobjData = new List<string>();
            string wobj = string.Format("TASK PERS wobjdata {0}:=[FALSE,TRUE," + @"""""" + ",[[{1},{2},{3}],[{4},{5},{6},{7}]],[[0,0,0],[1,0,0,0]]];", name, pt.X, pt.Y, pt.Z, q1, q2, q3, q4);
            wobjData.Add(wobj);
            return wobjData;
        }

        /// <summary>
        /// Define wobjdata from plane.
        /// </summary>
        /// <param name="pl">Plane</param>
        /// <param name="name">Name of wobjdata variable</param>
        /// <returns></returns>
        public static List<string> WobjAtPlane([DefaultArgumentAttribute("Plane.ByOriginNormal(Point.ByCoordinates(0,0,0),Vector.ByCoordinates(0,0,1))")] Plane pl, string name = "w")
        {
            List<string> wobjData = new List<string>();
            List<double> quats = RobotUtils.PlaneToQuaternian(pl);
            string wobj = string.Format("TASK PERS wobjdata {0}:=[FALSE,TRUE," + @"""""" + ",[[{1},{2},{3}],[{4},{5},{6},{7}]],[[0,0,0],[1,0,0,0]]];", name, pl.Origin.X, pl.Origin.Y, pl.Origin.Z, quats[0], quats[1], quats[2], quats[3]);
            wobjData.Add(wobj);
            return wobjData;
        }

    }

    /// <summary>
    /// Create instructions.
    /// </summary>
    public class Instructions
    {
        /// <summary>
        /// Rapid Instructions
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public Instructions()
        { }


        /// <summary>
        /// Create a linear movement instruction.
        /// </summary>
        /// <param name="targets">Robot target</param>
        /// <param name="speed">Speed data (rounds to default in RobotStudio)</param>
        /// <param name="zone">Zone data (rounds to default in RobotStudio)</param>
        /// <param name="setName">Unique name for this instruction</param>
        /// <param name="toolName">Active tool</param>
        /// <param name="wobjName">Active work-object</param>
        /// <param name="defaultSpeeds">If true it will round the speed values to robot studio default values</param>
        /// <param name="DoNames"></param>
        /// /// <param name="DoVals"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "cnstList", "instList" })]
        public static Dictionary<string, List<string>> MoveLDO(List<RobTarget> targets, [DefaultArgumentAttribute("{100}")] List<int> speed, [DefaultArgumentAttribute("{DO10_1}")] List<string> DoNames, [DefaultArgumentAttribute("{0}")] List<int> DoVals, [DefaultArgumentAttribute("{0}")] List<int> zone, string setName = "set0", string toolName = "tool0", string wobjName = "wobj0", bool defaultSpeeds = false)
        {
            // setup
            List<string> cnstList = new List<string>();
            List<string> instList = new List<string>();

            UtilFuncs.MoveCommand(UtilFuncs.MoveType.MoveLDO, targets, speed, zone, DoNames, DoVals, setName, toolName, wobjName, defaultSpeeds, out cnstList, out instList);


            //end step
            return new Dictionary<string, List<string>>
            {
                {"cnstList", cnstList},
                {"instList", instList},
                };
        }



        /// <summary>
        /// Create a linear movement instruction.
        /// </summary>
        /// <param name="targets">Robot target</param>
        /// <param name="speed">Speed data (rounds to default in RobotStudio)</param>
        /// <param name="zone">Zone data (rounds to default in RobotStudio)</param>
        /// <param name="setName">Unique name for this instruction</param>
        /// <param name="toolName">Active tool</param>
        /// <param name="wobjName">Active work-object</param>
        /// <param name="defautSpeeds">If true it will round the speed values to robot studio default values</param>
        /// <returns></returns>
        [MultiReturn(new[] { "cnstList", "instList" })]
        public static Dictionary<string, List<string>> MoveL(List<RobTarget> targets, [DefaultArgumentAttribute("{100}")] List<int> speed, [DefaultArgumentAttribute("{0}")] List<int> zone, string setName = "set0", string toolName = "tool0", string wobjName = "wobj0", bool defaultSpeeds = false)
        {
            // setup
            List<string> cnstList = new List<string>();
            List<string> instList = new List<string>();

            UtilFuncs.MoveCommand(UtilFuncs.MoveType.MoveL, targets, speed, zone, null, null, setName, toolName, wobjName, defaultSpeeds, out cnstList, out instList);
           

            //end step
            return new Dictionary<string, List<string>>
            {
                {"cnstList", cnstList},
                {"instList", instList},
                };
        }

        /// <summary>
        /// Create a joint movement instruction.
        /// </summary>
        /// <param name="targets">Robot target</param>
        /// <param name="speed">Speed data (rounds to default in RobotStudio)</param>
        /// <param name="zone">Zone data (rounds to default in RobotStudio)</param>
        /// <param name="setName">Unique name of this instruction</param>
        /// <param name="toolName">Active tool</param>
        /// <param name="wobjName">Active work-object</param>
        /// <returns></returns>
        [MultiReturn(new[] { "cnstList", "instList" })]
        public static Dictionary<string, List<string>> MoveJ(List<RobTarget> targets, [DefaultArgumentAttribute("{100}")] List<int> speed, [DefaultArgumentAttribute("{0}")] List<int> zone, string setName = "set0", string toolName = "tool0", string wobjName = "wobj0", bool defaultSpeeds = true)
        {
            // setup
            List<string> cnstList = new List<string>();
            List<string> instList = new List<string>();
   
            UtilFuncs.MoveCommand(UtilFuncs.MoveType.MoveJ, targets, speed, zone, null, null, setName, toolName, wobjName, defaultSpeeds, out cnstList, out instList);


            // end step
            return new Dictionary<string, List<string>>
            {
                {"cnstList", cnstList},
                {"instList", instList},
                };
        }
        /// <summary>
        /// Create a abs joint movement instruction.
        /// </summary>
        /// <param name="targets">Robot target</param>
        /// <param name="speed">Speed data (rounds to default in RobotStudio)</param>
        /// <param name="zone">Zone data (rounds to default in RobotStudio)</param>
        /// <param name="setName">Unique name of this instruction</param>
        /// <param name="toolName">Active tool</param>
        /// <param name="wobjName">Active work-object</param>
        /// <returns></returns>
        [MultiReturn(new[] { "cnstList", "instList" })]
        public static Dictionary<string, List<string>> MoveAbsJ(List<RobTarget> targets, [DefaultArgumentAttribute("{100}")] List<int> speed, [DefaultArgumentAttribute("{0}")] List<int> zone, string setName = "set0", string toolName = "tool0", string wobjName = "wobj0", bool defaultSpeeds = true)
        {
            // setup
            List<string> cnstList = new List<string>();
            List<string> instList = new List<string>();

            UtilFuncs.MoveCommand(UtilFuncs.MoveType.MoveAbsJ, targets, speed, zone, null, null, setName, toolName, wobjName, defaultSpeeds, out cnstList, out instList);


            // end step
            return new Dictionary<string, List<string>>
            {
                {"cnstList", cnstList},
                {"instList", instList},
                };
        }
    
        /// <summary>
        /// Create a circular movement instruction.
        /// </summary>
        /// <param name="cirTarget">Robot target (through point)</param>
        /// <param name="toTarget">Robot target (destination)</param>
        /// <param name="speed">Speed data (rounds to default in RobotStudio)</param>
        /// <param name="zone">Zone data (rounds to default in RobotStudio)</param>
        /// <param name="setName">Unique name for this instruction</param>
        /// <param name="toolName">Active tool</param>
        /// <param name="wobjName">Active work-object</param>
        /// <returns></returns>
        [MultiReturn(new[] { "cnstList", "instList" })]
        public static Dictionary<string, List<string>> MoveC(List<RobTarget> cirTarget, List<RobTarget> toTarget, [DefaultArgumentAttribute("{100}")] List<int> speed, [DefaultArgumentAttribute("{0}")] List<int> zone, string setName = "set0", string toolName = "tool0", string wobjName = "wobj0")
        {
            // setup
            List<string> cnstList = new List<string>();
            List<string> instList = new List<string>();
            int cnt;

            // target instructions
            cnt = 0;

            string zn;
            int spd;
            foreach (var target in toTarget)
            {
                if (cnt < toTarget.Count)
                {
                    if (cnt == speed.Count) { speed.Add(speed[0]); }
                    if (cnt == zone.Count) { zone.Add(zone[0]); }
                }
                    spd = RobotUtils.closestSpeed(speed[cnt]);
                    zn = RobotUtils.closestZone(zone[cnt]);
  

                cnstList.Add( $"CONST robtarget cir{setName}{cnt}:={cirTarget[cnt]}; CONST robtarget to{setName}{cnt}:={target};") ;
                instList.Add($"MoveC cir{setName}{cnt}, to{setName}{cnt}, v{spd},{zn},{toolName}\\WObj:={wobjName};");

                cnt++;
            }

            // end step
            return new Dictionary<string, List<string>>
            {
                {"cnstList", cnstList},
                {"instList", instList},
                };
        }



        /// <summary>
        /// Create custom instruction from string.
        /// </summary>
        /// <param name="instructions"></param>
        /// <returns></returns>
        public static List<string> customInstruction([DefaultArgumentAttribute("{\"WaitTime 3\"}")] List<string> instructions)
        {
            List<string> instList = new List<string>();
            foreach (string inst in instructions)
            {
                instList.Add(string.Format("{0};", inst));
            }
            return instList;
        }

        /// <summary>
        /// Insert instructions into list at specified index.
        /// </summary>
        /// <param name="instList">Initial list of instructions</param>
        /// <param name="instructions">List of instructions to insert</param>
        /// <param name="index">List of indices at which to insert</param>
        /// <returns></returns>
        public static List<string> insertInstAtIndex(List<string> instList, List<string> instructions, List<int> index)
        {
            int cnt = 0;
            foreach (var dex in index)
            {
                if (cnt == instructions.Count) { instructions.Add(instructions[0]); }
                instList.Insert(dex + cnt, string.Format("{0};", instructions[cnt]));
                cnt++;
            }
            return instList;
        }

    }

    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// Write program to file.
    /// </summary>
    public class Write
    {
        [IsVisibleInDynamoLibrary(false)]
        public Write()
        { }

        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Merge and write data to a destination.
        /// </summary>
        /// <param name="filePath">"C:\...\myPath.prg"</param>
        /// <param name="cnstList">List of constants</param>
        /// <param name="instList">List of instructions</param>
        /// <param name="toolList">List of tooldata</param>
        /// <param name="wobjList">List of work-object data</param>
        /// <returns></returns>
        [MultiReturn(new[] { "filePath", "robotCode" })]
        public static Dictionary<string, string> createRapidCode(string filePath, List<string> cnstList, List<string> instList, List<string> toolList, List<string> wobjList)
        {
            // setup
            var cnstBuilder = new StringBuilder();
            var instBuilder = new StringBuilder();
            var toolBuilder = new StringBuilder();
            var wobjBuilder = new StringBuilder();
            foreach (string cnst in cnstList)
            {
                string cnst2 = "\n" + cnst;
                if (!cnst2.EndsWith(";")) { cnst2 = cnst2 + ";"; }
                cnstBuilder.Append(cnst2);
            }
            foreach (string inst in instList)
            {
                string inst2 ="\n"+ inst;
                if (!inst2.EndsWith(";")) { inst2 = inst2 + ";"; }
                instBuilder.Append(inst2);
            }
            foreach (string tool in toolList)
            {
                string tool2 = tool;
                if (!tool2.EndsWith(";")) { tool2 = tool2 + ";"; }
                toolBuilder.Append(tool2);
            }
            foreach (string wobj in wobjList)
            {
                string wobj2 = wobj;
                if (!wobj2.EndsWith(";")) { wobj2 = wobj2 + ";"; }
                wobjBuilder.Append(wobj2);
            }

            // create rapid
            string r = "";
            using (var tw = new StreamWriter(filePath, false))
            {
                string content = @"MODULE MainModule
    !program Data : Created by Dyanmo TORO
    {0}
    {1}

    !Target Data
    {2}

    !Routine
    PROC main()
        ConfL\Off;
        SingArea\Wrist;
        rStart;
        Stop;
    ENDPROC
                    
    PROC rStart()
    !instructions
    {3}
    RETURN;
    ENDPROC
    ENDMODULE";

                r = string.Format(content, toolBuilder.ToString(), wobjBuilder.ToString(), cnstBuilder.ToString(), instBuilder.ToString());

                tw.Write(r);
                tw.Flush();
                tw.Close();
            }

            // end step
            return new Dictionary<string, string>
            {
                {"filePath", filePath},
                {"robotCode", r}
            };

        }



        /// <summary>
        /// Merge and write data to a destination.
        /// </summary>
        /// <param name="filePath">"C:\...\myPath.prg"</param>
        /// <param name="cnstList">List of constants</param>
        /// <param name="instList">List of instructions</param>
        /// <param name="toolList">List of tooldata</param>
        /// <param name="wobjList">List of work-object data</param>
        /// <returns></returns>
        [MultiReturn(new[] { "filePath", "robotCode" })]
        public static Dictionary<string, string> createRapid0(string filePath, List<string> cnstList, List<string> instList, List<string> toolList, List<string> wobjList)
        {
            // setup
            var cnstBuilder = new StringBuilder();
            var instBuilder = new StringBuilder();
            var toolBuilder = new StringBuilder();
            var wobjBuilder = new StringBuilder();
            foreach (string cnst in cnstList)
            {
                string cnst2 = "\n\t" + cnst;
                if (!cnst2.EndsWith(";")) { cnst2 = cnst2 + ";"; }
                cnstBuilder.Append(cnst2);
            }
            foreach (string inst in instList)
            {
                string inst2 = "\n\t\t" + inst;
                if (!inst2.EndsWith(";")) { inst2 = inst2 + ";"; }
                instBuilder.Append(inst2);
            }
            foreach (string tool in toolList)
            {
                string tool2 = "\n\t" + tool;
                if (!tool2.EndsWith(";")) { tool2 = tool2 + ";"; }
                toolBuilder.Append(tool2);
            }
            foreach (string wobj in wobjList)
            {
                string wobj2 = "\n\t" + wobj;
                if (!wobj2.EndsWith(";")) { wobj2 = wobj2 + ";"; }
                wobjBuilder.Append(wobj2);
            }

            // create rapid
            string r = "";
            using (var tw = new StreamWriter(filePath, false))
            {
                r = string.Format("MODULE MainModule\n" +
                                            "\t! Program data\n" +
                                            "{0}" +
                                            "{1}" +
                                            "\n" +
                                            "\t! Target data" +
                                            "{2}\n" +
                                            "\n" +
                                            "\t! Routine\n" +
                                            "\tPROC main()\n" +
                                            "\t\tConfL\\Off;\n" +
                                            "\t\tSingArea\\Wrist;\n" +
                                            "\t\trStart;\n" +
                                            "\t\tStop;\n" +
                                            "\tENDPROC\n" +
                                            "\n" +
                                            "\tPROC rStart()\n" +
                                            "\t\t! instructions" +
                                            "{3}" +
                                            "\n" +
                                            "\t\tRETURN;\n" +
                                            "\tENDPROC\n" +
                                            "\n" +
                                            "ENDMODULE\n",
                toolBuilder.ToString(), wobjBuilder.ToString(), cnstBuilder.ToString(), instBuilder.ToString());

                tw.Write(r);
                tw.Flush();
                tw.Close();
            }
            
            // end step
            return new Dictionary<string, string>
            {
                {"filePath", filePath},
                {"robotCode", r}
            };

        }


        /// <summary>
        /// Merge and write data to a destination.
        /// </summary>
        /// <param name="filePath">"C:\...\myPath.prg"</param>
        /// <param name="cnstList">List of constants</param>
        /// <param name="instList">List of instructions</param>
        /// <returns></returns>
        [MultiReturn(new[] { "filePath", "robotCode" })]
        public static Dictionary<string, string> createRapid1(string filePath, List<string> cnstList, List<string> instList)
        {
            // setup
            var cnstBuilder = new StringBuilder();
            var instBuilder = new StringBuilder();
            foreach (string cnst in cnstList)
            {
                string cnst2 = "\n\t" + cnst;
                if (!cnst2.EndsWith(";")) { cnst2 = cnst2 + ";"; }
                cnstBuilder.Append(cnst2);
            }
            foreach (string inst in instList)
            {
                string inst2 = "\n\t\t" + inst;
                if (!inst2.EndsWith(";")) { inst2 = inst2 + ";"; }
                instBuilder.Append(inst2);
            }

            // create rapid
            string r = "";
            using (var tw = new StreamWriter(filePath, false))
            {
                r = string.Format("MODULE MainModule\n" +
                                            "\n\t! Target data" +
                                            "{0}\n" +
                                            "\n" +
                                            "\t! Routine\n" +
                                            "\tPROC main()\n" +
                                            "\t\tConfL\\Off;\n" +
                                            "\t\tSingArea\\Wrist;\n" +
                                            "\t\trStart;\n" +
                                            "\t\tStop;\n" +
                                            "\tENDPROC\n" +
                                            "\n" +
                                            "\tPROC rStart()\n" +
                                            "\n\t\t! instructions" +
                                            "{1}\n" +
                                            "\t\tRETURN;\n" +
                                            "\tENDPROC\n" +
                                            "\n" +
                                            "ENDMODULE\n",
                cnstBuilder.ToString(), instBuilder.ToString());

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


    }

    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// Create  utility.
    /// </summary>
    public class Utilities
    {


        /// <summary>
        /// Get list of quaternions from a plane.
        /// </summary>
        /// <param name="plane">The plane</param>
        /// <returns></returns>
        public static List<double> QuatListAtPlane([DefaultArgumentAttribute("Plane.ByOriginNormal(Point.ByCoordinates(0,0,0),Vector.ByCoordinates(0,0,1))")] Plane plane)
        {
            List<double> quats = new List<double>();

            if (plane != null)
            {
                List<double> quatDoubles = RobotUtils.PlaneToQuaternian(plane);
                quats.Add(quatDoubles[0]);
                quats.Add(quatDoubles[1]);
                quats.Add(quatDoubles[2]);
                quats.Add(quatDoubles[3]);
            }
            return quats;
        }

        /// <summary>
        /// Insert an item into a list at specified index.
        /// </summary>
        /// <param name="list">Initial list</param>
        /// <param name="item">Item to insert</param>
        /// <param name="index">Index at which to insert</param>
        /// <returns></returns>
        public static List<object> InsertAtIndex(List<object> list, List<object> item, List<int> index)
        {
            int cnt = 0;
            foreach (var dex in index)
            {
                if (item.Count == cnt) { item.Add(item[0]); }
                if (dex <= list.Count + 1) { list.Insert(dex + cnt, item[cnt]); }
                if (dex > list.Count + 1) { list.Add(item[cnt]); }
                cnt++;
            }
            return list;
        }
        /// <summary>
        /// Insert an item into a list at specified index.
        /// </summary>
        /// <param name="list">Initial list</param>
        /// <param name="item">Item to insert</param>
        /// <param name="index">Index at which to insert</param>
        /// <returns></returns>
        public static List<object> InsertAtFirstLast(List<object> LstFirst, List<object> LstMiddle, List<object> LstLast)
        {
      
            List<object> combList = new List<object>(LstMiddle.Count+2);
            combList.Add(LstFirst[0]);
            combList.AddRange(LstMiddle);
            combList.Add(LstLast[0]);

            
            return combList;
        }

        [MultiReturn(new[] { "First", "Middle", "Last" })]
        public static Dictionary<string, List<object>> ListFistLast(List<object> list )
        {
            List<object> firstItm = new List<object>();
            List<object> middleItms = new List<object>();
            List<object> lastItm = new List<object>();

                firstItm.Add(list[0]);
                for (int i = 1; i < list.Count - 2; i++)
                {
                    middleItms.Add(list[i]);
                }
                lastItm.Add(list[list.Count - 1]);
       

            

            return new Dictionary<string, List<object>>
            {
                {"First", firstItm},
                {"Middle", middleItms},
                {"Last", lastItm}
            };
        }


        /// <summary>
        /// Insert a group of items into a list at specified index.
        /// </summary>
        /// <param name="list">Initial list</param>
        /// <param name="group">Group of items to insert</param>
        /// <param name="index">Index at which to insert</param>
        /// <returns></returns>
        public static List<object> InsertGroupAtIndex(List<object> list, List<object> group, List<int> index)
        {
            int cnt = 0;
            foreach (var dex in index)
            {
                int newdex = dex + cnt * group.Count;
                if (newdex <= list.Count + 1) { list.InsertRange(newdex, group); }
                if (newdex > list.Count + 1) { list.AddRange(group); }
                cnt++;
            }
            return list;
        }

        /// <summary>
        /// Combine two lists by items and indices.
        /// </summary>
        /// <param name="listA"></param>
        /// <param name="listB"></param>
        /// <param name="indexA"></param>
        /// <param name="indexB"></param>
        /// <returns></returns>
        public static object[] CombineListsByIndices(List<object> listA, List<object> listB, List<int> indexA, List<int> indexB)
        {
            object[] myList = new object[indexA.Count + indexB.Count];
            for (int i = 0; i < indexA.Count; i++)
            {
                myList[indexA[i]] = listA[i];
            }
            for (int j = 0; j < indexB.Count; j++)
            {
                myList[indexB[j]] = listB[j];
            }
            return myList;
        }

        /// <summary>
        /// Until specified length, zero-pad a value at left.
        /// </summary>
        /// <param name="val">Initial value</param>
        /// <param name="numDigits">Total number of digits</param>
        /// <returns></returns>
        public static string ZeroPadLeft(double val, int numDigits)
        {
            string valStr = val.ToString().PadLeft(numDigits, '0');
            return valStr;
        }

        /// <summary>
        /// Until specified length, zero-pad a value at right.
        /// </summary>
        /// <param name="val">Initial value</param>
        /// <param name="numDigits">Total number of digits</param>
        /// <returns></returns>
        public static string ZeroPadRight(double val, int numDigits)
        {
            string valStr = val.ToString().PadRight(numDigits, '0');
            return valStr;
        }

        /// <summary>
        /// Create file at destination from data in Dynamo.
        /// </summary>
        /// <param name="filePath">Write to destination</param>
        /// <param name="robData">Data to write</param>
        /// <returns></returns>
        public static string DataToFile(string filePath, List<string> robData)
        {
            var dataBuilder = new StringBuilder();
            foreach (string data in robData) { dataBuilder.Append(data); }

            using (var lines = new StreamWriter(filePath, false))
            {
                lines.Write(dataBuilder.ToString());
                lines.Flush();
            }
            return filePath;
        }

        /// <summary>
        /// Read data in Dynamo from file at destination.
        /// </summary>
        /// <param name="filePath">Read from destination</param>
        /// <returns></returns>
        public static string FileToData(string filePath)
        {
            string robData;
            using (var data = new StreamReader(filePath, false))
            {
                robData = data.ReadToEnd();
            }
            return robData;
        }

        /// <summary>
        /// Sort points by Z value.
        /// </summary>
        /// <param name="points">Point list</param>
        /// <returns></returns>
        public static List<Point> sortPointsByZ(List<Point> points)
        {
            points = points.OrderBy(a => a.Z).ToList();
            return points;
        }

        /// <summary>
        /// Sort vectors by Z value.
        /// </summary>
        /// <param name="vectors">Vectors list</param>
        /// <returns></returns>
        public static List<Vector> sortVectorsByZ(List<Vector> vectors)
        {
            vectors = vectors.OrderBy(a => a.Z).ToList();
            return vectors;
        }

        /// <summary>
        /// Sort planes by Z value.
        /// </summary>
        /// <param name="planes">Plane list</param>
        /// <returns></returns>
        public static List<Plane> sortPlanesByZ(List<Plane> planes)
        {
            planes = planes.OrderBy(a => a.Origin.Z).ToList();
            return planes;
        }

        /// <summary>
        /// Sort coordinate systems by Z value.
        /// </summary>
        /// <param name="coordSys">Coordinate system list</param>
        /// <returns></returns>
        public static List<CoordinateSystem> sortPlanesByZ(List<CoordinateSystem> coordSys)
        {
            coordSys = coordSys.OrderBy(a => a.Origin.Z).ToList();
            return coordSys;
        }




        /// <summary>
        /// Sort points by directionality about arbitrary pole.
        /// </summary>
        /// <param name="pointList"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "sorted", "indices" })]
        public static Dictionary<string, List<Object>> sortPolar1_Point(List<Point> pointList)
        {
            List<double> angList = new List<double>();
            List<Object> newList = new List<Object>();

            foreach (Point p in pointList)
            {
                double x = p.X;
                double y = p.Y;
                double d = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                double xnorm = x / d;
                double ynorm = y / d;
                double t = Math.Atan2(ynorm, xnorm);
                if (y < 0) { t = t - 180; }
                angList.Add(t);
            }

            var sortedAng = angList
                .Select((x, i) => new KeyValuePair<int, int>((int)x, i))
                .OrderBy(x => x.Key)
                .ToList();

            List<Object> indices = sortedAng.Select(x => (Object)x.Value).ToList();
            foreach (int i in indices)
            {
                newList.Add(pointList[i]);
            }
            return new Dictionary<string, List<Object>>
            {
                {"sorted", newList},
                {"indices", indices}
            };
        }



        /// <summary>
        /// Sort vectors by directionality about arbitrary pole.
        /// </summary>
        /// <param name="vecList"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "sorted", "indices" })]
        public static Dictionary<string, List<Object>> sortPolar1_Vector(List<Vector> vecList)
        {
            List<double> angList = new List<double>();
            List<Object> newList = new List<Object>();

            foreach (Vector v in vecList)
            {
                double x = v.X;
                double y = v.Y;
                double d = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                double xnorm = x / d;
                double ynorm = y / d;
                double t = Math.Atan2(ynorm, xnorm);
                if (y < 0) { t = t - 180; }
                angList.Add(t);
            }

            var sortedAng = angList
                .Select((x, i) => new KeyValuePair<int, int>((int)x, i))
                .OrderBy(x => x.Key)
                .ToList();

            List<Object> indices = sortedAng.Select(x => (Object)x.Value).ToList();
            foreach (int i in indices)
            {
                newList.Add(vecList[i]);
            }
            return new Dictionary<string, List<Object>>
            {
                {"sorted", newList},
                {"indices", indices}
            };
        }



        /// <summary>
        /// Sort planes directionality about arbitrary pole.
        /// </summary>
        /// <param name="planeList">List of Planes</param>
        /// <returns></returns>
        [MultiReturn(new[] { "sorted", "indices" })]
        public static Dictionary<string, List<Object>> sortPolar1_Plane(List<Plane> planeList)
        {
            List<double> angList = new List<double>();
            List<Object> newList = new List<Object>();

            foreach (Plane p in planeList)
            {
                double x = p.Normal.X;
                double y = p.Normal.Y;
                double d = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                double xnorm = x / d;
                double ynorm = y / d;
                double t = Math.Atan2(ynorm, xnorm);
                if (y < 0) { t = t - 180; }
                angList.Add(t);
            }

            var sortedAng = angList
                .Select((x, i) => new KeyValuePair<int, int>((int)x, i))
                .OrderBy(x => x.Key)
                .ToList();

            List<Object> indices = sortedAng.Select(x => (Object)x.Value).ToList();
            foreach (int i in indices)
            {
                newList.Add(planeList[i]);
            }
            return new Dictionary<string, List<Object>>
            {
                {"sorted", newList},
                {"indices", indices},
            };
        }



        /// <summary>
        /// Sort coordinate systems directionality about arbitrary pole.
        /// </summary>
        /// <param name="csList">List of coordinate systems</param>
        /// <returns></returns>
        [MultiReturn(new[] { "sorted", "indices" })]
        public static Dictionary<string, List<Object>> sortPolar1_CoordSys(List<CoordinateSystem> csList)
        {
            List<double> angList = new List<double>();
            List<Object> newList = new List<Object>();

            foreach (CoordinateSystem cs in csList)
            {
                double x = cs.ZAxis.X;
                double y = cs.ZAxis.Y;
                double d = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                double xnorm = x / d;
                double ynorm = y / d;
                double t = Math.Atan2(ynorm, xnorm);
                if (y < 0) { t = t - 180; }

                angList.Add(t);
            }

            var sortedAng = angList
                .Select((x, i) => new KeyValuePair<int, int>((int)x, i))
                .OrderBy(x => x.Key)
                .ToList();

            List<Object> indices = sortedAng.Select(x => (Object)x.Value).ToList();
            foreach (int i in indices)
            {
                newList.Add(csList[i]);
            }
            return new Dictionary<string, List<Object>>
            {
                {"sorted", newList},
                {"indices", indices}
            };
        }



        /// <summary>
        /// Sort points by directionality about average pole and shift.
        /// </summary>
        /// <param name="vectorList">List of points</param>
        /// <param name="shift">Shift value</param>
        /// <returns></returns>
        [MultiReturn(new[] { "sorted", "indices" })]
        public static Dictionary<string, List<Object>> sortPolar2_Vector(List<Point> vectorList, int shift = 0)
        {
            List<Object> newList = new List<Object>();
            List<double> paramList = new List<double>();

            List<double> vx = vectorList.Select(p => p.X).ToList();
            List<double> vy = vectorList.Select(p => p.Y).ToList();
            List<double> vz = vectorList.Select(p => p.Z).ToList();

            Vector vecAvg = Vector.ByCoordinates(vx.Average(), vy.Average(), vz.Average());
            Circle guide = Circle.ByCenterPointRadiusNormal(Point.ByCoordinates(0, 0, 0), 1, vecAvg);
            for (int i = 0; i < vectorList.Count(); i++)
            {
                Point v = Point.ByCoordinates(vx[i], vy[i], vz[i]);
                double param = guide.ParameterAtPoint(guide.ClosestPointTo(v));
                paramList.Add(param);
            }

            var sortedParams = paramList
                .Select((x, i) => new KeyValuePair<int, int>((int)x, i))
                .OrderBy(x => x.Key)
                .ToList();

            List<Object> indices = sortedParams.Select(x => (Object)x.Value).ToList();
            foreach (int i in indices) { newList.Add(vectorList[i]); }

            List<Object> shifted = new List<Object>();
            if (shift < 0) { shift = newList.Count - shift % newList.Count - 1; }
            if (Math.Abs(shift) >= newList.Count) { shift = shift % newList.Count; }
            shifted = newList.GetRange(shift, newList.Count - shift);
            shifted.AddRange(newList.GetRange(0, shift));

            return new Dictionary<string, List<Object>>
            {
                {"sorted", shifted},
                {"indices", indices},
            };
        }



        /// <summary>
        /// Sort vectors by directionality about average pole and shift.
        /// </summary>
        /// <param name="vecList">List of vectors</param>
        /// <param name="shift">Shift value</param>
        /// <returns></returns>
        [MultiReturn(new[] { "sorted", "indices" })]
        public static Dictionary<string, List<Object>> sortPolar2_Vector(List<Vector> vecList, int shift = 0)
        {
            List<Object> newList = new List<Object>();
            List<double> paramList = new List<double>();

            List<double> vx = vecList.Select(p => p.X).ToList();
            List<double> vy = vecList.Select(p => p.Y).ToList();
            List<double> vz = vecList.Select(p => p.Z).ToList();

            Vector vecAvg = Vector.ByCoordinates(vx.Average(), vy.Average(), vz.Average());
            Circle guide = Circle.ByCenterPointRadiusNormal(Point.ByCoordinates(0, 0, 0), 1, vecAvg);
            for (int i = 0; i < vecList.Count(); i++)
            {
                Point v = Point.ByCoordinates(vx[i], vy[i], vz[i]);
                double param = guide.ParameterAtPoint(guide.ClosestPointTo(v));
                paramList.Add(param);
            }

            var sortedParams = paramList
                .Select((x, i) => new KeyValuePair<int, int>((int)x, i))
                .OrderBy(x => x.Key)
                .ToList();

            List<Object> indices = sortedParams.Select(x => (Object)x.Value).ToList();
            foreach (int i in indices) { newList.Add(vecList[i]); }

            List<Object> shifted = new List<Object>();
            if (shift < 0) { shift = newList.Count - shift % newList.Count - 1; }
            if (Math.Abs(shift) >= newList.Count) { shift = shift % newList.Count; }
            shifted = newList.GetRange(shift, newList.Count - shift);
            shifted.AddRange(newList.GetRange(0, shift));

            return new Dictionary<string, List<Object>>
            {
                {"sorted", shifted},
                {"indices", indices},
            };
        }




        /// <summary>
        /// Sort planes by directionality about average pole and shift.
        /// </summary>
        /// <param name="planeList">List of planes</param>
        /// <param name="shift">Shift value</param>
        /// <returns></returns>
        [MultiReturn(new[] { "sorted", "indices" })]
        public static Dictionary<string, List<Object>> sortPolar2_Plane(List<Plane> planeList, int shift = 0)
        {
            List<Object> newList = new List<Object>();
            List<double> paramList = new List<double>();

            List<double> vx = planeList.Select(p => p.Normal.X).ToList();
            List<double> vy = planeList.Select(p => p.Normal.Y).ToList();
            List<double> vz = planeList.Select(p => p.Normal.Z).ToList();

            Vector vecAvg = Vector.ByCoordinates(vx.Average(), vy.Average(), vz.Average());
            Circle guide = Circle.ByCenterPointRadiusNormal(Point.ByCoordinates(0, 0, 0), 1, vecAvg);
            for (int i = 0; i < planeList.Count(); i++)
            {
                Point v = Point.ByCoordinates(vx[i], vy[i], vz[i]);
                double param = guide.ParameterAtPoint(guide.ClosestPointTo(v));
                paramList.Add(param);
            }

            var sortedParams = paramList
                .Select((x, i) => new KeyValuePair<int, int>((int)x, i))
                .OrderBy(x => x.Key)
                .ToList();

            List<Object> indices = sortedParams.Select(x => (Object)x.Value).ToList();
            foreach (int i in indices) { newList.Add(planeList[i]); }

            List<Object> shifted = new List<Object>();
            if (shift < 0) { shift = newList.Count - shift % newList.Count - 1; }
            if (Math.Abs(shift) >= newList.Count) { shift = shift % newList.Count; }
            shifted = newList.GetRange(shift, newList.Count - shift);
            shifted.AddRange(newList.GetRange(0, shift));

            return new Dictionary<string, List<Object>>
            {
                {"sorted", shifted},
                {"indices", indices},
            };
        }



        /// <summary>
        /// Sort coordinate systems by directionality about average pole and shift.
        /// </summary>
        /// <param name="csList">List of coordinate systems</param>
        /// <param name="shift">Shift value</param>
        /// <returns></returns>
        [MultiReturn(new[] { "sorted", "indices" })]
        public static Dictionary<string, List<Object>> sortPolar2_CoordSys(List<CoordinateSystem> csList, int shift = 0)
        {
            List<Object> newList = new List<Object>();
            List<double> paramList = new List<double>();

            List<double> vx = csList.Select(p => p.ZAxis.X).ToList();
            List<double> vy = csList.Select(p => p.ZAxis.Y).ToList();
            List<double> vz = csList.Select(p => p.ZAxis.Z).ToList();

            Vector vecAvg = Vector.ByCoordinates(vx.Average(), vy.Average(), vz.Average());
            Circle guide = Circle.ByCenterPointRadiusNormal(Point.ByCoordinates(0, 0, 0), 1, vecAvg);
            for (int i = 0; i < csList.Count(); i++)
            {
                Point v = Point.ByCoordinates(vx[i], vy[i], vz[i]);
                double param = guide.ParameterAtPoint(guide.ClosestPointTo(v));
                paramList.Add(param);
            }

            var sortedParams = paramList
                .Select((x, i) => new KeyValuePair<int, int>((int)x, i))
                .OrderBy(x => x.Key)
                .ToList();

            List<Object> indices = sortedParams.Select(x => (Object)x.Value).ToList();
            foreach (int i in indices) { newList.Add(csList[i]); }

            List<Object> shifted = new List<Object>();
            if (shift < 0) { shift = newList.Count - shift % newList.Count - 1; }
            if (Math.Abs(shift) >= newList.Count) { shift = shift % newList.Count; }
            shifted = newList.GetRange(shift, newList.Count - shift);
            shifted.AddRange(newList.GetRange(0, shift));

            return new Dictionary<string, List<Object>>
            {
                {"sorted", shifted},
                {"indices", indices},
            };
        }



        /// <summary>
        /// Shift the contents of a list by value.
        /// </summary>
        /// <param name="lst">List of objects</param>
        /// <param name="shift">Shift value</param>
        /// <returns></returns>
        public static List<Object> shiftList(List<Object> lst, int shift = 0)
        {
            List<Object> shifted = new List<Object>();
            if (shift < 0) { shift = lst.Count - shift % lst.Count - 1; }
            if (Math.Abs(shift) >= lst.Count) { shift = shift % lst.Count; }
            shifted = lst.GetRange(shift, lst.Count - shift);
            shifted.AddRange(lst.GetRange(0, shift));
            return shifted;
        }



        /// <summary>
        /// Get item at index in range 0 to 1.
        /// </summary>
        /// <param name="lst">List of objects</param>
        /// <param name="index">Index value from 0 to 1</param>
        /// <returns></returns>
        public static Object getItemAtParam(List<Object> lst, double index = 0)
        {
            if (index == 1) { index = 0; }
            int i = (int)(index * lst.Count());
            Object item = lst[i];
            return item;
        }



        /// <summary>
        /// Create a set from only the unique items in a list of points.
        /// </summary>
        /// <param name="points">List of points</param>
        /// <returns></returns>
        [MultiReturn(new[] { "passed", "failed" })]
        public static Dictionary<string, List<Point>> getUnique_Point(List<Point> points)
        {
            List<Point> passed = new List<Point>();
            List<Point> failed = new List<Point>();
            List<double[]> checkList = new List<double[]>();

            foreach (Point p in points)
            {
                double[] check = new double[] { p.X, p.Y, p.Z };
                check = check.Select(val => Math.Round(val, 4)).ToArray();
                if (!checkList.Any(check.SequenceEqual))
                {
                    passed.Add(p);
                    checkList.Add(check);
                }
                else
                {
                    failed.Add(p);
                }
            }

            return new Dictionary<string, List<Point>>
            {
                {"passed", passed},
                {"failed", failed}
            };
        }



        /// <summary>
        /// Create a set from only the unique items in a list of vectors.
        /// </summary>
        /// <param name="vectors">List of vectors</param>
        /// <returns></returns>
        [MultiReturn(new[] { "passed", "failed" })]
        public static Dictionary<string, List<Vector>> getUnique_Vector(List<Vector> vectors)
        {
            List<Vector> passed = new List<Vector>();
            List<Vector> failed = new List<Vector>();
            List<double[]> checkList = new List<double[]>();

            foreach (Vector v in vectors)
            {
                double[] check = new double[] { v.X, v.Y, v.Z };
                check = check.Select(val => Math.Round(val, 4)).ToArray();
                if (!checkList.Any(check.SequenceEqual))
                {
                    passed.Add(v);
                    checkList.Add(check);
                }
                else
                {
                    failed.Add(v);
                }

            }

            return new Dictionary<string, List<Vector>>
            {
                {"passed", passed},
                {"failed", failed}
            };
        }



        /// <summary>
        /// Create a set from only the unique items in a list of planes.
        /// </summary>
        /// <param name="planes">List of planes</param>
        /// <param name="option">Test origin and axes (true) or just axes (false)</param>
        /// <returns></returns>
        [MultiReturn(new[] { "passed", "failed" })]
        public static Dictionary<string, List<Plane>> getUnique_Plane(List<Plane> planes, bool option = false)
        {
            List<Plane> passed = new List<Plane>();
            List<Plane> failed = new List<Plane>();
            List<double[]> checkList = new List<double[]>();

            foreach (Plane p in planes)
            {
                double[] check = new double[] { };
                if (option)
                {
                    check = new double[] { p.Origin.X, p.Origin.Y, p.Origin.Z, p.Normal.X, p.Normal.Y, p.Normal.Z, p.XAxis.X, p.XAxis.Y, p.XAxis.Z };
                }
                else
                {
                    check = new double[] { p.Normal.X, p.Normal.Y, p.Normal.Z, p.XAxis.X, p.XAxis.Y, p.XAxis.Z };
                }
                check = check.Select(val => Math.Round(val, 4)).ToArray();
                if (!checkList.Any(check.SequenceEqual))
                {
                    passed.Add(p);
                    checkList.Add(check);
                }
                else
                {
                    failed.Add(p);
                }
            }

            return new Dictionary<string, List<Plane>>
            {
                {"passed", passed},
                {"failed", failed}
            };
        }



        /// <summary>
        /// Create a set from only the unique items in a list of coordinate systems.
        /// </summary>
        /// <param name="coordSys">List of coordinate systems</param>
        /// <param name="option">Test origin and axes (true) or just axes (false)</param>
        /// <returns></returns>
        [MultiReturn(new[] { "passed", "failed" })]
        public static Dictionary<string, List<CoordinateSystem>> getUnique_CoordSys(List<CoordinateSystem> coordSys, bool option = false)
        {
            List<CoordinateSystem> passed = new List<CoordinateSystem>();
            List<CoordinateSystem> failed = new List<CoordinateSystem>();
            List<double[]> checkList = new List<double[]>();

            foreach (CoordinateSystem c in coordSys)
            {
                double[] check = new double[] { };
                if (option)
                {
                    check = new double[] { c.Origin.X, c.Origin.Y, c.Origin.Z, c.ZAxis.X, c.ZAxis.Y, c.ZAxis.Z, c.XAxis.X, c.XAxis.Y, c.XAxis.Z };
                }
                else
                {
                    check = new double[] { c.ZAxis.X, c.ZAxis.Y, c.ZAxis.Z, c.XAxis.X, c.XAxis.Y, c.XAxis.Z };
                }
                check = check.Select(v => Math.Round(v, 4)).ToArray();
                if (!checkList.Any(check.SequenceEqual))
                {
                    passed.Add(c);
                    checkList.Add(check);
                }
                else
                {
                    failed.Add(c);
                }
            }

            return new Dictionary<string, List<CoordinateSystem>>
            {
                {"passed", passed},
                {"failed", failed}
            };
        }




        /// <summary>
        /// Aligns XAxis of plane to guide vector (default: World XAxis)
        /// </summary>
        /// <param name="planeList">List of planes</param>
        /// <param name="vec">Alignment vector for XAxes</param>
        /// <param name="degree">Angle multiplier</param>
        /// <returns></returns>
        public static List<Plane> alignByXAxis_Plane(List<Plane> planeList, [DefaultArgumentAttribute("{Vector.ByCoordinates(1,0,0)}")] Vector vec, double degree = 1)
        {
            List<Plane> newPlList = new List<Plane>();

            foreach (Plane p in planeList)
            {
                Vector projectedVector = Vector.ByCoordinates(p.XAxis.X, p.XAxis.Y, 0);
                double dot = p.XAxis.Dot(projectedVector);
                //double angle = Math.Acos(dot) * (-1) * degree * (180 / Math.PI);
                double angle = Math.Acos(dot) * (-1) * degree;
                Plane newPl = (Plane)p.Rotate(p.Origin, p.Normal, angle);
                newPlList.Add(newPl);
            }
            return newPlList;
        }



        /// <summary>
        /// Aligns XAxis of coordinate systems to guide vector (default: World XAxis)
        /// </summary>
        /// <param name="csList">List of coordinate systems</param>
        /// <param name="vec">Alignment vector for XAxes</param>
        /// <param name="degree">Angle multiplier</param>
        /// <returns></returns>
        public static List<CoordinateSystem> alignByXAxis_CoordSys(List<CoordinateSystem> csList, [DefaultArgumentAttribute("{Vector.ByCoordinates(1,0,0)}")] Vector vec, double degree = 1)
        {
            List<CoordinateSystem> newCSList = new List<CoordinateSystem>();

            foreach (CoordinateSystem cs in csList)
            {
                Vector projectedVector = Vector.ByCoordinates(cs.XAxis.X, cs.XAxis.Y, 0);
                double dot = cs.XAxis.Dot(projectedVector);
                //double angle = Math.Acos(dot) * (-1) * degree * (180 / Math.PI);
                double angle = Math.Acos(dot) * (-1) * degree;
                CoordinateSystem newCS = cs.Rotate(cs.Origin, cs.ZAxis, angle);
                newCSList.Add(newCS);
            }
            return newCSList;
        }





        /// <summary>
        /// Reverse normal and retain rotation of plane.
        /// </summary>
        /// <param name="planeList">List of planes</param>
        /// <param name="retain">Retain X-Axis?</param>
        /// <returns></returns>
        public static List<Plane> flip_Plane(List<Plane> planeList, bool retain = false)
        {
            List<Plane> newPlaneList = new List<Plane>();

            switch (retain)
            {
                case true:
                    {
                        foreach (Plane pl in planeList)
                        {
                            Plane newPl = Plane.ByOriginNormalXAxis(pl.Origin, pl.Normal.Reverse(), pl.XAxis);
                            newPlaneList.Add(newPl);
                        }
                        break;
                    }
                case false:
                    {
                        foreach (Plane pl in planeList)
                        {
                            Plane newPl = Plane.ByOriginNormalXAxis(pl.Origin, pl.Normal.Reverse(), pl.XAxis.Reverse());
                            newPlaneList.Add(newPl);
                        }
                        break;
                    }
            }

            return newPlaneList;
        }



        /// <summary>
        /// Reverse normal and retain rotation of coordinate system.
        /// </summary>
        /// <param name="csList">List of coordinate systems</param>
        /// <param name="retain">Retain X-Axis?</param>
        /// <returns></returns>
        public static List<CoordinateSystem> flip_CoordSys(List<CoordinateSystem> csList, bool retain)
        {
            List<CoordinateSystem> newCSList = new List<CoordinateSystem>();

            switch (retain)
            {
                case true:
                    {
                        foreach (CoordinateSystem cs in csList)
                        {
                            CoordinateSystem newCS = CoordinateSystem.ByPlane(Plane.ByOriginNormalXAxis(cs.Origin, cs.ZAxis.Reverse(), cs.XAxis));
                            newCSList.Add(newCS);
                        }
                        break;
                    }
                case false:
                    {
                        foreach (CoordinateSystem cs in csList)
                        {
                            CoordinateSystem newCS = CoordinateSystem.ByPlane(Plane.ByOriginNormalXAxis(cs.Origin, cs.ZAxis.Reverse(), cs.XAxis.Reverse()));
                            newCSList.Add(newCS);
                        }
                        break;
                    }
            }
            return newCSList;
        }



        /// <summary>
        /// Transform each sublist of planes to World XY plane using average ZAxis.
        /// </summary>
        /// <param name="planes">List of list of planes</param>
        /// <param name="guide">Guide plane</param>
        /// <returns></returns>
        public static List<List<Plane>> transform_Plane(List<List<Plane>> planes, [DefaultArgumentAttribute("{Plane.ByOriginNormalXAxis(Point.ByCoordinates(0,0,0), Vector.ByCoordinates(0,0,1), Vector.ByCoordinates(1,0,0))}")] Plane guide)
        {
            List<List<Plane>> newPlanes = new List<List<Plane>>();

            foreach (List<Plane> sub in planes)
            {
                Point origin = sub.Select(p => p.Origin).ToList()[0];
                List<Vector> zAxis = sub.Select(p => p.Normal).ToList();
                Vector zAxisAvg = Vector.ByCoordinates(zAxis.Average(v => v.X), zAxis.Average(v => v.Y), zAxis.Average(v => v.Z));
                Plane subFrame = Plane.ByOriginNormal(origin, zAxisAvg);

                CoordinateSystem subFrameTransform = subFrame.ContextCoordinateSystem;
                CoordinateSystem guideFrame = guide.ContextCoordinateSystem;

                List<Plane> newSub = new List<Plane>();
                foreach (Plane plane in sub)
                {
                    newSub.Add((Plane)plane.Transform(subFrameTransform, guideFrame));
                }
                newPlanes.Add(newSub);
            }
            return newPlanes;
        }



        /// <summary>
        /// Transform each sublist of coordinate systems to World XY coordinate systems using average ZAxis.
        /// </summary>
        /// <param name="coordSys">List of list of planes</param>
        /// <param name="guide">Guide coordinate system</param>
        /// <returns></returns>
        public static List<List<CoordinateSystem>> transform_CoordSys(List<List<CoordinateSystem>> coordSys, [DefaultArgumentAttribute("{CoordinateSystem.ByPlane(Plane.ByOriginNormalXAxis(Point.ByCoordinates(0,0,0), Vector.ByCoordinates(0,0,1), Vector.ByCoordinates(1,0,0)))}")] CoordinateSystem guide)
        {
            List<List<CoordinateSystem>> newCoordSys = new List<List<CoordinateSystem>>();

            foreach (List<CoordinateSystem> sub in coordSys)
            {
                Point origin = sub.Select(p => p.Origin).ToList()[0];
                List<Vector> zAxis = sub.Select(p => p.ZAxis).ToList();
                Vector zAxisAvg = Vector.ByCoordinates(zAxis.Average(v => v.X), zAxis.Average(v => v.Y), zAxis.Average(v => v.Z));
                Plane subFrame = Plane.ByOriginNormal(origin, zAxisAvg);

                CoordinateSystem subFrameTransform = subFrame.ContextCoordinateSystem;

                List<CoordinateSystem> newSub = new List<CoordinateSystem>();
                foreach (CoordinateSystem cs in sub)
                {
                    newSub.Add((CoordinateSystem)cs.Transform(subFrameTransform, guide));
                }
                newCoordSys.Add(newSub);
            }
            return newCoordSys;
        }


        /// <summary>
        /// Test plane normals against guide vector using angular tolerance.
        /// </summary>
        /// <param name="planeList">Planes to test</param>
        /// <param name="guide">Guide vector (Default: World -ZAxis)</param>
        /// <param name="tolerance">Angular tolerance (degrees)</param>
        /// <returns></returns>
        [MultiReturn(new[] { "passed", "failed" })]
        public static Dictionary<string, List<Plane>> testAngular1_Plane(List<Plane> planeList, [DefaultArgumentAttribute("{Vector.ByCoordinates(0,0,-1)}")] Vector guide, double tolerance = 120)
        {
            List<Plane> passed = new List<Plane>();
            List<Plane> failed = new List<Plane>();

            foreach (Plane p in planeList)
            {
                double dot = p.Normal.Dot(guide);
                double angle = Math.Acos(dot) * (180 / Math.PI);
                if (angle > tolerance)
                {
                    failed.Add(p);
                }
                else
                {
                    passed.Add(p);
                }
            }
            return new Dictionary<string, List<Plane>>
            {
                {"passed", passed},
                {"failed", failed}
            };
        }



        /// <summary>
        /// Test coordinate system Z-axes against guide vector using angular tolerance.
        /// </summary>
        /// <param name="csList">Coordinate systems to test</param>
        /// <param name="guide">Guide vector (Default: World -ZAxis)</param>
        /// <param name="tolerance">Angular tolerance (degrees)</param>
        /// <returns></returns>
        [MultiReturn(new[] { "passed", "failed" })]
        public static Dictionary<string, List<CoordinateSystem>> testAngular1_CoordSys(List<CoordinateSystem> csList, [DefaultArgumentAttribute("{Vector.ByCoordinates(0,0,-1)}")] Vector guide, double tolerance = 120)
        {
            List<CoordinateSystem> passed = new List<CoordinateSystem>();
            List<CoordinateSystem> failed = new List<CoordinateSystem>();

            foreach (CoordinateSystem cs in csList)
            {
                double dot = cs.ZAxis.Dot(guide);
                double angle = Math.Acos(dot) * (180 / Math.PI);
                if (angle > tolerance)
                {
                    failed.Add(cs);
                }
                else
                {
                    passed.Add(cs);
                }
            }
            return new Dictionary<string, List<CoordinateSystem>>
            {
                {"passed", passed},
                {"failed", failed}
            };
        }




        /// <summary>
        /// Test plane normals against each other using angular tolerance.
        /// </summary>
        /// <param name="planeList">Planes to test</param>
        /// <param name="tolerance">Angular tolerance (degrees)</param>
        /// <returns></returns>
        [MultiReturn(new[] { "passed", "failed" })]
        public static Dictionary<string, List<Plane>> testAngular2_Plane(List<Plane> planeList, double tolerance = 35)
        {
            List<Plane> passed = new List<Plane>();
            List<Plane> failed = new List<Plane>();

            List<Vector> normals = planeList.Select(p => p.Normal).ToList();

            for (int i = 0; i < planeList.Count(); i++)
            {
                for (int j = 0; j < planeList.Count(); j++)
                {
                    if (normals[i] != normals[j])
                    {
                        double dot = normals[i].Dot(normals[j]);
                        double angle = Math.Acos(dot) * (180 / Math.PI);

                        if (angle < tolerance && !failed.Contains(planeList[i]))
                        {
                            failed.Add(planeList[i]);
                        }
                    }
                }
            }

            passed = planeList.Except(failed).ToList();

            return new Dictionary<string, List<Plane>>
            {
                {"passed", passed},
                {"failed", failed}
            };
        }



        /// <summary>
        /// Test coordinate system Z-axes against each other using angular tolerance.
        /// </summary>
        /// <param name="csList">Coordinate systems to test</param>
        /// <param name="tolerance">Angular tolerance (degrees)</param>
        /// <returns></returns>
        [MultiReturn(new[] { "passed", "failed" })]
        public static Dictionary<string, List<CoordinateSystem>> testAngular2_CoordSys(List<CoordinateSystem> csList, double tolerance = 35)
        {
            List<CoordinateSystem> passed = new List<CoordinateSystem>();
            List<CoordinateSystem> failed = new List<CoordinateSystem>();

            List<Vector> normals = csList.Select(p => p.ZAxis).ToList();

            for (int i = 0; i < csList.Count(); i++)
            {
                for (int j = 0; j < csList.Count(); j++)
                {
                    if (normals[i] != normals[j])
                    {
                        double dot = normals[i].Dot(normals[j]);
                        double angle = Math.Acos(dot) * (180 / Math.PI);

                        if (angle < tolerance && !failed.Contains(csList[i]))
                        {
                            failed.Add(csList[i]);
                        }
                    }
                }
            }

            passed = csList.Except(failed).ToList();

            return new Dictionary<string, List<CoordinateSystem>>
            {
                {"passed", passed},
                {"failed", failed}
            };
        }


    }




    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////



    /// <summary>
    /// Communicate with robot controller.
    /// </summary>
    public class RobComm
    {
        [IsVisibleInDynamoLibrary(false)]
        public RobComm()
        {
            
        }

        internal static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// Scans network for controllers and returns SystemName, SystemID, Version, and IPAddress.
        /// </summary>
        /// <param name="run">True to run</param>
        /// <returns></returns>
        [MultiReturn(new[] { "robotController", "virtualController" })]
        public static Dictionary<string, List<string[]>> findControllers(bool run)
        {
            List<string[]> roboCtrl = new List<string[]> { };
            List<string[]> virtCtrl = new List<string[]> { };

            NetworkScanner scanner = null;

            if (run == true)
            {
                try
                {
                    //todo: this is crasing dynamo, find out why
                    try
                    {
                        scanner = new NetworkScanner();
                        scanner.Scan();
                    }
                    catch (Exception e)
                    {
                        
                        throw;
                    }
                    

                    
                }
                catch (Exception e)
                {
                    throw new  WarningException("Unable to connect to controler: " + e.Message);
                  
                }

                ControllerInfoCollection controllers = scanner.Controllers;
                foreach (ControllerInfo controller in controllers)
                {
                    if (controller.IsVirtual == false)
                    {
                        string[] eachController1 = new string[5]
                        {
                                controller.SystemName.ToString(),
                                controller.SystemId.ToString(),
                                controller.Availability.ToString(),
                                controller.Version.ToString(),
                                controller.IPAddress.ToString()
                        };
                        roboCtrl.Add(eachController1);
                    }

                    else
                    {
                        string[] eachController2 = new string[5]
                        {
                                controller.SystemName.ToString(),
                                controller.SystemId.ToString(),
                                controller.Availability.ToString(),
                                controller.Version.ToString(),
                                controller.IPAddress.ToString()
                        };
                        virtCtrl.Add(eachController2);
                    }
                }
            }
            return new Dictionary<string, List<string[]>>
            {
                {"robotController", roboCtrl},
                {"virtualController", virtCtrl}
            };
        }

        /// <summary>
        /// Send a .PRG file to controller.
        /// </summary>
        /// <param name="run">True to run</param>
        /// <param name="controllerData">Controller data</param>
        /// <param name="filePath">File to send</param>
        public static void ProgramFileToController(bool run, string[] controllerData, string filePath)
        {

            if (run == true)
            {
                Guid systemId = new Guid(controllerData[1]);
                Controller controller = new Controller(systemId);
                controller.Logon(UserInfo.DefaultUser);

                if (controller.Rapid.ExecutionStatus == ExecutionStatus.Stopped)

                {
                    Task newTask = controller.Rapid.GetTask("T_ROB1");
                    using (Mastership.Request(controller.Rapid))
                    {
                        newTask.LoadProgramFromFile(filePath, RapidLoadMode.Replace);
                    }
                }
                controller.Logoff();
            }
        }

    
        /// <summary>
        /// Read RobTargets and JointTargets for MainModule on controller.
        /// </summary>
        /// <param name="run">True to run</param>
        /// <param name="controllerData">Controller data</param>
        /// <returns></returns>
        [MultiReturn(new[] { "robTargets", "jointTargets" })]
        public static Dictionary<string, List<string>> getTargetData(bool run, string[] controllerData)
        {
            List<string> rTargets = new List<string>();
            List<string> jTargets = new List<string>();

            if (run == true)
            {
                Guid systemId = new Guid(controllerData[1]);
                Controller controller = new Controller(systemId);
                controller.Logon(UserInfo.DefaultUser);

                Task newTask = controller.Rapid.GetTask("T_ROB1");
                using (Mastership.Request(controller.Rapid))
                {
                    RapidSymbolSearchProperties sProp = RapidSymbolSearchProperties.CreateDefault();
                    sProp.Types = SymbolTypes.Data;
                    RapidSymbol[] datas = newTask.GetModule("MainModule").SearchRapidSymbol(sProp);
                    foreach (RapidSymbol rs in datas)
                    {
                        RapidData rd = controller.Rapid.GetTask("T_ROB1").GetModule("MainModule").GetRapidData(rs);
                        if (rd.Value is RobTarget)
                        { rTargets.Add(rd.Value.ToString()); }
                        if (rd.Value is JointTarget)
                        { jTargets.Add(rd.Value.ToString()); }
                    }
                }
            }

            return new Dictionary<string, List<string>>
            {
                {"robTargets", rTargets },
                {"jointTargets", jTargets }
            };
        }
        /*
       /// <summary>
       /// Read tooldata and wobjdata for MainModule on controller.
       /// </summary>
       /// <param name="run">True to run</param>
       /// <param name="controllerData">Controller data</param>
       /// <param name="moduleName">Module name: MainModule | BASE | user (case sensitive)</param>
       /// <returns></returns>
       [MultiReturn(new[] { "programData" , "currentData"}]
    })]
        public static Dictionary<string, List<string[]>> getProgramData(bool run, string[] controllerData, string moduleName)
        {
            List<string[]> progData = new List<string[]> { };
            //List<string[]> currData = new List<string[]> { };

            if (run == true)
            {
                Guid systemId = new Guid(controllerData[1]);
                Controller controller = new Controller(systemId);
                controller.Logon(UserInfo.DefaultUser);

                Task newTask = controller.Rapid.GetTask("T_ROB1");
               
                using (Mastership.Request(controller.Rapid))
                {
                    RapidSymbolSearchProperties sProp = RapidSymbolSearchProperties.CreateDefault();

                    sProp.Types = SymbolTypes.Data;
                    RapidSymbol[] progDatas = newTask.GetModule(moduleName).SearchRapidSymbol(sProp);
                    foreach (RapidSymbol rs in progDatas)
                    {
                        RapidData rd = controller.Rapid.GetTask("T_ROB1").GetModule(moduleName).GetRapidData(rs);
                        if ((rd.Value is ToolData) | (rd.Value is WobjData))
                        {
                            string[] eachProgData = new string[3]
                            {
                                rd.RapidType,
                                rd.Name,
                                rd.Value.ToString()
                            };
                            progData.Add(eachProgData);
                        }

                    }

                 
                    RapidSymbol[] currDatas = newTask.GetModule("MainModule").SearchRapidSymbol(sProp);
                    foreach (RapidSymbol rs in currDatas)
                    {
                        RapidData rd = controller.Rapid.GetTask("T_ROB1").GetModule("MainModule").GetRapidData(rs);
                        if ((rd.Value is ToolData) | (rd.Value is WobjData))
                        {
                            string[] eachCurr = new string[3]
                            {
                                rd.RapidType,
                                rd.Name,
                                rd.Value.ToString()
                            };
                            currData.Add(eachCurr);
                        }
                    }
                 

                }
            }

            return new Dictionary<string, List<string[]>>
            {
                {"programData", progData },{"currentData", currData}
            };
        }

        */

        /// <summary>
        /// Read RobTarget and JointTarget from current position.
        /// </summary>
        /// <param name="run">True to run</param>
        /// <param name="controllerData">Controller data</param>
        /// <returns></returns>
        [MultiReturn(new[] { "robotTarget", "jointTarget" }), CanUpdatePeriodically(true)]
        public static Dictionary<string, List<double[]>> getCurrentPosition(bool run, string[] controllerData)
        {
            List<double[]> robotTarget = new List<double[]>(4);
            List<double[]> jointTarget = new List<double[]>(2);

            if (run == true)
            {
                Guid systemId = new Guid(controllerData[1]);
                Controller controller = new Controller(systemId);
                controller.Logon(UserInfo.DefaultUser);

                Task newTask = controller.Rapid.GetTask("T_ROB1");
                using (Mastership.Request(controller.Rapid))
                {
                    
                    var rtv = newTask.GetRobTarget();
                    var jtv = newTask.GetJointTarget();
                    robotTarget.Add(new double[6] { rtv.Extax.Eax_a, rtv.Extax.Eax_a, rtv.Extax.Eax_b, rtv.Extax.Eax_c, rtv.Extax.Eax_d, rtv.Extax.Eax_e });
                    robotTarget.Add(new double[4] { rtv.Robconf.Cf1, rtv.Robconf.Cf4, rtv.Robconf.Cf6, rtv.Robconf.Cfx });
                    robotTarget.Add(new double[4] { rtv.Rot.Q1, rtv.Rot.Q2, rtv.Rot.Q3, rtv.Rot.Q4 });
                    robotTarget.Add(new double[3] { rtv.Trans.X, rtv.Trans.Y, rtv.Trans.Z });
                    jointTarget.Add(new double[6] { jtv.ExtAx.Eax_a, jtv.ExtAx.Eax_b, jtv.ExtAx.Eax_c, jtv.ExtAx.Eax_d, jtv.ExtAx.Eax_e, jtv.ExtAx.Eax_f });
                    jointTarget.Add(new double[6] { jtv.RobAx.Rax_1, jtv.RobAx.Rax_2, jtv.RobAx.Rax_3, jtv.RobAx.Rax_4, jtv.RobAx.Rax_5, jtv.RobAx.Rax_6 });
                }
            }

            return new Dictionary<string, List<double[]>>
            {
                {"robotTarget", robotTarget },
                {"jointTarget", jointTarget }
            };
        }



        /// <summary>
        /// Set current program pointer on controller.
        /// </summary>
        /// <param name="run">True to run</param>
        /// <param name="controllerData">Controller data</param>
        /// <param name="value"></param>
        public static void setProgramPointer(bool run, string[] controllerData, int value)
        {
            if (run == true)
            {
                Guid systemId = new Guid(controllerData[1]);
                Controller controller = new Controller(systemId);
                controller.Logon(UserInfo.DefaultUser);

                Task newTask = controller.Rapid.GetTask("T_ROB1");
                using (Mastership.Request(controller.Rapid))
                {
                    newTask.SetProgramPointer("MainModule", value);
                }
            }
        }


        /// <summary>
        /// Return row of motion pointer [0] and program pointer [1].
        /// </summary>
        /// <param name="run">True to run.</param>
        /// <param name="controllerData">Controller data</param>
        /// <returns></returns>
        public static List<int> getExecutionStatus(bool run, string[] controllerData)
        {
            List<int> variables = new List<int>();
            if (run == true)
            {
                Guid systemId = new Guid(controllerData[1]);
                Controller controller = new Controller(systemId);
                controller.Logon(UserInfo.DefaultUser);

                Task newTask = controller.Rapid.GetTask("T_ROB1");
                Module newModule = newTask.GetModule("MainModule");

                ProgramPosition mPos = newTask.MotionPointer;
                ProgramPosition pPos = newTask.ProgramPointer;
                int mRow = mPos.Range.Begin.Row;
                int pRow = pPos.Range.Begin.Row;

                variables.Add(mRow);
                variables.Add(pRow);
            }
            return variables;
        }


        /// <summary>
        /// Set current program pointer on controller.
        /// </summary>
        /// <param name="run">True to run</param>
        /// <param name="controllerData">Controller data</param>
        /// <param name="value"></param>
        public static string playFromPointerLoc(bool run, string[] controllerData)
        {
            StartResult startResult = StartResult.Ok;
            if (run == true)
            {
                Guid systemId = new Guid(controllerData[1]);
                Controller controller = new Controller(systemId);
                controller.Logon(UserInfo.DefaultUser);

                
                Task newTask = controller.Rapid.GetTask("T_ROB1");
                using (Mastership.Request(controller.Rapid))
                {
                    if(newTask.ExecutionStatus == TaskExecutionStatus.Stopped || newTask.ExecutionStatus == TaskExecutionStatus.Ready)
                        startResult = newTask.Start();
 
                }

                
            }

            return startResult.ToString();
        }

        /// <summary>
        /// Set current program pointer on controller.
        /// </summary>
        /// <param name="run">True to run</param>
        /// <param name="controllerData">Controller data</param>
        /// <param name="value"></param>
        [MultiReturn(new[] { "robTargets", "jointTargets" })]
        public static Dictionary<string, List<string>> TestTarget(bool run, string[] controllerData)
        {
            List<string> rTargets = new List<string>();
            List<string> jTargets = new List<string>();

            if (run == true)
            {
                Guid systemId = new Guid(controllerData[1]);
                Controller controller = new Controller(systemId);
                controller.Logon(UserInfo.DefaultUser);

                Task newTask = controller.Rapid.GetTask("T_ROB1");

                Module mod = newTask.GetModule("MainModule");


                using (Mastership.Request(controller.Rapid))
                {
                    var tasks = controller.Rapid.GetTasks();
                    RapidSymbolSearchProperties sProp = RapidSymbolSearchProperties.CreateDefault();
                    sProp.Types = SymbolTypes.Data;
                    RapidSymbol[] datas = newTask.GetModule("MainModule").SearchRapidSymbol(sProp);
                    foreach (RapidSymbol rs in datas)
                    {
                        RapidData rd = controller.Rapid.GetTask("T_ROB1").GetModule("MainModule").GetRapidData(rs);
                        if (rd.Value is JointTarget)
                        { jTargets.Add(rd.Value.ToString()); }
                      
                    }
                 
                    
                }
            }

            return new Dictionary<string, List<string>>
            {
                {"robTargets", rTargets },
                {"jointTargets", jTargets }
            };
        }

        /// <summary>
        /// Stop the simulation
        /// </summary>
        /// <param name="run">True to run</param>
        /// <param name="controllerData">Controller data</param>
        /// <param name="value"></param>
        public static void StopSim(bool run, string[] controllerData)
        {
          
            if (run == true)
            {
                Guid systemId = new Guid(controllerData[1]);
                Controller controller = new Controller(systemId);
                controller.Logon(UserInfo.DefaultUser);


                Task newTask = controller.Rapid.GetTask("T_ROB1");
                using (Mastership.Request(controller.Rapid))
                {
                    var execStatus = newTask.ExecutionStatus;
                    if (execStatus == TaskExecutionStatus.Running)
                        newTask.Stop(StopMode.Instruction);
                }
                controller.Logoff();

            }

        }

    }
   
    internal class UtilFuncs
    {
        public enum MoveType
        {
            MoveL, MoveJ, MoveC, MoveLDO, MoveAbsJ
        }

        /// <summary>
        /// Create a movement instruction.
        /// </summary>
        /// <param name="moveType"></param>
        /// <param name="targets">Robot target</param>
        /// <param name="speed">Speed data (rounds to default in RobotStudio)</param>
        /// <param name="zone">Zone data (rounds to default in RobotStudio)</param>
        /// <param name="DOnames">if using a MoveDO command sets the DO val  , true=1</param>
        /// <param name="DOvals">if using a MoveDO command sets the DO val  , true=1</param>
        /// <param name="setName">Unique name for this instruction</param>
        /// <param name="toolName">Active tool</param>
        /// <param name="wobjName">Active work-object</param>
        /// <param name="defautSpeeds">If true it will round the speed values to robot studio default values</param>
        /// <returns></returns>
        internal static bool MoveCommand(
            MoveType moveType,
            List<RobTarget> targets,
            List<object> speed,
            List<int> zone ,
            List<string> DOnames ,
            List<int> DOvals ,
            string setName ,
            string toolName ,
            string wobjName ,
            bool defautSpeeds ,
            out List<string> cnstList ,
            out List<string> instList)

        {
            // setup
            int tCt = targets.Count;
            List<string> m_cnstList = new List<string>(tCt);
            List<string> m_instList = new List<string>(tCt);
            List<int>m_speed = new List<int>(tCt);
            List<string>m_zone = new List<string>(tCt);
            List<string>m_DoName= new List<string>(tCt);
            List<int>m_DoVal = new List<int>(tCt);
            bool isDOmove = false;
            bool isCircMove = false;
            string moveInst = "";
            bool success = true;

            switch (moveType)
            {
                case MoveType.MoveL:
                    moveInst = "MoveL";
                    break;
                case MoveType.MoveJ:
                    moveInst = "MoveJ";
                    break;
                case MoveType.MoveC:
                    moveInst = "MoveC";
                    isCircMove = true;
                    break;
                case MoveType.MoveAbsJ:
                    moveInst = "MoveAbsJ";
                    break;
                case MoveType.MoveLDO:
                    moveInst = "MoveLDO";
                    isDOmove = true;
                    break;
                default:
                    moveInst = "MoveL";
                    break;
            }


            if (speed.Count != tCt && speed.Count != 1) throw new WarningException("The speed count must be equal to the target count or only one value");
            if (zone.Count != tCt && zone.Count != 1) throw new WarningException("The zone count must be equal to the target count or only one value");

            if (isDOmove)
            {
                if (DOnames.Count != tCt && DOnames.Count != 1) throw new WarningException("The DOnames count must be equal to the target count or only one value");
                if (DOvals.Count != tCt && DOvals.Count != 1) throw new WarningException("The DOvals count must be equal to the target count or only one value");
            }


            //speed
            /*
            if(speed.Count >1)
                foreach (var s in speed)
                {

                    if (defautSpeeds) m_speed.Add(RobotUtils.closestSpeed(s));
                    else m_speed.Add(s);
                }
            else 
            {
                if (defautSpeeds) m_speed = Enumerable.Repeat(RobotUtils.closestSpeed(speed[0]), tCt).ToList();
              
            else m_speed = Enumerable.Repeat(speed[0], tCt).ToList();
            }*/
            int sct = 0;
            foreach (var s in speed)
            {
                if (s is int)
                {
                    m_cnstList.Add($"VAR speeddata s_{setName}{sct} := [ {s}, 100, 100, 100 ];");
                }
                else if (s is Speed)
                {
                    var m_s = s as Speed;
                    m_cnstList.Add(m_s.SpeedData);
                }
                else
                {
                    success = false;
                    break;
                }
                
                sct++;
            }

            //zone
            if (m_zone.Count > 1)
                m_zone.AddRange(zone.Select(z => RobotUtils.closestZone(z)));
            else  m_zone = Enumerable.Repeat(RobotUtils.closestZone(zone[0]), tCt).ToList();

  

            //DO Names and Vals
            if (isDOmove)
            {
                if (m_DoName.Count > 1) m_DoName = DOnames;
                else m_DoName = Enumerable.Repeat(DOnames[0], tCt).ToList();

                if (m_DoVal.Count > 1) m_DoVal = DOvals;
                else m_DoVal = Enumerable.Repeat(DOvals[0], tCt).ToList();

                for (int i = 0; i < tCt; i++)
                {
                    m_cnstList.Add($"CONST robtarget {setName}{i}:={targets[i]};");
                    m_instList.Add($"{moveInst} {setName}{i},v{m_speed[i]},{m_zone[i]},{toolName}\\WObj:={wobjName} ,{m_DoName[i]}, {m_DoVal[i]};");
 
                }
            }
            else if (isCircMove)
            {
                for (int i = 0; i < tCt; i++)
                {
                    m_cnstList.Add($"CONST robtarget cir{setName}{i}:={targets[i]}; CONST robtarget to{setName}{i}:={targets[i]};");
                    m_instList.Add($"MoveC cir{setName}{i}, to{setName}{i}, {speed[i]},{zone[i]},{toolName}\\WObj:={wobjName};");
                }
            }
            else
            {
                for (int i = 0; i < tCt; i++)
                {
                    m_cnstList.Add($"CONST robtarget {setName}{i}:={targets[i]};");
                    m_instList.Add($"{moveInst} {setName}{i},v{m_speed[i]},{m_zone[i]},{toolName}\\WObj:={wobjName};");
                }
            }

          

            //end step

            cnstList = m_cnstList;
            instList = m_instList;

            return success;

        }


     /*
        internal static MeshBody MeshToMeshBody(Mesh DMesh)
        {
            
            List<MeshFace> faces = new List<MeshFace>(DMesh.FaceIndices.Length);
            List<ABBMath.Vector3> verts;
            foreach (var fi in DMesh.FaceIndices)
            {
                var face = new MeshFace();
                verts = new List<ABBMath.Vector3>((int)fi.Count);
                if (fi.Count == 3)
                {
                    face.WireIndices.AddRange(new List<int>(3) {(int) fi.A, (int) fi.B, (int) fi.C});
                    verts.AddRange(new List<Vector3>(3)
                    {
                        new Vector3(DMesh.VertexPositions[fi.A].X, DMesh.VertexPositions[fi.A].Y,
                            DMesh.VertexPositions[fi.A].Z),
                        new Vector3(DMesh.VertexPositions[fi.B].X, DMesh.VertexPositions[fi.B].Y,
                            DMesh.VertexPositions[fi.B].Z),
                        new Vector3(DMesh.VertexPositions[fi.C].X, DMesh.VertexPositions[fi.C].Y,
                            DMesh.VertexPositions[fi.C].Z)
                    });
                }
                else
                {
                    face.WireIndices.AddRange(new List<int>(4) {(int) fi.A, (int) fi.B, (int) fi.C, (int) fi.D});
                    verts.AddRange(new List<Vector3>(4)
                    {
                        new Vector3(DMesh.VertexPositions[fi.A].X, DMesh.VertexPositions[fi.A].Y,
                            DMesh.VertexPositions[fi.A].Z),
                        new Vector3(DMesh.VertexPositions[fi.B].X, DMesh.VertexPositions[fi.B].Y,
                            DMesh.VertexPositions[fi.B].Z),
                        new Vector3(DMesh.VertexPositions[fi.C].X, DMesh.VertexPositions[fi.C].Y,
                            DMesh.VertexPositions[fi.C].Z),
                            new Vector3(DMesh.VertexPositions[fi.D].X, DMesh.VertexPositions[fi.D].Y,
                            DMesh.VertexPositions[fi.D].Z)
                    });
                }




                faces.Add(face);
            }

            return new RSS.MeshBody(faces);
            
        }
      */
    }

}
















