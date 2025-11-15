using Cleaning.Data.Intefaces;
using Domain.CourseProjectCleaning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Cleaning.Data.JsonStorage
{
    public class ServicesRepository : IServicesRepository
    {
        private readonly List<Service> _services = ReadServices();
        private static int _nextId = GetNextId();

        public int Add(Service service)
        {
            service.Id = _nextId++;
            _services.Add(service);
            SaveServices();
            return service.Id;
        }

        public bool Delete(int id)
        {
            var serviceToDelete = _services.FirstOrDefault(s => s.Id == id);
            if (serviceToDelete != null)
            {
                _services.Remove(serviceToDelete);
                SaveServices();
                return true;
            }
            return false;
        }

        public List<Service> GetAll()
        {
            return _services.ToList();
        }

        public Service? GetById(int id)
        {
            return _services.FirstOrDefault(s => s.Id == id);
        }

        public bool Update(Service service)
        {
            var existingService = _services.FirstOrDefault(s => s.Id == service.Id);
            if (existingService != null)
            {
                existingService.Name = service.Name;
                existingService.PricePerSquareMeter = service.PricePerSquareMeter;
                existingService.RequiresArea = service.RequiresArea;
                SaveServices();
                return true;
            }
            return false;
        }

        private static List<Service> ReadServices()
        {
            try
            {
                var servicesJson = File.ReadAllText("database-services.json");
                if (string.IsNullOrWhiteSpace(servicesJson))
                {
                    // Создаем начальные данные услуг
                    var initialServices = new List<Service>
                    {
                        new Service { Id = 1, Name = "Влажная уборка", PricePerSquareMeter = 50m, RequiresArea = true },
                        new Service { Id = 2, Name = "Мытье окон", PricePerSquareMeter = 200m, RequiresArea = false },
                        new Service { Id = 3, Name = "Химчистка ковров", PricePerSquareMeter = 150m, RequiresArea = true },
                        new Service { Id = 4, Name = "Уборка после ремонта", PricePerSquareMeter = 100m, RequiresArea = true },
                        new Service { Id = 5, Name = "Генеральная уборка", PricePerSquareMeter = 80m, RequiresArea = true }
                    };
                    SaveInitialServices(initialServices);
                    return initialServices;
                }
                return JsonSerializer.Deserialize<List<Service>>(servicesJson) ?? [];
            }
            catch (FileNotFoundException)
            {
                var initialServices = new List<Service>
                {
                    new Service { Id = 1, Name = "Влажная уборка", PricePerSquareMeter = 50m, RequiresArea = true },
                    new Service { Id = 2, Name = "Мытье окон", PricePerSquareMeter = 200m, RequiresArea = false },
                    new Service { Id = 3, Name = "Химчистка ковров", PricePerSquareMeter = 150m, RequiresArea = true }
                };
                SaveInitialServices(initialServices);
                return initialServices;
            }
            catch (Exception)
            {
                File.WriteAllText("database-services.json", "[]");
                return [];
            }
        }

        private static void SaveInitialServices(List<Service> services)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var servicesJson = JsonSerializer.Serialize(services, options);
            File.WriteAllText("database-services.json", servicesJson);
        }

        private void SaveServices()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var servicesJson = JsonSerializer.Serialize(_services, options);
                File.WriteAllText("database-services.json", servicesJson);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при сохранении услуг: {ex.Message}");
            }
        }

        private static int GetNextId()
        {
            try
            {
                var services = ReadServices();
                return services.Count > 0 ? services.Max(s => s.Id) + 1 : 1;
            }
            catch
            {
                return 1;
            }
        }
    }
}