using System;

namespace BlazingDocs.Models
{
    public class PlanModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double PricePerUnit { get; set; }
        public int Quota { get; set; }
    }
}
