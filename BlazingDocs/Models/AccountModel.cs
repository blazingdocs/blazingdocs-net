using System;

namespace BlazingDocs.Models
{
    public class AccountModel
    {
        public Guid Id { get; set; }
        public PlanModel Plan { get; set; }
        public string ApiKey { get; set; }
        public string ObsoleteApiKey { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastSyncedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDisabled { get; set; }
    }
}