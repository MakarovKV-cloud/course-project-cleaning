using System;

namespace Domain.CourseProjectCleaning
{
    // Record для статистики заявок по месяцам
    public record MonthlyRequestsStatistic(
        int Year,
        int Month,
        string MonthName,
        int RequestsCount,
        decimal TotalIncome
    );
}