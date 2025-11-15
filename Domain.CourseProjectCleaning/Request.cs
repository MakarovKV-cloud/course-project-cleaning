using System;

namespace Domain.CourseProjectCleaning
{
    public class Request
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Area { get; set; }
        public string? RequestsServicesId { get; set; }
        public DateTime CleaningDate { get; set; }
        public int CityId { get; set; }
        public string? District { get; set; }
        public string? Address { get; set; }
        public decimal TotalCost { get; set; }
        public string? Status { get; set; }
        public int? CleanerId { get; set; }
        public int PaymentId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Дополнительное свойство для отображения
        public string? CleanerName { get; set; }
    }
}