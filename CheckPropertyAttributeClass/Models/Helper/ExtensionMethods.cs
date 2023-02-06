using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System;

namespace CheckPropertyAttributeClass.Models.Helper
{
    public static class ExtensionMethods
    {
        public static string ReplaceAppSettings(this string responseBody)
        {
            var r = new Regex("{{AppSettings\\((.*?)\\)}}");
            while (r.IsMatch(responseBody))
            {
                var match = r.Match(responseBody);
                var settingsName = match.Groups[1].ToString();
              //  var value = SystemSettings.RequestManagerOptions.MediatorOptions[settingsName];

              //  responseBody = responseBody.Replace(match.Groups[0].ToString(), value);

            }
            return responseBody;
        }

        public static string ReplaceAddedDatetimeYears(this string responseBody)
        {
            var r = new Regex("{{AddedDatetimeYear\\((.*?)\\)}}");
            var now = DateTime.Now;
            while (r.IsMatch(responseBody))
            {
                var match = r.Match(responseBody);
                var datetimePatern = match.Groups[1].ToString().Split(',');

                int.TryParse(datetimePatern[0], out var year);
                now = now.AddYears(year);

                responseBody = responseBody
                    .Replace(match.Groups[0].ToString(), now.ToString(datetimePatern[1]));

            }
            return responseBody;
        }
        public static string ReplaceAddedDatetimeMonths(this string responseBody)
        {
            var r = new Regex("{{AddedDatetimeMonth\\((.*?)\\)}}");
            var now = DateTime.Now;
            while (r.IsMatch(responseBody))
            {
                var match = r.Match(responseBody);
                var datetimePatern = match.Groups[1].ToString().Split(',');

                int.TryParse(datetimePatern[0], out var addValue);
                now = now.AddMonths(addValue);

                responseBody = responseBody
                    .Replace(match.Groups[0].ToString(), now.ToString(datetimePatern[1]));

            }
            return responseBody;
        }
        public static string ReplaceAddedDatetimeDays(this string responseBody)
        {
            var r = new Regex("{{AddedDatetimeDay\\((.*?)\\)}}");
            var now = DateTime.Now;
            while (r.IsMatch(responseBody))
            {
                var match = r.Match(responseBody);
                var datetimePatern = match.Groups[1].ToString().Split(',');

                int.TryParse(datetimePatern[0], out var addValue);
                now = now.AddDays(addValue);

                responseBody = responseBody
                    .Replace(match.Groups[0].ToString(), now.ToString(datetimePatern[1]));

            }
            return responseBody;
        }
        public static string ReplaceAddedDatetimeHours(this string responseBody)
        {
            var r = new Regex("{{AddedDatetimeHour\\((.*?)\\)}}");
            var now = DateTime.Now;
            while (r.IsMatch(responseBody))
            {
                var match = r.Match(responseBody);
                var datetimePatern = match.Groups[1].ToString().Split(',');

                int.TryParse(datetimePatern[0], out var addValue);
                now = now.AddHours(addValue);

                responseBody = responseBody
                    .Replace(match.Groups[0].ToString(), now.ToString(datetimePatern[1]));

            }
            return responseBody;
        }
        public static string ReplaceAddedDatetimeMinutes(this string responseBody)
        {
            var r = new Regex("{{AddedDatetimeMinute\\((.*?)\\)}}");
            var now = DateTime.Now;
            while (r.IsMatch(responseBody))
            {
                var match = r.Match(responseBody);
                var datetimePatern = match.Groups[1].ToString().Split(',');

                int.TryParse(datetimePatern[0], out var addValue);
                now = now.AddMinutes(addValue);

                responseBody = responseBody
                    .Replace(match.Groups[0].ToString(), now.ToString(datetimePatern[1]));

            }
            return responseBody;
        }
        public static string ReplaceAddedDatetimeSeconds(this string responseBody)
        {
            var r = new Regex("{{AddedDatetimeSecond\\((.*?)\\)}}");
            var now = DateTime.Now;
            while (r.IsMatch(responseBody))
            {
                var match = r.Match(responseBody);
                var datetimePatern = match.Groups[1].ToString().Split(',');

                int.TryParse(datetimePatern[0], out var addValue);
                now = now.AddSeconds(addValue);

                responseBody = responseBody
                    .Replace(match.Groups[0].ToString(), now.ToString(datetimePatern[1]));

            }
            return responseBody;
        }
        public static string ReplaceAddedDatetimeMiliseconds(this string responseBody)
        {
            var r = new Regex("{{AddedDatetimeMilisecond\\((.*?)\\)}}");
            var now = DateTime.Now;
            while (r.IsMatch(responseBody))
            {
                var match = r.Match(responseBody);
                var datetimePatern = match.Groups[1].ToString().Split(',');

                int.TryParse(datetimePatern[0], out var addValue);
                now = now.AddMilliseconds(addValue);

                responseBody = responseBody
                    .Replace(match.Groups[0].ToString(), now.ToString(datetimePatern[1]));

            }
            return responseBody;
        }


        public static string ReplaceAddedDatetimePersianYears(this string responseBody)
        {
            var r = new Regex("{{AddedPDatetimeYear\\((.*?)\\)}}");
            var now = DateTime.Now;
            while (r.IsMatch(responseBody))
            {
                var match = r.Match(responseBody);
                var datetimePatern = match.Groups[1].ToString().Split(',');
                var pc = new PersianCalendar();

                int.TryParse(datetimePatern[0], out var year);
                now = pc.AddYears(now, year);

                var datetime = new DateTime(pc.GetYear(now), 1, 1, pc.GetHour(now), pc.GetMinute(now), pc.GetSecond(now), (int)pc.GetMilliseconds(now));

                datetimePatern[1] = datetimePatern[1]
                        .Replace("MM", "{2}")
                        .Replace("M", "{1}")
                        .Replace("dd", "{4}")
                        .Replace("d", "{3}");

                responseBody = responseBody
                    .Replace(match.Groups[0].ToString(), datetime.ToString(datetimePatern[1]));

                responseBody = responseBody
                    .Replace("{1}", pc.GetMonth(now).ToString())
                    .Replace("{2}", pc.GetMonth(now).ToString().PadLeft(2, '0'))
                    .Replace("{3}", pc.GetDayOfMonth(now).ToString())
                    .Replace("{4}", pc.GetDayOfMonth(now).ToString().PadLeft(2, '0'));
            }
            return responseBody;
        }
        public static string ReplaceAddedDatetimePersianMonths(this string responseBody)
        {
            var r = new Regex("{{AddedPDatetimeMonth\\((.*?)\\)}}");
            var now = DateTime.Now;
            while (r.IsMatch(responseBody))
            {
                var match = r.Match(responseBody);
                var datetimePatern = match.Groups[1].ToString().Split(',');
                var pc = new PersianCalendar();

                int.TryParse(datetimePatern[0], out var month);
                now = pc.AddMonths(now, month);
                var datetime = new DateTime(pc.GetYear(now), 1, 1, pc.GetHour(now), pc.GetMinute(now), pc.GetSecond(now), (int)pc.GetMilliseconds(now));

                datetimePatern[1] = datetimePatern[1]
                    .Replace("MM", "{2}")
                    .Replace("M", "{1}")
                    .Replace("dd", "{4}")
                    .Replace("d", "{3}");

                responseBody = responseBody
                    .Replace(match.Groups[0].ToString(), datetime.ToString(datetimePatern[1]));

                responseBody = responseBody
                    .Replace("{1}", pc.GetMonth(now).ToString())
                    .Replace("{2}", pc.GetMonth(now).ToString().PadLeft(2, '0'))
                    .Replace("{3}", pc.GetDayOfMonth(now).ToString())
                    .Replace("{4}", pc.GetDayOfMonth(now).ToString().PadLeft(2, '0'));
            }
            return responseBody;
        }
        public static string ReplaceAddedDatetimePersianDays(this string responseBody)
        {
            var r = new Regex("{{AddedPDatetimeDay\\((.*?)\\)}}");
            var now = DateTime.Now;
            while (r.IsMatch(responseBody))
            {
                var match = r.Match(responseBody);
                var datetimePatern = match.Groups[1].ToString().Split(',');
                var pc = new PersianCalendar();

                int.TryParse(datetimePatern[0], out var days);
                now = pc.AddDays(now, days);
                var datetime = new DateTime(pc.GetYear(now), 1, 1, pc.GetHour(now), pc.GetMinute(now), pc.GetSecond(now), (int)pc.GetMilliseconds(now));

                datetimePatern[1] = datetimePatern[1]
                    .Replace("MM", "{2}")
                    .Replace("M", "{1}")
                    .Replace("dd", "{4}")
                    .Replace("d", "{3}");

                responseBody = responseBody
                    .Replace(match.Groups[0].ToString(), datetime.ToString(datetimePatern[1]));

                responseBody = responseBody
                    .Replace("{1}", pc.GetMonth(now).ToString())
                    .Replace("{2}", pc.GetMonth(now).ToString().PadLeft(2, '0'))
                    .Replace("{3}", pc.GetDayOfMonth(now).ToString())
                    .Replace("{4}", pc.GetDayOfMonth(now).ToString().PadLeft(2, '0'));
            }
            return responseBody;
        }
        public static string ReplaceAddedDatetimePersianHours(this string responseBody)
        {
            var r = new Regex("{{AddedPDatetimeHour\\((.*?)\\)}}");
            var now = DateTime.Now;
            while (r.IsMatch(responseBody))
            {
                var match = r.Match(responseBody);
                var datetimePatern = match.Groups[1].ToString().Split(',');
                var pc = new PersianCalendar();

                int.TryParse(datetimePatern[0], out var hour);
                now = pc.AddHours(now, hour);
                var datetime = new DateTime(pc.GetYear(now), 1, 1, pc.GetHour(now), pc.GetMinute(now), pc.GetSecond(now), (int)pc.GetMilliseconds(now));

                datetimePatern[1] = datetimePatern[1]
                    .Replace("MM", "{2}")
                    .Replace("M", "{1}")
                    .Replace("dd", "{4}")
                    .Replace("d", "{3}");

                responseBody = responseBody
                    .Replace(match.Groups[0].ToString(), datetime.ToString(datetimePatern[1]));

                responseBody = responseBody
                    .Replace("{1}", pc.GetMonth(now).ToString())
                    .Replace("{2}", pc.GetMonth(now).ToString().PadLeft(2, '0'))
                    .Replace("{3}", pc.GetDayOfMonth(now).ToString())
                    .Replace("{4}", pc.GetDayOfMonth(now).ToString().PadLeft(2, '0'));
            }
            return responseBody;
        }
        public static string ReplaceAddedDatetimePersianMinutes(this string responseBody)
        {
            var r = new Regex("{{AddedPDatetimeMinutes\\((.*?)\\)}}");
            var now = DateTime.Now;
            while (r.IsMatch(responseBody))
            {
                var match = r.Match(responseBody);
                var datetimePatern = match.Groups[1].ToString().Split(',');
                var pc = new PersianCalendar();

                int.TryParse(datetimePatern[0], out var minutes);
                now = pc.AddMinutes(now, minutes);
                var datetime = new DateTime(pc.GetYear(now), 1, 1, pc.GetHour(now), pc.GetMinute(now), pc.GetSecond(now), (int)pc.GetMilliseconds(now));

                datetimePatern[1] = datetimePatern[1]
                    .Replace("MM", "{2}")
                    .Replace("M", "{1}")
                    .Replace("dd", "{4}")
                    .Replace("d", "{3}");

                responseBody = responseBody
                    .Replace(match.Groups[0].ToString(), datetime.ToString(datetimePatern[1]));

                responseBody = responseBody
                    .Replace("{1}", pc.GetMonth(now).ToString())
                    .Replace("{2}", pc.GetMonth(now).ToString().PadLeft(2, '0'))
                    .Replace("{3}", pc.GetDayOfMonth(now).ToString())
                    .Replace("{4}", pc.GetDayOfMonth(now).ToString().PadLeft(2, '0'));
            }
            return responseBody;
        }
        public static string ReplaceAddedDatetimePersianSeconds(this string responseBody)
        {
            var r = new Regex("{{AddedPDatetimeSecond\\((.*?)\\)}}");
            var now = DateTime.Now;
            while (r.IsMatch(responseBody))
            {
                var match = r.Match(responseBody);
                var datetimePatern = match.Groups[1].ToString().Split(',');
                var pc = new PersianCalendar();

                int.TryParse(datetimePatern[0], out var seconds);
                now = pc.AddSeconds(now, seconds);
                var datetime = new DateTime(pc.GetYear(now), 1, 1, pc.GetHour(now), pc.GetMinute(now), pc.GetSecond(now), (int)pc.GetMilliseconds(now));

                datetimePatern[1] = datetimePatern[1]
                    .Replace("MM", "{2}")
                    .Replace("M", "{1}")
                    .Replace("dd", "{4}")
                    .Replace("d", "{3}");

                responseBody = responseBody
                    .Replace(match.Groups[0].ToString(), datetime.ToString(datetimePatern[1]));

                responseBody = responseBody
                    .Replace("{1}", pc.GetMonth(now).ToString())
                    .Replace("{2}", pc.GetMonth(now).ToString().PadLeft(2, '0'))
                    .Replace("{3}", pc.GetDayOfMonth(now).ToString())
                    .Replace("{4}", pc.GetDayOfMonth(now).ToString().PadLeft(2, '0'));
            }
            return responseBody;
        }
        public static string ReplaceAddedDatetimePersianMiliseconds(this string responseBody)
        {
            var r = new Regex("{{AddedPDatetimeMiliseconds\\((.*?)\\)}}");
            var now = DateTime.Now;
            while (r.IsMatch(responseBody))
            {
                var match = r.Match(responseBody);
                var datetimePatern = match.Groups[1].ToString().Split(',');
                var pc = new PersianCalendar();

                double.TryParse(datetimePatern[0], out var milisec);
                now = pc.AddMilliseconds(now, milisec);
                var datetime = new DateTime(pc.GetYear(now), 1, 1, pc.GetHour(now), pc.GetMinute(now), pc.GetSecond(now), (int)pc.GetMilliseconds(now));

                datetimePatern[1] = datetimePatern[1]
                    .Replace("MM", "{2}")
                    .Replace("M", "{1}")
                    .Replace("dd", "{4}")
                    .Replace("d", "{3}");

                responseBody = responseBody
                    .Replace(match.Groups[0].ToString(), datetime.ToString(datetimePatern[1]));

                responseBody = responseBody
                    .Replace("{1}", pc.GetMonth(now).ToString())
                    .Replace("{2}", pc.GetMonth(now).ToString().PadLeft(2, '0'))
                    .Replace("{3}", pc.GetDayOfMonth(now).ToString())
                    .Replace("{4}", pc.GetDayOfMonth(now).ToString().PadLeft(2, '0'));
            }
            return responseBody;
        }

        //public static string GetAccountTypeValue(this int index)
        //{
        //    return Enum.IsDefined(typeof(AccountTypeEnum), index) ? ((AccountTypeEnum)index).ToString() : "OTHER";
        //}
        public static bool ToSafeBool(this object obj)
        {
            bool.TryParse(obj.ToString(), out bool res);
            return res;
        }

        public static string ToSafeString(this object obj)
        {
            // returns the left-hand operand if the operand is not null; otherwise it returns string.Empty.
            return (obj ?? string.Empty).ToString();
        }

        //public static double GetProgressedDateTime(this RequestResponseModel requestResponseModel, DateTime? finishedTime = null)
        //{
        //    try
        //    {
        //        finishedTime = finishedTime ?? DateTime.Now;
        //        return (finishedTime.Value - requestResponseModel.RequestDateTime).TotalMilliseconds;
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //}

        //public static async Task<byte[]> GetMultipartBodyBytes(this byte[] inputBytes, string contentType)
        //{
        //    var bodyStream = new MemoryStream(inputBytes);
        //    var content = (HttpContent)new StreamContent(bodyStream);
        //    content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(contentType);
        //    var multipartProvider = new MultipartMemoryStreamProvider();
        //    var multipartContent = await content.ReadAsMultipartAsync(multipartProvider);
        //    var bytes = await multipartContent.Contents.FirstOrDefault()?.ReadAsByteArrayAsync();
        //    if (bytes != null)
        //        inputBytes = bytes;

        //    return inputBytes;
        //}

        public static string ToSafeSepahDate(this object obj)
        {
            var str = obj.ToSafeString().Replace("/", "");
            return (obj != null) ? (str.Length > 8) ? str.Substring(0, 8) : str : string.Empty;
        }

        public static string ToZeroDefault(this object obj)
        {
            string result = obj.ToSafeString();
            result = string.IsNullOrWhiteSpace(result) ? "0" : result;
            return result;
        }

        public static int ToSafeInt(this object obj, int defaultValue = 0)
        {
            var flag = int.TryParse(obj.ToSafeString(), out int res);
            return flag ? res : defaultValue;
        }
        public static long ToSafeLong(this string obj, long defaultValue = 0)
        {
            var flag = long.TryParse(obj, out long res);
            return flag ? res : defaultValue;
        }
        public static double ToSafeDouble(this string obj, double defaultValue = 0)
        {
            var flag = double.TryParse(obj, out double res);
            return flag ? res : defaultValue;
        }

        public static decimal ToSafeDecimal(this string obj, decimal defaultValue = 0)
        {
            var flag = decimal.TryParse(obj, out decimal res);
            return flag ? res : defaultValue;
        }

        public static string ToSafeNullEmpty(this string obj, string result)
        {
            if (!string.IsNullOrEmpty(obj))
                return obj;
            else
                return result;
        }

        public static string ToChangeValueOfNotNullEmpty(this string obj, string result)
        {
            if (!string.IsNullOrEmpty(obj))
                return result;
            else
                return string.Empty;
        }

        public static string ToSafeDigit(this string str)
        {
            return !string.IsNullOrWhiteSpace(str) && str.All(char.IsDigit) ? str : "0";
        }


        //public static T JsonDeserialize<T>(this string jsonString)
        //{
        //    var result = default(T);
        //    List<string> errors = new List<string>();
        //    List<ErrorItemWithValue> errorList = null;
        //    try
        //    {
        //        result = JsonConvert.DeserializeObject<T>(jsonString, new JsonSerializerSettings()
        //        {
        //            Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
        //            {
        //                if (errorList == null)
        //                    errorList = new List<ErrorItemWithValue>();


        //                var exception = new JException(args.ErrorContext.Error.Message, new List<ErrorItem> { new ErrorItemWithValue { Code = 1038 } });
        //                if (args.ErrorContext.Error is JException error)
        //                    exception = error;

        //                var pathSections = args.ErrorContext?.Path?.SplitString(".")?.ToList();
        //                var errorPropertyName = !(args.ErrorContext.Member is null) && !decimal.TryParse(args.ErrorContext.Member.ToSafeString(), out _) ? args.ErrorContext.Member.ToString() : pathSections.LastOrDefault();
        //                errors.Add(errorPropertyName);

        //                string path;
        //                if (!string.IsNullOrEmpty(args.ErrorContext.Path) && args.ErrorContext.Path.ToLower() != errorPropertyName.ToLower())
        //                    path = args.ErrorContext.Path.ToLower().EndsWith($".{errorPropertyName.ToLower()}")
        //                    ? args.ErrorContext.Path
        //                    : $"{args.ErrorContext.Path}.{errorPropertyName}";
        //                else
        //                    path = errorPropertyName;

        //                foreach (ErrorItemWithValue item in exception.ErrorItems)
        //                    if (!errorList.Any(a => a.Code == item.Code && string.Equals(a.ParamName, errorPropertyName, StringComparison.OrdinalIgnoreCase) && string.Equals(a.ParamPath, path, StringComparison.OrdinalIgnoreCase)))
        //                    {
        //                        var val = item.Values != null && item.Values.Any() ? item.Values : null;
        //                        if (val == null)
        //                        {
        //                            try
        //                            {
        //                                var originalVal = JObject.Parse(jsonString).SelectToken(path);
        //                                if (originalVal != null && originalVal.ToString() != JValue.CreateNull().ToString())
        //                                    val = new List<object>() { originalVal };
        //                            }
        //                            catch (Exception)
        //                            {
        //                            }
        //                        }
        //                        errorList.Add(new ErrorItemWithValue
        //                        {
        //                            ParamName = errorPropertyName,
        //                            Code = item.Code,
        //                            Desc = item.Desc,
        //                            ParamPath = path,
        //                            Values = val
        //                        });


        //                    };

        //                args.ErrorContext.Handled = true;
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Data.Add("Type", typeof(T));
        //    }

        //    if (result == null || errors.Any())
        //    {
        //        var exception = new JsonSerializationException();
        //        exception.Data.Add("JsonSerializationRequiredError", errors);
        //        StaticLogger.Logger.Warning($"JsonDeserilizerValidation. Errors: {errors.Aggregate((a, b) => a + "," + b)}");
        //        throw new MediatorException(exception.Message, 1038, errorList, errors);
        //    }

        //    return result;
        //}


        //public static T JsonDeserializeWithCheckDuplicate<T>(this string jsonString)
        //{
        //    var result = default(T);
        //    List<string> errors = new List<string>();
        //    List<ErrorItemWithValue> errorList = null;
        //    try
        //    {
        //        JToken.Parse(jsonString.ToLower(), new JsonLoadSettings { DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Error });

        //        result = JsonDeserialize<T>(jsonString);
        //    }
        //    catch (JsonReaderException e)
        //    {
        //        if (errorList == null)
        //            errorList = new List<ErrorItemWithValue>();

        //        var originalVal = JToken.Parse(jsonString).SelectToken(e.Path);
        //        errorList.Add(new ErrorItemWithValue
        //        {
        //            ParamName = e.Path.SplitString(".")?.ToList().LastOrDefault(),
        //            Code = 1733,
        //            Desc = "ورودی درخواست دارای پارامتر تکراری می باشد",
        //            ParamPath = e.Path,
        //            Values = new List<object>() { originalVal }
        //        });
        //        throw new MediatorException(e.Message, 1038, errorList, errors);
        //    }

        //    return result;
        //}

        public static T JsonDeserializeUnsafe<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
        //public static string ToJson(this object obj, bool setDatetimeFormat, string datetimeFormat = "")
        //{
        //    var result = "";

        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(datetimeFormat))
        //            datetimeFormat = SystemSettings.RequestManagerOptions.DatetimeFormat;


        //        result = SystemSettings.RequestManagerOptions.NormalizeDatetimeOnJson && setDatetimeFormat ?
        //            JsonConvert.SerializeObject(obj, new IsoDateTimeConverter { DateTimeFormat = datetimeFormat, Culture = CultureInfo.InvariantCulture }) :
        //            JsonConvert.SerializeObject(obj);
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return result;
        //}
        public static string ToJson(this object obj)
        {
            var result = "";

            try
            {
                result = JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public static double DoubleToNonExponential(this object obj)
        {
            double result = 0;

            try
            {
                result = Convert.ToDouble(obj);
            }
            catch (Exception ex)
            {
           //     StaticLogger.Logger.Error(ex, "DoubleToNonExponential: " + ex.Message);
            }

            return result;

        }


        //public static string ToSafeJson(this object obj)
        //{
        //    var result = "";
        //    try
        //    {

        //        if (SystemSettings.RequestManagerOptions != null && SystemSettings.RequestManagerOptions.NormalizeDatetimeOnJson)
        //            result = JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
        //            {
        //                DateFormatString = SystemSettings.RequestManagerOptions.DatetimeFormat,
        //                Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
        //                {
        //                    args.ErrorContext.Handled = true;
        //                }
        //            });
        //        else
        //            result = JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
        //            {
        //                Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
        //                {
        //                    args.ErrorContext.Handled = true;
        //                }
        //            });
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return result;
        //}

        //public static string ToJsonWithSafeTime(this object obj, string dateTimeFormat = "")
        //{
        //    var result = "";

        //    try
        //    {
        //        var format = string.IsNullOrWhiteSpace(dateTimeFormat)
        //            ? SystemSettings.RequestManagerOptions?.DatetimeFormat
        //            : dateTimeFormat;

        //        result = JsonConvert.SerializeObject(obj, new IsoDateTimeConverter { DateTimeFormat = format, Culture = CultureInfo.InvariantCulture });
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return result;
        //}

        /*public static void GetPropertyValue(Type type, string propertyName, ref string displayName)
        {
            MemberInfo property = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
            {
                foreach (PropertyInfo item in type.GetProperties())
                {
                    GetPropertyValue(item.PropertyType, propertyName, ref displayName);
                }
            }
            else
            {
                var attribute = property.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>().SingleOrDefault();
                displayName = string.IsNullOrWhiteSpace(attribute?.Description) ? propertyName : attribute?.Description;
            }
        }*/


        //public static async Task<T> XmlDeserialize<T>(this string xmlString)
        //{
        //    return await Task.Run(() =>
        //    {
        //        var reader = new XmlTextReader(new StringReader(xmlString));
        //        var serializer = new XmlSerializer(typeof(T));
        //        var serializedResult = (T)serializer.Deserialize(reader);
        //        return Task.FromResult(serializedResult);
        //    });
        //}
        public static string ToHMAC_SHA256(this string message, string secret)
        {
            var encoding = new UTF8Encoding();
            var key = encoding.GetBytes(secret);
            var myhmacsha256 = new HMACSHA256(key);
            var byteArray = encoding.GetBytes(message);
            var stream = new MemoryStream(byteArray);
            var result = myhmacsha256.ComputeHash(stream)
                .Aggregate("", (s, e) => s + $"{e:x2}", s => s.ToUpper());

            return result;
        }
        public static string ToSha256(this string rawData)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return bytes.Aggregate("", (s, e) => s + $"{e:x2}", s => s.ToUpper());
            }
        }

        public static string SignWithRsa(this string text, string key)
        {
            var provider = new RSACryptoServiceProvider();
            provider.FromXmlString(key);
            var signature = provider.SignHash(text.ToHashText(), CryptoConfig.MapNameToOID("SHA1"));
            return signature.Aggregate("", (s, e) => s + $"{e:x2}", s => s.ToUpper());
        }

        public static bool Verify(this string text, string signature, string key)
        {
            var provider = new RSACryptoServiceProvider();
            provider.FromXmlString(key);
            var hash = text.ToHashText();

            var result = provider.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), FromHex(signature));
            return result;
        }
        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }
        private static byte[] ToHashText(this string text)
        {
            var sha1Hasher = new SHA1Managed();
            var encoding = new UTF8Encoding();
            var data = encoding.GetBytes(text);
            var hash = sha1Hasher.ComputeHash(data);
            return hash;
        }

        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            TValue value;
            return dict.TryGetValue(key, out value) ? value : defaultValue;
        }

        public static int GetStringCheckSum(this string input) => input.Sum(c => c);

        public static string ToGregorianDate(this string shamsiDate)
        {
            var splitedShamsiDate = shamsiDate.Split('/');

            PersianCalendar pc = new PersianCalendar();
            DateTime dt = new DateTime(splitedShamsiDate[0].ToSafeInt(), splitedShamsiDate[1].ToSafeInt(), splitedShamsiDate[2].ToSafeInt(), pc);
            string gregorianDate = dt.ToShortDateString().ToSafeString();
            var splitedGregorian = gregorianDate.Split('/');
            return splitedGregorian[2] + "/" + splitedGregorian[1] + "/" + splitedGregorian[0];
        }

        //public static (string docType, string faName, string accountNumber) GetDocumentType(this string input)
        //{
        //    if (input.Contains("@"))
        //        return (DocumentType.Topic.ToString().ToUpper(), "سرفصل", input.Replace("@", ""));

        //    if (Regex.Match(input, SystemSettings.RequestManagerOptions.IBANTypePattern).Success)
        //        return (DocumentType.Deposit.ToString().ToUpper(), "شبا", input);

        //    if (Regex.Match(input, SystemSettings.RequestManagerOptions.AccountTypePattern).Success)
        //        return (DocumentType.Account.ToString().ToUpper(), "حساب", input);

        //    //if (Regex.Match(input, StaticOptions.Options.DepositTypePattern).Success)
        //    return (DocumentType.Deposit.ToString().ToUpper(), "سپرده", input);

        //    //return Regex.Match(input, StaticOptions.Options.AccountTypePattern).Success ?
        //    //    (DocumentType.Account.ToString().ToUpper() ,"حساب"): 
        //    //    (DocumentType.Topic.ToString().ToUpper(),"سرفصل");
        //}
        public static string GetDocumentName(this string input)
        {
            var res = "";
            switch (input.ToUpper())
            {
                case "TOPIC":
                    res = "سرفصل";
                    break;
                case "DEPOSIT":
                    res = "سپرده";
                    break;
                case "ACCOUNT":
                    res = "حساب";
                    break;
                case "VIRTUALBOX":
                    res = "صندوق مجازی";
                    break;
            }
            return res;
        }
        public static string[] SplitString(this string input, string splitter)
        {
            return input.Split(new[] { splitter }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string ToSepahDate(this DateTime dateTime)
        {
            var pc = new PersianCalendar();
            return $"{pc.GetYear(dateTime):0000}{pc.GetMonth(dateTime):00}{pc.GetDayOfMonth(dateTime):00}";
        }

        public static string ToSepahTime(this DateTime dateTime)
        {
            return $"{dateTime.Hour:00}{dateTime.Minute:00}{dateTime.Second:00}{dateTime.Millisecond:000}";
        }

        public static DateTime? ToDateTimeWithFormatUnsafe(this string input, string format)
        {
            return DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : (DateTime?)null;
        }

        public static DateTime ToDateTimeWithFormat(this string input, string format)
        {
            return DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : DateTime.Now;
        }

        public static string ToDateTimeStringWithFormat(this string input, string inputFormat, string outputFormat)
        {
            return ToDateTimeWithFormat(input, inputFormat).ToString(outputFormat);
        }

        public static string ToGregorianDateTimeStringWithFormat(this string input, string inputFormat, string outputFormat)
        {
            var datetime = ToDateTimeWithFormat(input, inputFormat);
            PersianCalendar pc = new PersianCalendar();
            DateTime dt = new DateTime(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second, datetime.Millisecond, pc);
            return dt.ToString(outputFormat);
        }

        //public static bool IsPersianDateValid(this string input, out PersianDateTimeHelper datetime)
        //{
        //    return PersianDateTimeHelper.TryParse(input, out datetime);
        //}

        //public static bool IsPersianDateValid(this string input)
        //{
        //    return PersianDateTimeHelper.TryParse(input, out var datetime);
        //}

        //public static string CheckPersianDateReturnByFormat(this string input, string outPutFormat = "")
        //{
        //    var flag = PersianDateTimeHelper.TryParse(input, out var persianDate);
        //    return flag ? persianDate.ToString(string.IsNullOrEmpty(outPutFormat) ? "yyyy/MM/dd" : outPutFormat) : string.Empty;
        //}

        public static bool ValidateNationalCode(this string input)
        {
            int i;
            if (!int.TryParse(input, out i))
                return false;

            input = input.PadLeft(10, '0');
            var sum = input.Substring(0, 9)
                .Select((a, index) => Math.Abs(int.Parse(a.ToString()) * (index - 10)))
                .Aggregate((a, b) => a + b);

            var controllingData = int.Parse(input.Substring(9, 1));
            var mod = sum % 11;
            var x = mod < 2 ? controllingData == mod : controllingData == 11 - mod;


            return x;
        }
        public static string Between(this string text, string left, string right)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            return Regex.Match(text, $"{left}(.*){right}", RegexOptions.IgnoreCase | RegexOptions.Multiline)?.Groups[1]?.Value;
        }

        public static string ReplaceByIndexStep(this string text, string pattern, string replace, int step)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var matchCollection = Regex.Matches(text, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            if (matchCollection.Count > step)
            {
                var match = matchCollection[step];
                text = text.Remove(match.Index, match.Length).Insert(match.Index, replace);
            }

            return text;
        }

        public static bool IsNonStringClass(this Type type)
        {
            if (type.IsString() || !type.IsClass || type.IsArray || type.IsGenericType)
                return false;
            return true;
        }
        public static bool IsString(this Type type)
        {
            if (type == null || type != typeof(string))
                return false;
            return true;
        }
        //public static bool IsNonStringGenericEnumerable(this Type type)
        //{
        //    if (type.IsString() || !type.IsGenericType || type.GetGenericTypeDefinition() != typeof(List<>) || type.GetGenericArguments()[0]?.FullName == "System.String" || type.GetGenericArguments()[0].IsValueType)
        //        return false;
        //    return typeof(IEnumerable).IsAssignableFrom(type);
        //}
        public static bool IsNonStringArrayRefType(this Type type)
        {
            if (type.IsString() || !type.IsArray || type.GetElementType().IsValueType)
                return false;
            return true;
        }

        private static long ConvertShamsiDateTimeToTimeStampInMillisecond(string date)
        {
            var Datetime = date.ToDateTimeWithFormat("yyyy/MM/dd HH:mm:ss:FFF");
            var GeorgianDate = date.ToGregorianDateTimeStringWithFormat("yyyy/MM/dd HH:mm:ss:fff", "yyyy-MM-dd HH:mm:ss:fff").ToDateTimeWithFormat("yyyy-MM-dd HH:mm:ss:fff").AddMilliseconds(Datetime.Millisecond);

            DateTime unixStart = new DateTime(1970, 1, 1);
            return (long)Math.Floor((GeorgianDate.ToUniversalTime() - unixStart).TotalMilliseconds);
        }

        //public static string UnixTimeStampToShamsiDateTime(string unixTimeStamp)
        //{
        //    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        //    dateTime = dateTime.AddMilliseconds(long.Parse(unixTimeStamp)).ToLocalTime();
        //    return dateTime.Date.GetShamsiDateString().AddSlashToDateString() + " " + dateTime.ToString("hh:mm:ss.fff");
        //}
    }

}
