using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace demo_DI
{
    public class Container : IoContainer
    {
        private Dictionary<object, object> _mappings;
       
        public Container()
        {
            _mappings = new Dictionary<object, object>();
        }
        
        public void Register<T>() where T : class
        {
            foreach(var type in typeof(T).GetInterfaces())
            {
                _mappings.Add(typeof(T),type);
            }
            
        }

        public void Register<T, R>() where R : class, T
        {
            if (typeof(T).IsAssignableFrom(typeof(R)))
            {
                _mappings.Add(typeof(T), typeof(R));
            }
            else

            {
                throw new Exception();
            }
        }

            public void Register<T>(Func<T> factory)
        {
            _mappings.Add(typeof(T), factory);
        }

        public T Resolve<T>()
        {
           
           if (typeof(T).IsClass)
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
           else 
            {
                bool containsValue = _mappings.ContainsValue(typeof(T));
                bool containsKey = _mappings.ContainsKey(typeof(T));
                if (containsKey)
                {
                    if (  _mappings[typeof(T)] is Delegate)
                    {
                        return ((Func<T>)_mappings[typeof(T)]).Invoke();
                    }
                    else
                    {
                        return (T)Activator.CreateInstance((Type)_mappings[typeof(T)]);
                    }
                }
                else if (containsValue)
                {
                    foreach(var t in _mappings.Keys)
                    {
                       
                        if ((Type)_mappings[t]==typeof(T))
                        {
                            if ((Type)t == typeof(Delegate))
                            {
                                return ((Func<T>)_mappings[t]).Invoke();
                            }
                            else
                            {
                                return (T)Activator.CreateInstance((Type)t);
                            }
                        }
                    }
                }
            }
           throw new Exception();
        }
    }
}
