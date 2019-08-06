using System;
using System.Collections.Generic;
using System.Text;


    public class ListofPairs<T1, T2> : List<KeyValuePair<T1, T2>>
    {
        public void Add(T1 key, T2 value)
        {
            var element = new KeyValuePair<T1, T2>(key, value);
            this.Add(element);            
        }

        //public ListofPairs<T1, T2> this[T1 index]
        //{
        //    get{ return }
        //    set{}
        //}
        public System.Collections.ICollection Values
        {
            get
            {
                List<T2> valuesReturn = new List<T2>();
                foreach (var key in this)
                {
                    valuesReturn.Add(key.Value);
                }
                return valuesReturn;
            }
        }
    }

