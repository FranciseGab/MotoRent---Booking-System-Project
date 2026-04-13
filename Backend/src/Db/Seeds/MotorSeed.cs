using src.db.models;

namespace src.db.seeds
{
    public static class MotorSeed
    {
        public static List<MotorModel> GetMotors()
        {
            return new List<MotorModel>
            {
                new MotorModel { Brand = "Honda Click", PricePerDay = 300, ImageUrl = "/assets/images/honda-click.jpg" },
                new MotorModel { Brand = "Aerox", PricePerDay = 400, ImageUrl = "/assets/images/aerox.jpg" },
                new MotorModel { Brand = "NMAX", PricePerDay = 450, ImageUrl = "/assets/images/nmax.jpg" },
                new MotorModel { Brand = "ADV", PricePerDay = 500, ImageUrl = "/assets/images/adv.jpg" },
                new MotorModel { Brand = "PCX", PricePerDay = 480, ImageUrl = "/assets/images/pcx.jpg" },
                new MotorModel { Brand = "Raider", PricePerDay = 350, ImageUrl = "/assets/images/raider.jpg" }
            };
        }
    }
}