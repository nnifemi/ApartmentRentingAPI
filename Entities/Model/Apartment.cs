using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Model
{
    public class Apartment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApartmentID { get; set; }
        public int LandlordID { get; set; }
        public string Location { get; set; }
        public string FlatNumber { get; set; }
        public string Description { get; set; }
        public string Amenities { get; set; }
        public decimal Price { get; set; }
        public bool Availability { get; set; }
    }
}
