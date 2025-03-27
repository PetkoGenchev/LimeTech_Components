namespace LimeTech_Components.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Constants.DataConstants;

    public class Component
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ComponentConstants.PartNameMaxLength)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(ComponentConstants.PartNameMaxLength)]
        public required string Producer { get; set; }


        [Required]
        [MaxLength(ComponentConstants.PartNameMaxLength)]
        public required string TypeOfProduct { get; set; }

        [Required]
        [Url]
        public required string ImageUrl { get; set; }

        [Required]
        public int Price { get; set; }

        public int PurchasedCount { get; set; }

        [Required]
        public int ProductionYear { get; set; }

        [Required]
        public required int PowerUsage { get; set; }

        public PartStatus Status { get; set; }

        [Required]
        public int StockCount { get; set; }

        public bool IsPublic { get; set; }


        public int BuildId { get; set; }

        [Required]
        public BuildCompatibility? BuildCompatibility { get; init; }

        public ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

        public ICollection<PurchaseHistory> PurchaseHistories { get; private set; } = new List<PurchaseHistory>();

    }


}