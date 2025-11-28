namespace Artillery.Data.Models
{
    public class CountryGun
    {
        public int CountryId { get; set; }

        public virtual Country Country { get; set; } = null!;

        public int GunId { get; set; }

        public virtual Gun Gun { get; set; } = null!;
    }
}