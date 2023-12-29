﻿using CaseMngmt.Models.Keywords;
using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.Templates
{
    public class TemplateRequest
    {
        [Required]
        public Guid CompanyId { get; set; }
        [Required]
        public List<KeywordRequest> KeywordRequests { get; set; }
    }

    public class TemplateViewRequest
    {
        [Required]
        public Guid TemplateId { get; set; }
        [Required]
        public Guid CompanyId { get; set; }
        [Required]
        public List<KeywordRequest> KeywordRequests { get; set; }
    }
}
