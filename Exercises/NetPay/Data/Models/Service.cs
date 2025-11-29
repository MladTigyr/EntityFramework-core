namespace NetPay.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Common.EntityValidation;
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxServiceNameLength)]
        public string ServiceName { get; set; } = null!;

        public virtual ICollection<Expense> Expenses { get; set; } 
            = new List<Expense>();

        public virtual ICollection<SupplierService> SuppliersServices { get; set; }
            = new List<SupplierService>();
    }
}