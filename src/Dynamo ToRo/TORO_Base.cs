using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Linq;
using Autodesk.DesignScript.Runtime;


namespace Dynamo_TORO
{
    [IsVisibleInDynamoLibrary(false)]
    public class TSpeed
    {
        public int v_tcp ;
        public int v_ori ;
        public int v_leax ;
        public int v_reax ;
        public string v_name;

        public string RapidCode;

        [IsVisibleInDynamoLibrary(false)]
        public TSpeed(string name, int tcp = 20, int ori =20, int leax =100, int reax =10)
        {
            v_name = name;
            v_tcp = tcp;
            v_ori = ori;
            v_leax = leax;
            v_reax = reax;

            RapidCode = $"VAR speeddata {v_name} := [ {v_tcp}, {v_ori}, {v_leax}, {v_reax} ];";

        }
        [IsVisibleInDynamoLibrary(false)]
        public override string ToString()
        {
            return RapidCode;
        }
    }
    [IsVisibleInDynamoLibrary(false)]
    public class TConstant
    {
        [IsVisibleInDynamoLibrary(false)]
        public enum ConsType
        {
         Speed,
         RobTarget   
        }

        public string Data;
        public ConsType CType;
        [IsVisibleInDynamoLibrary(false)]
        public TConstant(ConsType cType, string data  )
        {
            Data = data;
            CType = cType;
        }
        [IsVisibleInDynamoLibrary(false)]
        public override string ToString()
        {
            return Data;
        }
    }

}
