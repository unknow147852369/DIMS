namespace DIMSApis.Models.Output
{
    public class HostDoorLogoutput
    {
        public int DoorLogId { get; set; }
        public int? RoomlId { get; set; }
        public string? RoomName { get; set; }
        public string? DoorQrContent { get; set; }
        public string? DoorCondition { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? DoorLogStatus { get; set; }
    }
}