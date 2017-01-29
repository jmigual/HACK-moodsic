using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodsicApp
{
    class Track
    {
        String id { get; set; }
        String title { get; set; }
        String artist { get; set; }

        public Track (String id, String title = "", String artist = "");
    }
}
