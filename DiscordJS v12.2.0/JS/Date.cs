using System;

namespace JavaScript
{
    /// <summary>
    /// Represents a JavaScript Date. Enables basic storage and retrieval of dates and times.
    /// </summary>
    public class Date
    {
        private static readonly int[] DAYS_IN_MONTHS = new int[12]
        {
            31, // January
            28, // February
            31, // March
            30, // April
            31, // May
            30, // June
            31, // July
            31, // August
            30, // September
            31, // October
            30, // November
            31 // December
        };

        private DateTime dateValue;
        private long timeValue;

        private static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private Date(DateTime dateValue, long timeValue)
        {
            this.dateValue = dateValue;
            this.timeValue = timeValue;
        }

        public Date()
        {
            dateValue = DateTime.Now;
            timeValue = GetTimeValue(dateValue);
        }

        public Date(long ms)
        {
            timeValue = ms;
            dateValue = EPOCH.AddMilliseconds(ms).ToLocalTime();
        }

        public Date(int year, int month, int date = 1, int hours = 0, int minutes = 0, int seconds = 0, int ms = 0)
        {
            dateValue = CreateDateTime(DateTimeKind.Local, year, month, date, hours, minutes, seconds, ms);
            timeValue = GetTimeValue(dateValue);
        }

        private int GetDaysInYear(int year)
        {
            double y = year;
            return 365 * (year - 1970) + (int)Math.Floor((y - 1969) / 4) - (int)Math.Floor((y - 1901) / 100) + (int)Math.Floor((y - 1601) / 400);
        }

        private static long GetTimeValue(DateTime date) => GetTimeValueUTC(date.ToUniversalTime());
        private static long GetTimeValueUTC(DateTime date) => (long)Math.Floor(date.Subtract(EPOCH).TotalMilliseconds);
        private static DateTime CreateDateValueFromTimeValue(long timeValue) => EPOCH.AddMilliseconds(timeValue).ToLocalTime();
        private static DateTime CreateDateTime(DateTimeKind kind, int? year = null, int? month = null, int? date = null, int? hours = null, int? minutes = null, int? seconds = null, int? ms = null)
        {
            int y = year.HasValue ? year.Value : 0;
            int mon = month.HasValue ? month.Value : 0;
            int d = date.HasValue ? date.Value : 1;
            int hr = hours.HasValue ? hours.Value : 0;
            int min = minutes.HasValue ? minutes.Value : 0;
            int sec = seconds.HasValue ? seconds.Value : 0;
            int m = ms.HasValue ? ms.Value : 0;
            //if (0 <= y && y <= 99) y += 1900; // REMOVED BECAUSE YEARS 0-99 WILL BE MESSED UP
            //TODO: Floor month and date
            m++;
            return new DateTime(y, mon, d, hr, min, sec, m, kind);
        }
        private static DateTime CreateDateTime(DateTime reference, DateTimeKind kind, int? year = null, int? month = null, int? date = null, int? hours = null, int? minutes = null, int? seconds = null, int? ms = null)
        {
            int y = year.HasValue ? year.Value : reference.Year;
            int mon = month.HasValue ? month.Value : reference.Month;
            int d = date.HasValue ? date.Value : reference.Day;
            int hr = hours.HasValue ? hours.Value : reference.Hour;
            int min = minutes.HasValue ? minutes.Value : reference.Minute;
            int sec = seconds.HasValue ? seconds.Value : reference.Second;
            int m = ms.HasValue ? ms.Value : 0;
            return new DateTime(y, mon, d, hr, min, sec, m, kind);
        }

        /// <summary>
        /// Gets the time value represented right now
        /// </summary>
        /// <returns></returns>
        public static long Now() => GetTimeValueUTC(DateTime.UtcNow);

        /// <summary>
        /// Returns the number of milliseconds between midnight, January 1, 1970 Universal Coordinated Time (UTC) (or GMT) and the specified date.
        /// </summary>
        /// <param name="year">The full year designation is required for cross-century date accuracy. If year is between 0 and 99 is used, then year is assumed to be 1900 + year.</param>
        /// <param name="month">The month as a number between 0 and 11 (January to December).</param>
        /// <param name="date">The date as a number between 1 and 31.</param>
        /// <param name="hours">Must be supplied if minutes is supplied. A number from 0 to 23 (midnight to 11pm) that specifies the hour.</param>
        /// <param name="minutes">Must be supplied if seconds is supplied. A number from 0 to 59 that specifies the minutes.</param>
        /// <param name="seconds">Must be supplied if milliseconds is supplied. A number from 0 to 59 that specifies the seconds.</param>
        /// <param name="ms">A number from 0 to 999 that specifies the milliseconds.</param>
        /// <returns></returns>
        public static Date UTC(int year, int month = 0, int date = 1, int hours = 0, int minutes = 0, int seconds = 0, int ms = 0)
        {
            var dv = CreateDateTime(DateTimeKind.Utc, year, month, date, hours, minutes, seconds, ms);
            var t = GetTimeValueUTC(dv);
            dv = dv.ToLocalTime();
            return new Date(dv, t);
        }

        /// <summary>
        /// Parses a string containing a date, and returns the number of milliseconds between that date and midnight, January 1, 1970.
        /// </summary>
        /// <param name="str">A date string</param>
        /// <returns></returns>
        [Obsolete("To be implemented")]
        public static long Parse(string str)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the stored time value in milliseconds since midnight, January 1, 1970 UTC.
        /// </summary>
        /// <returns></returns>
        public long ValueOf() => timeValue;

        /// <summary>
        /// Gets the day-of-the-month, using local time.
        /// </summary>
        /// <returns></returns>
        public int GetDate() => dateValue.Day;

        /// <summary>
        /// Gets the day of the week, using local time.
        /// </summary>
        /// <returns></returns>
        public int GetDay() => (int)dateValue.DayOfWeek;

        /// <summary>
        /// Gets the year, using local time.
        /// </summary>
        /// <returns></returns>
        public int GetFullYear() => dateValue.Year;

        /// <summary>
        /// Gets the hours in a date, using local time.
        /// </summary>
        /// <returns></returns>
        public int GetHours() => dateValue.Hour;

        /// <summary>
        /// Gets the milliseconds of a Date, using local time.
        /// </summary>
        /// <returns></returns>
        public int GetMilliseconds() => dateValue.Millisecond;

        /// <summary>
        /// Gets the minutes of a Date object, using local time.
        /// </summary>
        /// <returns></returns>
        public int GetMinutes() => dateValue.Minute;

        /// <summary>
        /// Gets the month, using local time.
        /// </summary>
        /// <returns></returns>
        public int GetMonth() => dateValue.Month - 1;

        /// <summary>
        /// Gets the seconds of a Date object, using local time.
        /// </summary>
        /// <returns></returns>
        public int GetSeconds() => dateValue.Second;

        /// <summary>
        /// Gets the time value in milliseconds.
        /// </summary>
        /// <returns></returns>
        public long GetTime() => timeValue;

        /// <summary>
        /// Gets the difference in minutes between the time on the local computer and Universal Coordinated Time (UTC).
        /// </summary>
        /// <returns></returns>
        public int GetTimezoneOffset() => (int)Math.Floor(dateValue.ToUniversalTime().Subtract(dateValue).TotalMinutes);

        /// <summary>
        /// Gets the day-of-the-month, using Universal Coordinated Time (UTC).
        /// </summary>
        /// <returns></returns>
        public int GetUTCDate() => dateValue.ToUniversalTime().Day;

        /// <summary>
        /// Gets the day of the week using Universal Coordinated Time (UTC).
        /// </summary>
        /// <returns></returns>
        public int GetUTCDay() => (int)dateValue.ToUniversalTime().DayOfWeek;

        /// <summary>
        /// Gets the year using Universal Coordinated Time (UTC).
        /// </summary>
        /// <returns></returns>
        public int GetUTCFullYear() => dateValue.ToUniversalTime().Year;

        /// <summary>
        /// Gets the hours value in a Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <returns></returns>
        public int GetUTCHours() => dateValue.ToUniversalTime().Hour;

        /// <summary>
        /// Gets the milliseconds of a Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <returns></returns>
        public int GetUTCMilliseconds() => dateValue.ToUniversalTime().Millisecond;

        /// <summary>
        /// Gets the minutes of a Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <returns></returns>
        public int GetUTCMinutes() => dateValue.ToUniversalTime().Minute;

        /// <summary>
        /// Gets the month of a Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <returns></returns>
        public int GetUTCMonth() => dateValue.ToUniversalTime().Month - 1;

        /// <summary>
        /// Gets the seconds of a Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <returns></returns>
        public int GetUTCSeconds() => dateValue.ToUniversalTime().Second;


        /// <summary>
        /// Sets the numeric day-of-the-month value of the Date object using local time.
        /// </summary>
        /// <param name="date">A numeric value equal to the day of the month.</param>
        /// <returns></returns>
        public long SetDate(int date)
        {
            dateValue = CreateDateTime(dateValue, DateTimeKind.Local, null, null, date);
            timeValue = GetTimeValue(dateValue);
            return timeValue;
        }

        /// <summary>
        /// Sets the year of the Date object using local time.
        /// </summary>
        /// <param name="year">A numeric value for the year.</param>
        /// <returns></returns>
        public long SetFullYear(int year) => SetFullYear(year, dateValue.Month - 1, dateValue.Day);

        /// <summary>
        /// Sets the year of the Date object using local time.
        /// </summary>
        /// <param name="year">A numeric value for the year.</param>
        /// <param name="month">A zero-based numeric value for the month (0 for January, 11 for December). Must be specified if numDate is specified.</param>
        /// <returns></returns>
        public long SetFullYear(int year, int month) => SetFullYear(year, month, dateValue.Day);

        /// <summary>
        /// Sets the year of the Date object using local time.
        /// </summary>
        /// <param name="year">A numeric value for the year.</param>
        /// <param name="month">A zero-based numeric value for the month (0 for January, 11 for December). Must be specified if numDate is specified.</param>
        /// <param name="date">A numeric value equal for the day of the month.</param>
        /// <returns></returns>
        public long SetFullYear(int year, int month, int date)
        {
            dateValue = CreateDateTime(dateValue, DateTimeKind.Local, year, month + 1, date);
            timeValue = GetTimeValue(dateValue);
            return timeValue;
        }

        /// <summary>
        /// Sets the hour value in the Date object using local time.
        /// </summary>
        /// <param name="hour">A numeric value equal to the hours value.</param>
        /// <returns></returns>
        public long SetHours(int hour) => SetHours(hour, dateValue.Minute, dateValue.Second, dateValue.Millisecond);

        /// <summary>
        /// Sets the hour value in the Date object using local time.
        /// </summary>
        /// <param name="hour">A numeric value equal to the hours value.</param>
        /// <param name="min">A numeric value equal to the minutes value.</param>
        /// <returns></returns>
        public long SetHours(int hour, int min) => SetHours(hour, min, dateValue.Second, dateValue.Millisecond);

        /// <summary>
        /// Sets the hour value in the Date object using local time.
        /// </summary>
        /// <param name="hour">A numeric value equal to the hours value.</param>
        /// <param name="min">A numeric value equal to the minutes value.</param>
        /// <param name="sec">A numeric value equal to the seconds value.</param>
        /// <returns></returns>
        public long SetHours(int hour, int min, int sec) => SetHours(hour, min, sec, dateValue.Millisecond);

        /// <summary>
        /// Sets the hour value in the Date object using local time.
        /// </summary>
        /// <param name="hour">A numeric value equal to the hours value.</param>
        /// <param name="min">A numeric value equal to the minutes value.</param>
        /// <param name="sec">A numeric value equal to the seconds value.</param>
        /// <param name="ms">A numeric value equal to the milliseconds value.</param>
        /// <returns></returns>
        public long SetHours(int hour, int min, int sec, int ms)
        {
            dateValue = CreateDateTime(dateValue, DateTimeKind.Local, null, null, null, hour, min, sec, ms);
            timeValue = GetTimeValue(dateValue);
            return timeValue;
        }

        /// <summary>
        /// Sets the milliseconds value in the Date object using local time.
        /// </summary>
        /// <param name="ms">A numeric value equal to the millisecond value.</param>
        /// <returns></returns>
        public long SetMilliseconds(int ms)
        {
            dateValue = CreateDateTime(dateValue, DateTimeKind.Local, null, null, null, null, null, null, ms);
            timeValue = GetTimeValue(dateValue);
            return timeValue;
        }

        /// <summary>
        /// Sets the minutes value in the Date object using local time.
        /// </summary>
        /// <param name="min">A numeric value equal to the minutes value.</param>
        /// <returns></returns>
        public long SetMinutes(int min) => SetMinutes(min, dateValue.Second, dateValue.Millisecond);

        /// <summary>
        /// Sets the minutes value in the Date object using local time.
        /// </summary>
        /// <param name="min">A numeric value equal to the minutes value.</param>
        /// <param name="sec">A numeric value equal to the seconds value.</param>
        /// <returns></returns>
        public long SetMinutes(int min, int sec) => SetMinutes(min, sec, dateValue.Millisecond);

        /// <summary>
        /// Sets the minutes value in the Date object using local time.
        /// </summary>
        /// <param name="min">A numeric value equal to the minutes value.</param>
        /// <param name="sec">A numeric value equal to the seconds value.</param>
        /// <param name="ms">A numeric value equal to the milliseconds value.</param>
        /// <returns></returns>
        public long SetMinutes(int min, int sec, int ms)
        {
            dateValue = CreateDateTime(dateValue, DateTimeKind.Local, null, null, null, null, min, sec, ms);
            timeValue = GetTimeValue(dateValue);
            return timeValue;
        }

        /// <summary>
        /// Sets the month value in the Date object using local time.
        /// </summary>
        /// <param name="month">A numeric value equal to the month. The value for January is 0, and other month values follow consecutively.</param>
        /// <returns></returns>
        public long SetMonth(int month) => SetMonth(month, dateValue.Day);

        /// <summary>
        /// Sets the month value in the Date object using local time.
        /// </summary>
        /// <param name="month">A numeric value equal to the month. The value for January is 0, and other month values follow consecutively.</param>
        /// <param name="date">A numeric value representing the day of the month. If this value is not supplied, the value from a call to the getDate method is used.</param>
        /// <returns></returns>
        public long SetMonth(int month, int date)
        {
            dateValue = CreateDateTime(dateValue, DateTimeKind.Local, null, month + 1, date);
            timeValue = GetTimeValue(dateValue);
            return timeValue;
        }

        /// <summary>
        /// Sets the seconds value in the Date object using local time.
        /// </summary>
        /// <param name="sec">A numeric value equal to the seconds value.</param>
        /// <returns></returns>
        public long SetSeconds(int sec) => SetSeconds(sec, dateValue.Millisecond);

        /// <summary>
        /// Sets the seconds value in the Date object using local time.
        /// </summary>
        /// <param name="sec">A numeric value equal to the seconds value.</param>
        /// <param name="ms">A numeric value equal to the milliseconds value.</param>
        /// <returns></returns>
        public long SetSeconds(int sec, int ms)
        {
            dateValue = CreateDateTime(dateValue, DateTimeKind.Local, null, null, null, null, null, sec, ms);
            timeValue = GetTimeValue(dateValue);
            return timeValue;
        }

        /// <summary>
        /// Sets the date and time value in the Date object.
        /// </summary>
        /// <param name="time">A numeric value representing the number of elapsed milliseconds since midnight, January 1, 1970 GMT.</param>
        /// <returns></returns>
        public long SetTime(long time)
        {
            timeValue = time;
            dateValue = CreateDateValueFromTimeValue(time);
            return timeValue;
        }

        /// <summary>
        /// Sets the numeric day of the month in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="date">A numeric value equal to the day of the month.</param>
        /// <returns></returns>
        public long SetUTCDate(int date)
        {
            dateValue = CreateDateTime(dateValue, DateTimeKind.Utc, null, null, date).ToLocalTime();
            timeValue = GetTimeValue(dateValue);
            return timeValue;
        }

        /// <summary>
        /// Sets the year value in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="year">A numeric value for the year.</param>
        /// <returns></returns>
        public long SetUTCFullYear(int year) => SetUTCFullYear(year, dateValue.Month - 1, dateValue.Day);

        /// <summary>
        /// Sets the year value in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="year">A numeric value for the year.</param>
        /// <param name="month">A numeric value equal to the month. The value for January is 0, and other month values follow consecutively.</param>
        /// <returns></returns>
        public long SetUTCFullYear(int year, int month) => SetUTCFullYear(year, month, dateValue.Day);

        /// <summary>
        /// Sets the year value in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="year">A numeric value for the year.</param>
        /// <param name="month">A numeric value equal to the month. The value for January is 0, and other month values follow consecutively.</param>
        /// <param name="date">A numeric value equal to the day of the month.</param>
        /// <returns></returns>
        public long SetUTCFullYear(int year, int month, int date)
        {
            dateValue = CreateDateTime(dateValue, DateTimeKind.Utc, year, month + 1, date).ToLocalTime();
            timeValue = GetTimeValue(dateValue);
            return timeValue;
        }

        /// <summary>
        /// Sets the hours value in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="hour">A numeric value equal to the hours value.</param>
        /// <returns></returns>
        public long SetUTCHours(int hour) => SetUTCHours(hour, dateValue.Minute, dateValue.Second, dateValue.Millisecond);

        /// <summary>
        /// Sets the hours value in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="hour">A numeric value equal to the hours value.</param>
        /// <param name="min">A numeric value equal to the minutes value.</param>
        /// <returns></returns>
        public long SetUTCHours(int hour, int min) => SetUTCHours(hour, min, dateValue.Second, dateValue.Millisecond);

        /// <summary>
        /// Sets the hours value in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="hour">A numeric value equal to the hours value.</param>
        /// <param name="min">A numeric value equal to the minutes value.</param>
        /// <param name="sec">A numeric value equal to the seconds value.</param>
        /// <returns></returns>
        public long SetUTCHours(int hour, int min, int sec) => SetUTCHours(hour, min, sec, dateValue.Millisecond);

        /// <summary>
        /// Sets the hours value in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="hour">A numeric value equal to the hours value.</param>
        /// <param name="min">A numeric value equal to the minutes value.</param>
        /// <param name="sec">A numeric value equal to the seconds value.</param>
        /// <param name="ms">A numeric value equal to the milliseconds value.</param>
        /// <returns></returns>
        public long SetUTCHours(int hour, int min, int sec, int ms)
        {
            dateValue = CreateDateTime(dateValue, DateTimeKind.Utc, null, null, null, hour, min, sec, ms).ToLocalTime();
            timeValue = GetTimeValue(dateValue);
            return timeValue;
        }

        /// <summary>
        /// Sets the milliseconds value in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="ms">A numeric value equal to the millisecond value.</param>
        /// <returns></returns>
        public long SetUTCMilliseconds(int ms)
        {
            dateValue = CreateDateTime(dateValue, DateTimeKind.Utc, null, null, null, null, null, null, ms).ToLocalTime();
            timeValue = GetTimeValue(dateValue);
            return timeValue;
        }

        /// <summary>
        /// Sets the minutes value in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="min">A numeric value equal to the minutes value.</param>
        /// <returns></returns>
        public long SetUTCMinutes(int min) => SetUTCMinutes(min, dateValue.Second, dateValue.Millisecond);

        /// <summary>
        /// Sets the minutes value in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="min">A numeric value equal to the minutes value.</param>
        /// <param name="sec">A numeric value equal to the seconds value.</param>
        /// <returns></returns>
        public long SetUTCMinutes(int min, int sec) => SetUTCMinutes(min, sec, dateValue.Millisecond);

        /// <summary>
        /// Sets the minutes value in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="min">A numeric value equal to the minutes value.</param>
        /// <param name="sec">A numeric value equal to the seconds value.</param>
        /// <param name="ms">A numeric value equal to the milliseconds value.</param>
        /// <returns></returns>
        public long SetUTCMinutes(int min, int sec, int ms)
        {
            dateValue = CreateDateTime(dateValue, DateTimeKind.Utc, null, null, null, null, min, sec, ms).ToLocalTime();
            timeValue = GetTimeValue(dateValue);
            return timeValue;
        }

        /// <summary>
        /// Sets the month value in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="month">A numeric value equal to the month. The value for January is 0, and other month values follow consecutively.</param>
        /// <returns></returns>
        public long SetUTCMonth(int month) => SetUTCMonth(month, dateValue.Day);

        /// <summary>
        /// Sets the month value in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="month">A numeric value equal to the month. The value for January is 0, and other month values follow consecutively.</param>
        /// <param name="date">A numeric value representing the day of the month. If it is not supplied, the value from a call to the getUTCDate method is used.</param>
        /// <returns></returns>
        public long SetUTCMonth(int month, int date)
        {
            dateValue = CreateDateTime(dateValue, DateTimeKind.Utc, null, month + 1, date).ToLocalTime();
            timeValue = GetTimeValue(dateValue);
            return timeValue;
        }

        /// <summary>
        /// Sets the seconds value in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="sec">A numeric value equal to the seconds value.</param>
        /// <returns></returns>
        public long SetUTCSeconds(int sec) => SetUTCSeconds(sec, dateValue.Millisecond);


        /// <summary>
        /// Sets the seconds value in the Date object using Universal Coordinated Time (UTC).
        /// </summary>
        /// <param name="sec">A numeric value equal to the seconds value.</param>
        /// <param name="ms">A numeric value equal to the milliseconds value.</param>
        /// <returns></returns>
        public long SetUTCSeconds(int sec, int ms)
        {
            dateValue = CreateDateTime(dateValue, DateTimeKind.Utc, null, null, null, null, null, sec, ms).ToLocalTime();
            timeValue = GetTimeValue(dateValue);
            return timeValue;
        }

        private static readonly string[] WEEK_DAYS_STRINGS = new string[7]
        {
            "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"
        };

        private static readonly string[] MONTH_STRINGS = new string[12]
        {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        };

        private string GetDateString()
        {
            string weekday = WEEK_DAYS_STRINGS[(int)dateValue.DayOfWeek];
            string month = MONTH_STRINGS[dateValue.Month - 1];
            string day = dateValue.Day.ToString();
            while (day.Length < 2)
                day = "0" + day;
            string year = dateValue.Year.ToString();
            while (year.Length < 4)
                year = "0" + year;
            return weekday + " " + month + " " + day + " " + year;
        }

        private string GetTimeString()
        {
            string hour = dateValue.Hour.ToString();
            while (hour.Length < 2)
                hour = "0" + hour;
            string minute = dateValue.Minute.ToString();
            while (minute.Length < 2)
                minute = "0" + minute;
            string second = dateValue.Second.ToString();
            while (second.Length < 2)
                second = "0" + second;
            return hour + ":" + minute + ":" + second + " GMT";
        }

        private string GetTimeZoneString()
        {
            int offset = GetTimezoneOffset();
            string offsetSign = "+";
            if (offset < 0)
            {
                offsetSign = "-";
                offset = -offset;
            }
            string offsetMin = (offset % 60).ToString();
            while (offsetMin.Length < 2)
                offsetMin = "0" + offsetMin;
            string offsetHour = (offset / 60).ToString();
            while (offsetHour.Length < 2)
                offsetHour = "0" + offsetHour;
            string tzName = ""; // Let tzName be an implementation-defined string that is either the empty string or the string-concatenation of the code unit 0x0020 (SPACE), the code unit 0x0028 (LEFT PARENTHESIS), an implementation-dependent timezone name, and the code unit 0x0029 (RIGHT PARENTHESIS).
            return offsetSign + offsetHour + offsetMin + tzName;
        }

        /// <summary>
        /// Returns a date converted to a string using Universal Coordinated Time (UTC).
        /// </summary>
        /// <returns></returns>
        public string ToUTCString()
        {
            string weekday = WEEK_DAYS_STRINGS[(int)dateValue.DayOfWeek];
            string month = MONTH_STRINGS[dateValue.Month - 1];
            string day = dateValue.Day.ToString();
            while (day.Length < 2)
                day = "0" + day;
            string year = dateValue.Year.ToString();
            while (year.Length < 4)
                year = "0" + year;
            return weekday + ", " + month + " " + day + " " + year + " " + GetTimeString();
        }

        /// <summary>
        /// Returns a date as a string value in ISO format.
        /// </summary>
        /// <returns></returns>
        public string ToISOString()
        {
            string month = dateValue.Month.ToString();
            while (month.Length < 2)
                month = "0" + month;
            string day = dateValue.Day.ToString();
            while (day.Length < 2)
                day = "0" + day;
            string year = dateValue.Year.ToString();
            while (year.Length < 4)
                year = "0" + year;

            string hour = dateValue.Hour.ToString();
            while (hour.Length < 2)
                hour = "0" + hour;
            string minute = dateValue.Minute.ToString();
            while (minute.Length < 2)
                minute = "0" + minute;
            string second = dateValue.Second.ToString();
            while (second.Length < 2)
                second = "0" + second;
            string ms = dateValue.Millisecond.ToString();
            while (ms.Length < 3)
                ms = "0" + ms;

            int offset = GetTimezoneOffset();
            string offsetSign = "+";
            if (offset < 0)
            {
                offsetSign = "-";
                offset = -offset;
            }
            string offsetMin = (offset % 60).ToString();
            while (offsetMin.Length < 2)
                offsetMin = "0" + offsetMin;
            string offsetHour = (offset / 60).ToString();
            while (offsetHour.Length < 2)
                offsetHour = "0" + offsetHour;

            return year + "-" + month + "-" + day + "T" + hour + ":" + minute + ":" + second + "." + ms + offsetSign + offsetHour + ":" + offsetMin;
        }

        /// <summary>
        /// Used by the JSON.stringify method to enable the transformation of an object's data for JavaScript Object Notation (JSON) serialization.
        /// </summary>
        /// <returns></returns>
        public string ToJSON() => ToISOString();

        /// <summary>
        /// Returns a date as a string value.
        /// </summary>
        /// <returns></returns>
        public string ToDateString() => GetDateString();

        /// <summary>
        /// Returns a time as a string value.
        /// </summary>
        /// <returns></returns>
        public string ToTimeString()
        {
            return GetTimeString() + GetTimeZoneString();
        }

        /// <summary>
        /// Returns a string representation of a date. The format of the string depends on the locale.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => GetDateString() + " " + GetTimeString() + GetTimeZoneString();
    }
}