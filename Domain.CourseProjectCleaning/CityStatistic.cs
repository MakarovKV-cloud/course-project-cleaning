using System;

namespace Domain.CourseProjectCleaning
{

    public record CityStatistic(
        string CityName,
        int RequestsCount,
        decimal TotalIncome
    );


}
