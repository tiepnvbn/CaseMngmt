﻿using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.CaseKeywords
{
    public class CaseKeywordRequest
    {
        [Required]
        public Guid CaseId { get; set; }
        [Required]
        public List<CaseKeywordValue> KeywordValues { get; set; }
    }
}