using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data.Sql;
using System.Data;


namespace GMSConApp
{
    public sealed class GMSUtil
    {
        static string[] powerONES = new string[] {"","One","Two","Three","Four","Five","Six","Seven","Eight","Nine","Ten",
                                                     "Eleven","Twelve","Thirteen","Fourteen","Fifteen","Sixteen","Seventeen",
                                                     "Eighteen","Nineteen"};
        static string[] powerTENS = new string[] {"","Ten","Twenty","Thirty","Forty","Fifty","Sixty","Seventy","Eighty",
                                                     "Ninety"};
        static string[] powerTHOU = new string[] { "", "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion" };

        static char[] hexDigits = {   '0', '1', '2', '3', '4', '5', '6', '7',
                                        '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

        /// <summary>
        /// The Default Date's value. Set to 01/01/1900.
        /// </summary>                                  
        public static readonly DateTime DEFAULTNODATE = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      
        /// <summary>
        /// The Default Serial No. Set to UNASSIGNED.
        /// </summary>                                  
        public static readonly string DEFAULTITEMSERIAL = "UNASSIGNED";

        #region TranslateNumberText
        /// <summary>
        /// Static function to Translate Numbers into Text
        /// </summary>
        /// <param name="number">Integer value of the number</param>
        /// <returns>String value</returns>
        public static string TranslateNumberText(int number)
        {
            if (number < 0)
                return "Error: Negative Amount.";

            if (number == 0)
                return "Zero";

            //---------------------------
            byte bytPowerLevel = 0;
            string strReturnValue = "";
            bool blnEnded = false;

            string strAmountStr = number.ToString();

            while (strAmountStr.Length > 0 && blnEnded == false)
            {
                int intValue = 0;

                if (strAmountStr.Length >= 3)
                    intValue = ToInt(strAmountStr.Substring(strAmountStr.Length - 3));
                else
                    intValue = ToInt(strAmountStr);

                int intDigit1 = intValue / 100;
                int intLast2 = intValue % 100;
                int intDigit2 = intLast2 / 10;
                int intDigit3 = intValue % 10;

                if (bytPowerLevel > 0 &&
                    (intDigit1 + intDigit2 + intDigit3) > 0)
                {
                    strReturnValue = powerTHOU[bytPowerLevel] + " " + strReturnValue;
                    strReturnValue = strReturnValue.Trim();
                }

                if (intLast2 > 0)
                {
                    //use ones only
                    if (intLast2 < 20)
                        strReturnValue = powerONES[intLast2] + " " + strReturnValue;
                    else
                        strReturnValue = powerTENS[intDigit2] + " " + powerONES[intDigit3] + " " + strReturnValue;
                }

                if (intDigit1 > 0)
                {
                    strReturnValue = powerONES[intDigit1] + " Hundred " + strReturnValue;
                }

                if ((strAmountStr.Length - 3) > 0)
                    strAmountStr = strAmountStr.Substring(0, strAmountStr.Length - 3);
                else
                    blnEnded = true;

                bytPowerLevel++;
            }


            return strReturnValue;
        }
        #endregion

        #region TranslateMoneyText
        /// <summary>
        /// Static function to Translate a Decimal value into money string, 
        /// e.g. 1001.10 = One Thousand and one dollars and one cent
        /// </summary>
        /// <param name="decimalValue">Decimal value to translate</param>
        /// <returns>String value</returns>
        public static string TranslateMoneyText(decimal decimalValue)
        {
            string[] arrValue = decimalValue.ToString("F").Split('.');

            if (arrValue.Length == 2)
            {
                string strReturnValue = "";

                strReturnValue = TranslateNumberText(ToInt(arrValue[0]));

                if (ToInt(arrValue[1]) == 0)
                    strReturnValue += " Only ";
                else
                    strReturnValue += " And Cents " + TranslateNumberText(ToInt(arrValue[1])) + " Only ";

                return strReturnValue;
            }
            return "Error: Invalid Decimal";
        }
        #endregion

        #region ConvertMoney
        /// <summary>
        /// Static function to Convert Money. Crucial for eliminating additional decimal places that can cause inaccurate values
        /// </summary>
        /// <param name="obj">Object to convert</param>
        /// <returns>Decimal value</returns>
        public static decimal ConvertMoney(object obj)
        {
            if (Convert.IsDBNull(obj) || obj == null)
                return 0M;

            try
            {
                decimal dec;
                if (obj is decimal)
                    dec = (decimal)obj;
                else
                    dec = Convert.ToDecimal(obj);

                return (decimal)System.Data.SqlTypes.SqlDecimal.Round((System.Data.SqlTypes.SqlDecimal)dec, 2);
            }
            catch
            {
                return 0M;
            }
        }
        #endregion

        #region ToInt
        /// <summary>
        /// Static function to check and return the value of an object that might be an Integer.
        /// </summary>
        /// <param name="obj">Object to be tested.</param>
        /// <returns>Integer value parsed. Returns zero if fail or empty.</returns>
        public static int ToInt(object obj)
        {
            if (Convert.IsDBNull(obj) || obj == null)
                return 0;

            try
            {
                if (obj is int)
                    return (int)obj;

                return int.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region ToByte
        /// <summary>
        /// Static function to check and return the value of an object that might be a Byte.
        /// </summary>
        /// <param name="obj">Object to be tested.</param>
        /// <returns>Byte value parsed. Returns zero if fail or empty.</returns>
        public static byte ToByte(object obj)
        {
            if (Convert.IsDBNull(obj) || obj == null)
                return 0;

            try
            {
                if (obj is byte)
                    return (byte)obj;

                return byte.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region ToShort
        /// <summary>
        /// Static function to check and return the value of an object that might be a Short.
        /// </summary>
        /// <param name="obj">Object to be tested.</param>
        /// <returns>Short value parsed. Returns zero if fail or empty.</returns>
        public static short ToShort(object obj)
        {
            if (Convert.IsDBNull(obj) || obj == null)
                return 0;

            try
            {
                if (obj is short)
                    return (short)obj;

                return short.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region ToDouble
        /// <summary>
        /// Static function to check and return the value of an object that might be an Double.
        /// </summary>
        /// <param name="obj">Object to be tested.</param>
        /// <returns>Double value parsed. Returns zero if fail or empty.</returns>
        public static double ToDouble(object obj)
        {
            if (Convert.IsDBNull(obj) || obj == null)
                return 0;

            try
            {
                if (obj is double)
                    return (double)obj;

                return double.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region ToLong
        /// <summary>
        /// Static function to check and return the value of an object that might be an long.
        /// </summary>
        /// <param name="obj">Object to be tested.</param>
        /// <returns>long value parsed. Returns zero if fail or empty.</returns>
        public static long ToLong(object obj)
        {
            if (Convert.IsDBNull(obj) || obj == null)
                return 0;

            try
            {
                if (obj is long)
                    return (long)obj;

                return long.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region ToFloat
        /// <summary>
        /// Static function to check and return the value of an object that might be a Float.
        /// </summary>
        /// <param name="obj">Object to be tested.</param>
        /// <returns>Float value parsed. Returns zero if fail or empty.</returns>
        public static float ToFloat(object obj)
        {
            if (Convert.IsDBNull(obj) || obj == null)
                return 0;

            try
            {
                if (obj is float)
                    return (float)obj;

                return float.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region ToDecimal
        /// <summary>
        /// Static function to check and return the value of an object that might be a Decimal.
        /// </summary>
        /// <param name="obj">Object to be tested.</param>
        /// <returns>Decimal value parsed. Returns zero if fail or empty.</returns>
        public static decimal ToDecimal(object obj)
        {
            if (Convert.IsDBNull(obj) || obj == null)
                return 0;

            try
            {
                if (obj is decimal)
                    return (decimal)obj;

                return decimal.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region ToStr
        /// <summary>
        /// Static function to check and return the value of an object that might be a string
        /// </summary>
        /// <param name="obj">Object to be tested.</param>
        /// <returns>String value parsed. Returns "" if fail or empty.</returns>
        public static string ToStr(object obj)
        {
            if (obj == null || Convert.IsDBNull(obj))
                return String.Empty;

            try
            {
                if (obj is string)
                    return (string)obj;

                return Convert.ToString(obj);
            }
            catch
            {
                return String.Empty;
            }
        }
        #endregion

        #region ToDate
        /// <summary>                                    
        /// Static function to check and return the value of a Date Object
        /// </summary>                                                    
        /// <param name="obj">Object to be tested</param>                 
        /// <returns>DateTime object passed. If null, set to default date of '01/01/1900'</returns>
        public static DateTime ToDate(object obj)
        {
            if (Convert.IsDBNull(obj))
                return DEFAULTNODATE;

            try
            {
                return DateTime.Parse(obj.ToString());
            }
            catch
            {
                return DEFAULTNODATE;
            }
        }
        #endregion

        #region CleanDate
        /// <summary>
        /// Static function to clean a DateTime variable's Time, e.g. 2005-05-01 21:22 to 2005-05-01 00:00
        /// </summary>
        /// <param name="dateToClear">DateTime value to clear</param>
        /// <returns>Cleaned DateTime</returns>
        public static DateTime CleanDate(DateTime dateToClear)
        {
            return new DateTime(dateToClear.Year, dateToClear.Month, dateToClear.Day);
        }
        #endregion

        #region ToGUID
        /// <summary>
        /// Static function to test if a given object is a GUID or not.
        /// </summary>
        /// <param name="obj">Object to be tested.</param>
        /// <returns>True or False of the Test.</returns>
        public static bool ToGUID(object obj)
        {
            if (Convert.IsDBNull(obj))
                return false;

            try
            {
                if (obj is Guid)
                    return true;

                if (obj is String)
                {
                    Regex myRegx = new Regex(@"\{?[a-fA-F\d]{1,8}-(?:[a-fA-F\d]{1,4}-){3}[a-fA-F\d]{1,12}\}?");
                    return myRegx.IsMatch((string)obj);
                }
            }
            catch
            {
            }

            return false;
        }
        #endregion

        #region ToHexString
        /// <summary>
        /// Static function to Change a Byte array to a Hex STring
        /// </summary>
        /// <param name="bytes">Byte array</param>
        /// <returns>Hex string</returns>
        public static string ToHexString(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return "";

            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return new string(chars);
        }
        #endregion

               

        #region TruncDisplay
        /// <summary>
        /// Static function to Truncate a String for Display purposes by replacing the characters
        /// at the specified length with '...'.
        /// </summary>
        /// <param name="input">String to truncate</param>
        /// <param name="lengthToCut">Length of the Characters to cut off at</param>
        /// <returns>Trunctated String</returns>         		                         		
        public static string TruncDisplay(string input, int lengthToCut)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (input.Length > lengthToCut)
                    return input.Substring(0, lengthToCut - 3).TrimEnd() + "...";
                else
                    return input;
            }
            return "";
        }
        #endregion

        #region TruncText
        /// <summary>
        /// Static function to Truncate a String for persist purposes
        /// </summary>
        /// <param name="input">String to truncate</param>
        /// <param name="lengthToCut">Length of the Characters to cut off at</param>
        /// <returns>Trunctated String</returns>         		                         		
        public static string TruncText(string input, int lengthToCut)
        {
            if (!string.IsNullOrEmpty(input))
            {
                if (input.Length > lengthToCut)
                    return input.Substring(0, lengthToCut);
                else
                    return input;
            }
            return "";
        }
        #endregion

        #region ToJson
        /// <summary>
        /// Static function to convert dataset to json format dynamically
        /// </summary>   		                         
        public static List<Dictionary<string, string>> ToJson(DataSet ds, int tablenumber)
        {
            if (Convert.IsDBNull(ds) || ds == null)
                return new List<Dictionary<string, string>>();

            try
            {
                List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[tablenumber].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tablenumber].Rows)
                    {
                        Dictionary<string, string> list2 = new Dictionary<string, string>();
                        foreach (DataColumn dc in ds.Tables[tablenumber].Columns)
                        {
                            string s = dc.ColumnName.ToString();
                            list2.Add(s, dr[s].ToString());
                        }
                        list.Add(list2);
                    }
                    return list;
                }
                else
                    return new List<Dictionary<string, string>>();
            }
            catch
            {
                return new List<Dictionary<string, string>>();
            }
        }


        #endregion



    }
}
