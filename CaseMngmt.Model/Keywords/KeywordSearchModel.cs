using System.ComponentModel.DataAnnotations;

namespace CaseMngmt.Models.Keywords
{
    public class KeywordSearchModel : ICloneable
    {
        public Guid KeywordId { get; set; }
        [MaxLength(256)]
        public string? KeywordName { get; set; }
        public int? MaxLength { get; set; }
        public List<string> Metadata { get; set; }
        public int Order { get; set; }
        public Guid TypeId { get; set; }
        public string? TypeName { get; set; }
        public string? TypeValue { get; set; }
        public string Value { get; set; } = string.Empty;
        public bool? QueryFrom { get; set; }
        public bool? QueryTo { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
