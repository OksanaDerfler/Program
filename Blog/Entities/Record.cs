using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Record
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateStart { get; set;}
        public string Tag { get; set; }
        public int Like { get; set; }
        public string Nick { get; set; }
        public object Picture { get; set; }
    }

}
