using System;
using NodaTime;

namespace TreeOfAKind.Application.Configuration
{
    public static class DateTimeExtensions
    {
        public static LocalDate GetDate(this DateTime dateTimeOffset)
        {
            return LocalDate.FromDateTime(dateTimeOffset);
        }
    }
}
