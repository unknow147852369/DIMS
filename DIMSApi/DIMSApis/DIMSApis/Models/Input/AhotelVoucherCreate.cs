using System.ComponentModel.DataAnnotations;

namespace DIMSApis.Models.Input
{
    public class AhotelVoucherCreate
    {
        public int HotelId { get; set; }
        [Required(ErrorMessage = "Can't be empty")]
        public string? VoucherName { get; set; }
        [Required(ErrorMessage = "Can't be empty")]
        public string? VoucherCode { get; set; }
        [Required(ErrorMessage = "Can't be empty"), Range(1, 100, ErrorMessage = "Please enter a value bigger than {1}")]
        public int? VoucherSale { get; set; }
        [Required(ErrorMessage = "Can't be empty"), Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int? Quantitylimited { get; set; }
        [Required(ErrorMessage = "Can't be empty")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "Can't be empty")]
        public DateTime? EndDate { get; set; }
    }
}
