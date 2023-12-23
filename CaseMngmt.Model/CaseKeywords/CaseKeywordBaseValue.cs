using CaseMngmt.Models.GenericValidation;
using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.CaseKeywords
{
    public class CaseKeywordBaseValue
    {
        [Required]
        public Guid KeywordId { get; set; }
        [Required]
        public string Value { get; set; }
        public bool IsRequired { get; set; }
        public int? MaxLength { get; set; }
        public string TypeName { get; set; }

        public bool Validate()
        {
            // TODO : register generic type
            try
            {
                if (IsRequired && string.IsNullOrEmpty(Value))
                {
                    return false;
                }
                if (!DataTypeDictionary.DataTypeAlias.TryGetValue(TypeName, out var type))
                {
                    return false;
                }

                var genericValidator = new GenericValidator();
                if (MaxLength != null)
                {
                    genericValidator.Register<string>(t => t.Length <= MaxLength);
                }
                else
                {
                    genericValidator.Register<string>(t => true);
                }

                var validator = genericValidator.Retrieve(type);

                return validator(Value);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public class CaseKeywordValue : CaseKeywordBaseValue
    {
        public string KeywordName { get; set; }
        public Guid TypeId { get; set; }
        public int Order { get; set; }
        public bool Searchable { get; set; }
    }
}
