using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseMngmt.Models.CompanyTemplates
{

    public class CompanyTemplate
    {
        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        public Guid TemplateId { get; set; }
    }
}
