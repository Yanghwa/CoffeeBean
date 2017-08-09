using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public partial class Bean : BaseModel
    {
        public Bean()
        {
            Coffees = new HashSet<CoffeeBean>();
        }
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public string BeanId { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Bean Name")]
        public string Name { get; set; }
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        [Display(Name = "Coffee")]
        [InverseProperty("Bean")]
        public virtual ICollection<CoffeeBean> Coffees { get; set; } = new HashSet<CoffeeBean>();

        [Display(Name = "Ratings")]
        [InverseProperty("Bean")]
        public virtual ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();

        [NotMapped]
        public decimal OverallRating
        {
            get
            {
                if (Ratings.Count() > 0)
                {
                    return (Ratings.Average(x => x.Rank));
                }
                else
                {
                    return (9);
                }

            }
        }

        public override string ToString()
        {
            return String.Format("{0}", Name);
        }

    }
}