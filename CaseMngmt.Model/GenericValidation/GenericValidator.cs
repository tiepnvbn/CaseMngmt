namespace CaseMngmt.Models.GenericValidation
{
    public class GenericValidator
    {
        private Dictionary<Type, Delegate> _validators = new Dictionary<Type, Delegate>();

        public void Register<T>(Func<T, bool> validator)
        {
            _validators[typeof(T)] = validator;
        }

        public Func<T, bool> Retrieve<T>()
        {
            if (_validators.ContainsKey(typeof(T)))
            {
                return (Func<T, bool>)_validators[typeof(T)];
            }
            return t => false;
        }

        public Func<object, bool> Retrieve(Type type)
        {
            Delegate x = (Delegate)GetType()
                .GetMethod("Retrieve", new Type[] { })
                .MakeGenericMethod(type).Invoke(this, null);
            Func<object, bool> y = o => (bool)x.DynamicInvoke(o);
            Console.WriteLine(1);
            return y;
        }

        public void Deregister<T>()
        {
            if (_validators.ContainsKey(typeof(T)))
            {
                _validators.Remove(typeof(T));
            }
        }
    }
}
