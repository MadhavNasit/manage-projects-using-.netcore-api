using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageProjects.Models
{
    public class AddUpdateProject
    {
        public int id { get; set; }
        public string ProjectName { get; set; }
        public string Image { get; set; }
        public IFormFile ProjectImage { get; set; }
        public int Duration { get; set; }
        public int Cost { get; set; }
        public string Description { get; set; }
    }
}
