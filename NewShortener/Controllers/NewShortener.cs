using System;
using NFX;
using NFX.Wave.MVC;
using NewShortener.Models;
using NewShortener.Pages;
using NFX.DataAccess.CRUD;
using NFX.DataAccess.MySQL;

namespace NewShortener.Controllers
{
    public class NewShortener : Controller
    {
        [Action]
        public object Index()
        {
            return new Index();
        }

        [Action]
        public object Shorten(string link)
        {
            if (link.Trim() == "")
                return "";

            return GetShortLink(link);
        }

        private string GetShortLink(string link)
        {
            var query = new Query("Data.Scripts.GetShortLink", typeof(Links))
                        {
                            new Query.Param("p_link", link)
                        };
            var appContext = App.DataStore as MySQLDataStore;
            Links linksRow = appContext.LoadOneRow(query) as Links;
            if (linksRow == null)
            {
                linksRow = new Links
                {
                    Link = link,
                    Short_Link = GetGuid()
                };
                appContext.Insert(linksRow);
            };
            return @"http://localhost:8080/NewShortener/ToRedirect?s=" + linksRow.Short_Link;
        }

        private string GetGuid()
        {
            string s = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            s = s.Replace("/", "_");
            s = s.Replace("+", "-");
            return s.Substring(0, 22);
        }

        [Action]
        public object ToRedirect(string s)
        {
            string link = GetLink(s);

            if (link == "")
                return new Index();

            if (!link.StartsWith(@"http://") &&
                !link.StartsWith(@"https://"))
            {
                return new Redirect(@"http://" + link);
            }
            return new Redirect(link);
        }

        private string GetLink(string link)
        {
            var query = new Query("Data.Scripts.GetLink", typeof(Links))
                        {
                            new Query.Param("p_short_link", link)
                        };
            var appContext = App.DataStore as MySQLDataStore;
            Links linksRow = appContext.LoadOneRow(query) as Links;
            if (linksRow == null)
            {
                return "";
            };
            return linksRow.Link;
        }
    }
}
