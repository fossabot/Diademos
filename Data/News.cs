using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// TODO: Get Unirest for C#

namespace Diademos.Data
{
    public class News
    {
        public Task<Article[]> GetForecastAsync(DateTime startDate, String orderby, int feedInd)
        {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(feedInd, feedInd + 10).Select(index => new Article
            {
                Date = ,
                Publisher = ,
                Author = ,
                Summary = ,
                Contents = ,
                URL = ,
                Tags = ,
            }).ToArray());
        }
    }
}

/*
 * Hoaxy:
Task<HttpResponse<MyClass>> response = Unirest.get("https://api-hoaxy.p.rapidapi.com/articles?sort_by=relevant&use_lucene_syntax=true&query=canonical_url%253A+%2522" + URL +"%2522")
.header("X-RapidAPI-Host", "api-hoaxy.p.rapidapi.com")
.header("X-RapidAPI-Key", "c594e3fb9emsh95f74bae064e059p1e1959jsnc4df23dcc4ef")
.asJson();
*/
