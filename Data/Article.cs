using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diademos.Data
{
    public class Article
    {
        public DateTime DatePublished { get; set; }

        public string Publisher { get; set; }

        public string Author { get; set; }

        public string Summary { get; set; }

        public string Contents { get; set; }

        public string URL { get; set; }

        public string Tags { get; set; }
    }
}
