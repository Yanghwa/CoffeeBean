using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public partial class CoffeeBean : BaseModel
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public string CoffeeBeanId { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Coffee Name")]
        public string CoffeeId { get; set; }

        public virtual Coffee Coffee { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Bean Name")]
        public string BeanId { get; set; }

        public virtual Bean Bean { get; set; }

    }
}