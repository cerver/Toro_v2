using Autodesk.DesignScript.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamoText;

namespace WireFrameToRobot
{
    /// <summary>
    /// class that represents a label
    /// contains raw label geometry, geometry aligned to face
    ///  and the ID of the surface
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class Label<T>: IDisposable
        where T : ILabelAble
    {

        public string LabelString { get; set; }
        public List<Curve> LabelGeometry { get; set; }
        public List<Curve> AlignedLabelGeometry { get; set; }
        public T AttachedGeo { get; set; }


        private IEnumerable<Curve> GenLabelGeometryFromId(string id, double scale = 1.0)
        {
            List<Curve> textgeo = Text.FromStringOriginAndScale(id, Point.ByCoordinates(0, 0, 0), scale) as List<Curve>;
            return textgeo;

        }

        /// <summary>
        /// align the label geometry to the solid that it represents
        /// </summary>
        /// <param name="geo"></param>
        /// <returns></returns>
        private IEnumerable<Curve> AlignGeoToSolid(IEnumerable<Curve> geo)
        {
            var oldgeo = new List<IDisposable>();
            var approxCenter = AttachedGeo.GeometryToLabel.Centroid();
            var norm = Vector.ZAxis();
            var solidBB = AttachedGeo.GeometryToLabel.BoundingBox;
            var newpoint = approxCenter.Translate(0, 0, solidBB.MaxPoint.Z - solidBB.MinPoint.Z) as Point;
            var plane = Plane.ByOriginNormal(newpoint,norm);
            var finalCordSystem = CoordinateSystem.ByPlane(plane);

            oldgeo.Add(approxCenter);
            oldgeo.Add(norm);
            oldgeo.Add(solidBB);
            oldgeo.Add(plane);
            oldgeo.Add(newpoint);
            //oldgeo.Add(finalCordSystem);
            
            // find bounding box of set of curves
            var textBoudingBox = BoundingBox.ByGeometry(geo);
            oldgeo.Add(textBoudingBox);
            // find the center of this box and use as start point
            var textmin = textBoudingBox.MinPoint;
            var textmax = textBoudingBox.MaxPoint;
            var textminvec = textmin.AsVector();

            var textCenter = textmin.Add((
                textmax.Subtract(textminvec))
                .AsVector().Scale(.5));

            oldgeo.Add(textBoudingBox);
            oldgeo.Add(textCenter);
            oldgeo.Add(textmin);
            oldgeo.Add(textmax);
            oldgeo.Add(textminvec);


            var transVector = Vector.ByTwoPoints(textCenter, Point.ByCoordinates(0, 0, 0));

            var geoIntermediateTransform = geo.Select(x => x.Translate(transVector)).Cast<Curve>().AsEnumerable();
            oldgeo.AddRange(geoIntermediateTransform);
            //oldgeo.Add(transVector);

            var finalTransformedLabel = geoIntermediateTransform.Select(x => x.Transform(finalCordSystem)).Cast<Curve>();
            foreach (IDisposable item in oldgeo)
            {
                item.Dispose();
            }

            return finalTransformedLabel;

        }

        public Label (T labelAbleItem, double labelscale = 1.0)
        {
            LabelString = labelAbleItem.ID;
            AttachedGeo = labelAbleItem;
            LabelGeometry = GenLabelGeometryFromId(LabelString, labelscale).ToList();
            AlignedLabelGeometry = AlignGeoToSolid(LabelGeometry).ToList();
        }

        public void Dispose()
        {
            LabelGeometry.ForEach(x => x.Dispose());
        }

    }

    public interface ILabelAble
    {
        Solid GeometryToLabel { get; }
        String ID { get; }

        List<Curve> GetLabels(double scale);

    }


}
