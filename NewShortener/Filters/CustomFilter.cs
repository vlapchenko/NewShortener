using System.Collections.Generic;
using NewShortener.Models;
using NFX;
using NFX.DataAccess.CRUD;
using NFX.DataAccess.MySQL;
using NFX.Environment;
using NFX.Wave;
using System.Text.RegularExpressions;

namespace NewShortener.Filters
{
    public class CustomFilter : WorkFilter
    {
        #region .ctor

        public CustomFilter(WorkDispatcher dispatcher, string name, int order) : base(dispatcher, name, order)
        {
        }

        public CustomFilter(WorkDispatcher dispatcher, IConfigSectionNode confNode) : base(dispatcher, confNode)
        {
        }

        public CustomFilter(WorkHandler handler, string name, int order) : base(handler, name, order)
        {
        }

        public CustomFilter(WorkHandler handler, IConfigSectionNode confNode) : base(handler, confNode)
        {
        }

        #endregion

        protected override void DoFilterWork(WorkContext work, IList<WorkFilter> filters, int thisFilterIndex)
        {
            var code = work.Request.Url.AbsolutePath.Substring(1);
            if (IsShortLink(code))
            {
                var link = GetLink(code);
                work.Response.RedirectAndAbort(link);
            }
            this.InvokeNextWorker(work, filters, thisFilterIndex);
        }

        private bool IsShortLink(string s)
        {
            Regex rgx = new Regex(@"\A[\w-]{22}\Z");
            if (rgx.IsMatch(s))
                return true;
            return false;
        }

        private string GetLink(string code)
        {
            var query = new Query("Data.Scripts.GetLink", typeof(Links))
                        {
                            new Query.Param("p_short_link", code)
                        };
            var appContext = App.DataStore as MySQLDataStore;
            Links linksRow = appContext.LoadOneRow(query) as Links;

            if (linksRow == null)
                return @"http://localhost:8080/newshortener/index";

            var link = linksRow.Link;

            if (!link.StartsWith(@"http://") && !link.StartsWith(@"https://"))
                link = @"http://" + link;

            return link;
        }
    }
}
