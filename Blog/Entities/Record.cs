using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Record
    {
        public int id { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public DateTime dateStart { get; set;}
        public string tag { get; set; }
        public int like { get; set; }
        public string nick { get; set; }
        public object picture { get; set; }
    }

}
