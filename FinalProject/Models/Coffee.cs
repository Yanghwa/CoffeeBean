using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public partial class Coffee : BaseModel
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public string CoffeeId { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Coffee Name")]
        public string Name { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        [Display(Name = "Beans")]
        [InverseProperty("Coffee")]
        public virtual ICollection<CoffeeBean> Beans { get; set; } = new HashSet<CoffeeBean>();

        public override string ToString()
        {
            return String.Format("{0}", Name);
        }
    }
}