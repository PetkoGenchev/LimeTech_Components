namespace LimeTech_Components.Server.Constants
{
    public class DataConstants
    {
        public class ComponentConstants
        {
            //public const int PartNameMinLenght = 2;
            public const int PartNameMaxLength = 50;
            public const int MinPartPrice = 1;
            public const int MaxPartPrice = 10000;
            //public const int MinPartYear = 2000;
            public int MaxPartYear = DateTime.Now.Year;
            public const int MinPartPowerUsage = 1;
            public const int MaxPartPowerUsage = 5000;
        }

        public class DiscountConstants
        {
            public const int MinPartDiscount = 0;
            public const int MaxPartDiscount = 100;
        }

        public class BasketConstants
        {


        }


        public class UserProfile
        {
            public const int FullNameMinLength = 2;
            public const int FullNameMaxLength = 50;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
        }

    }
}
