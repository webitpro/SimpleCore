using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Core.Library;

namespace Core.Models
{
    public class EmailFormatAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string email = value as string;
            if (!string.IsNullOrEmpty(email))
            {
                return Utils.Validate.EmailAddress(email);
            }
            return false;
        }
    }

    public class URLFormatAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string url = value as string;
            if (!string.IsNullOrEmpty(url))
            {
                return Utils.Validate.URL(url);
            }
            return false;
        }
    }

    public class PhoneFormatAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string phone = value as string;
            if (!string.IsNullOrEmpty(phone))
            {
                return Utils.Validate.USPhone(phone);
            }

            return false;
        }
    }

    public class PasswordFormatAttribute : ValidationAttribute
    {
        public bool LowerStrength { get; set; }

        public override bool IsValid(object value)
        {
            bool isValid = true;
            string psw = value as string;
            if (!string.IsNullOrEmpty(psw))
            {
                isValid = Library.Utils.Validate.PasswordFormat(psw, LowerStrength);
            }
            return isValid;
        }
    }

    public class ZipCodeFormatAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string zipCode = value as string;
            if (!string.IsNullOrEmpty(zipCode))
            {
                return Utils.Validate.USZipCode(zipCode); 
            }
            return false;
        }
    }

    public class ExpressionTypeAttribute : ValidationAttribute
    {
        public Library.Type.xPression xType { get; set; }
        public bool xOperator { get; set; }

        public override bool IsValid(object value)
        {
            bool isValid = true;
            string expression = value as string;
            if (!string.IsNullOrEmpty(expression))
            {
                if (xOperator == true)
                {
                    if (Utils.Validate.ExpressionType(expression) != xType)
                    {
                        isValid = false;
                    }
                }
                else
                {
                    if (Utils.Validate.ExpressionType(expression) == xType)
                    {
                        isValid = false;
                    }
                }
            }
            return isValid;
        }
    }

    public class ExpressionTypeAdvancedAttribute : ValidationAttribute
    {
        public Library.Type.xPression xType { get; set; }
        public Library.Type.xPression xTypeAlt { get; set; }
        public bool xOperator { get; set; }

        public override bool IsValid(object value)
        {
            bool isValid = false;
            string expression = value as string;
            if (!string.IsNullOrEmpty(expression))
            {
                Library.Type.xPression xPression = Utils.Validate.ExpressionType(expression);
                if (xOperator == true)
                {

                    if (xPression == xType || xPression == xTypeAlt)
                    {
                        isValid = true;
                    }
                }
                else
                {
                    if (xPression != xTypeAlt && xPression != xTypeAlt)
                    {
                        isValid = true;
                    }
                }
            }
            return isValid;
        }
    }

    public class MinimumLengthAttribute : ValidationAttribute
    {
        public int MinLength { get; set; }
        public override bool IsValid(object value)
        {
            string s = value as string;
            if (!string.IsNullOrEmpty(s))
            {
                return s.Length >= MinLength;
            }
            return false;
        }
    }
}