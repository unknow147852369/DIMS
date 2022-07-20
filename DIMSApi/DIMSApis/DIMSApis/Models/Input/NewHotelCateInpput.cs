using System.ComponentModel.DataAnnotations;

namespace DIMSApis.Models.Input
{
    public class NewHotelCateInpput
    {
        [Required(ErrorMessage = "Can't be empty")]
        public int? HotelId { get; set; }
        [Required(ErrorMessage = "Can't be empty")]
        public string? CategoryName { get; set; }
        public string? CateDescrpittion { get; set; }
        [Required(ErrorMessage = "Can't be empty"), Range(1, double.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public double? PriceDefault { get; set; }
        [Required(ErrorMessage = "Can't be empty"), Range(1, 365, ErrorMessage = "Please enter a value bigger than {1}")]
        public int? Quanity { get; set; }
    }
}
