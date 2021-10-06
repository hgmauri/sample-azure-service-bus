﻿using System;

namespace Sample.AzureServiceBus.WebApi.Core.Models
{
    public class ClientModel
    {
        public string? ClientId { get; set; }
        public string? Name { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
