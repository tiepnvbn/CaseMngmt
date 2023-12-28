using CaseMngmt.Models.GenericValidation;
using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.CaseKeywords
{
    public class CaseKeywordBaseValue
    {
        [Required]
        public Guid KeywordId { get; set; }
        [MaxLength(256)]
        public string KeywordName { get; set; }
        [Required]
        public string Value { get; set; }
        public Guid TypeId { get; set; }
        public string? TypeName { get; set; }
        public string TypeValue { get; set; }
        public bool IsRequired { get; set; }
        public int? MaxLength { get; set; }
        public bool Searchable { get; set; }
        public int Order { get; set; }
        public IEnumerable<string> Metadata { get; set; }
        public bool Validate()
        {
            try
            {
                if (IsRequired && string.IsNullOrEmpty(Value))
                {
                    return false;
                }
                Type type;
                if (!DataTypeDictionary.DataTypeAlias.TryGetValue(TypeValue.ToLower(), out type))
                {
                    return false;
                }

                var genericValidator = new GenericValidator();
                return genericValidator.IsValid(type, Value, MaxLength);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
    //public class CaseKeywordValue : CaseKeywordBaseValue
    //{
    //    [Required]
    //    public Guid CaseId { get; set; }
    //    public string CaseName { get; set; }
    //}
}
