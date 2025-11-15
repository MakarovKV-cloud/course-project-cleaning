using System;

namespace Domain.CourseProjectCleaning
{
        // Record для общей статистики
        public record OverallStatistic(
        int TotalUsers,
        int TotalRequests,
        int TotalCleaners,
        decimal TotalIncome,
        int PendingRequests,
        int CompletedRequests
    );
}