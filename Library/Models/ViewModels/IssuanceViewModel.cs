using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Models.ViewModels
{
    public class IssuanceViewModel
    {

        [Required]
        public int BookId { get; set; }
        [Required]
        public int ClientId { get; set; }
        [Required]
        public DateTime RequiredReturningDate { get; set; }

    }
}