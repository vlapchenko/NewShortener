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
                return "It is empty string";

            return GetShortLink(link);
        }

        public string GetShortLink(string link)
        {
            var query = new Query("Data.Scripts.GetShortLink")
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
                    Short_Link = Guid.NewGuid().ToString("N")
                };
                appContext.Insert(linksRow);
            };
            return @"linksRow:" + linksRow.Short_Link;
        }

    }
}
