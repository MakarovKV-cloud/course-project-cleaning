using Domain.CourseProjectCleaning;
using System.Collections.Generic;

namespace Cleaning.Data.Intefaces
{
    public interface IServicesRepository
    {
        List<Service> GetAll();
        Service? GetById(int id);
        int Add(Service service);
        bool Update(Service service);
        bool Delete(int id);
    }
}