using LimeTech_Components.Server.Data;
using LimeTech_Components.Server.Data.Models;

namespace LimeTech_Components.Server.Infrastructure
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LimeTechDbContext>();

            if (!context.BuildCompatibilities.Any())
            {
                context.BuildCompatibilities.AddRange(new List<BuildCompatibility>
                {
                    new BuildCompatibility {Name = "AM4", StartYear = 2016, EndYear = 2022 },
                    new BuildCompatibility {Name = "AM5", StartYear = 2022, EndYear = 2027 },
                    new BuildCompatibility {Name = "LGA 1700", StartYear = 2021, EndYear = 2024 },
                    new BuildCompatibility {Name = "LGA 1851", StartYear = 2026, EndYear = 2026 },
                    new BuildCompatibility {Name = "All", StartYear = 2015, EndYear = 2050 },
                });

                await context.SaveChangesAsync();
            }



            if (!context.Components.Any())
            {
                context.Components.AddRange(new List<Component>
                {
                    new Component {Name = "AMD Ryzen 7 9800X3D", TypeOfProduct = "Processor", Price = 1150, PurchasedCount = 10,
                    ProductionYear = 2024, PowerUsage = 120, Status = PartStatus.Available, StockCount = 30, IsPublic = true, BuildId = 2,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/AMDRyzen79800X3D.jpg"},
                    new Component {Name = "AMD Ryzen 7 7700X", TypeOfProduct = "Processor", Price = 690, PurchasedCount = 55,
                    ProductionYear = 2022, PowerUsage = 105, Status = PartStatus.Available, StockCount = 3, IsPublic = true, BuildId = 2,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/AMDRyzen77700X.jpg"},
                    new Component {Name = "AMD Ryzen 5 5600X", TypeOfProduct = "Processor", Price = 265, PurchasedCount = 33,
                    ProductionYear = 2020, PowerUsage = 65, Status = PartStatus.Available, StockCount = 5, IsPublic = true, BuildId = 1,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/AMDRyzen55600X.jpg"},
                    new Component {Name = "Intel Core i7-14700", TypeOfProduct = "Processor", Price = 750, PurchasedCount = 15,
                    ProductionYear = 2023, PowerUsage = 65, Status = PartStatus.Available, StockCount = 22, IsPublic = true, BuildId = 3,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/IntelCoreI7-14700.jpg"},
                    new Component {Name = "Intel Core Ultra 7 265K", TypeOfProduct = "Processor", Price = 950, PurchasedCount = 3,
                    ProductionYear = 2024, PowerUsage = 125, Status = PartStatus.Available, StockCount = 8, IsPublic = true, BuildId = 4,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/IntelCoreUltra7265K.jpg"},
                    new Component {Name = "Intel Core Ultra 5 245K", TypeOfProduct = "Processor", Price = 720, PurchasedCount = 21,
                    ProductionYear = 2024, PowerUsage = 125, Status = PartStatus.Available, StockCount = 11, IsPublic = true, BuildId = 4,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/IntelCoreUltra5245K.jpg"},

                    new Component {Name = "MSI MPG B550 Gaming Plus", TypeOfProduct = "Motherboard", Price = 250, PurchasedCount = 15,
                    ProductionYear = 2024, PowerUsage = 50, Status = PartStatus.Available, StockCount = 5, IsPublic = true, BuildId = 1,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/MSIMPGB550GamingPlus.jpg"},
                    new Component {Name = "ASUS ROG Strix B550-F Gaming WIFI II", TypeOfProduct = "Motherboard", Price = 420, PurchasedCount = 10,
                    ProductionYear = 2021, PowerUsage = 50, Status = PartStatus.Available, StockCount = 10, IsPublic = true, BuildId = 1,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/ASUSROGStrixB550-FGamingWIFIII.jpg"},
                    new Component {Name = "GIGABYTE B650E AORUS Elite X AX ICE", TypeOfProduct = "Motherboard", Price = 460, PurchasedCount = 60,
                    ProductionYear = 2024, PowerUsage = 50, Status = PartStatus.Available, StockCount = 15, IsPublic = true, BuildId = 2,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/GIGABYTEB650EAORUSEliteXAXICE.jpg"},
                    new Component {Name = "ASUS ROG Strix X670E-E Gaming WIFI", TypeOfProduct = "Motherboard", Price = 920, PurchasedCount = 5,
                    ProductionYear = 2022, PowerUsage = 50, Status = PartStatus.Available, StockCount = 30, IsPublic = true, BuildId = 2,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/ASUSROGStrixX670E-EGamingWIFI.jpg"},
                    new Component {Name = "MSI MAG Z790 Tomahawk Max WIFI", TypeOfProduct = "Motherboard", Price = 560, PurchasedCount = 18,
                    ProductionYear = 2023, PowerUsage = 50, Status = PartStatus.Available, StockCount = 1, IsPublic = true, BuildId = 3,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/MSIMAGZ790TomahawkMaxWIFI.jpg"},
                    new Component {Name = "ASUS TUF Gaming B760-Plus WIFI", TypeOfProduct = "Motherboard", Price = 320, PurchasedCount = 44,
                    ProductionYear = 2023, PowerUsage = 50, Status = PartStatus.Available, StockCount = 111, IsPublic = true, BuildId = 3,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/ASUSTUFGamingB760-PlusWIFI.jpg"},
                    new Component {Name = "ASUS ROG Maximus Z890 Apex", TypeOfProduct = "Motherboard", Price = 1760, PurchasedCount = 2,
                    ProductionYear = 2024, PowerUsage = 50, Status = PartStatus.Available, StockCount = 13, IsPublic = true, BuildId = 4,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/ASUSROGMaximusZ890Apex.jpg"},
                    new Component {Name = "MSI PRO Z890-P WIFI", TypeOfProduct = "Motherboard", Price = 540, PurchasedCount = 11,
                    ProductionYear = 2024, PowerUsage = 50, Status = PartStatus.Available, StockCount = 4, IsPublic = true, BuildId = 4,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/MSIPROZ890-PWIFI.jpg"},

                    new Component {Name = "Seasonic Prime PX-1600", TypeOfProduct = "Power Supply", Price = 780, PurchasedCount = 3,
                    ProductionYear = 2023, PowerUsage = 0, Status = PartStatus.Available, StockCount = 2, IsPublic = true, BuildId = 5,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/PRIME-TX-1600-ATX30.jpg"},
                    new Component {Name = "Seasonic Vertex PX-850", TypeOfProduct = "Power Supply", Price = 420, PurchasedCount = 15,
                    ProductionYear = 2023, PowerUsage = 0, Status = PartStatus.Available, StockCount = 33, IsPublic = true, BuildId = 5,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/Vertex-PX-850.jpg"},
                    new Component {Name = "ASUS ROG Loki Platinum ROG-LOKI-1000P", TypeOfProduct = "Power Supply", Price = 580, PurchasedCount = 6,
                    ProductionYear = 2023, PowerUsage = 0, Status = PartStatus.Available, StockCount = 3, IsPublic = true, BuildId = 5,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/ASUSROGLokiPlatinumROG-LOKI.jpg"},
                    new Component {Name = "Corsair SF Series Platinum SF850", TypeOfProduct = "Power Supply", Price = 430, PurchasedCount = 55,
                    ProductionYear = 2024, PowerUsage = 0, Status = PartStatus.Available, StockCount = 22, IsPublic = true, BuildId = 5,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/CP-9020256-EU.jpg"},

                    new Component {Name = "G.Skill Trident Z Neo DIMM Kit 16GB F4-3600C", TypeOfProduct = "Memory", Price = 125, PurchasedCount = 48,
                    ProductionYear = 2019, PowerUsage = 5, Status = PartStatus.Available, StockCount = 22, IsPublic = true, BuildId = 5,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/GSkillTridentZNeo3600.jpg"},
                    new Component {Name = "Corsair Vengeance RGB PRO SL Black DIMM Kit 32GB", TypeOfProduct = "Memory", Price = 166, PurchasedCount = 32,
                    ProductionYear = 2021, PowerUsage = 10, Status = PartStatus.Available, StockCount = 18, IsPublic = true, BuildId = 5,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/CorsairVengeanceRGB32GB.jpg"},
                    new Component {Name = "G.Skill Ripjaws M5 RGB Black DIMM Kit 32GB", TypeOfProduct = "Memory", Price = 270, PurchasedCount = 41,
                    ProductionYear = 2024, PowerUsage = 10, Status = PartStatus.Available, StockCount = 31, IsPublic = true, BuildId = 5,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/G.SkillRipjawsM532GB.jpg"},
                    new Component {Name = "Corsair Vengeance DIMM Kit 32GB", TypeOfProduct = "Memory", Price = 230, PurchasedCount = 21,
                    ProductionYear = 2024, PowerUsage = 10, Status = PartStatus.Available, StockCount = 15, IsPublic = true, BuildId = 5,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/CorsairVengeance32GB6000.jpg"},

                    new Component {Name = "ASUS Dual GeForce RTX 4060 Ti EVO OC", TypeOfProduct = "Video Card", Price = 880, PurchasedCount = 15,
                    ProductionYear = 2023, PowerUsage = 160, Status = PartStatus.Available, StockCount = 9, IsPublic = true, BuildId = 5,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/DUAL-RTX4060TI-O8G-EVO.jpg"},
                    new Component {Name = "MSI GeForce RTX 4070 Ti SUPER 16G GAMING X SLIM", TypeOfProduct = "Video Card", Price = 2000, PurchasedCount = 52,
                    ProductionYear = 2023, PowerUsage = 285, Status = PartStatus.Available, StockCount = 4, IsPublic = true, BuildId = 5,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/MSIGeForceRTX4070TiSUPER.jpg"},
                    new Component {Name = "Sapphire Nitro+ Radeon RX 7900 XT", TypeOfProduct = "Video Card", Price = 1643, PurchasedCount = 76,
                    ProductionYear = 2022, PowerUsage = 368, Status = PartStatus.Available, StockCount = 11, IsPublic = true, BuildId = 5,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/SapphireNitro+RadeonRX7900XTVapor-X.jpg"},
                    new Component {Name = "ASRock Steel Legend Radeon RX 7700 XT OC", TypeOfProduct = "Video Card", Price = 980, PurchasedCount = 49,
                    ProductionYear = 2023, PowerUsage = 245, Status = PartStatus.Available, StockCount = 1, IsPublic = true, BuildId = 5,
                    ImageUrl = "https://localhost:7039/Images/ComponentImages/ASRockSteelLegendRadeonRX7700XTOC.jpg"},



                });

                await context.SaveChangesAsync();
            }

        }
    }
}
