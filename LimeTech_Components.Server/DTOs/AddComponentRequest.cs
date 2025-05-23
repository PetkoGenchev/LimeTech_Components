﻿using LimeTech_Components.Server.Data.Models;

namespace LimeTech_Components.Server.DTOs
{
    public class AddComponentRequest : IComponentDTO
    {
        public required string Name { get; init; }
        public required string Producer { get; init; }
        public required string TypeOfProduct { get; init; }

        public required string ImageUrl { get; init; }

        public int Price { get; init; }

        public int PurchasedCount { get; init; } = 0;

        public int ProductionYear { get; init; }

        public int PowerUsage { get; init; }

        public PartStatus Status { get; init; }

        public int StockCount { get; init; }

        public bool IsPublic { get; init; }




    }
}
