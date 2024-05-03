namespace Entities.Model.DTOs
{
    public class ApartmentDTO
    {
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
