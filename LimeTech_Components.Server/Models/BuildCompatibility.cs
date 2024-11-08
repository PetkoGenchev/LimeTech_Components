namespace LimeTech_Components.Server.Models
{
    public class BuildCompatibility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public IEnumerable<Component> Components { get; init; } = new List<Component>();
    }
}
