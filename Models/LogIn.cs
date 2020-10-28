using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ManageProjects.Models
{
    public class LogIn
    {
        [Required]
        [DisplayName("Email")]
        public string Username { get; set; }

        [Required]
        [DataType("Password")]
        public string Password { get; set; }
    }
}
