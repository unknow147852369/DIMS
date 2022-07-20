using System.ComponentModel.DataAnnotations;

namespace DIMSApis.Models.Input
{
    public class FeedBackInput
    {
        [Range(0, 10)]
        public double? Rating { get; set; }

        public string? Comment { get; set; }
    }
}