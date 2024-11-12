namespace LimeTech_Components.Server.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Constants.DataConstants;

    public class Component
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ComponentConstants.PartNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int ProductionYear { get; set; }

        [Required]
        public int PowerUsage { get; set; }

        public PartStatus Status { get; set; }

        public int BuildId { get; set; }

        public BuildCompatibility BuildCompatibility { get; init; }

    }
}
