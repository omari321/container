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
        private List<object> _list;
       
        public Container()
        {
            _list=new List<object>();
        }
        
        public void Register<T>() where T : class
        {

               _list.Add(typeof(T));
        }

        public void Register<T, R>() where R : class, T
        {
            if (typeof(T).IsAssignableFrom(typeof(R)))
            {
                _list.Add(typeof(R));
            }
        }

            public void Register<T>(Func<T> factory)
        {
            _list.Add(factory);
        }

        public T Resolve<T>()
        {
            Console.WriteLine(typeof(T));
            foreach(var item in _list)
            {
                Console.WriteLine(item.GetType());
                if (typeof(Delegate).IsAssignableFrom(item.GetType()))
                {   
                  
                  return ((Func<T>)item).Invoke();
                }
                else if (typeof(T).IsAssignableFrom((Type)item))
                {
                   return (T)Activator.CreateInstance((Type)item);
                }
              }
             throw new Exception();
        }
    }
}
