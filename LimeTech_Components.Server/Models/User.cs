namespace LimeTech_Components.Server.Models
{
    using Microsoft.AspNetCore.Identity;
    public class User : IdentityUser
    {
        public string PublicID { get; init; } = Guid.NewGuid().ToString();


        public string FullName { get; set; }

        public IEnumerable<Component> ComponentBasket { get; init; } = new List<Component>();
    }
}
