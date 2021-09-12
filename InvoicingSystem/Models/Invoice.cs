using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InvoicingSystem.Models
{
    public enum Invoice_Status_Enum
    {
        [Display(Name = "paid")]
        paid,
        [Display(Name = "unpaid")]
        unpaid
    }

    [Table("invoices")]
    public class Invoice
    {
        public Invoice()
        {
            this.Invoice_Items = new HashSet<Invoice_Item>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id
        {
            get;
            set;
        }
        [Column("client")]
        [StringLength(255)]
        public string Client
        {
            get;
            set;
        }
        [Column("invoice_amount")]
        public Nullable<Decimal> Invoice_Amount
        {
            get;
            set;
        }
        [Column("invoice_amount_plus_vat")]
        public Nullable<Decimal> Invoice_Amount_Plus_VAT
        {
            get;
            set;
        }
        [Column("vat_rate")]
        public Nullable<Decimal> VAT_Rate
        {
            get;
            set;
        }

        private string _invoice_status;
        [Column("invoice_status")]
        public string Invoice_Status
        {
            get { return this._invoice_status; }
            set { this._invoice_status = GetEnumValue<Invoice_Status_Enum>(value).ToString(); }
        }

        [Column("invoice_date", TypeName = "date")]
        public Nullable<DateTime> Invoice_Date
        {
            get;
            set;
        }
        [Column("created_at")]
        public Nullable<DateTime> Created_At
        {
            get;
            set;
        }

        public virtual ICollection<Invoice_Item> Invoice_Items 
        { 
            get;
            set; 
        }

        public static T GetEnumValue<T>(string str) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }
            T val;
            return Enum.TryParse<T>(str, true, out val) ? val : default(T);
        }
    }
}