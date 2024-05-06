using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Issuance
    {
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }
        [Required]
        public int ClientId { get; set; }
        [Required]
        public DateTime IssuanceDate { get; set; }
        [Required]
        public DateTime RequiredReturningDate { get; set; }

        public DateTime? ActualReturningDate { get; set; }

        public IssuanceStatus Status { get; set; }
    }
}
