using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserMicroservice.Model
{
   public enum ResponseEnum : ushort
    {
        OK = 1,
        BadRequest = 2,
        ConnectionLost = 3
    }


    public class ResponseGlobal
    {
        public ResponseEnum rspEnum { get; set; }

        public string Message { get; set; }
    }
}
