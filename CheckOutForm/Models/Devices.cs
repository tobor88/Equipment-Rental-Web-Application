using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckOutForm.Models
{
    public class Devices
    {
        public int DevicesID { get; set; }
        [Required, StringLength(30, ErrorMessage = "Device Type cannot be longer than 30 characters."), Display(Name = "Device Type"), RegularExpression(@"(^[a-zA-Z0-9 _.-]*[a-zA-Z0-9 _.-])*$", ErrorMessage = "You are only allowed to use the following special characters . _ - ")]
        public string DeviceType { get; set; }
        [Required, StringLength(18, ErrorMessage = "Asset Tag cannot be longer than 18 characters."), Display(Name = "Asset Tag"), RegularExpression(@"(^[a-zA-Z0-9 _.-]*[a-zA-Z0-9 _.-])*$", ErrorMessage = "You are only allowed to use the following special characters _ . - ")]
        public string NameTag { get; set; }
        [Required, Display(Name = "Assigned User"), StringLength(30, ErrorMessage = "Assigned User cannot be longer than 30 characters."), RegularExpression(@"(^[a-zA-Z0-9 _.-]*[a-zA-Z0-9 _.-])*$", ErrorMessage = "You are only allowed to use the following special characters _ . - ")]
        public string Assignee { get; set; }
        [Display(Name = "Domain"), StringLength(30, ErrorMessage = "Domain cannot be longer than 30 characters."), RegularExpression(@"(^[a-zA-Z0-9 .]*[a-zA-Z0-9 .])*$", ErrorMessage = "You are only allowed to use the following special characters . ")]
        public string Domain { get; set; }
        [Display(Name = "Entry Date"), DataType(DataType.Date)]
        public DateTime EntryDate { get; set; }
        [Required, Display(Name = "Brand"), StringLength(30, ErrorMessage = "Brand cannot be longer than 30 characters."), RegularExpression(@"(^[a-zA-Z0-9 _.-]*[a-zA-Z0-9 _.-])*$", ErrorMessage = "You are only allowed to use the following special characters _ . - ")]
        public string Brand { get; set; }
        [StringLength(30, ErrorMessage = "Model cannot be longer than 30 characters."), Display(Name = "Model"), RegularExpression(@"(^[a-zA-Z0-9 _.-]*[a-zA-Z0-9 _.-])*$", ErrorMessage = "You are only allowed to use the following special characters _ . - ")]
        public string Model { get; set; }
        [StringLength(50, ErrorMessage = "Serial Number cannot be longer than 50 characters."), Display(Name = "Serial#"), RegularExpression(@"(^[a-zA-Z0-9 /:_.-]*[a-zA-Z0-9 /:_.-])*$", ErrorMessage = "You are only allowed to use the following special characters / : _ . - ")]
        public string Serial { get; set; }
        [StringLength(30, ErrorMessage = "OS cannot be longer than 30 characters."), Display(Name = "OS"), RegularExpression(@"(^[a-zA-Z0-9 _.-]*[a-zA-Z0-9 _.-])*$", ErrorMessage = "You are only allowed to use the following special characters _ . - ")]
        public string OS { get; set; }
        [StringLength(30, ErrorMessage = "CPU cannot be longer than 30 characters."), Display(Name = "CPU"), RegularExpression(@"(^[a-zA-Z0-9 _.-]*[a-zA-Z0-9 _.-])*$", ErrorMessage = "You are only allowed to use the following special characters _ . - ")]
        public string CPU { get; set; }
        [Display(Name = "CPU Speed"), StringLength(30, ErrorMessage = "CPU Speed cannot be longer than 30 characters."), RegularExpression(@"(^[a-zA-Z0-9 _.-]*[a-zA-Z0-9 _.-])*$", ErrorMessage = "You are only allowed to use the following special characters _ . - ")]
        public string ProcessorSpeed { get; set; }
        [StringLength(30, ErrorMessage = "RAM cannot be longer than 30 characters."), Display(Name = "RAM"), RegularExpression(@"(^[a-zA-Z0-9 _.-]*[a-zA-Z0-9 _.-])*$", ErrorMessage = "You are only allowed to use the following special characters _ . - ")]
        public string RAM { get; set; }
        [Display(Name = "HD Size"), StringLength(30, ErrorMessage = "HD Size cannot be longer than 30 characters."), RegularExpression(@"(^[a-zA-Z0-9 _.-]*[a-zA-Z0-9 _.-])*$", ErrorMessage = "You are only allowed to use the following special characters _ . - ")]
        public string DriveSize { get; set; }
        [Display(Name = "HD Type"), StringLength(6, ErrorMessage = "HD Type cannot be longer than 6 characters."), RegularExpression(@"(^[a-zA-Z0-9 _.-]*[a-zA-Z0-9 _.-])*$", ErrorMessage = "You are only allowed to use the following special characters _ . - ")]
        public string HardDriveType { get; set; }
        [StringLength(6), Display(Name = "Optical"), RegularExpression(@"(^[a-zA-Z0-9 _.-]+[a-zA-Z0-9 _.-])*$", ErrorMessage = "You are only allowed to use the following special characters _ . - ")]
        public string Optical { get; set; }
        [Display(Name = "Screen Size"), RegularExpression(@"(^[0-9]*[0-9])*$", ErrorMessage = "Use of special characters is not allowed.")]
        public int ScreenSize { get; set; }
        [RegularExpression(@"(^[a-zA-Z0-9 /:_.-]*[a-zA-Z0-9 /:_.-])*$", ErrorMessage = "You are only allowed to use the following special characters / : _ . - "), StringLength(20, ErrorMessage = "User name cannot be longer than 20 characters."), Display(Name = "User")]
        public string User { get; set; }
        [Display(Name = "Password"), StringLength(26, ErrorMessage = "Password cannot be longer than 26 characters."), RegularExpression(@"(^[a-zA-Z0-9 ,/;:'""!@#$%^&*()+.-?]*[a-zA-Z0-9 ,/;:'""!@#$%^&*()_+.-=?])*$")] // DataType(DataType.Password),
        public string Pwd { get; set; }
        [RegularExpression(@"(^[a-zA-Z0-9 ,/;:'""!@#$%^&*()_+.-=?]*[a-zA-Z0-9 ,/;:'""!@#$%^&*()_+.-=?])*$", ErrorMessage = "You are using a special character that is not allowed."), StringLength(90, ErrorMessage = "Note field cannot be longer than 90 characters.")]
        public string Retired { get; set; }
        [RegularExpression(@"(^[a-zA-Z0-9 ,/;:'""!@#$%^&*()_+.-=?]*[a-zA-Z0-9 ,/;:'""!@#$%^&*()_+.-=?])*$", ErrorMessage = "You are using a special character that is not allowed."), StringLength(90, ErrorMessage = "Note field cannot be longer than 90 characters.")]
        public string Notes { get; set; }
        public ICollection<Rentals> Rentals { get; set; }
    }
}
