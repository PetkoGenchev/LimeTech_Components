namespace LimeTech_Components.Server.Models
{
    public class Component
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public int ProductionYear { get; set; }

        public int PowerUsage { get; set; }

        public PartStatus Status { get; set; }

        public int BuildId { get; set; }

        public BuildCompatibility BuildCompatibility { get; init; }

    }
}
