using CoffeeBean.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Rating : BaseModel
    {
        [Key]
        [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
        public string RatingId { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "User")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Bean")]
        public string BeanId { get; set; }

        [ForeignKey("BeanId")]
        public virtual Bean Bean { get; set; }

        [Range(0, 9)]
        [Display(Name = "Rank")]
        public decimal Rank { get; set; }
    }
}