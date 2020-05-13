using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LuneauAuthentication.Inputs
{
    public class OrganizationInput
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
    }
}
