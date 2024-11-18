namespace LimeTech_Components.Server.Services.Components.Models
{
    public class ComponentQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int ComponentsPerPage { get; init; }

        public int TotalComponents { get; init; }

        public IEnumerable<ComponentServiceModel> Components { get; set; }
    }
}
