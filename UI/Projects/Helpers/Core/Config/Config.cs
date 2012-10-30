using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Core.Helpers
{

    public static partial class Config
    {
        //APP SETTINGS OCNFIGURATION
        public static class Error
        {
            public static class Message
            {
                public static string Generic { get { return ConfigurationManager.AppSettings["GenericErrorMessage"]; } }

                public static string Header { get { return ConfigurationManager.AppSettings["ErrorMessageHeader"]; } }

                public static string PasswordFormat { get { return ConfigurationManager.AppSettings["PasswordFormatMessage"]; } }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //DYNAMIC CONFIGURATION
        public static eConfig Section
        {
            get { return (eConfig)ConfigurationManager.GetSection("eConfig"); }
        }

        public static DeploymentFor ActiveConfiguration
        {
            get { return Section.ActiveConfiguration; }
        }

    }

    public class eConfig : ConfigurationSection
    {
        [ConfigurationProperty("environment", IsRequired = true, DefaultValue = "development")]
        public string Environment
        {
            get { return this["environment"] as string; }
        }

        public DeploymentFor ActiveConfiguration
        {
            get
            {
                switch (Environment)
                {
                    case "development":
                        return this.Development;
                    case "staging":
                        return this.Staging;
                    case "production":
                        return this.Production;
                }
                return null;
            }
        }

        [ConfigurationProperty("development", IsRequired = false)]
        public DeploymentFor Development
        {
            get { return this["development"] as DeploymentFor; }
        }

        [ConfigurationProperty("staging", IsRequired = false)]
        public DeploymentFor Staging
        {
            get { return this["staging"] as DeploymentFor; }
        }

        [ConfigurationProperty("production", IsRequired = false)]
        public DeploymentFor Production
        {
            get { return this["production"] as DeploymentFor; }
        }
    }

    public class DeploymentFor : ConfigurationElement
    {
        [ConfigurationProperty("controllerExtension", IsRequired = true, DefaultValue = "")]
        public string ControllerExtension
        {
            get { return this["controllerExtension"] as string; }
        }

        [ConfigurationProperty("domain", IsRequired = false, DefaultValue = "simplecore.com")]
        public string Domain
        {
            get { return this["domain"] as string; }
        }

        [ConfigurationProperty("database", IsRequired = false, DefaultValue = "development")]
        public string Database
        {
            get { return this["database"] as string; }
        }

        [ConfigurationProperty("mail", IsRequired = false)]
        public MailConfiguration Mail
        {
            get { return this["mail"] as MailConfiguration; }
        }

    }

    public class MailConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("host", IsRequired = false, DefaultValue = "simplecore.com")]
        public string Host
        {
            get { return this["host"] as string; }
        }

        [ConfigurationProperty("port", IsRequired = false, DefaultValue = 25)]
        public int Port
        {
            get { return (int)this["port"]; }
        }

        [ConfigurationProperty("from", IsRequired = false, DefaultValue = "dragan@webitprofessionals.com")]
        public string From
        {
            get { return this["from"] as string; }
        }

        [ConfigurationProperty("to", IsRequired = false, DefaultValue = "drusnov@magnersanborn.com")]
        public string To
        {
            get { return this["to"] as string; }
        }

    }
}
