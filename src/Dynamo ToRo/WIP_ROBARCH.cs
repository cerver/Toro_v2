using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using WireFrameToRobot;

namespace Dynamo_TORO
{
    /// <summary>
    /// Container for RobArch workflow
    /// </summary>
    public class WIP_ROBARCH
    {



        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////



        /// <summary>
        /// Create geometry representation of tool.
        /// </summary>
        /// <param name="frame">Tool frame at drill tip</param>
        /// <returns></returns>
        private static Solid vis_tool(Plane frame)
        {
            List<Solid> model = new List<Solid>();
            CoordinateSystem cs = CoordinateSystem.ByPlane(frame);
            Vector x = frame.XAxis;
            Vector y = frame.YAxis;
            Vector z = frame.Normal.Reverse();
            double h = frame.Origin.Z;

            Solid bit0 = Cone.ByCoordinateSystemHeightRadii(cs.Translate(z, 2), 2, 3, 0.1);
            Solid bit1 = Cone.ByCoordinateSystemHeightRadii(cs.Translate(z, 68 + 2), 68, 3, 3);
            Solid bit2 = Cone.ByCoordinateSystemHeightRadii(cs.Translate(z, 5 + 68 + 2), 5, 5, 5);
            Solid bit3 = Cone.ByCoordinateSystemHeightRadii(cs.Translate(z, 5 + 5 + 68 + 2), 5, 4, 4);
            Solid bod0 = Cone.ByCoordinateSystemHeightRadii(cs.Translate(z, 3 + 5 + 5 + 68 + 2), 3, 12, 12);
            Solid bod1 = Cone.ByCoordinateSystemHeightRadii(cs.Translate(z, 80 + 3 + 5 + 5 + 68 + 2), 80, 24, 24);
            Solid bod2 = Cone.ByCoordinateSystemHeightRadii(cs.Translate(z, 80 + 80 + 3 + 5 + 5 + 68 + 2), 80, 34, 34);
            Solid bod3 = Cuboid.ByLengths(cs.Translate(z, 40 + 80 + 3 + 5 + 5 + 68 + 2).Translate(y, 20), 40, 40, 40);
            Solid leg0 = Cone.ByCoordinateSystemHeightRadii(cs.Translate(z, h).Translate(x, 40).Translate(y, 40), h - (80 + 3 + 5 + 5 + 68 + 2), 4, 4);
            Solid leg1 = Cone.ByCoordinateSystemHeightRadii(cs.Translate(z, h).Translate(x, 40).Translate(y, -40), h - (80 + 3 + 5 + 5 + 68 + 2), 4, 4);
            Solid leg2 = Cone.ByCoordinateSystemHeightRadii(cs.Translate(z, h).Translate(x, -40).Translate(y, -40), h - (80 + 3 + 5 + 5 + 68 + 2), 4, 4);
            Solid leg3 = Cone.ByCoordinateSystemHeightRadii(cs.Translate(z, h).Translate(x, -40).Translate(y, 40), h - (80 + 3 + 5 + 5 + 68 + 2), 4, 4);
            Solid box0 = Cuboid.ByLengths(cs.Translate(z, 3 + 80 + 3 + 5 + 5 + 68 + 2), 96, 96, 10);
            Solid box1 = Cuboid.ByLengths(cs.Translate(z, h - 5), 96, 96, 10);
            
            model.Add(bit0);
            model.Add(bit1);
            model.Add(bit2);
            model.Add(bit3);
            model.Add(bod0);
            model.Add(bod1);
            model.Add(bod2);
            model.Add(bod3);
            model.Add(leg0);
            model.Add(leg1);
            model.Add(leg2);
            model.Add(leg3);
            model.Add(box0);
            model.Add(box1);

            Solid solid = Solid.ByUnion(model);
            return solid;
        }



        /// <summary>
        /// Create geometry representation of wobj.
        /// </summary>
        /// <param name="frame">Wobj frame at cube centroid</param>
        /// <returns></returns>
        private static Solid vis_wobj(Plane frame)
        {
            List<Solid> model = new List<Solid>();
            CoordinateSystem cs = CoordinateSystem.ByPlane(frame);
            Vector z = frame.Normal.Reverse();
            double h = frame.Origin.Z;

            //Solid cube = Cuboid.ByLengths(cs, 40, 40, 40);
            Solid chu0 = Cylinder.ByCoordinateSystemHeightRadii(cs.Translate(cs.ZAxis.Reverse(), 2 + 20), 2, 10, 10);
            Solid bod0 = Cylinder.ByCoordinateSystemHeightRadii(cs.Translate(cs.ZAxis.Reverse(), 18 + 2 + 20), 18, 22, 22);
            Solid bod1 = Cuboid.ByLengths(cs.Translate(z, h - 18 - (h - 18 - 18 - 2 - 20) / 2), 46, 46, h - 18 - 18 - 2 - 20);
            Solid bod2 = Cylinder.ByCoordinateSystemHeightRadii(cs.Translate(z, h), 18, 22, 22);

            //model.Add(cube);
            model.Add(chu0);
            model.Add(bod0);
            model.Add(bod1);
            model.Add(bod2);

            Solid solid = Solid.ByUnion(model);
            return solid;
        }




        /*
        /// <summary>
        /// Create geometry representation of targets using wobj and tool.
        /// </summary>
        /// <param name="holeFrames">List of target frames</param>
        /// <param name="blockFrame">Wobj frame</param>
        /// <param name="drillFrame">Tool frame</param>
        /// <param name="nodeIndex">Index of node</param>
        /// <param name="poseIndex">Index of pose</param>
        /// <returns></returns>
        [MultiReturn(new[] { "tool", "wobj", "block", "holes" })]
        public static Dictionary<string, List<Object>> vis_transform2(List<List<Plane>> holeFrames, Plane blockFrame, Plane drillFrame, int nodeIndex = 0, int poseIndex = 0)
        {

            // setup
            List<Object> outputTool = new List<Object>();
            List<Object> outputWobj = new List<Object>();
            List<Object> outputBloc = new List<Object>();
            List<Object> outputCSysHoles = new List<Object>();

            // create wobj and tool solids
            Solid wobj = vis_wobj(blockFrame);
            Solid tool = vis_tool(drillFrame);
            CoordinateSystem worldCS = CoordinateSystem.ByOrigin(0, 0, 0);
            CoordinateSystem toolCS = CoordinateSystem.ByPlane(drillFrame);

            // transform and translate targets on wobj to tool
            List<Object> holeCSList = new List<Object>();
            foreach (Plane hole in holeFrames[nodeIndex])
            {
                CoordinateSystem holeCS = CoordinateSystem.ByPlane(hole);
                CoordinateSystem holeCSTransformed = holeCS.Transform(worldCS, toolCS);
                CoordinateSystem holeCSTransformedTranslated = holeCSTransformed.Translate(holeCSTransformed.ZAxis.Reverse(), 10);
                holeCSList.Add((Object) holeCSTransformedTranslated);
            }

            // transform wobj to target in tool space
            CoordinateSystem wobjCS = CoordinateSystem.ByPlane(blockFrame);
            CoordinateSystem wobjCSTransformed = wobjCS.Transform(wobjCS, (CoordinateSystem) holeCSList[poseIndex]);
            Geometry wobjTransformed = wobj.Transform(wobjCS, wobjCSTransformed);
            
            // create wireframe model of block
            Edge[] blocTransformed = (Cuboid.ByLengths(wobjCSTransformed, 40, 40, 40).Edges);
            foreach (Edge edge in blocTransformed) { outputBloc.Add(edge.CurveGeometry); }

            // output
            outputTool.Add((Object)tool);
            outputWobj.Add((Object)wobjTransformed);
            return new Dictionary<string, List<Object>>
            {
                { "tool", outputTool },
                { "wobj", outputWobj },
                { "block", outputBloc },
                { "holes", holeCSList }
            };
        }
        */



        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////






        /// <summary>
        /// Sort planes by directionality about average pole and shift.
        /// </summary>
        /// <param name="planeList">List of planes</param>
        /// <param name="shift">Shift value</param>
        /// <returns></returns>
        private static List<Plane> sortPolar3_Plane(List<Plane> planeList, int shift = 0)
        {
            List<Plane> newList = new List<Plane>();
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

            List<Plane> shifted = new List<Plane>();
            if (shift < 0) { shift = newList.Count - shift % newList.Count - 1; }
            if (Math.Abs(shift) >= newList.Count) { shift = shift % newList.Count; }
            shifted = newList.GetRange(shift, newList.Count - shift);
            shifted.AddRange(newList.GetRange(0, shift));

            return shifted;
        }



        /// <summary>
        /// Sort planes by directionality about average pole and shift.
        /// </summary>
        /// <param name="planeList">List of planes</param>
        /// <param name="shift">Shift value</param>
        /// <returns></returns>
        private static List<int> sortPolar3_Index(List<Plane> planeList, int shift = 0)
        {
            List<Plane> newList = new List<Plane>();
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

            List<int> indices = sortedParams.Select(x => (int)x.Value).ToList();
            foreach (int i in indices) { newList.Add(planeList[i]); }

            List<Plane> shifted = new List<Plane>();
            if (shift < 0) { shift = newList.Count - shift % newList.Count - 1; }
            if (Math.Abs(shift) >= newList.Count) { shift = shift % newList.Count; }
            shifted = newList.GetRange(shift, newList.Count - shift);
            shifted.AddRange(newList.GetRange(0, shift));

            return indices;
        }



        /// <summary>
        /// Test strut normals of nodes against guide vector using angular tolerance.
        /// </summary>
        /// <param name="nodeList">List of nodes to test</param>
        /// <param name="guide">Guide vector (Default: World +ZAxis)</param>
        /// <param name="tolerance">Angular tolerance (degrees)</param>
        /// <returns></returns>
        [MultiReturn(new[] { "passedNodes", "failedNodes", "failedStruts" })]
        public static Dictionary<string, Object> testAngular1_Node(List<Node> nodeList, [DefaultArgumentAttribute("{Vector.ByCoordinates(0,0,1)}")] Vector guide, double tolerance = 120)
        {
            List<Node> passedNodes = new List<Node>();
            List<Node> failedNodes = new List<Node>();
            List<List<Strut>> passedStrutsList = new List<List<Strut>>();
            List<List<Strut>> failedStrutsList = new List<List<Strut>>();

            foreach (Node node in nodeList)
            {
                List<Strut> passedStruts = new List<Strut>();
                List<Strut> failedStruts = new List<Strut>();
                foreach (Strut s in node.Struts)
                {
                    Plane p = s.CutPlaneAtOrigin;
                    double dot = p.Normal.Dot(guide);
                    double angle = Math.Acos(dot) * (180 / Math.PI);
                    if (angle > tolerance)
                    {
                        failedStruts.Add(s);
                    }
                    else
                    {
                        passedStruts.Add(s);
                    }
                }


                if (failedStruts.Count() > 0)
                {
                    failedNodes.Add(node);
                    failedStrutsList.Add(failedStruts);
                }
                else
                {
                    passedNodes.Add(node);
                    passedStrutsList.Add(passedStruts);
                }
            }

            return new Dictionary<string, Object>
            {
                {"passedNodes", passedNodes },
                {"failedNodes", failedNodes },
                {"failedStruts", failedStrutsList}
            };
        }




        /// <summary>
        /// Test strut normals of node against each other using angular tolerance.
        /// </summary>
        /// <param name="nodeList">List of nodes to test</param>
        /// <param name="tolerance">Angular tolerance (degrees)</param>
        /// <returns></returns>
        [MultiReturn(new[] { "passedNodes", "failedNodes", "failedStruts" })]
        public static Dictionary<string, Object> testAngular2_Node(List<Node> nodeList, double tolerance = 35)
        {
            List<Node> passedNodes = new List<Node>();
            List<Node> failedNodes = new List<Node>();
            List<List<Strut>> passedStrutsList = new List<List<Strut>>();
            List<List<Strut>> failedStrutsList = new List<List<Strut>>();

            foreach (Node node in nodeList)
            {
                List<Strut> passedStruts = new List<Strut>();
                List<Strut> failedStruts = new List<Strut>();

                List<Strut> strutList = node.Struts;
                List<Vector> normals = strutList.Select(p => p.CutPlaneAtOrigin.Normal).ToList();

                for (int i = 0; i < strutList.Count(); i++)
                {
                    for (int j = 0; j < strutList.Count(); j++)
                    {
                        if (normals[i] != normals[j])
                        {
                            double dot = normals[i].Dot(normals[j]);
                            double angle = Math.Acos(dot) * (180 / Math.PI);

                            if (angle < tolerance && !failedStruts.Contains(strutList[i]))
                            {
                                failedStruts.Add(strutList[i]);
                            }
                        }
                    }
                }

                passedStruts = strutList.Except(failedStruts).ToList();

                if (failedStruts.Count > 0)
                {
                    failedNodes.Add(node);
                    failedStrutsList.Add(failedStruts);
                }
                else
                {
                    passedNodes.Add(node);
                }
            }

            return new Dictionary<string, Object>
            {
                {"passedNodes", passedNodes },
                {"failedNodes", failedNodes },
                {"failedStruts", failedStrutsList}
            };
        }



        /*
        /// <summary>
        /// Aligns XAxis of strut frame on node to guide vector (default: World XAxis)
        /// </summary>
        /// <param name="nodeList">List of nodes</param>
        /// <param name="vec">Alignment vector for XAxes</param>
        /// <param name="degree">Angle multiplier</param>
        /// <returns></returns>
        public static List<Node> alignByXAxis_Node(List<Node> nodeList, [DefaultArgumentAttribute("{Vector.ByCoordinates(1,0,0)}")] Vector vec, double degree = 1)
        {
            //List<Node> newNodeList = new List<Node>();

            foreach (Node node in nodeList)
            {
                List<Line> newLineList = new List<Line>();
                foreach (Strut strut in node.Struts)
                {
                    Plane p = strut.TransformedCutPlane;
                    Vector projectedVector = Vector.ByCoordinates(p.XAxis.X, p.XAxis.Y, 0);
                    double dot = p.XAxis.Dot(projectedVector);
                    double angle = Math.Acos(dot) * (-1) * degree * (180 / Math.PI);

                    // rebuild node
                    Line newLine = (Line) strut.LineRepresentation.Rotate(p.Origin, p.Normal, angle);
                    newLineList.Add(newLine);
                }

                Node newNode = Node.ByPointsLinesAndGeoOrientationStrategy(node.Center, newLineList, 6, node.NodeGeometry);
                newNodeList.Add(newNode);
            }

            return nodeList;
        }
        */



        /// <summary>
        /// Reverse normal and retain rotation of plane.
        /// </summary>
        /// <param name="plane">Plane to flip</param>
        /// <param name="retain">Retain X-Axis?</param>
        /// <returns></returns>
        private static Plane flip_Plane2(Plane plane, bool retain = true)
        {
            switch (retain)
            {
                case true:
                    {
                        Plane newPl = Plane.ByOriginNormalXAxis(plane.Origin, plane.Normal.Reverse(), plane.XAxis);
                        plane = newPl;
                        break;
                    }
                case false:
                    {
                        Plane newPl = Plane.ByOriginNormalXAxis(plane.Origin, plane.Normal.Reverse(), plane.XAxis.Reverse());
                        plane = newPl;
                        break;
                    }
            }
            return plane;
        }





        /// <summary>
        /// For each point get list of and reorient all lines and get list of planes.
        /// </summary>
        /// <param name="lines">List of lines</param>
        /// <param name="points">List of points</param>
        /// <returns></returns>
        [MultiReturn(new[] { "lineList", "planeList" })]
        public static Dictionary<string, List<List<Object>>> getLinesPlaneAtPoint(List<Line> lines, List<Point> points)
        {
            List<List<Object>> lineListList = new List<List<Object>>();
            List<List<Object>> planeListList = new List<List<Object>>();
            List<Point> errPoints = new List<Point>();

            foreach (Point p in points)
            {
                List<Object> lineList = new List<Object>();
                List<Object> planeList = new List<Object>();

                List<bool> boolList = new List<bool>();

                foreach (Line l in lines)
                {
                    bool intersection = p.DoesIntersect(l);
                    if (intersection)
                    {
                        bool meets = l.StartPoint.IsAlmostEqualTo(p);
                        if (meets)
                        {
                            lineList.Add(l);
                            Plane pl = l.PlaneAtParameter(0);
                            planeList.Add((Object)pl);
                        }
                        else
                        {
                            Line lRev = (Line) l.Reverse();
                            lineList.Add((Object) lRev);
                            Plane pl = lRev.PlaneAtParameter(0);
                            planeList.Add((Object) pl);
                        }
                    }
                }

                lineListList.Add(lineList);
                planeListList.Add(planeList);
            }

            return new Dictionary<string, List<List<Object>>>
            {
                {"lineList", lineListList},
                {"planeList", planeListList}
            };
        }



        /// <summary>
        /// For each point get list of and reorient all lines and get list of planes.
        /// </summary>
        /// <param name="lines">List of lines</param>
        /// <param name="points">List of points</param>
        /// <returns></returns>
        [MultiReturn(new[] { "lineList", "planeList" })]
        public static Dictionary<string, List<List<Object>>> getLinesCoordSysAtPoint(List<Line> lines, List<Point> points)
        {
            List<List<Object>> lineListList = new List<List<Object>>();
            List<List<Object>> CSListList = new List<List<Object>>();
            List<Point> errPoints = new List<Point>();

            foreach (Point p in points)
            {
                List<Object> lineList = new List<Object>();
                List<Object> csList = new List<Object>();

                List<bool> boolList = new List<bool>();

                foreach (Line l in lines)
                {
                    bool intersection = p.DoesIntersect(l);
                    if (intersection)
                    {
                        bool meets = l.StartPoint.IsAlmostEqualTo(p);
                        if (meets)
                        {
                            lineList.Add(l);
                            Plane pl = l.PlaneAtParameter(0);
                            CoordinateSystem cs = CoordinateSystem.ByPlane(pl);
                            csList.Add((Object)cs);
                        }
                        else
                        {
                            Line lRev = (Line)l.Reverse();
                            lineList.Add((Object)lRev);
                            Plane pl = lRev.PlaneAtParameter(0);
                            CoordinateSystem cs = CoordinateSystem.ByPlane(pl);
                            csList.Add((Object)cs);
                        }
                    }
                }

                lineListList.Add(lineList);
                CSListList.Add(csList);
            }

            return new Dictionary<string, List<List<Object>>>
            {
                {"lineList", lineListList},
                {"planeList", CSListList}
            };
        }




        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////



        private static string ToolRobHold(Plane p, bool r)
        {
            List<double> q = Utilities.QuatListAtPlane(p);
            Point o = p.Origin;
            string rB = r.ToString().ToUpper();
            string t = "[" + rB + ", [[" + o.X + "," + o.Y + "," + o.Z + "], [" + q[0] + "," + q[1] + "," + q[2] + "," + q[3] + "]], [1,[0,0,0.001],[1,0,0,0],0,0,0]]";
            return t;
        }

        private static string WobjRobHold(Plane p, bool r)
        {
            List<double> q = Utilities.QuatListAtPlane(p);
            Point o = p.Origin;
            string rB = r.ToString().ToUpper();
            string w = "[" + rB + ", " + rB + ", \"\", [[" + o.X + "," + o.Y + "," + o.Z + "], [" + q[0] + "," + q[1] + "," + q[2] + "," + q[3] + "]], [[0,0,0], [1,0,0,0]]]";
            return w;
        }

        private static string rtarget(Plane p)
        {
            List<double> quatDoubles = RobotUtils.PlaneToQuaternian(p);
            string target = string.Format(
                    "[[{0},{1},{2}], [{3},{4},{5},{6}], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]]",
                    p.Origin.X, p.Origin.Y, p.Origin.Z, quatDoubles[0], quatDoubles[1], quatDoubles[2], quatDoubles[3]);
            return target;
        }

        private static string jtarget(double j0, double j1, double j2, double j3, double j4, double j5)
        {
            string target = string.Format(
                "[[{0},{1},{2},{3},{4},{5}], [9E9,9E9,9E9,9E9,9E9,9E9]]", j0, j1, j2, j3, j4, j5);
            return target;
        }



        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////


            
        /// <summary>
        /// Create routine for an ABB robot with stationary drill and mobile workpiece.
        /// </summary>
        /// <param name="directory">Directory to write files ("C:\")</param>
        /// <param name="nodes">A list of nodes</param>
        /// <returns>filePaths</returns>
        public static List<string> createDrillRoutine_Nodes(string directory, List<Node> nodes)
        {
            // create list of filenames
            List<string> outputFiles = new List<string>();

            // begin main
            foreach (Node node in nodes)
            {
                // name files
                string filename = string.Format("{0}\\{1}.prg", directory, node.ID.ToString());
                outputFiles.Add(filename);

                // setup  sub
                var targBuilder = new StringBuilder();
                var moveBuilder = new StringBuilder();

                // sort planes
                List<Plane> strutPlanes = node.Struts.Select(s => s.AlignedCutPlaneAtOrigin(Vector.ByCoordinates(0, 1, 0))).ToList();

                List<Plane> sortedPlanes = sortPolar3_Plane(strutPlanes, 0);
                List<int> sortedIndices = sortPolar3_Index(strutPlanes, 0);

                for (int index = 0; index < strutPlanes.Count; index++)
                {
                    // get this plane
                    Plane plane = sortedPlanes[index];
                    string identity = node.Struts[sortedIndices[index]].ID;

                    // perform correction
                    Plane flippedPlane = flip_Plane2(plane, true);
                    Plane hole = Plane.ByOriginNormalXAxis(flippedPlane.Origin, flippedPlane.Normal, flippedPlane.YAxis);

                    if (Math.Abs(hole.Normal.X) <= Math.Abs(hole.Normal.Y) && hole.Normal.Y >= 0)
                    {
                        hole = (Plane) hole.Rotate(hole, -90);
                    }

                    if (Math.Abs(hole.Normal.X) < Math.Abs(hole.Normal.Y) && hole.Normal.Y < 0)
                    {
                        hole = (Plane) hole.Rotate(hole, 90);
                    }

                    if ((Math.Acos(Math.Abs(Vector.ByCoordinates(-1, 0, 0).Dot(hole.Normal))) < 45) || (Math.Acos(Math.Abs(Vector.ByCoordinates(0, -1, 0).Dot(hole.Normal))) <= 45))
                    {
                        hole = (Plane)hole.Rotate(hole.Origin, hole.Normal, 180);
                    }


                    // create targets
                    targBuilder.Append(string.Format("\n"));
                    targBuilder.Append(string.Format("\n\t! {0};", identity));
                    targBuilder.Append(string.Format("\n\tVAR robtarget S{0}0 := {1};", index, rtarget((Plane)(hole.Translate(hole.Normal, -120)))));
                    targBuilder.Append(string.Format("\n\tVAR robtarget S{0}1 := {1};", index, rtarget((Plane)(hole.Translate(hole.Normal, -50)))));
                    targBuilder.Append(string.Format("\n\tVAR robtarget S{0}2 := {1};", index, rtarget((Plane)(hole.Translate(hole.Normal, -10)))));

                    // create movement instructions
                    moveBuilder.Append(string.Format("\n"));
                    moveBuilder.Append(string.Format("\n\t\tTPWrite(\"Drilling... {0}, {1} of {2}.\");", identity, index + 1, node.Struts.Count()));
                    moveBuilder.Append(string.Format("\n\t\tMoveL S{0}0, {1}, {2}, drill\\WObj:=block;", index, "v200", "z30"));
                    moveBuilder.Append(string.Format("\n\t\tMoveL S{0}1, {1}, {2}, drill\\WObj:=block;", index, "v100", "z5"));
                    moveBuilder.Append(string.Format("\n\t\tMoveL S{0}2, {1}, {2}, drill\\WObj:=block;", index, "rate", "fine"));
                    moveBuilder.Append(string.Format("\n\t\tMoveL S{0}1, {1}, {2}, drill\\WObj:=block;", index, "rate", "fine"));
                    moveBuilder.Append(string.Format("\n\t\tMoveL S{0}0, {1}, {2}, drill\\WObj:=block;", index, "v100", "z5"));
                    //moveBuilder.Append(string.Format("\n\t\tMoveL RelTool(S{0}0, 0, 50, 0), {1}, {2}, drill\\WObj:=block;", index, "v200", "z30"));

                    // create safe movement to next
                    if (index < node.Struts.Count() - 1)
                    {
                        //moveBuilder.Append(string.Format("\n\t\tMoveAbsJ CalcJointT(RelTool(S{0}0, 0, 50, 0), drill\\WObj:=block), {1}, {2}, drill\\WObj:=block;", index + 1, "v200", "z5"));
                        moveBuilder.Append(string.Format("\n\t\tMoveAbsJ CalcJointT(S{0}0, drill\\WObj:=block), {1}, {2}, drill\\WObj:=block;", index + 1, "v200", "z5"));
                    }
                }


                // create rapid
                string r = "";
                using (var tw = new StreamWriter(filename, false))
                {
                    r =
                        "MODULE MainModule" +
                        "\n" +
                        "\n\t" + "! " + node.ID.ToString() + "\n" + targBuilder.ToString() +
                        "\n" +
                        "\n" +
                        "\n\t" + "! ROUTINE" +
                        "\n\t" + "PROC main()" +
                        "\n\t\t" + "ConfL\\Off;" +
                        "\n\t\t" + "SingArea\\Wrist;" +
                        "\n" +
                        "\n\t\t" + "TPErase;" +
                        "\n\t\t" + "TPWrite(\"This is " + node.ID.ToString() + "\");" +
                        "\n" +
                        "\n\t\t" + "TPWrite(\"Check block && drill!\");" +
                        "\n\t\t" + "MoveAbsJ j0, v200, z5, tool0;" +
                        "\n\t\t" + "MoveAbsJ j1, v200, z5, tool0;" + moveBuilder.ToString() +
                        "\n" +
                        "\n\t\t" + "TPWrite(\"Returning to start...\");" +
                        "\n\t\t" + "MoveAbsJ j1, v200, z5, tool0;" +
                        "\n\t\t" + "MoveAbsJ j0, v200, z5, tool0;" +
                        "\n" +
                        "\n\t\t" + "TPWrite(\"Node " + node.ID.ToString() + " complete!\");" +
                        "\n" +
                        "\n\t\t" + "Stop;" +
                        "\n\t" + "ENDPROC" +
                        "\n" +
                        "\n" + "ENDMODULE"
                        ;

                    tw.Write(r);
                    tw.Flush();
                }
            }

            // end step
            return outputFiles;
        }



        /// <summary>
        /// Create routine for an ABB robot with stationary drill and mobile workpiece.
        /// </summary>
        /// <param name="directory">Directory to write files ("C:\")</param>
        /// <param name="planes">A list of planes to drill</param>
        /// <returns>filePaths</returns>
        public static List<string> createDrillRoutine_Planes(string directory, List<Plane> planes)
        {
            // create list of filenames
            List<string> outputFiles = new List<string>();

            // name files
            string filename = string.Format("{0}\\test.prg", directory);
            outputFiles.Add(filename);

            // setup  sub
            int index = 0;
            var targBuilder = new StringBuilder();
            var moveBuilder = new StringBuilder();

            // sort planes
            List<Plane> newPlanes = sortPolar3_Plane(planes, 0);

            // setup target
            foreach (Plane plane in newPlanes)
            {
                // perform correction
                Plane flippedPlane = flip_Plane2(plane, true);
                Plane hole = Plane.ByOriginNormalXAxis(flippedPlane.Origin, flippedPlane.Normal, flippedPlane.YAxis);

                if (Math.Abs(hole.Normal.X) <= Math.Abs(hole.Normal.Y) && hole.Normal.Y >= 0)
                {
                    hole = (Plane)hole.Rotate(hole, -90);
                }

                if (Math.Abs(hole.Normal.X) < Math.Abs(hole.Normal.Y) && hole.Normal.Y < 0)
                {
                    hole = (Plane)hole.Rotate(hole, 90);
                }

                if ((Math.Acos(Math.Abs(Vector.ByCoordinates(-1, 0, 0).Dot(hole.Normal))) < 45) || (Math.Acos(Math.Abs(Vector.ByCoordinates(0, -1, 0).Dot(hole.Normal))) <= 45))
                {
                    hole = (Plane)hole.Rotate(hole.Origin, hole.Normal, 180);
                }

                // create targets
                targBuilder.Append(string.Format("\n"));
                targBuilder.Append(string.Format("\n\t! {0};", index));
                targBuilder.Append(string.Format("\n\tVAR robtarget S{0}0 := {1};", index, rtarget((Plane)(hole.Translate(hole.Normal, -120)))));
                targBuilder.Append(string.Format("\n\tVAR robtarget S{0}1 := {1};", index, rtarget((Plane)(hole.Translate(hole.Normal, -50)))));
                targBuilder.Append(string.Format("\n\tVAR robtarget S{0}2 := {1};", index, rtarget((Plane)(hole.Translate(hole.Normal, -10)))));

                // create movement instructions
                moveBuilder.Append(string.Format("\n"));
                moveBuilder.Append(string.Format("\n\t\tTPWrite(\"Drilling... {0}, {1} of {2}.\");", index, index + 1, index));
                moveBuilder.Append(string.Format("\n\t\tMoveL S{0}0, {1}, {2}, drill\\WObj:=block;", index, "v200", "z30"));
                moveBuilder.Append(string.Format("\n\t\tMoveL S{0}1, {1}, {2}, drill\\WObj:=block;", index, "v100", "z5"));
                moveBuilder.Append(string.Format("\n\t\tMoveL S{0}2, {1}, {2}, drill\\WObj:=block;", index, "rate", "fine"));
                moveBuilder.Append(string.Format("\n\t\tMoveL S{0}1, {1}, {2}, drill\\WObj:=block;", index, "rate", "fine"));
                moveBuilder.Append(string.Format("\n\t\tMoveL S{0}0, {1}, {2}, drill\\WObj:=block;", index, "v100", "z5"));
                //moveBuilder.Append(string.Format("\n\t\tMoveL RelTool(S{0}0, 0, 50, 0), {1}, {2}, drill\\WObj:=block;", index, "v200", "z30"));

                // create safe movement to next
                if (index < planes.Count - 1)
                {
                    //moveBuilder.Append(string.Format("\n\t\tMoveAbsJ CalcJointT(RelTool(S{0}0, 0, 50, 0), drill\\WObj:=block), {1}, {2}, drill\\WObj:=block;", index + 1, "v200", "z5"));
                    moveBuilder.Append(string.Format("\n\t\tMoveAbsJ CalcJointT(S{0}0, drill\\WObj:=block), {1}, {2}, drill\\WObj:=block;", index + 1, "v200", "z5"));
                }

                // update index
                index += 1;
            }

            // create rapid
            string r = "";
            using (var tw = new StreamWriter(filename, false))
            {
                r =
                    "MODULE MainModule" +
                    "\n" +
                    "\n" + targBuilder.ToString() +
                    "\n" +
                    "\n\t" + "! ROUTINE" +
                    "\n\t" + "PROC main()" +
                    "\n\t\t" + "ConfL\\Off;" +
                    "\n\t\t" + "SingArea\\Wrist;" +
                    "\n" +
                    "\n\t\t" + "TPErase;" +
                    "\n" +
                    "\n\t\t" + "TPWrite(\"Check block && drill!\");" +
                    "\n\t\t" + "MoveAbsJ j0, v200, z5, tool0;" +
                    "\n\t\t" + "MoveAbsJ j1, v200, z5, tool0;" + moveBuilder.ToString() +
                    "\n" +
                    "\n\t\t" + "TPWrite(\"Returning to start...\");" +
                    "\n\t\t" + "MoveAbsJ j1, v200, z5, tool0;" +
                    "\n\t\t" + "MoveAbsJ j0, v200, z5, tool0;" +
                    "\n" +
                    "\n\t\t" + "TPWrite(\"Node complete!\");" +
                    "\n" +
                    "\n\t\t" + "Stop;" +
                    "\n\t" + "ENDPROC" +
                    "\n" +
                    "\n" + "ENDMODULE"
                    ;

                tw.Write(r);
                tw.Flush();
            }

            // end step
            return outputFiles;
        }










    }
}
