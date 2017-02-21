using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.RapidDomain;
using ABB.Robotics.RobotStudio;
using ABB.Robotics.RobotStudio.Stations;
using ABB.Robotics.RobotStudio.Stations.Forms;
using  ABB.Robotics.Math;


using DGeom = Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Dynamo.Controls;
using Dynamo.Graph;
using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Dynamo.Wpf;

using ProtoCore.AST.AssociativeAST;
using Dynamo.Events;

using Microsoft.Win32;
using RobotStudio.API.Internal;
using LogMessage = Dynamo.Logging.LogMessage;
using String = System.String;
using ABBTask = ABB.Robotics.Controllers.RapidDomain.Task;

namespace Dynamo_TORO.SimFunctions
{
  
 
    internal class VirtualStation
    {
        public Station tStation;
        public RsTask rsTask;
        public Mechanism robot;
        private RsWorkObject rsWobj = null;
        private Project activeProject;
        private RsIrc5Controller controller;

        [IsVisibleInDynamoLibrary(false)]
        public VirtualStation(string stationFile)
        {
            try
            {
                if(!RobotStudioAPI.Initialized)
                    RobotStudioAPI.Initialize();

                activeProject = Station.Load(stationFile, true);
                tStation = activeProject as Station;
                
                rsTask = tStation.ActiveTask;
                robot = rsTask.Mechanism;
                rsWobj = rsTask.ActiveWorkObject;

                tStation.Irc5Controllers[0].StartAsync(VirtualControllerRestartMode.IStart,
                    new List<Mechanism>(1) {robot});

               

            }
            catch (Exception e)
            {
                
                 
            }

   

        }

        [IsVisibleInDynamoLibrary(false)]
        public bool isTargetReachable(DGeom.Plane targetPlane)
        {
            var target = CreateTarget(targetPlane);
            var isOk = robot.Task.Mechanism.CanReachAsync(target.RobTarget, target.WorkObject, rsTask.ActiveTool);
            isOk.Wait();

            return isOk.Result;
        }


        [IsVisibleInDynamoLibrary(false)]
        public async Task<double[]> CalulateIK(DGeom.Plane targetPlane)
        {
                double[] jointRot = new double[6];
                RsTarget rst = CreateTarget(targetPlane);
            try
            {
                //robot.CalculateInverseKinematicsAsync(rst, rsTask.ActiveTool, true);
                var jData = await robot.Task.Mechanism.CalculateInverseKinematicsAsync(rst, rsTask.ActiveTool, false);
                return jData;

            }
            catch (ABB.Robotics.BaseException exception)
            {
                throw;
            }
            catch (Exception exception)
            {

                throw;
            }
            
            return jointRot;



        }

        private void CreateRsWorkobject(string wobjName = "myWobj")
        {
       
            
            try
            {
  
                #region RsWorkObjectPoint1
                // Create a RsWorkObject.
                RsWorkObject newWobj = new RsWorkObject();

                // Get a valid RAPID name for the workobject and assign it.
                newWobj.Name = rsTask.GetValidRapidName(wobjName, "_", 1);
                #endregion

                // Set the frame size to twice the default size.
                newWobj.FrameSize = newWobj.FrameSize * 2;

                // Set the userframe to be a fixed coordinate system.
                newWobj.UserFrameProgrammed = true;

                // Translate the user frame.
                newWobj.UserFrame.X = 0.5;
                newWobj.UserFrame.Y = 0.2;
                newWobj.UserFrame.Z = 0.2;

                #region RsWorkobjectPoint2
                // Translate the object frame.
                newWobj.ObjectFrame.X = 0.5;
                newWobj.ObjectFrame.Y = 0.5;
                newWobj.ObjectFrame.Z = 0.5;

                // Rotate the object frame (pi radians around each axis).
                newWobj.ObjectFrame.RX = Math.PI;
                newWobj.ObjectFrame.RY = Math.PI;
                newWobj.ObjectFrame.RZ = Math.PI;
                #endregion

                // Display the wobj in the graphics.
                newWobj.Visible = true;

                // Display the name of the wobj in the graphics.
                newWobj.ShowName = true;

                // The wobj is not held by the robot.
                newWobj.RobotHold = false;

                rsWobj = newWobj;
                #region RsWorkobjectPoint3
                // Add the wobj to the DataDeclarations of the ActiveTask.
                rsTask.DataDeclarations.Add(newWobj);

                // Set the wobj as the active workobject of the ActiveTask.
                rsTask.ActiveWorkObject =
                    (RsWorkObject)rsTask.FindDataDeclarationFromModuleScope(newWobj.Name, newWobj.ModuleName);
                #endregion
            }
            catch
            {

            }


        }

        [IsVisibleInDynamoLibrary(false)]
        public RsTarget CreateTarget(DGeom.Plane targetPlane)
        {
            try
            {
                //create robtarget
                RsRobTarget robTarget = new RsRobTarget();
                robTarget.Name = rsTask.GetValidRapidName("Target", "_", 1);

                //translation
                double[] trans = PlaneToTransform(targetPlane);
                robTarget.Frame.Translation = new Vector3(trans[0],trans[1],trans[2]);
                robTarget.Frame.RX = trans[3];
                robTarget.Frame.RY = trans[4];
                robTarget.Frame.RZ = trans[5];

                //add robtargets to datadeclaration
               // rsTask.DataDeclarations.Add(robTarget);

                //create target
                RsTarget target = new RsTarget(rsWobj, robTarget);
                target.Name = robTarget.Name;
                target.Attributes.Add(target.Name, true);
              
                //add targets to active task
                //rsTask.Targets.Add(target);

                return target;
            }
            catch (Exception exception)
            {
                
            }
            return null;
        }

        private double[] PlaneToTransform(DGeom.Plane pl)
        {
            double[] result = new double[6];
            //position
            result[0] = pl.Origin.X;
            result[1] = pl.Origin.Y;
            result[2] = pl.Origin.Z;

            //rotation
            result[3] = (DGeom.Vector.XAxis().AngleWithVector(pl.XAxis) * Math.PI)/180;
            result[3] = (DGeom.Vector.YAxis().AngleWithVector(pl.YAxis) * Math.PI) / 180;
            result[3] = (DGeom.Vector.ZAxis().AngleWithVector(pl.Normal) * Math.PI) / 180;

            return result;

        }

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

    }
}
