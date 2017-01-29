using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodsicApp
{
    class Track
    {
        public String id { get; set; }
        public String title { get; set; }
        public String artist { get; set; }

        public Track(String id, String title = "", String artist = "") { }
    }
}
