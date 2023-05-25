using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SuperShop.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50, ErrorMessage ="The filed {0} can't contain {1} characters lenght.")]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        //0:C2 o C = currency
        public decimal Price { get; set; }

        [Display(Name = "Image")]
        //Nomeia o campo na web
        public string ImageUrl { get; set; }

        [Display(Name ="Last Purchase")]
        public DateTime? LastPurchase { get; set; }

        [Display(Name = "Last Sale")]
        public DateTime? LastSale { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [DisplayFormat(DataFormatString ="{0:N2}", ApplyFormatInEditMode =false)]
        public double Stock { get; set; }
    }
}
