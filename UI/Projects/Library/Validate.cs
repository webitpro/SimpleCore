using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections;

namespace Core.Library
{

    public static partial class Utils
    {
        /// <summary>
        /// Contains methods to validate data
        /// </summary>
        public static class Validate
        {
            private static Regex p_EmailValidator = new Regex(@"^(?:[a-zA-Z0-9_'^&amp;/+-])+(?:\.(?:[a-zA-Z0-9_'^&amp;/+-])+)*@(?:(?:\[?(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\.){3}(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\]?)|(?:[a-zA-Z0-9-]+\.)+(?:[a-zA-Z]){2,}\.?)$");
            private static Regex p_UrlValidator = new Regex(@"^(?:^(?:http|https|ftp)\://)?[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(?::[a-zA-Z0-9]*)?/?(?:[a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*$");
            private static Regex p_timeValidator = new Regex(@"^((0?[1-9]|1[012]):([0-5]\d) ([AP]M))|((0[1-9]|1[0-9]|2[0-4]):([0-5]\d):([0-5]\d))$");
            private static Regex p_dateFormatValidator = new Regex(@"^(0?[1-9]|1[012])[/](0?[1-9]|[12][0-9]|3[01])[/](19|20)?\d\d$");
            private static Regex p_USZipValidator = new Regex(@"^([1-9][0-9]{4})(-[0-9]{4})?$");
            private static Regex p_USPhoneValidator = new Regex(@"^(([1-9][0-9]{2})[\-\.]([1-9][0-9]{2})[\-\.]([0-9]{4})|([1-9][0-9]{2}) ([1-9][0-9]{2}) ([0-9]{4})|(([(][1-9][0-9]{2}[)]) ([1-9][0-9]{2})-([0-9]{4})))( [x] ([1-9][0-9]{1,4}))?$");

           

            /// <summary>
            /// Validates an expression type
            /// </summary>
            /// <param name="strExpression">(string) string to validate</param>
            /// <returns>(ExpressionType) Empty, Alpha, Numeric, AlphaNumeric, NonAlphaNumeric, Mix</returns>
            public static Type.xPression ExpressionType(string strExpression)
            {
                bool blnNumeric = false;
                bool blnAlpha = false;
                bool blnSpecChar = false;

                if (strExpression.Length > 0)
                {
                    foreach (char c in strExpression)
                    {
                        if (Char.IsDigit(c))
                            blnNumeric = true;
                        else if (Char.IsLetter(c))
                            blnAlpha = true;
                        else if (!Char.IsLetterOrDigit(c) && c != ' ')
                            blnSpecChar = true;

                        if (blnAlpha && blnNumeric && blnSpecChar)
                        {
                            break;
                        }
                    }

                    if (blnNumeric && !blnAlpha)
                    {
                        if (blnSpecChar)
                        {
                            return Type.xPression.NumericMix;
                        }
                        else
                        {
                            return Type.xPression.Numeric;
                        }
                    }

                    if (!blnNumeric && blnAlpha)
                    {
                        if (blnSpecChar)
                        {
                            return Type.xPression.AlphaMix;
                        }
                        else
                        {
                            return Type.xPression.Alpha;
                        }
                    }

                    if (blnNumeric && blnAlpha)
                    {
                        if (blnSpecChar)
                        {
                            return Type.xPression.Mix;
                        }
                        else
                        {
                            return Type.xPression.AlphaNumeric;
                        }
                    }


                    if (!blnNumeric && !blnAlpha)
                    {
                        if (blnSpecChar)
                        {
                            return Type.xPression.SpecialCharacters;
                        }
                        else
                        {
                            return Type.xPression.Empty;
                        }
                    }
                }

                return Type.xPression.Empty;
            }

            /// <summary>
            /// Validates an E-mail
            /// </summary>
            /// <param name="strEmail">(string) E-mail address</param>
            /// <returns>(bool) true/false</returns>
            public static bool EmailAddress(string strEmail)
            {
                if (strEmail.Length > 0)
                {
                    return p_EmailValidator.IsMatch(strEmail);
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Validates a URL
            /// </summary>
            /// <param name="strURL">(string) URL address</param>
            /// <returns>(bool) true/false</returns>
            public static bool URL(string strURL)
            {
                if (strURL.Length > 0)
                    return p_UrlValidator.IsMatch(strURL);
                else
                    return false;
            }

            /// <summary>
            /// Validates US phone number
            /// </summary>
            /// <param name="phone">(string) phone number, dash [-] or dot[.] are allowed for deliminator between AC, Prefix, Suffix</param>
            /// <returns>(bool) true/false</returns>
            public static bool USPhone(string phone)
            {
                if (phone.Length > 0)
                {
                    return p_USPhoneValidator.IsMatch(phone);
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Validate phone number
            /// </summary>
            /// <param name="strCountryPrefix">(string) country prefix</param>
            /// <param name="strAC">(string) area code</param>
            /// <param name="strPrefix">(string) prefix</param>
            /// <param name="strSuffix">(string) suffix</param>
            /// <param name="strExt">(string) extension or empty string if doesn't exist</param>
            /// <returns>(bool) true/false</returns>
            public static bool Phone(string strCountryPrefix, string strAC, string strPrefix, string strSuffix, string strExt)
            {
                bool blnExt = true;
                if (strExt != "")
                {
                    //phone with extension
                    if (strExt.Length >= 1 && strExt.Length <= 5)
                    {
                        if (ExpressionType(strExt) != Type.xPression.Numeric)
                        {
                            blnExt = false;
                        }
                    }
                    else
                    {
                        blnExt = false;
                    }
                }

                if (blnExt)
                {
                    if (ExpressionType(strCountryPrefix) == Type.xPression.Numeric)
                    {
                        if (strCountryPrefix == "1")
                        {
                            //US / CA 1 509 456 5576
                            if (strAC.Length == 3 && strPrefix.Length == 3 && strSuffix.Length == 4)
                            {
                                if (ExpressionType(strAC) == Type.xPression.Numeric && ExpressionType(strPrefix) == Type.xPression.Numeric && ExpressionType(strSuffix) == Type.xPression.Numeric)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            //INTERNATIONAL 387 33 618 015 | 387 33 44 818
                            if (strAC.Length >= 2 && strPrefix.Length >= 2 && strSuffix.Length >= 3)
                            {
                                if (ExpressionType(strAC) == Type.xPression.Numeric && ExpressionType(strPrefix) == Type.xPression.Numeric && ExpressionType(strSuffix) == Type.xPression.Numeric)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }

            /// <summary>
            /// Validates Password format
            /// Default strength format: 6+ characters, 1+ lower alpha character, 1+ upper alpha character, 1+ numeric character, symbols allowed but not required 
            /// Lower strength format: 6+ characters alpha-numeric or mix (alpha-numeric + symbols)
            /// </summary>
            /// <param name="strPsw">(string) password</param>
            /// <param name="lowerStrength">(bool) true/false for lower strength password validation</param>
            /// <returns>(bool) true/false</returns>
            public static bool PasswordFormat(string strPsw, bool lowerStrength)
            {
                if (lowerStrength)
                {
                    Type.xPression exPression = ExpressionType(strPsw);

                    return (strPsw.Length > 5 && (exPression == Type.xPression.AlphaNumeric || exPression == Type.xPression.Mix));
                }

                if (strPsw.Length >= 6)
                {
                    Type.xPression exPression = ExpressionType(strPsw);

                    if (exPression == Type.xPression.AlphaNumeric || exPression == Type.xPression.Mix)
                    {
                        bool blnUpper = false;
                        bool blnLower = false;

                        foreach (char c in strPsw)
                        {
                            if (Char.IsUpper(c))
                            {
                                blnUpper = true;
                            }
                            else if (Char.IsLower(c))
                            {
                                blnLower = true;
                            }

                            if (blnUpper && blnLower)
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }

            /// <summary>
            /// Validates Default password strength format
            /// Default strength format: 6+ characters, 1+ lower alpha character, 1+ upper alpha character, 1+ numeric character, symbols allowed but not required 
            /// </summary>
            /// <param name="strPsw">(string) password</param>
            /// <returns>(bool) true/false</returns>
            public static bool PasswordFormat(string strPsw)
            {
                return PasswordFormat(strPsw, false);
            }

            /// <summary>
            /// Validates US zip code
            /// </summary>
            /// <param name="strZip">(string) US zip code</param>
            /// <returns>(bool) true/false</returns>
            public static bool USZipCode(string strZip)
            {
                if (strZip.Length > 0)
                {
                    if (strZip.Substring(1) != "0")
                    {
                        return p_USZipValidator.IsMatch(strZip);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Validates Zip Code (US and/or International)
            /// </summary>
            /// <param name="strZip">(string) numeric, alpha-numeric or mix(alpha-numeric with symbols (ex - )</param>
            /// <param name="bUSZipCode">(bool) true/false</param>
            /// <returns>(bool) true/false</returns>
            public static bool ZipCode(string strZip, bool bUSZipCode)
            {
                if (strZip.Length > 0)
                {
                    if (bUSZipCode)
                    {
                        //US Zip Code
                        return USZipCode(strZip);
                    }
                    else
                    {
                        //INTERNATIONAL Zip Code
                        if (ExpressionType(strZip) == Type.xPression.Numeric || ExpressionType(strZip) == Type.xPression.AlphaNumeric || ExpressionType(strZip) == Type.xPression.Mix)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }


            // validate cc number (Master Card, Visa, American Express, Diners  Club/Carte Blanche, enRoute, Discover, JCB
            /// <summary>
            /// Validates credit card number
            /// Master Card, Visa, American Express, Diners Club/Carte Blanche, enRoute, Discover, JCB
            /// </summary>
            /// <param name="strCC">(string) credit card number</param>
            /// <returns>(bool) true/false</returns>
            public static bool CreditCardNumber(string strCC)
            {
                try
                {
                    ArrayList arlCheckNumbers = new ArrayList();

                    int intCardLength = strCC.Length;

                    //double the value of alternate digits, starting with the second digit
                    //from the right, i.e. back to front.
                    //Loop through starting at the end
                    for (int i = intCardLength - 2; i >= 0; i = i - 2)
                    {
                        //now read the contents at each index, this
                        //can then be stored as an array of integers
                        //double the number returned
                        arlCheckNumbers.Add(Int32.Parse(strCC[i].ToString()) * 2);
                    }

                    int intCheckSum = 0; //will hold the total sum of all checksum digits

                    //second stage, add separate digits of all products
                    for (int j = 0; j <= arlCheckNumbers.Count - 1; j++)
                    {
                        int intDoubleDigitsSum = 0; //will hold the sum of all digits

                        //determine if current number has more than one digit
                        if ((int)arlCheckNumbers[j] > 9)
                        {
                            int intDigitLength = ((int)arlCheckNumbers[j]).ToString().Length;
                            //add count to each digit
                            for (int x = 0; x < intDigitLength; x++)
                            {
                                intDoubleDigitsSum += Int32.Parse(((int)arlCheckNumbers[j]).ToString()[x].ToString());
                            }
                        }
                        else
                        {
                            //single digit, just add it by itself
                            intDoubleDigitsSum += (int)arlCheckNumbers[j];
                        }
                        intCheckSum += intDoubleDigitsSum; // add sum to the total sum

                    }

                    //stage 3, add the unaffected digits
                    //add all the digits that we didn't double still starting from the 
                    //right but this time we'll start from the rightmost number with
                    //alternatig digits
                    int intOriginalSum = 0;
                    for (int y = intCardLength - 1; y >= 0; y = y - 2)
                    {
                        intOriginalSum = intOriginalSum + Int32.Parse(strCC[y].ToString());
                    }
                    //perform the final calculation, if the sum Mod 10 results in 0 then
                    //it's valid, otherwise its false


                    return (((intOriginalSum + intCheckSum) % 10) == 0);
                }
                catch
                {
                    return false;
                }

            }

            public enum ccType
            {
                MasterCard, 
                VISA, 
                American_Express, 
                Diners_Club_Carte_Blanche, 
                enRoute, 
                Discover, 
                JCB
            }
            /// <summary>
            /// Returns Card Type
            /// </summary>
            /// <param name="strCC">(string) credit card number</param>
            /// <param name="cardType">(ccType) card type</param>
            /// <returns>(bool) </returns>
            public static bool CardType(string strCC, ccType cardType)
            {
                string strCardType = "";

                if (strCC.Length == 16)
                {
                    if (Int32.Parse(strCC.Substring(0, 2)) >= 51 && Int32.Parse(strCC.Substring(0, 2)) <= 55)
                    {
                        strCardType = "MasterCard";
                    }
                    if (Int32.Parse(strCC.Substring(0, 1)) == 4)
                    {
                        strCardType = "VISA";
                    }
                    if (Int32.Parse(strCC.Substring(0, 4)) == 6011)
                    {
                        strCardType = "Discover";
                    }
                    if (Int32.Parse(strCC.Substring(0, 1)) == 3)
                    {
                        strCardType = "JCB";
                    }
                }
                else
                {
                    if (strCC.Length == 15)
                    {
                        if (Int32.Parse(strCC.Substring(0, 2)) == 34 || Int32.Parse(strCC.Substring(0, 2)) == 37)
                        {
                            strCardType = "American_Express";
                        }
                        if (Int32.Parse(strCC.Substring(0, 4)) == 2014 || Int32.Parse(strCC.Substring(0, 4)) == 2149)
                        {
                            strCardType = "enRoute";
                        }
                        if (Int32.Parse(strCC.Substring(0, 4)) == 2131 || Int32.Parse(strCC.Substring(0, 4)) == 1800)
                        {
                            strCardType = "JCB";
                        }
                    }
                    else
                    {
                        if (strCC.Length == 14)
                        {
                            if (Int32.Parse(strCC.Substring(0, 2)) == 36 || Int32.Parse(strCC.Substring(0, 2)) == 38 || (Int32.Parse(strCC.Substring(0, 3)) >= 300 && Int32.Parse(strCC.Substring(0, 3)) <= 305))
                            {
                                strCardType = "Diners_Club_Carte_Blache";
                            }
                        }
                        else
                        {
                            if (strCC.Length == 13)
                            {
                                if (Int32.Parse(strCC.Substring(0, 1)) == 4)
                                {
                                    strCardType = "VISA";
                                }
                            }
                            else
                            {
                                strCardType = "Unknown";
                            }
                        }
                    }
                }

                return ((strCardType.Equals(cardType.ToString())) ? true : false);
            }

            /// <summary>
            /// Validates date
            /// Checks for # of days in a month and leap years
            /// </summary>
            /// <param name="intMonth">(int) month</param>
            /// <param name="intDay">(int) day</param>
            /// <param name="intYear">(int) year</param>
            /// <returns>(bool) true/false</returns>
            public static bool Date(int intMonth, int intDay, int intYear)
            {
                if (intMonth == 1 || intMonth == 3 || intMonth == 5 || intMonth == 7 || intMonth == 8 || intMonth == 10 || intMonth == 12)
                {
                    return (intDay > 0 && intDay <= 31);
                }
                else if (intMonth == 2)
                {
                    if (DateTime.IsLeapYear(intYear))
                    {
                        return (intDay > 0 && intDay <= 29);
                    }
                    else
                    {
                        return (intDay > 0 && intDay <= 28);
                    }
                }
                else if (intMonth > 0 && intMonth < 12)
                {
                    return (intDay > 0 && intDay <= 30);
                }

                return false;
            }

            /// <summary>
            /// Validates if date format is valid
            /// accepted format [m]m/[d]d/[yy]yy
            /// dates between 1/1/1900 and 12/31/2099
            /// </summary>
            /// <param name="date">(string) date</param>
            /// <returns>(bool) true/false</returns>
            public static bool DateFormat(string date)
            {
                if (date.Length > 0)
                {
                    return p_dateFormatValidator.IsMatch(date);
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Validate if time format is valid
            /// accepted format [h]h:mm {A|P}M, hh:mm:ss
            /// </summary>
            /// <param name="time">(string) time</param>
            /// <returns>(bool) true/false</returns>
            public static bool Time(string time)
            {
                if (time.Length > 0)
                {
                    return p_timeValidator.IsMatch(time);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
