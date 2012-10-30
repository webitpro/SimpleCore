using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace Core.Library
{
    public static partial class Utils
    {
        /// <summary>
        /// Contains methods to send email
        /// </summary>
        public static class Email
        {
            private delegate void SendMailFunc(MailMessage msg);

            /**************************/
            /* Begin helper functions */
            /**************************/

            private static void P_SendMessageUsingNet(MailMessage msg)
            {
                try
                {
                    SmtpClient smtp = new SmtpClient();
                    smtp.Timeout = 900000;
                    smtp.Send(msg);
                }
                catch
                {
                    SmtpClient smtp = new SmtpClient();
                    smtp.Timeout = 900000;
                    smtp.Send(msg);
                }
            }

            private static void P_SendMessage(MailMessage msg, string server, int port)
            {
                try
                {
                    SmtpClient smtp = new SmtpClient(server, port);
                    smtp.Timeout = 900000;
                    smtp.Send(msg);
                }
                catch
                {
                    SmtpClient smtp = new SmtpClient(server);
                    smtp.Timeout = 900000;
                    smtp.Send(msg);
                }
            }

            private static void P_SendMessageExchange(MailMessage msg, string server, int port, string username, string password)
            {
                try
                {
                    SmtpClient smtp = new SmtpClient(server, port);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(username, password);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Timeout = 900000;
                    smtp.Send(msg);
                }
                catch
                {
                    SmtpClient smtp = new SmtpClient(server);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(username, password);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Timeout = 900000;
                    smtp.Send(msg);
                }
            }

            private static MailMessage P_CreateMailMessage(string from, string to, string subject, string body, bool bHtml)
            {
                MailMessage msg = new MailMessage(from, to, subject, body);
                msg.IsBodyHtml = bHtml;

                return msg;
            }

            private static MailMessage P_CreateMailMessage(string from, string[] arrTo, string subject, string body, bool bHtml)
            {
                StringBuilder toSB = new StringBuilder();
                foreach (string to in arrTo)
                {
                    toSB.Append(to);
                    toSB.Append(",");
                }

                return P_CreateMailMessage(from, toSB.ToString(), subject, body, bHtml);
            }

            private static void P_SendSingleRecipientNoAttachment(string from, string to, string subject, string body, bool bHtml, SendMailFunc sender)
            {
                MailMessage msg = P_CreateMailMessage(from, to, subject, body, bHtml);

                sender(msg);
            }

            private static void P_SendMultiRecipientNoAttachment(string from, string[] arrTo, string subject, string body, bool bHtml, SendMailFunc sender)
            {
                MailMessage msg = P_CreateMailMessage(from, arrTo, subject, body, bHtml);

                sender(msg);
            }

            private static void P_SendSingleRecipientSingleAttachment(string from, string to, string subject, string body, bool bHtml, string attachment, SendMailFunc sender)
            {
                MailMessage msg = P_CreateMailMessage(from, to, subject, body, bHtml);

                msg.Attachments.Add(new Attachment(attachment));

                sender(msg);
            }

            private static void P_SendMultiRecipientSingleAttachment(string from, string[] arrTo, string subject, string body, bool bHtml, string attachment, SendMailFunc sender)
            {
                MailMessage msg = P_CreateMailMessage(from, arrTo, subject, body, bHtml);

                msg.Attachments.Add(new Attachment(attachment));

                sender(msg);
            }

            private static void P_SendSingleRecipientMultiAttachment(string from, string to, string subject, string body, bool bHtml, string[] arrAttachments, SendMailFunc sender)
            {
                MailMessage msg = P_CreateMailMessage(from, to, subject, body, bHtml);

                foreach (string attach in arrAttachments)
                {
                    msg.Attachments.Add(new Attachment(attach));
                }

                sender(msg);
            }

            private static void P_SendMultiRecipientMultiAttachment(string from, string[] arrTo, string subject, string body, bool bHtml, string[] arrAttachments, SendMailFunc sender)
            {
                MailMessage msg = P_CreateMailMessage(from, arrTo, subject, body, bHtml);

                foreach (string attach in arrAttachments)
                {
                    msg.Attachments.Add(new Attachment(attach));
                }

                sender(msg);
            }

            /************************/
            /* End helper functions */
            /************************/


            /****************************/
            /* Begin UsingNet functions */
            /****************************/

            /// <summary>
            /// Send email using Net configuration (Web.config) to a single receipient
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="to">(string) recipient's email address</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>
            public static void sendEmailUsingNet(string from, string to, string subject, string body, bool bHtml)
            {
                P_SendSingleRecipientNoAttachment(from, to, subject, body, bHtml, P_SendMessageUsingNet);
            }

            /// <summary>
            /// Send email using Net configuration (Web.config) to multiple receipients
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="arrTo">(string []) array of recipient's email addresses</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>
            public static void sendEmailUsingNet(string from, string[] arrTo, string subject, string body, bool bHtml)
            {
                P_SendMultiRecipientNoAttachment(from, arrTo, subject, body, bHtml, P_SendMessageUsingNet);
            }

            /// <summary>
            /// Send email using Net configuration (Web.config) to single receipient with a single attachment
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="to">(string) recipient's email address</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>
            /// <param name="attachment">(string) attachment file path</param>
            public static void sendEmailUsingNet(string from, string to, string subject, string body, bool bHtml, string attachment)
            {
                P_SendSingleRecipientSingleAttachment(from, to, subject, body, bHtml, attachment, P_SendMessageUsingNet);
            }

            /// <summary>
            /// Send email using Net configuration (Web.config) to multiple receipients with a single attachment
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="arrTo">(string []) array of recipient's email addresses</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>
            /// <param name="attachment">(string) attachment file path</param>
            public static void sendEmailUsingNet(string from, string[] arrTo, string subject, string body, bool bHtml, string attachment)
            {
                P_SendMultiRecipientSingleAttachment(from, arrTo, subject, body, bHtml, attachment, P_SendMessageUsingNet);
            }

            /// <summary>
            /// Send email using Net configuration (Web.config) to a single receipient with multiple attachments
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="to">(string) recipient's email address</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>
            /// <param name="arrAttachments">(string[]) an array of attachment file paths</param>
            public static void sendEmailUsingNet(string from, string to, string subject, string body, bool bHtml, string[] arrAttachments)
            {
                P_SendSingleRecipientMultiAttachment(from, to, subject, body, bHtml, arrAttachments, P_SendMessageUsingNet);
            }

            /// <summary>
            /// Send email using Net configuration (Web.config) to multiple receipients with multiple attachments
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="arrTo">(string[]) an array of recipient's email addresses<param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>
            /// <param name="arrAattachments">(string[]) an array of attachment file paths</param>
            public static void sendEmailUsingNet(string from, string[] arrTo, string subject, string body, bool bHtml, string[] arrAttachments)
            {
                P_SendMultiRecipientMultiAttachment(from, arrTo, subject, body, bHtml, arrAttachments, P_SendMessageUsingNet);
            }

            /****************************/
            /* End UsingNet functions   */
            /****************************/


            /*****************************/
            /* Begin host/port functions */
            /*****************************/

            /// <summary>
            /// Send email to a single receipient
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="to">(string) recipient's email address</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>
            /// <param name="host">(string) IP/url of smtp</param>
            /// <param name="port">(int) port</param>
            public static void sendEmail(string from, string to, string subject, string body, bool bHtml, string host, int port)
            {
                SendMailFunc sender = (msg) => P_SendMessage(msg, host, port);

                P_SendSingleRecipientNoAttachment(from, to, subject, body, bHtml, sender);
            }

            /// <summary>
            /// Send email to multiple receipients
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="arrTo">(string []) an array of recipient's email addresses</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>
            /// <param name="host">(string) IP/url of smtp</param>
            /// <param name="port">(int) port</param>
            public static void sendEmail(string from, string[] arrTo, string subject, string body, bool bHtml, string host, int port)
            {
                SendMailFunc sender = (msg) => P_SendMessage(msg, host, port);

                P_SendMultiRecipientNoAttachment(from, arrTo, subject, body, bHtml, sender);
            }

            /// <summary>
            /// Send email to a single receipient with single attachment
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="to">(string) recipient's email address</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>         
            /// <param name="attachment">(string) attachment file path</param>
            /// <param name="host">(string) IP/url of smtp</param>
            /// <param name="port">(int) port</param>
            public static void sendEmail(string from, string to, string subject, string body, bool bHtml, string attachment, string host, int port)
            {
                SendMailFunc sender = (msg) => P_SendMessage(msg, host, port);

                P_SendSingleRecipientSingleAttachment(from, to, subject, body, bHtml, attachment, sender);
            }

            /// <summary>
            /// Send email to multiple receipients with single attachment
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="arrTo">(string []) an array of recipient's email addresses</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>         
            /// <param name="attachment">(string) attachment file path</param>
            /// <param name="host">(string) IP/url of smtp</param>
            /// <param name="port">(int) port</param>
            public static void sendEmail(string from, string[] arrTo, string subject, string body, bool bHtml, string attachment, string host, int port)
            {
                SendMailFunc sender = (msg) => P_SendMessage(msg, host, port);

                P_SendMultiRecipientSingleAttachment(from, arrTo, subject, body, bHtml, attachment, sender);
            }

            /// <summary>
            /// Send email to a single receipient with mulitple attachments
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="to">(string) recipient's email address</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>         
            /// <param name="arrAttachments">(string []) an array of attachment file paths</param>
            /// <param name="host">(string) IP/url of smtp</param>
            /// <param name="port">(int) port</param>
            public static void sendEmail(string from, string to, string subject, string body, bool bHtml, string[] arrAttachments, string host, int port)
            {
                SendMailFunc sender = (msg) => P_SendMessage(msg, host, port);

                P_SendSingleRecipientMultiAttachment(from, to, subject, body, bHtml, arrAttachments, sender);
            }

            /// <summary>
            /// Send email to multiple receipients with multiple attachments
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="arrTo">(string []) an array of recipient's email addresses</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>         
            /// <param name="arrAttachments">(string []) an array of attachment file paths</param>
            /// <param name="host">(string) IP/url of smtp</param>
            /// <param name="port">(int) port</param>
            public static void sendEmail(string from, string[] arrTo, string subject, string body, bool bHtml, string[] arrAttachments, string host, int port)
            {
                SendMailFunc sender = (msg) => P_SendMessage(msg, host, port);

                P_SendMultiRecipientMultiAttachment(from, arrTo, subject, body, bHtml, arrAttachments, sender);
            }

            /*****************************/
            /* End host/port functions   */
            /*****************************/


            /******************************/
            /* Begin Exchange functions   */
            /******************************/

            /// <summary>
            /// Send email using Exchange to a single receipient
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="to">(string) recipient's email address</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>
            /// <param name="host">(string) IP/url of smtp</param>
            /// <param name="port">(int) port</param>
            /// <param name="strUID">(string) username for exhange</param>
            /// <param name="strPsw">(string) password for exchange</param>
            public static void sendEmailExchange(string from, string to, string subject, string body, bool bHtml, string host, int port, string strUID, string strPsw)
            {
                SendMailFunc sender = (msg) => P_SendMessageExchange(msg, host, port, strUID, strPsw);

                P_SendSingleRecipientNoAttachment(from, to, subject, body, bHtml, sender);
            }

            /// <summary>
            /// Send email using Exchange to multiple receipients
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="arrTo">(string []) an array of recipient's email addresses</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>
            /// <param name="host">(string) IP/url of smtp</param>
            /// <param name="port">(int) port</param>
            /// <param name="strUID">(string) username for exhange</param>
            /// <param name="strPsw">(string) password for exchange</param>
            public static void sendEmailExchange(string from, string[] arrTo, string subject, string body, bool bHtml, string host, int port, string strUID, string strPsw)
            {
                SendMailFunc sender = (msg) => P_SendMessageExchange(msg, host, port, strUID, strPsw);

                P_SendMultiRecipientNoAttachment(from, arrTo, subject, body, bHtml, sender);
            }

            /// <summary>
            /// Send email using Exchange to a single receipient with a single attachment
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="to">(string) recipient's email address</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>           
            /// <param name="attachment">(string) attachment file path</param>
            /// <param name="host">(string) IP/url of smtp</param>
            /// <param name="port">(int) port</param>
            /// <param name="strUID">(string) username for exhange</param>
            /// <param name="strPsw">(string) password for exchange</param>
            public static void sendEmailExchange(string from, string to, string subject, string body, bool bHtml, string attachment, string host, int port, string strUID, string strPsw)
            {
                SendMailFunc sender = (msg) => P_SendMessageExchange(msg, host, port, strUID, strPsw);

                P_SendSingleRecipientSingleAttachment(from, to, subject, body, bHtml, attachment, sender);
            }

            /// <summary>
            /// Send email using Exchange to multiple receipients with single attachment
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="arrTo">(string []) an array of recipient's email addresses</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>           
            /// <param name="attachment">(string) attachment file path</param>
            /// <param name="host">(string) IP/url of smtp</param>
            /// <param name="port">(int) port</param>
            /// <param name="strUID">(string) username for exhange</param>
            /// <param name="strPsw">(string) password for exchange</param>
            public static void sendEmailExchange(string from, string[] arrTo, string subject, string body, bool bHtml, string attachment, string host, int port, string strUID, string strPsw)
            {
                SendMailFunc sender = (msg) => P_SendMessageExchange(msg, host, port, strUID, strPsw);

                P_SendMultiRecipientSingleAttachment(from, arrTo, subject, body, bHtml, attachment, sender);
            }

            /// <summary>
            /// Send email using Exchange to a single receipient with multiple attachments
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="to">(string) recipient's email address</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>           
            /// <param name="arrAttachments">(string []) an array of attachment file paths</param>
            /// <param name="host">(string) IP/url of smtp</param>
            /// <param name="port">(int) port</param>
            /// <param name="strUID">(string) username for exhange</param>
            /// <param name="strPsw">(string) password for exchange</param>
            public static void sendEmailExchange(string from, string to, string subject, string body, bool bHtml, string[] arrAttachments, string host, int port, string strUID, string strPsw)
            {
                SendMailFunc sender = (msg) => P_SendMessageExchange(msg, host, port, strUID, strPsw);

                P_SendSingleRecipientMultiAttachment(from, to, subject, body, bHtml, arrAttachments, sender);
            }

            /// <summary>
            /// Send email using Exchange to multiple receipients with multiple attachments
            /// </summary>
            /// <param name="from">(string) sender's email address</param>
            /// <param name="arrTo">(string []) an array of recipient's email addresses</param>
            /// <param name="subject">(string) message subject</param>
            /// <param name="body">(string) message body/copy</param>
            /// <param name="bHtml">(bool) HTML format or plain text</param>           
            /// <param name="arrAttachments">(string []) an array of attachment file paths</param>
            /// <param name="host">(string) IP/url of smtp</param>
            /// <param name="port">(int) port</param>
            /// <param name="strUID">(string) username for exhange</param>
            /// <param name="strPsw">(string) password for exchange</param>
            public static void sendEmailExchange(string from, string[] arrTo, string subject, string body, bool bHtml, string[] arrAttachments, string host, int port, string strUID, string strPsw)
            {
                SendMailFunc sender = (msg) => P_SendMessageExchange(msg, host, port, strUID, strPsw);

                P_SendMultiRecipientMultiAttachment(from, arrTo, subject, body, bHtml, arrAttachments, sender);
            }

            /****************************/
            /* End Exchange functions   */
            /****************************/
        }
    }
}