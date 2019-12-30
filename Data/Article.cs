using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diademos.Data
{
    public class Article
    {
        public DateTime DatePublished { get; set; }

        public String Title { get; set; }

        public string Publisher { get; set; }

        public string Author { get; set; }

        public string Summary { get; set; }

        public string Contents { get; set; }

        public string Thumbnail { get; set; }

        public string URL { get; set; }

        public ICollection<string> Tags { get; set; }

        public double VeritabilityRating { get; set; }

        public int MoodPolarity { get; set; }

    }
}
