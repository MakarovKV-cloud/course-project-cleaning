using System;

    namespace Domain.CourseProjectCleaning
    {
    // Record для статистики по услугам
    public record ServiceStatistic(
    string ServiceName,
    int UsageCount,
    decimal TotalIncome
    );
}