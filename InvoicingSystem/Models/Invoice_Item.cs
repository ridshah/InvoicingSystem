using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InvoicingSystem.Models
{
    [Table("invoice_items")]
    public class Invoice_Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id
        {
            get;
            set;
        }
        [Column("invoice_id")]
        [ForeignKey("Invoice")]
        public int Invoice_Id
        {
            get;
            set;
        }
        [Column("name")]
        [StringLength(255)]
        public string Name
        {
            get;
            set;
        }
        [Column("amount")]
        public Nullable<Decimal> Amount
        {
            get;
            set;
        }
        [Column("created_at")]
        public Nullable<DateTime> Created_At
        {
            get;
            set;
        } = DateTime.Now;


        public virtual Invoice Invoice { get; set; }
    }
}