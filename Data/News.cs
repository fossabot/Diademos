using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;

namespace Diademos.Data
{
    public class News
    {
        public Task<Article[]> GetArticlesAsync(String query, int feedInd)
        {

            var client = new RestClient("https://search-news-feed.p.rapidapi.com/articles?q=" + query);
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "search-news-feed.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "c594e3fb9emsh95f74bae064e059p1e1959jsnc4df23dcc4ef");
            IRestResponse response = client.Execute(request);
            Article currentArticle = JsonConvert.DeserializeObject<Article>(response.ToString());

            var client = new RestClient("https://saidimu-newscuria-v1.p.rapidapi.com/url/tags/");
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-rapidapi-host", "saidimu-newscuria-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "ca56a08701msh032249109e6f378p138b70jsnf57cd4f98f52");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "url=" + currentArticle.link, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return Task.FromResult(Enumerable.Range(feedInd, feedInd + 10).Select(index => new Article
            {
                Date = currentArticle.published,
                Publisher = currentArticle.source,
                Author = currentArticle.author,
                Summary = currentArticle.summary,
                Contents = ,
                URL = currentArticle.link,
                Tags = ,
            }).ToArray());
        }
    }
}

/*
 * TODO: Implement URL encoding for the strings passed to APIs
 * Hoaxy:
var client = new RestClient("https://api-hoaxy.p.rapidapi.com/articles?sort_by=relevant&use_lucene_syntax=true&query=canonical_url%253A%20%2522" + URL + "%2522");
var request = new RestRequest(Method.GET);
request.AddHeader("x-rapidapi-host", "api-hoaxy.p.rapidapi.com");
request.AddHeader("x-rapidapi-key", "ca56a08701msh032249109e6f378p138b70jsnf57cd4f98f52");
IRestResponse response = client.Execute(request);

 * News Search:
var client = new RestClient("https://search-news-feed.p.rapidapi.com/articles?q=" + query);
var request = new RestRequest(Method.GET);
request.AddHeader("x-rapidapi-host", "search-news-feed.p.rapidapi.com");
request.AddHeader("x-rapidapi-key", "c594e3fb9emsh95f74bae064e059p1e1959jsnc4df23dcc4ef");
IRestResponse response = client.Execute(request);
*/
