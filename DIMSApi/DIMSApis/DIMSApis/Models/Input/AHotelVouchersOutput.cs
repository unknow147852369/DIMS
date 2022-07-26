using System.ComponentModel.DataAnnotations;

namespace DIMSApis.Models.Input
{
    public class AHotelVouchersInput
    {
        public int VoucherId { get; set; }
        public int HotelId { get; set; }
        public string VoucherName { get; set; }
        public string VoucherCode { get; set; }
        [Required(ErrorMessage = "Can't be empty"), Range(1, 100, ErrorMessage = "Please enter a value bigger than {1}")]
        public int? VoucherSale { get; set; }
        [Required(ErrorMessage = "Can't be empty"), Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int? Quantitylimited { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }
    }
}
