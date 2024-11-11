namespace LimeTech_Components.Server.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Constants.DataConstants;
    public class BuildCompatibility
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(PartNameMaxLength)]
        public string Name { get; set; }


        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public IEnumerable<Component> Components { get; init; } = new List<Component>();
    }
}
