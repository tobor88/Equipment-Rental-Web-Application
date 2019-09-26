using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckOutForm.Models
{
    public class CurrentStatus
    {
        public int CurrentStatusID { get; set; }

        [Required, StringLength(30, ErrorMessage = "Device Type cannot be longer than 30 characters."), Display(Name = "Approval Status"), RegularExpression(@"(^[a-zA-Z0-9 _.-]*[a-zA-Z0-9 _.-])*$", ErrorMessage = "You are only allowed to use the following special characters . _ - ")]
        public string ApprovalStatus { get; set; }

        public int? RentalsID { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public ICollection<Rentals> Rentals { get; set; }
    }
}
