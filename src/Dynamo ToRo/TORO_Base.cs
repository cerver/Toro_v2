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

    internal class Speed
    {
        public int v_tcp ;
        public int v_ori ;
        public int v_leax ;
        public int v_reax ;
        public string v_name;

        public string SpeedData;

 
        public void speed(string name, int tcp = 20, int ori =20, int leax =100, int reax =10)
        {
            v_name = name;
            v_tcp = tcp;
            v_ori = ori;
            v_leax = leax;
            v_reax = reax;

            SpeedData = $"VAR speeddata {v_name} := [ {v_tcp}, {v_ori}, {v_leax}, {v_reax} ];";
        }

        
    }
}
