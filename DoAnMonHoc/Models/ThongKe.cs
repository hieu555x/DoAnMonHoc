using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Runtime.Serialization;

namespace DoAnMonHoc.Models
{
    [DataContract]
    public class ThongKe
    {
        public ThongKe(string label,int y)
        {
            this.label = label;
            this.y = y;
        }

        [DataMember(Name = "label")]
        public string label = "";

        [DataMember(Name = "y")]
        public Nullable<int> y = null;
    }
}