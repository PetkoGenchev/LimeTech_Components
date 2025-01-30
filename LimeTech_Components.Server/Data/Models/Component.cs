namespace LimeTech_Components.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Constants.DataConstants;

    public class Component
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ComponentConstants.PartNameMaxLength)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(ComponentConstants.PartNameMaxLength)]
        public string? TypeOfProduct { get; set; }

        [Required]
        [Url]
        public string? ImageUrl { get; set; }

        [Required]
        public int Price { get; set; }

        public int PurchasedCount { get; set; }

        [Required]
        public int ProductionYear { get; set; }

        [Required]
        public int? PowerUsage { get; set; }

        public PartStatus Status { get; set; }

        [Required]
        public int StockCount { get; set; }

        public bool IsPublic { get; set; }


        public int BuildId { get; set; }


        [Required]
        public BuildCompatibility BuildCompatibility { get; init; }


        public int PurchaseId { get; set; }

        [Required]
        public PurchaseHistory PurchaseHistory { get; init; }

 //       public List<BasketItem> BasketItems { get; private set; } = new List<BasketItem>();
    }


}