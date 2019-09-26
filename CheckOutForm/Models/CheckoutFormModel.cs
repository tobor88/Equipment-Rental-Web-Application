using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckOutForm.Models
{
    public class CheckoutFormModel
    {
        [BindProperty]
        public string PostedMessage { get; set; } = "Thank you for your request. You will be contacted shortly.";
        [BindProperty, Required, StringLength(40, ErrorMessage = "Name cannot be longer than 40 characters."), RegularExpression(@"(^[a-zA-Z0-9 ,_.-]*[a-zA-Z0-9 ,/:&*()_+.-])*$", ErrorMessage = "Only the following special characters are allowed , / : & * ( ) _ + . -")]
        public string Name { get; set; }
        [BindProperty, Required, StringLength(40, ErrorMessage = "Manager cannot be longer than 40 characters."), RegularExpression(@"(^[a-zA-Z0-9 @_.-]*[a-zA-Z0-9 @_.-])*$", ErrorMessage = "Only the following special characters are allowed @ _ . -")]
        public string Manager { get; set; }
        [BindProperty, Required, StringLength(40, ErrorMessage = "Email cannot be longer than 40 characters."), RegularExpression(@"(^[a-zA-Z0-9 @_.-]*[a-zA-Z0-9 @_.-])*$", ErrorMessage = "Only the following special characters are allowed @ _ . -")]
        public string Email { get; set; }
        [BindProperty]
        public int Sizes { get; set; }
        [BindProperty]
        public List<SelectListItem> Laptop { get; set; }
        [BindProperty]
        public int Projectors { get; set; }
        public List<SelectListItem> Projector { get; set; }
        [BindProperty]
        public bool Application { get; set; }
        [BindProperty]
        public bool CreativeCloud { get; set; }
        [BindProperty]
        public bool Bitwarden { get; set; }
        [BindProperty]
        public bool ProtonMail { get; set; }
        [BindProperty]
        public bool NordVPN { get; set; }
        [BindProperty]
        public bool Office365 { get; set; }
        [BindProperty, StringLength(250, ErrorMessage = "Reasons cannot be longer than 250 characters.")]
        public string Reasons { get; set; }
        [BindProperty, StringLength(250, ErrorMessage = "Information cannot be longer than 250 characters.")]
        public string Information { get; set; }
        [BindProperty, DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true), DataType(DataType.Date)]
        public DateTime DateRange { get; set; } = DateTime.UtcNow;
        [BindProperty, DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true), DataType(DataType.Date)]
        public DateTime DateStart { get; set; } = DateTime.UtcNow;
        [BindProperty, DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true), DataType(DataType.Date)]
        public DateTime DateEnd { get; set; } = DateTime.UtcNow.AddDays(30);
        [BindProperty, StringLength(6, ErrorMessage = "Hotspots cannot be longer than 6 digits.")]
        public string HotSpots { get; set; }
    }
}
