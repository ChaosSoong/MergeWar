using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCZZ.Models
{
    public class Json_LocaMap
    {
        public int ID { get; set; }
        public string lng { get; set; }
        public string lat { get; set; }
        public string loadMessage { get; set; }
        public int statis { get; set; }
    }
}