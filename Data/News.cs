using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using CodeHollow.FeedReader;
using System.Net;
using HtmlAgilityPack;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Web;
using System.Diagnostics;
using Microsoft.JSInterop;

namespace Diademos.Data
{
    public class News
    {

        public static Task<string[]> GetArticleCSS(string pageContents, string publisher)
        {
            if (publisher.Equals("BBC Chinese") || publisher.Equals("BBC News"))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(pageContents);
                List<string> styleData = new List<string>();
                var nodesStyleLinks = doc.DocumentNode.SelectNodes("//link/@href");
                var nodesStyleTags = doc.DocumentNode.SelectNodes("//style");
                var linkTags = nodesStyleLinks.Select(x => x.OuterHtml).ToList();
                var styleTags = nodesStyleTags.Select(x => x.OuterHtml).ToList();
                foreach(var linkTag in linkTags)
                {
                    styleData.Add(linkTag);
                }
                foreach(var styleTag in styleTags)
                {
                    styleData.Add(styleTag);
                }
                return Task.FromResult(styleData.ToArray());
            }
            else
            {
                return Task.FromResult(Array.Empty<string>());
            }
        }

        [Obsolete]
        public static Task<string[]> GetArticleUrls(string feedUrl)
        {
            List<string> articleUrlList = new List<string>();
            var feed = FeedReader.Read(HttpUtility.UrlDecode(feedUrl));
            foreach (var item in feed.Items)
            {
                articleUrlList.Add(item.Link);
            }
            return Task.FromResult(articleUrlList.ToArray());
        }
        public static Task<string> GetContents(string url, string publisher)
        {
            if (publisher.Equals("BBC Chinese") || publisher.Equals("BBC News"))
            {
                return Task.FromResult("<head>\n" + BBCParse(GetResponseString(url))[2] + "</head>\n" + BBCParse(GetResponseString(url))[0]);
            }
            else
            {
                return Task.FromResult(GetResponseString(url));
            }
        }
        public static string GetResponseString(string url)
        {
            using WebClient client = new WebClient();
            return client.DownloadString(url);
        }

        public static String[] BBCParse(string pageContents)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pageContents);
            var nodesImg = doc.DocumentNode.SelectNodes("//img[@class='js-image-replace']");
            //var nodesImg = doc.DocumentNode.SelectNodes("//img");
            var nodesArticle = doc.DocumentNode.SelectNodes("//div[@class='story-body__inner']//*[not(@class = 'story-image-copyright')]");
            var nodesHead = doc.DocumentNode.SelectNodes("//head");
            if (nodesImg != null && nodesArticle == null)
            {
                var imgList = nodesImg.Select(x => x.OuterHtml).ToList();
                return new string[] { "", imgList[0] };
            }
            else if (nodesArticle != null && nodesImg == null)
            {
                var articleContents = nodesArticle.Select(x => x.OuterHtml).ToList();
                return new string[] { articleContents[0], "" };
            }
            else if (nodesArticle == null && nodesImg == null)
            {
                return new string[] { "", "" };
            }
            else
            {
                var imgList = nodesImg.Select(x => x.OuterHtml).ToList();
                var articleContents = nodesArticle.Select(x => x.OuterHtml).ToList();
                var head = nodesHead.Select(x => x.OuterHtml).ToList();
                if (imgList[0].Equals(""))
                {
                    foreach (var img in imgList)
                    {
                        if (!img.Equals(""))
                        {
                            return new string[] { articleContents[0], img };
                        }
                    }
                }
                return new string[] { articleContents[0], imgList[0], head[0] };



            }
        }



        [Obsolete]
        public static Task<Article[]> GetArticlesAsync(String feedUrl, String publisher, int feedInd, int numOnPage)
        {
            List<Article> articlesList = new List<Article>();
            var feed = FeedReader.ReadAsync(feedUrl);

            for (int i = feedInd; i < feedInd + numOnPage; i++)
            {
                string pageContents = GetResponseString(feed.Result.Items.ElementAt(i).Link);
                if (feedInd == feed.Result.Items.Count - 1)
                {
                    return Task.FromResult(articlesList.ToArray());
                }
                else
                {
                    if (!feed.Result.Items.ElementAt(i).Title.Equals("BBC中文合作伙伴"))
                    {
                        if (publisher.Equals("BBC Chinese") || publisher.Equals("BBC News"))
                        {
                            articlesList.Add(new Article { Contents = BBCParse(pageContents)[0], Thumbnail = BBCParse(pageContents)[1], Publisher = publisher, URL = feed.Result.Items.ElementAt(i).Link, Title = feed.Result.Items.ElementAt(i).Title, Author = feed.Result.Items.ElementAt(i).Author, DatePublished = (System.DateTime)feed.Result.Items.ElementAt(i).PublishingDate, Summary = feed.Result.Items.ElementAt(i).Description, Tags = feed.Result.Items.ElementAt(i).Categories });
                        }
                        else
                        {
                            articlesList.Add(new Article { Publisher = publisher, URL = feed.Result.Items.ElementAt(i).Link, Title = feed.Result.Items.ElementAt(i).Title, Author = feed.Result.Items.ElementAt(i).Author, DatePublished = (System.DateTime)feed.Result.Items.ElementAt(i).PublishingDate, Contents = feed.Result.Items.ElementAt(i).Content, Summary = feed.Result.Items.ElementAt(i).Description, Tags = feed.Result.Items.ElementAt(i).Categories });
                        }
                    }
                }
            }

            /*
            foreach (var item in feed.Result.Items)
            {
                if (!item.Title.Equals("BBC中文合作伙伴"))
                {
                    if (publisher.Equals("BBC Chinese") || publisher.Equals("BBC News"))
                    {
                        articlesList.Add(new Article { Contents = BBCParse(item.Link)[0], Thumbnail = BBCParse(item.Link)[1], Publisher = publisher, URL = item.Link, Title = item.Title, Author = item.Author, DatePublished = (System.DateTime)item.PublishingDate, Summary = item.Description, Tags = item.Categories });
                    }
                    else
                    {
                        articlesList.Add(new Article { Publisher = publisher, URL = item.Link, Title = item.Title, Author = item.Author, DatePublished = (System.DateTime)item.PublishingDate, Contents = item.Content, Summary = item.Description, Tags = item.Categories });
                    }
                    //articlesList.Add(new Article { Publisher = publisher, URL = item.Link, Title = item.Title, Author = item.Author, DatePublished = (System.DateTime) item.PublishingDate, Contents = item.Content, Summary = item.Description, Tags = item.Categories });
                }
            */




                /*
                string pageContents = String.Empty;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(item.Link);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader streamReader = new StreamReader(responseStream))
                {
                    pageContents = streamReader.ReadToEnd();
                }
                if (publisher.Equals("BBC Chinese") || publisher.Equals("BBC News"))
                {
                    BBCParse(pageContents);
                    articlesList.Add(new Article { Contents = BBCParse(pageContents)[0], Thumbnail = BBCParse(pageContents)[1], Publisher = publisher, URL = item.Link, Title = item.Title, Author = item.Author, DatePublished = (System.DateTime)item.PublishingDate, Summary = item.Description, Tags = item.Categories });
                }
                else
                {
                    articlesList.Add(new Article { Publisher = publisher, URL = item.Link, Title = item.Title, Author = item.Author, DatePublished = (System.DateTime)item.PublishingDate, Contents = item.Content, Summary = item.Description, Tags = item.Categories });
                }
                */
                //articlesList.Add(new Article { Publisher = publisher, URL = item.Link, Title = item.Title, Author = item.Author, DatePublished = (System.DateTime) item.PublishingDate, Contents = item.Content, Summary = item.Description, Tags = item.Categories });
            //}

            return Task.FromResult(articlesList.ToArray());
        }

        [Obsolete]
        public static Article[] GetArticles(String feedUrl, String publisher, int feedInd, int numOnPage)
        {
            List<Article> articlesList = new List<Article>();
            var feed = FeedReader.ReadAsync(feedUrl);

            for (int i = feedInd; i < feedInd + numOnPage; i++)
            {
                if (feedInd == feed.Result.Items.Count - 1)
                {
                    return articlesList.ToArray();
                }
                else
                {
                    if (!feed.Result.Items.ElementAt(i).Title.Equals("BBC中文合作伙伴"))
                    {
                        if (publisher.Equals("BBC Chinese") || publisher.Equals("BBC News"))
                        {
                            articlesList.Add(new Article { Contents = BBCParse(feed.Result.Items.ElementAt(i).Link)[0], Thumbnail = BBCParse(feed.Result.Items.ElementAt(i).Link)[1], Publisher = publisher, URL = feed.Result.Items.ElementAt(i).Link, Title = feed.Result.Items.ElementAt(i).Title, Author = feed.Result.Items.ElementAt(i).Author, DatePublished = (System.DateTime)feed.Result.Items.ElementAt(i).PublishingDate, Summary = feed.Result.Items.ElementAt(i).Description, Tags = feed.Result.Items.ElementAt(i).Categories });
                        }
                        else
                        {
                            articlesList.Add(new Article { Publisher = publisher, URL = feed.Result.Items.ElementAt(i).Link, Title = feed.Result.Items.ElementAt(i).Title, Author = feed.Result.Items.ElementAt(i).Author, DatePublished = (System.DateTime)feed.Result.Items.ElementAt(i).PublishingDate, Contents = feed.Result.Items.ElementAt(i).Content, Summary = feed.Result.Items.ElementAt(i).Description, Tags = feed.Result.Items.ElementAt(i).Categories });
                        }
                    }
                }
            }

            /*
            foreach (var item in feed.Result.Items)
            {
                if (!item.Title.Equals("BBC中文合作伙伴"))
                {
                    if (publisher.Equals("BBC Chinese") || publisher.Equals("BBC News"))
                    {
                        articlesList.Add(new Article { Contents = BBCParse(item.Link)[0], Thumbnail = BBCParse(item.Link)[1], Publisher = publisher, URL = item.Link, Title = item.Title, Author = item.Author, DatePublished = (System.DateTime)item.PublishingDate, Summary = item.Description, Tags = item.Categories });
                    }
                    else
                    {
                        articlesList.Add(new Article { Publisher = publisher, URL = item.Link, Title = item.Title, Author = item.Author, DatePublished = (System.DateTime)item.PublishingDate, Contents = item.Content, Summary = item.Description, Tags = item.Categories });
                    }
                    //articlesList.Add(new Article { Publisher = publisher, URL = item.Link, Title = item.Title, Author = item.Author, DatePublished = (System.DateTime) item.PublishingDate, Contents = item.Content, Summary = item.Description, Tags = item.Categories });
                }
            */




            /*
            string pageContents = String.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(item.Link);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader streamReader = new StreamReader(responseStream))
            {
                pageContents = streamReader.ReadToEnd();
            }
            if (publisher.Equals("BBC Chinese") || publisher.Equals("BBC News"))
            {
                BBCParse(pageContents);
                articlesList.Add(new Article { Contents = BBCParse(pageContents)[0], Thumbnail = BBCParse(pageContents)[1], Publisher = publisher, URL = item.Link, Title = item.Title, Author = item.Author, DatePublished = (System.DateTime)item.PublishingDate, Summary = item.Description, Tags = item.Categories });
            }
            else
            {
                articlesList.Add(new Article { Publisher = publisher, URL = item.Link, Title = item.Title, Author = item.Author, DatePublished = (System.DateTime)item.PublishingDate, Contents = item.Content, Summary = item.Description, Tags = item.Categories });
            }
            */
            //articlesList.Add(new Article { Publisher = publisher, URL = item.Link, Title = item.Title, Author = item.Author, DatePublished = (System.DateTime) item.PublishingDate, Contents = item.Content, Summary = item.Description, Tags = item.Categories });
            //}

            return articlesList.ToArray();
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
