using System;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;

namespace Core
{
    /// <summary>
    ///     Simple action result for ouputting a RSS feed
    /// </summary>
    public class RssActionResult : ActionResult
    {
        private SyndicationFeed p_Feed = null;

        /// <summary>
        ///     Constructor for RSS action result
        /// </summary>
        /// <param name="feed">SyndicationFeed object to output</param>
        public RssActionResult(SyndicationFeed feed)
        {
            p_Feed = feed;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";

            if (p_Feed != null)
            {
                Rss20FeedFormatter rssFormatter = new Rss20FeedFormatter(p_Feed);
                using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
                {
                    rssFormatter.WriteTo(writer);
                }
            }
            else
            {
                throw new Exception("Feed is null");
            }
        }
    }
}
