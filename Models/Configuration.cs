//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KeyPay.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.InteropServices;

    public partial class Configuration
    {

        public int ID { get; set; }

        [Required(ErrorMessage = "Intacct URL is required.")]
        public string IntacctURL { get; set; }

        [Required(ErrorMessage = "Intacct Company ID is required.")]
        public string IntacctCompanyID { get; set; }

        //[Required]
        public byte[] IntacctUserName { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Intacct User Name is required.")]
        public string strIntacctUserName { get; set; }

        //[Required]
        public byte[] IntacctPassword { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Intacct Password is required.")]
        public string strIntacctPassword { get; set; }

        [Required(ErrorMessage = "Intacct Sender ID is required.")]
        public string IntacctSenderID { get; set; }

        //[Required]
        public byte[] IntacctSenderPassword { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Intacct Sender Password is required.")]
        public string strIntacctSenderPassword { get; set; }

        [Required(ErrorMessage = "Journal URL is required.")]
        public string JournalURL { get; set; }

        //[Required]
        public byte[] KeyPayAPI { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "KeyPay API is required.")]
        public string strKeyPayAPI { get; set; }

        [Required(ErrorMessage = "Business ID is required.")]
        public string BusinessID { get; set; }

        [Required(ErrorMessage = "Journal is required.")]
        public string Journal { get; set; }

        [Required]
        public string PostingMode { get; set; }

        [Required(ErrorMessage = "Log files path is required.")]
        public string LogFiles { get; set; }

        [Required(ErrorMessage = "From email address is required.")]
        [DataType(DataType.EmailAddress)]
        public string FromEmail { get; set; }

        //[Required]
        public byte[] EmailPassword { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Email Password is required.")]
        public string strEmailPassword { get; set; }

        [Required(ErrorMessage = "SMTP Server is required.")]
        public string SMTPServer { get; set; }

        [Required(ErrorMessage = "Email Port is required.")]
        public string EmailPort { get; set; }

        [Required]
        public bool SSL { get; set; }

        [Required(ErrorMessage = "To email address is required.")]
        [DataType(DataType.EmailAddress)]
        public string ToEmail { get; set; }

        [DataType(DataType.EmailAddress)]
        public string CCEmail { get; set; }

        public int Department { get; set; }

        public int Location { get; set; }

        public int Project { get; set; }

        public string EmailUser { get; set; }

    }

    public enum PostingModes
    {
        Draft,
        Posted
    }

}
