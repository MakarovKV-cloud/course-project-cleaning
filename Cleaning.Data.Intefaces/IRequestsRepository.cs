using Domain.CourseProjectCleaning;

namespace Cleaning.Data.Intefaces
{
    public interface IRequestsRepository
    {
        List<Request> GetAllRequests();

        bool Delete(int id);
        int Add(Request request);
        int Update(Request request);
    }
}
