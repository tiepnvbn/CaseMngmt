namespace CaseMngmt.Models.GenericValidation
{
    public class GenericValidator
    {
        public bool IsValid(Type type, string value, int? maxlength)
        {
            try
            {
                object result = null;
                if (type.IsValueType)
                {
                    result = Activator.CreateInstance(type);
                }
                

                // Get a reference to the TryParse method for this type
                var tryParseMethod = type.GetMethod("TryParse", new[] { typeof(string), type.MakeByRefType() });
                // Call TryParse
                var success = tryParseMethod.Invoke(null, new[] { value, result }) is bool;
                if (success)
                {
                    if (maxlength != null)
                    {
                        return value.Length <= maxlength;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
