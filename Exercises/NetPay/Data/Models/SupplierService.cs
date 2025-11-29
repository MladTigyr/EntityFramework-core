namespace NetPay.Data.Models
{
    public class SupplierService
    {
        public int SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; } = null!;

        public int ServiceId { get; set; }

        public virtual Service Service { get; set; } = null!;
    }
}