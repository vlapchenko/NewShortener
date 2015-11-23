using System;
using NewShortener.Models;
using NewShortener.Pages;
using NFX;
using NFX.DataAccess.CRUD;
using NFX.DataAccess.MySQL;
using NFX.Wave.MVC;

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
            return @"http://localhost:8080/" + linksRow.Short_Link;
        }

        private string GetGuid()
        {
            string s = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            s = s.Replace("/", "_");
            s = s.Replace("+", "-");
            return s.Substring(0, 22);
        }

    }
}
