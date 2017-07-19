using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HCZZ.ModeDB
{
    public class UnknownDevice
    {
        public long PageNum { get; set; }
        public long ID { get; set; }
        public string AP_MAC { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; set; }
		public  string OutIpAddress { get; set; }
        public  string AreaName { get; set; }


    }
}
