using System.ComponentModel.DataAnnotations;

namespace CheckOutForm.Models.DataGroups
{
    public class DeviceTypeGroup
    {
        [Display(Name = "Device Type")]
        public string DeviceTypes { get; set; }
        [Display(Name = "Device Count")]
        public int DeviceTypesCount { get; set; }
        public int RetiredCount { get; set; }
        public int AvailableLoaners { get; set; }
        public int ADevices { get; set; }
        public int BDevices { get; set; }
        public int CDevices { get; set; }
        public int DDevices { get; set; }
        public int ARetired { get; set; }
        public int BRetired { get; set; }
        public int CRetired { get; set; }
        public int DRetired { get; set; }
    }
}
