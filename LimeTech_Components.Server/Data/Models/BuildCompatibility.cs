namespace LimeTech_Components.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Constants.DataConstants;
    public class BuildCompatibility
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ComponentConstants.PartNameMaxLength)]
        public string? Name { get; set; }

        [Required]
        public int? StartYear { get; set; }

        [Required]
        public int? EndYear { get; set; }

        public ICollection<Component> Components { get; init; } = new List<Component>();
    }
}
