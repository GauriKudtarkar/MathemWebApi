using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryDatesGenerator.Models
{
    public class Product
    {
        [Required]
        public int productId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public HashSet<DayOfWeek> DeliveryDays  { get; set; }

        [Required]
        public ProductType TypeOfProduct { get; set; }

        [Required]
        public int DaysInAdvance { get; set; }
        public int Quantity { get; set; }

    }
}
