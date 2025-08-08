using System.Globalization;

namespace PersonalAccounting.Utils;

public static class DateTimeExtensions
{
    public static string ToShamsiDate(this DateTime dateTime)
    {
        var pc = new PersianCalendar();
        var year = pc.GetYear(dateTime);
        var month = pc.GetMonth(dateTime);
        var day = pc.GetDayOfMonth(dateTime);
        return string.Format("{0:0000}/{1:00}/{2:00}", year, month, day);
    }

    public static DateTime FromShamsi(string persianDate)
    {
        // persianDate expected format: yyyy/MM/dd
        var parts = persianDate.Split('/', '-', '.');
        if (parts.Length < 3) throw new FormatException("Invalid Persian date format");
        int y = int.Parse(parts[0]);
        int m = int.Parse(parts[1]);
        int d = int.Parse(parts[2]);
        var pc = new PersianCalendar();
        return pc.ToDateTime(y, m, d, 0, 0, 0, 0);
    }
}