using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using CheckPropertyAttributeClass.Models.Helper;
using static CheckPropertyAttributeClass.Models.Exceptions.MediatorException;
using CheckPropertyAttributeClass.Models.Exceptions;

namespace Student
{
    public class ESBJsonConverter : JsonConverter
    {
        public string Pattern { get; set; }

        public ESBJsonConverter()
        {

        }
        public ESBJsonConverter(string patern)
        {
            Pattern = patern;
        }
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        private string ReplaceFunctionalPatterns(string input)
        {
            if (!input.Contains("{{"))
                return input;

            while (input.Contains("{{"))
            {
                input = input.ReplaceAddedDatetimeYears()
                    .ReplaceAddedDatetimeMonths()
                    .ReplaceAddedDatetimeDays()
                    .ReplaceAddedDatetimeHours()
                    .ReplaceAddedDatetimeMinutes()
                    .ReplaceAddedDatetimeSeconds()
                    .ReplaceAddedDatetimeMiliseconds()
                    .ReplaceAddedDatetimePersianYears()
                    .ReplaceAddedDatetimePersianMonths()
                    .ReplaceAddedDatetimePersianDays()
                    .ReplaceAddedDatetimePersianHours()
                    .ReplaceAddedDatetimePersianMinutes()
                    .ReplaceAddedDatetimePersianSeconds()
                    .ReplaceAddedDatetimePersianMiliseconds();
            }

            return input;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (Pattern.Contains("{{AppSettings"))
                Pattern = Pattern.ReplaceAppSettings();

            var validations = Pattern.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var errList = new List<ErrorItem>();
            var errorMsg = "";
            foreach (var item in validations)
            {
                var rsCode = 1038;
                var patternItems = item.Split(new[] { "=>" }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();

                if (patternItems.Length == 2)
                    if (patternItems[1].Contains(","))
                    {
                        rsCode = patternItems[1].Split(',')[0].ToSafeInt();
                        if (patternItems[1].Split(',').Length == 2)
                            errorMsg = patternItems[1].Split(',')[1];
                    }
                    else
                        rsCode = patternItems[1].ToSafeInt();

                var switchParamRegex = new Regex("\\(.*\\)");
                var switchParam = switchParamRegex.Replace(patternItems[0], "").ToLower();

                string min = null, max = null;
                Match match;
                string[] perItem;

                if (!new[] { "nonemptystring", "nonemptynumericstring" }.Contains(switchParam) && string.IsNullOrWhiteSpace(reader?.Value.ToSafeString()))
                    continue;


                switch (switchParam.ToLower())
                {
                    case "nonemptynumericstring":

                        if (reader.TokenType != JsonToken.String)
                        {
                            errList.Add(new ErrorItemWithValue { Code = 1038, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });
                            continue;
                        }
                        var val = (string)reader.Value;

                        if (string.IsNullOrWhiteSpace(val) || !double.TryParse(val, out var doubleResult))
                        {
                            errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });
                            continue;
                        }

                        match = switchParamRegex.Match(patternItems[0]);
                        var checkZero = match.Groups[0].ToSafeString()
                            .Replace(")", "")
                            .Replace("(", "");

                        if (!string.IsNullOrWhiteSpace(checkZero))
                            if (checkZero.ToSafeBool() && doubleResult == 0)
                                errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });

                        break;

                    case "numericstring":

                        if (reader.TokenType != JsonToken.String)
                        {
                            errList.Add(new ErrorItemWithValue { Code = 1038, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace((string)reader.Value) || !double.TryParse((string)reader.Value, out _))
                        {
                            errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });
                            continue;
                        }

                        break;
                    case "nonemptystring":
                        if (reader.TokenType != JsonToken.String)
                        {
                            errList.Add(new ErrorItemWithValue { Code = 1038, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });

                            continue;
                        }
                        var value = (string)reader.Value;
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });
                        }
                        break;
                    case "length":
                        match = switchParamRegex.Match(patternItems[0]);
                        perItem = match.Groups[0].ToSafeString()
                            .Replace(")", "")
                            .Replace("(", "")
                           .Split(':');


                        if (!string.IsNullOrWhiteSpace(perItem[0]))
                            min = perItem[0];
                        if (perItem.Length == 2)
                            max = perItem[1];

                        if (!IsValidLength(reader, min.ToSafeInt(), max.ToSafeInt()))
                        {
                            errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });
                        }
                        break;
                    case "range":
                        match = switchParamRegex.Match(patternItems[0]);
                        var x = match.Groups[0].ToSafeString();
                        x = ReplaceFunctionalPatterns(x);
                        perItem = x
                            .Replace(")", "")
                            .Replace("(", "")
                           .Split(new[] { "::" }, StringSplitOptions.None);

                        if (!string.IsNullOrWhiteSpace(perItem[0]))
                            min = perItem[0];
                        if (perItem.Length == 2 && !string.IsNullOrWhiteSpace(perItem[1]))
                            max = perItem[1];

                        if (!IsValidRange(reader, objectType, min, max))
                        {
                            errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });
                        }
                        break;
                    case "regex":
                        match = switchParamRegex.Match(patternItems[0]);
                        var regPatern = match.Groups[0].ToSafeString()
                            .Replace(")", "")
                            .Replace("(", "");

                        var regRegex = new Regex(regPatern);
                        if (!regRegex.IsMatch(reader.Value.ToSafeString()))
                            errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });

                        break;
                    case "whitelist":
                        match = switchParamRegex.Match(patternItems[0]);
                        perItem = match.Groups[0].ToSafeString()
                            .Replace(")", "")
                            .Replace("(", "")
                           .Split(',');

                        if (!perItem.Contains(reader.Value.ToString()))
                        {
                            errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });
                        }

                        break;
                    case "blacklist":
                        match = switchParamRegex.Match(patternItems[0]);
                        perItem = match.Groups[0].ToSafeString()
                            .Replace(")", "")
                            .Replace("(", "")
                            .Split(',');

                        if (perItem.Contains(reader.Value.ToString()))
                        {
                            errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });
                        }

                        break;
                    case "datetime":
                        match = switchParamRegex.Match(patternItems[0]);
                        var datetimeFormat = match.Groups[0].ToSafeString()
                            .Replace(")", "")
                            .Replace("(", "");

                        if (!string.IsNullOrWhiteSpace(datetimeFormat))
                        {
                            if (!DateTime.TryParseExact(reader.Value.ToSafeString(), datetimeFormat,
                                new CultureInfo("en-US"), DateTimeStyles.None, out _))
                            {
                                errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });
                            }
                        }
                        else
                        {
                            if (!DateTime.TryParse(reader.Value.ToSafeString(), out _))
                            {
                                errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });
                            }
                        }
                        break;

                    case "pdatetime":
                        match = switchParamRegex.Match(patternItems[0]);

                        if (!PersianDateTimeHelper.TryParse(reader.Value.ToSafeString(), out _))
                        {
                            errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });

                            continue;
                        }

                        var pdatetimeFormat = match.Groups[0].ToSafeString()
                            .Replace(")", "")
                            .Replace("(", "");

                        if (!string.IsNullOrWhiteSpace(pdatetimeFormat))
                        {
                            if (pdatetimeFormat.Length != reader.Value.ToSafeString().Length)
                            {
                                errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });

                                continue;
                            }

                            var inputDate = reader.Value.ToSafeString();


                            if (pdatetimeFormat.Length != inputDate.Length ||
                                pdatetimeFormat.Split('/').Length != inputDate.Split('/').Length ||
                                pdatetimeFormat.Split('-').Length != inputDate.Split('-').Length ||
                                pdatetimeFormat.Split(':').Length != inputDate.Split(':').Length)
                            {
                                errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });

                                continue;
                            }

                            pdatetimeFormat = pdatetimeFormat
                                .Replace("\\", "|")
                                .Replace("/", "|")
                                .Replace("-", "|")
                                .Replace(":", "|")
                                .Replace(".", "|");

                            inputDate = inputDate
                                .Replace("\\", "|")
                                .Replace("/", "|")
                                .Replace("-", "|")
                                .Replace(":", "|")
                                .Replace(".", "|");

                            var pdatetimeFormatArr = pdatetimeFormat.Split('|');
                            var inputDateArr = inputDate.Split('|');


                            if (pdatetimeFormatArr.Length != inputDateArr.Length ||
                                pdatetimeFormatArr.Where((t, j) => t.Length != inputDateArr[j].Length).Any())
                                errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });

                        }

                        break;
                    case "isiban":
                        var iban = ((string)reader.Value).ToLower();
                        iban = !iban.StartsWith("ir") ? "ir" + iban : iban;
                        iban = iban.Replace(" ", "");
                        var isIban = Regex.IsMatch(iban, "^[a-zA-Z]{2}\\d{2} ?\\d{4} ?\\d{4} ?\\d{4} ?\\d{4} ?[\\d]{0,2}", RegexOptions.Compiled);
                        if (!isIban || iban.Length < 26)
                        {
                            errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });
                            continue;
                        }

                        var get4FirstDigit = iban.Substring(0, 4);
                        var replacedGet4FirstDigit = get4FirstDigit.ToLower().Replace("i", "18").Replace("r", "27");
                        var removedShebaFirst4Digit = iban.Replace(get4FirstDigit, "");
                        var newSheba = removedShebaFirst4Digit + replacedGet4FirstDigit;
                        var finalLongData = Convert.ToDecimal(newSheba);
                        var finalReminder = finalLongData % 97;
                        if (finalReminder != 1)
                            errList.Add(new ErrorItemWithValue { Code = rsCode, Desc = errorMsg, Values = reader?.Value == null ? null : new List<object> { reader.Value } });

                        break;
                }
            }

            if (errList.Any())
                throw CreateException("ESBJsonConverter exception.", reader, errList);

            return reader.Value != null ? Convert.ChangeType(reader.Value, Nullable.GetUnderlyingType(objectType) ?? objectType) : null;
        }


        private bool IsValidRange(JsonReader reader, Type objectType, string min, string max)
        {
            var res = true;
            var switchStatement = Nullable.GetUnderlyingType(objectType)?.Name?.ToLower() ?? objectType.Name.ToLower();

            if (!string.IsNullOrWhiteSpace(min))
            {
                switch (switchStatement)
                {
                    case "sbyte": res = Convert.ToSByte(reader.Value) >= Convert.ToSByte(min); break;
                    case "Byte": res = Convert.ToByte(reader.Value) >= Convert.ToByte(min); break;
                    case "int16": res = Convert.ToInt16(reader.Value) >= Convert.ToInt16(min); break;
                    case "uint16": res = Convert.ToUInt16(reader.Value) >= Convert.ToUInt16(min); break;
                    case "int32": res = Convert.ToInt32(reader.Value) >= Convert.ToInt32(min); break;
                    case "uint32": res = Convert.ToUInt32(reader.Value) >= Convert.ToUInt32(min); break;
                    case "int64": res = Convert.ToInt64(reader.Value) >= Convert.ToInt64(min); break;
                    case "uint64": res = Convert.ToUInt64(reader.Value) >= Convert.ToUInt64(min); break;
                    case "single": res = Convert.ToSingle(reader.Value) >= Convert.ToSingle(min); break;
                    case "double": res = Convert.ToDouble(reader.Value) >= Convert.ToDouble(min); break;
                    case "decimal": res = Convert.ToDecimal(reader.Value) >= Convert.ToDecimal(min); break;
                    case "datetime": res = Convert.ToDateTime(reader.Value) >= Convert.ToDateTime(min); break;
                    case "string":
                        if (reader.Value.ToSafeString() != min.ToSafeString() && new[] { reader.Value.ToSafeString(), min.ToSafeString() }.OrderBy(a => a).First() == reader.Value.ToSafeString())
                            res = false;

                        break;
                    default:
                        res = false;
                        break;
                }

                if (!res)
                    return false;
            }

            if (string.IsNullOrWhiteSpace(max)) return true;

            switch (switchStatement)
            {
                case "sbyte": res = Convert.ToSByte(reader.Value) <= Convert.ToSByte(max); break;
                case "byte": res = Convert.ToByte(reader.Value) <= Convert.ToByte(max); break;
                case "int16": res = Convert.ToInt16(reader.Value) <= Convert.ToInt16(max); break;
                case "uint16": res = Convert.ToUInt16(reader.Value) <= Convert.ToUInt16(max); break;
                case "int32": res = Convert.ToInt32(reader.Value) <= Convert.ToInt32(max); break;
                case "uint32": res = Convert.ToUInt32(reader.Value) <= Convert.ToUInt32(max); break;
                case "int64": res = Convert.ToInt64(reader.Value) <= Convert.ToInt64(max); break;
                case "uint64": res = Convert.ToUInt64(reader.Value) <= Convert.ToUInt64(max); break;
                case "single": res = Convert.ToSingle(reader.Value) <= Convert.ToSingle(max); break;
                case "double": res = Convert.ToDouble(reader.Value) <= Convert.ToDouble(max); break;
                case "decimal": res = Convert.ToDecimal(reader.Value) <= Convert.ToDecimal(max); break;
                case "datetime": res = Convert.ToDateTime(reader.Value) <= Convert.ToDateTime(max); break;
                case "string":
                    if (reader.Value.ToSafeString() != max.ToSafeString() && new[] { reader.Value.ToSafeString(), max.ToSafeString() }.OrderByDescending(a => a).First() == reader.Value.ToSafeString())
                        res = false;

                    break;
                default: res = false; break;
            }

            return res;
        }

        private bool IsValidLength(JsonReader reader, int? min, int? max)
        {
            if (min.HasValue)
                if (reader.Value.ToSafeString().Length < min)
                    return false;

            if (!max.HasValue) return true;

            return !(reader.Value.ToSafeString().Length > max);
        }

        public Exception CreateException(string message, JsonReader reader, List<ErrorItem> errorList)
        {
            var info = (IJsonLineInfo)reader;
            return new JException($"{message} Path '{reader.Path}', line {info.LineNumber}, position {info.LinePosition}.", errorList);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
    public class NonEmptyStringConverter : JsonConverter
    {
        public int RsCode { get; set; }

        public NonEmptyStringConverter()
        {
            RsCode = 1038;
        }
        public NonEmptyStringConverter(int rsCode)
        {
            RsCode = rsCode;
        }
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw CreateException($"Expected string value, but found {reader.TokenType}.", reader);
            }
            var value = (string)reader.Value;
            if (string.IsNullOrWhiteSpace(value))
                throw CreateException("Non-empty string required.", reader);

            return value;
        }

        public Exception CreateException(string message, JsonReader reader)
        {
            var info = (IJsonLineInfo)reader;
            return new JException($"{message} Path '{reader.Path}', line {info.LineNumber}, position {info.LinePosition}.", new List<ErrorItem> { new ErrorItemWithValue { Code = RsCode, Values = reader?.Value == null ? null : new List<object> { reader.Value } } });
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

    public class NonEmptyNumericStringConverter : NonEmptyStringConverter
    {
        readonly bool _checkGreaterThanZero;
        public NonEmptyNumericStringConverter()
        {
            _checkGreaterThanZero = false;
            RsCode = 1038;
        }

        public NonEmptyNumericStringConverter(bool checkGreaterThanZero)
        {
            _checkGreaterThanZero = checkGreaterThanZero;
            RsCode = 1038;
        }

        public NonEmptyNumericStringConverter(int rsCode)
        {
            RsCode = rsCode;
            _checkGreaterThanZero = false;
        }

        public NonEmptyNumericStringConverter(bool checkGreaterThanZero, int rsCode)
        {
            RsCode = rsCode;
            _checkGreaterThanZero = checkGreaterThanZero;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
                throw CreateException($"Expected  numeric string value, but found {reader.TokenType}.", reader);

            var value = (string)reader.Value;

            if (string.IsNullOrWhiteSpace(value) || !double.TryParse(value, out var doubleResult))
                throw CreateException("Non-empty numeric string required.", reader);

            if (_checkGreaterThanZero && doubleResult == 0)
                throw CreateException("Non-empty numeric string greater than zero required.", reader);

            return value;
        }

    }

    public class ValidateLengthRange : JsonConverter
    {
        int _min;
        int _max;
        private bool _required;
        public int RsCode { get; set; }
        public ValidateLengthRange(int rsCode, string period, bool required)
        {
            var perItem = period.Split(':');

            _min = perItem[0].ToSafeInt();
            if (perItem.Length == 2)
                _max = perItem[1].ToSafeInt();

            _required = required;
            RsCode = rsCode;
        }
        public ValidateLengthRange(int rsCode, string period)
        {
            var perItem = period.Split(':');

            _min = perItem[0].ToSafeInt();
            if (perItem.Length == 2)
                _max = perItem[1].ToSafeInt();
            _required = false;
            RsCode = rsCode;
        }
        public ValidateLengthRange(string range, bool required)
        {
            var rangeItem = range.Split(':');

            _min = rangeItem[0].ToSafeInt();
            if (rangeItem.Length == 2)
                _max = rangeItem[1].ToSafeInt();

            _required = required;
            RsCode = 1038;
        }
        public ValidateLengthRange(string range)
        {
            var rangeItem = range.Split(':');

            _min = rangeItem[0].ToSafeInt();
            if (rangeItem.Length == 2)
                _max = rangeItem[1].ToSafeInt();
            _required = false;
            RsCode = 1038;
        }
        public ValidateLengthRange(int minLength, int maxLength, bool required)
        {
            _min = minLength;
            _max = maxLength;
            _required = required;
        }
        public ValidateLengthRange(int minLength, int maxLength)
        {
            _min = minLength;
            _max = maxLength;
            _required = false;
        }
        public ValidateLengthRange(int length, bool required)
        {
            _min = length;
            _max = length;
            _required = required;
        }
        public ValidateLengthRange(int length)
        {
            _min = length;
            _max = length;
            _required = false;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            int? len = null;
            string value;
            switch (reader.TokenType)
            {
                case JsonToken.String:
                    {
                        value = (string)reader.Value;
                        if (!string.IsNullOrEmpty(value))
                            len = value.Length;
                        break;
                    }
                case JsonToken.Float:
                    {
                        value = ((double?)reader.Value)?.ToString();
                        if (!string.IsNullOrEmpty(value))
                            len = value.Length;
                        break;
                    }
                case JsonToken.Integer:
                    {
                        value = ((long?)reader.Value)?.ToString();
                        if (!string.IsNullOrEmpty(value))
                            len = value.Length;
                        break;
                    }
                case JsonToken.Null:
                    {
                        len = null;
                        break;
                    }
                default:
                    throw CreateException($"Expected string/number value, but found {reader.TokenType}.", reader);
            }

            if (len == null)
            {
                if (_required)
                    throw CreateException("Non-empty string/number required.", reader);
            }
            else
                if ((_min != 0 && len < _min) || (_max != 0 && len > _max))
                throw CreateException($"Expected value OutOfRange min:{_min} max:{_max} valueLen:{len}.", reader);

            switch (objectType.Name)
            {
                case "SByte": return Convert.ToSByte(reader.Value);
                case "Byte": return Convert.ToByte(reader.Value);
                case "Int16": return Convert.ToInt16(reader.Value);
                case "UInt16": return Convert.ToUInt16(reader.Value);
                case "Int32": return Convert.ToInt32(reader.Value);
                case "UInt32": return Convert.ToUInt32(reader.Value);
                case "Int64": return Convert.ToInt64(reader.Value);
                case "UInt64": return Convert.ToUInt64(reader.Value);
                case "Single": return Convert.ToSingle(reader.Value);
                case "Double": return Convert.ToDouble(reader.Value);
                case "Decimal": return Convert.ToDecimal(reader.Value);
                case "String": return Convert.ToString(reader.Value);
                default: return reader.Value;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return new[] { "string", "Byte", "SByte", "Int16", "UInt16", "Int32", "UInt32", "Int64", "UInt64", "Single", "Double", "Decimal" }.Any(a => a.ToLower() == objectType.Name.ToLower());
        }

        private Exception CreateException(string message, JsonReader reader)
        {
            var info = (IJsonLineInfo)reader;
            return new JException($"{message} Path '{reader.Path}', line {info.LineNumber}, position {info.LinePosition}.", new List<ErrorItem> { new ErrorItemWithValue { Code = RsCode, Values = reader?.Value == null ? null : new List<object> { reader.Value } } });
        }
    }
}
