using Cleaning.Data.Intefaces;
using Domain.CourseProjectCleaning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cleaning.Data.JsonStorage
{
    public class RequestsRepository : IRequestsRepository
    {
        private readonly List<Request> _requests = ReadRequests();
        private static int _nextId = GetNextId();

        public int Add(Request request)
        {
            request.Id = _nextId++;
            _requests.Add(request);
            SaveRequests();
            return request.Id;
        }

        public bool Delete(int id)
        {
            var requestToDelete = _requests.FirstOrDefault(r => r.Id == id);
            if (requestToDelete != null)
            {
                _requests.Remove(requestToDelete);
                SaveRequests();
                return true;
            }
            return false;
        }

        public List<Request> GetAllRequests()
        {
            return _requests.ToList();
        }

        public int Update(Request request)
        {
            var existingRequest = _requests.FirstOrDefault(r => r.Id == request.Id);
            if (existingRequest != null)
            {
                existingRequest.UserId = request.UserId;
                existingRequest.Area = request.Area;
                existingRequest.RequestsServicesId = request.RequestsServicesId;
                existingRequest.CleaningDate = request.CleaningDate;
                existingRequest.CityId = request.CityId;
                existingRequest.District = request.District;
                existingRequest.Address = request.Address;
                existingRequest.TotalCost = request.TotalCost;
                existingRequest.Status = request.Status;
                existingRequest.CleanerId = request.CleanerId;
                existingRequest.PaymentId = request.PaymentId;
                existingRequest.CreatedAt = request.CreatedAt;

                SaveRequests();
                return existingRequest.Id;
            }
            return -1; 
        }

        private static List<Request> ReadRequests()
        {
            try
            {
                var requestsJson = File.ReadAllText("database-requests.json");

                if (string.IsNullOrWhiteSpace(requestsJson))
                {
                    return [];
                }

                return JsonSerializer.Deserialize<List<Request>>(requestsJson) ?? [];
            }
            catch (FileNotFoundException)
            {
                // Файл не существует, создаем пустой список
                File.WriteAllText("database-requests.json", "[]");
                return [];
            }
            catch (Exception)
            {
                // В случае других ошибок также возвращаем пустой список
                File.WriteAllText("database-requests.json", "[]");
                return [];
            }
        }

        private void SaveRequests()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var requestsJson = JsonSerializer.Serialize(_requests, options);
                File.WriteAllText("database-requests.json", requestsJson);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                throw new Exception($"Ошибка при сохранении заявок: {ex.Message}");
            }
        }

        private static int GetNextId()
        {
            try
            {
                var requests = ReadRequests();
                return requests.Count > 0 ? requests.Max(r => r.Id) + 1 : 1;
            }
            catch
            {
                return 1;
            }
        }

        // Дополнительные методы для удобства работы с заявками
        public List<Request> GetRequestsByUserId(int userId)
        {
            return _requests.Where(r => r.UserId == userId).ToList();
        }

        public List<Request> GetRequestsByCleanerId(int cleanerId)
        {
            return _requests.Where(r => r.CleanerId == cleanerId).ToList();
        }

        public Request GetRequestById(int id)
        {
            return _requests.FirstOrDefault(r => r.Id == id);
        }

        public bool UpdateStatus(int requestId, string status)
        {
            var request = _requests.FirstOrDefault(r => r.Id == requestId);
            if (request != null)
            {
                request.Status = status;
                SaveRequests();
                return true;
            }
            return false;
        }

        public bool AssignCleaner(int requestId, int cleanerId)
        {
            var request = _requests.FirstOrDefault(r => r.Id == requestId);
            if (request != null)
            {
                request.CleanerId = cleanerId;
                SaveRequests();
                return true;
            }
            return false;
        }
    }
}