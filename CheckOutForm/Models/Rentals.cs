using System;
using System.ComponentModel.DataAnnotations;

namespace CheckOutForm.Models
{
    public class Rentals
    {
        public int RentalsID { get; set; }
        [Display(Name = "Renter's Name"), StringLength(50), RegularExpression(@"(^[a-zA-Z0-9 _.-]*[a-zA-Z0-9 _.-])*$", ErrorMessage = "You are only allowed to use the following special characters _ . - ")]
        public string RenterName { get; set; }

        [Display(Name = "Start Date"), DataType(DataType.Date)]
        public DateTime BeginRental { get; set; }
 
        [Display(Name = "Return Date"), DataType(DataType.Date)]
        public DateTime EndRental { get; set; }
        [Display(Name = "Approved By"), StringLength(50), RegularExpression(@"(^[a-zA-Z0-9 _.-]*[a-zA-Z0-9 _.-])*$", ErrorMessage = "You are only allowed to use the following special characters _ . - ")]
        public string ApprovedBy { get; set; }
        [Display(Name = "Notes"), StringLength(250), RegularExpression(@"(^[a-zA-Z0-9 ,/;:'""!@#$%^&*()_+.-=?]*[a-zA-Z0-9 ,/;:'""!@#$%^&*()_+.-=?])*$", ErrorMessage = "You are using a special character that is not allowed.")]
        public string Info { get; set; }

        
        public int? CurrentStatusID { get; set; }
        public CurrentStatus CurrentStatus { get; set; }

        public int? DevicesID { get; set; }
        public Devices Devices { get; set; }
    }
}
