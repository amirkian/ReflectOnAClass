using System.Globalization;
using System.Text.RegularExpressions;
using System;

namespace CheckPropertyAttributeClass.Models.Helper
{
    public class PersianDateTimeHelper
    {
        private readonly DateTime _dateTime;
        private PersianCalendar _persianCalendar;
        public static PersianDateTimeHelper Now => new PersianDateTimeHelper(DateTime.Now);
        private PersianCalendar PersianCalendar
        {
            get
            {
                if (_persianCalendar != null) return _persianCalendar;
                _persianCalendar = new PersianCalendar();
                return _persianCalendar;
            }
        }
        public int Year
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return DateTime.MinValue.Year;
                return PersianCalendar.GetYear(_dateTime);
            }
        }
        public int Month
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return DateTime.MinValue.Month;
                return PersianCalendar.GetMonth(_dateTime);
            }
        }
        public int Day
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return DateTime.MinValue.Day;
                return PersianCalendar.GetDayOfMonth(_dateTime);
            }
        }
        public int Hour
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return 12;
                return PersianCalendar.GetHour(_dateTime);
            }
        }
        public int Minute
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return 0;
                return PersianCalendar.GetMinute(_dateTime);
            }
        }
        public int Second
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return 0;
                return PersianCalendar.GetSecond(_dateTime);
            }
        }
        public int Millisecond
        {
            get
            {
                if (_dateTime <= DateTime.MinValue) return 0;
                return (int)PersianCalendar.GetMilliseconds(_dateTime);
            }
        }

        public PersianDateTimeHelper(int persianYear, int persianMonth, int persianDay)
        {
            _dateTime = PersianCalendar.ToDateTime(persianYear, persianMonth, persianDay, 0, 0, 0, 0);
        }
        public PersianDateTimeHelper(int persianYear, int persianMonth, int persianDay, int hour, int minute)
        {
            _dateTime = PersianCalendar.ToDateTime(persianYear, persianMonth, persianDay, hour, minute, 0, 0);
        }
        public PersianDateTimeHelper(int persianYear, int persianMonth, int persianDay, int hour, int minute, int second)
        {
            _dateTime = PersianCalendar.ToDateTime(persianYear, persianMonth, persianDay, hour, minute, second, 0);
        }
        public PersianDateTimeHelper(int persianYear, int persianMonth, int persianDay, int hour, int minute, int second, int milliseconds)
        {
            _dateTime = PersianCalendar.ToDateTime(persianYear, persianMonth, persianDay, hour, minute, second, milliseconds);
        }
        public PersianDateTimeHelper(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                _dateTime = DateTime.MinValue;
                return;
            }
            _dateTime = dateTime.Value;
        }
        private PersianDateTimeHelper(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public string ToString(string format)
        {
            if (string.IsNullOrEmpty(format)) format = "yyyy/MM/dd HH:mm:ss";
            var dateTimeString = format.Trim();
            dateTimeString = dateTimeString.Replace("yyyy", Year.ToString(CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("MM", Month.ToString("00", CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("dd", Day.ToString("00", CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("HH", Hour.ToString("00", CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("mm", Minute.ToString("00", CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("ss", Second.ToString("00", CultureInfo.InvariantCulture));
            dateTimeString = dateTimeString.Replace("fff", Millisecond.ToString("000", CultureInfo.InvariantCulture));

            return dateTimeString;
        }

        public static PersianDateTimeHelper ParseShort(string PersianDate)
        {
            try
            {
                PersianDate = Regex.Replace(PersianDate, @"[^\d]", string.Empty);
                if (PersianDate.ToString().Length != 8)
                    throw new InvalidCastException("Numeric persian date must have a format like 13990215.");

                var isNumeric = int.TryParse(PersianDate, out int numericPersianDate);
                if (isNumeric)
                {
                    var year = numericPersianDate / 10000;
                    var day = numericPersianDate % 100;
                    var month = numericPersianDate / 100 % 100;
                    return new PersianDateTimeHelper(year, month, day);
                }
                else
                    throw new InvalidCastException("Numeric persian date must have a format like 13990215.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PersianDateTimeHelper Parse12Char(string PersianDate)
        {
            try
            {
                PersianDate = Regex.Replace(PersianDate, @"[^\d]", string.Empty);
                if (PersianDate.ToString().Length != 12)
                    throw new InvalidCastException("Numeric persian date must have a format like 139902301240.");

                var isNumeric = long.TryParse(PersianDate, out long numericPersianDateTime);
                if (isNumeric)
                {
                    var year = numericPersianDateTime / 100000000;
                    var month = numericPersianDateTime / 1000000 % 100;
                    var day = numericPersianDateTime / 10000 % 100;
                    var hour = numericPersianDateTime / 100 % 100;
                    var minute = numericPersianDateTime % 100;

                    return new PersianDateTimeHelper((int)year, (int)month, (int)day, (int)hour, (int)minute);
                }
                else
                    throw new InvalidCastException("Numeric persian date must have a format like 139902301240.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static PersianDateTimeHelper Parse14Char(string PersianDate)
        {
            try
            {
                PersianDate = Regex.Replace(PersianDate, @"[^\d]", string.Empty);
                if (PersianDate.ToString().Length != 14)
                    throw new InvalidCastException("Numeric persian date must have a format like 13990230124050.");

                var isNumeric = long.TryParse(PersianDate, out long numericPersianDateTime);
                if (isNumeric)
                {
                    var year = numericPersianDateTime / 10000000000;
                    var month = numericPersianDateTime / 100000000 % 100;
                    var day = numericPersianDateTime / 1000000 % 100;
                    var hour = numericPersianDateTime / 10000 % 100;
                    var minute = numericPersianDateTime / 100 % 100;
                    var second = numericPersianDateTime % 100;

                    return new PersianDateTimeHelper((int)year, (int)month, (int)day, (int)hour, (int)minute, (int)second);
                }
                else
                    throw new InvalidCastException("Numeric persian date must have a format like 13990230124050.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int ParsInt(PersianDateTimeHelper PersianDate)
        {
            if (PersianDate == null) return 0;
            return int.TryParse(PersianDate.Year.ToString("0000") + PersianDate.Month.ToString("00") + PersianDate.Day.ToString("00"), out var number) ? number : 0;
        }

        public static PersianDateTimeHelper Parselong(string PersianDate)
        {
            try
            {
                PersianDate = Regex.Replace(PersianDate, @"[^\d]", string.Empty);
                if (PersianDate.ToString().Length != 17)
                    throw new InvalidCastException("Numeric persian date must have a format like 13990230124050123.");

                var isNumeric = long.TryParse(PersianDate, out long numericPersianDateTime);
                if (isNumeric)
                {
                    var year = numericPersianDateTime / 10000000000000;
                    var month = numericPersianDateTime / 100000000000 % 100;
                    var day = numericPersianDateTime / 1000000000 % 100;
                    var hour = numericPersianDateTime / 10000000 % 100;
                    var minute = numericPersianDateTime / 100000 % 100;
                    var second = numericPersianDateTime / 1000 % 100;
                    var millisecond = numericPersianDateTime % 1000;
                    return new PersianDateTimeHelper((int)year, (int)month, (int)day, (int)hour, (int)minute, (int)second, (int)millisecond);
                }
                else
                    throw new InvalidCastException("Numeric persian date must have a format like 13990230124050123.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool TryParse(string persianDateTimeInString, out PersianDateTimeHelper result)
        {
            if (string.IsNullOrEmpty(persianDateTimeInString))
            {
                result = null;
                return false;
            }
            try
            {
                persianDateTimeInString = Regex.Replace(persianDateTimeInString, @"[^\d]", string.Empty);
                if (persianDateTimeInString.ToString().Length == 8)
                {
                    result = ParseShort(persianDateTimeInString);
                    return true;
                }
                else if (persianDateTimeInString.ToString().Length == 12)
                {
                    result = Parse12Char(persianDateTimeInString);
                    return true;
                }
                else if (persianDateTimeInString.ToString().Length == 14)
                {
                    result = Parse14Char(persianDateTimeInString);
                    return true;
                }
                else if (persianDateTimeInString.ToString().Length == 17)
                {
                    result = Parselong(persianDateTimeInString);
                    return true;
                }
                else
                    throw new InvalidCastException("Numeric persian date must have a format like 13990215 or 139902153012 or 13990230124050 or 13990230124050123.");
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public PersianDateTimeHelper Add(TimeSpan timeSpan)
        {
            return new PersianDateTimeHelper(_dateTime.Add(timeSpan));
        }
        public PersianDateTimeHelper AddYears(int years)
        {
            return new PersianDateTimeHelper(PersianCalendar.AddYears(_dateTime, years));
        }
        public PersianDateTimeHelper AddDays(int days)
        {
            return new PersianDateTimeHelper(PersianCalendar.AddDays(_dateTime, days));
        }
        public PersianDateTimeHelper AddMonths(int months)
        {
            return new PersianDateTimeHelper(PersianCalendar.AddMonths(_dateTime, months));
        }
        public PersianDateTimeHelper AddHours(int hours)
        {
            return new PersianDateTimeHelper(_dateTime.AddHours(hours));
        }
        public PersianDateTimeHelper AddMinutes(int minutes)
        {
            return new PersianDateTimeHelper(_dateTime.AddMinutes(minutes));
        }
        public PersianDateTimeHelper AddSeconds(int seconds)
        {
            return new PersianDateTimeHelper(_dateTime.AddSeconds(seconds));
        }
        public PersianDateTimeHelper AddMilliseconds(int milliseconds)
        {
            return new PersianDateTimeHelper(_dateTime.AddMilliseconds(milliseconds));
        }
        public DateTime ToDateTime()
        {
            return _dateTime;
        }
        public override int GetHashCode()
        {
            return _dateTime.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (!(obj is PersianDateTimeHelper)) return false;
            var persianDateTime = (PersianDateTimeHelper)obj;
            return _dateTime == persianDateTime.ToDateTime();
        }

        public static implicit operator DateTime(PersianDateTimeHelper persianDateTime)
        {
            return persianDateTime.ToDateTime();
        }
        public static bool operator ==(PersianDateTimeHelper persianDateTime1, PersianDateTimeHelper persianDateTime2)
        {
            return persianDateTime1.Equals(persianDateTime2);
        }
        public static bool operator !=(PersianDateTimeHelper persianDateTime1, PersianDateTimeHelper persianDateTime2)
        {
            return !persianDateTime1.Equals(persianDateTime2);
        }
        public static bool operator >(PersianDateTimeHelper persianDateTime1, PersianDateTimeHelper persianDateTime2)
        {
            return persianDateTime1.ToDateTime() > persianDateTime2.ToDateTime();
        }
        public static bool operator <(PersianDateTimeHelper persianDateTime1, PersianDateTimeHelper persianDateTime2)
        {
            return persianDateTime1.ToDateTime() < persianDateTime2.ToDateTime();
        }
        public static bool operator >=(PersianDateTimeHelper persianDateTime1, PersianDateTimeHelper persianDateTime2)
        {
            return persianDateTime1.ToDateTime() >= persianDateTime2.ToDateTime();
        }
        public static bool operator <=(PersianDateTimeHelper persianDateTime1, PersianDateTimeHelper persianDateTime2)
        {
            return persianDateTime1.ToDateTime() <= persianDateTime2.ToDateTime();
        }
        public static PersianDateTimeHelper operator +(PersianDateTimeHelper persianDateTime1, TimeSpan timeSpanToAdd)
        {
            DateTime dateTime1 = persianDateTime1;
            return new PersianDateTimeHelper(dateTime1.Add(timeSpanToAdd));
        }
        public static TimeSpan operator -(PersianDateTimeHelper persianDateTime1, PersianDateTimeHelper persianDateTime2)
        {
            DateTime dateTime1 = persianDateTime1;
            DateTime dateTime2 = persianDateTime2;
            return dateTime1 - dateTime2;
        }
    }
}
