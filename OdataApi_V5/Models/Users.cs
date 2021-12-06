using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OdataApi_V5.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public DateTime Created { get; set; }
        public DateTime Closed { get; set; }
    }
}
