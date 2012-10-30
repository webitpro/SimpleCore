using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Library;

namespace Core.Helpers
{
    public static partial class Error
    {

        public static void Exception(Exception ex, string location)
        {
            try
            {
                Utils.Email.sendEmail(
                    Config.ActiveConfiguration.Mail.From,
                    Config.ActiveConfiguration.Mail.To,
                    "Error Handler Notification",
                    "<b>Location: " + location + "<br /><b>Exception Message: </b>" + ex.Message,
                    true,
                    Config.ActiveConfiguration.Mail.Host,
                    Config.ActiveConfiguration.Mail.Port);
            }
            catch
            {

            }
        }

        public static void Invalid(ref bool valid)
        {
            valid = valid && false;
        }

        public static void Valid(ref bool valid)
        {
            valid = valid && true;
        }

        public class Message
        {
            public string Id { get; set; }
            public string Text { get; set; }
        }
    }
}