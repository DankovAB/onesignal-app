using System;

namespace BaseApp.Web.Code.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFormat(this DateTime value)
        {
            return value.ToString("yyyy/MM/dd HH:mm");
        }
    }
}
