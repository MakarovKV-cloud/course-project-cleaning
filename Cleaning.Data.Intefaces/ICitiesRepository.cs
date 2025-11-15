using Domain.CourseProjectCleaning;
using System.Collections.Generic;

namespace Cleaning.Data.Intefaces
{
    public interface ICitiesRepository
    {
        List<City> GetAll();
        City? GetById(int id);
        int Add(City city);
        bool Update(City city);
        bool Delete(int id);
    }
}