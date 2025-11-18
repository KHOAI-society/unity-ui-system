using System;
using System.Collections.Generic;

namespace Khoai
{
    public class Reactive<T>
    {
        private T value;
        public event Action<T> OnChanged;

        public T Value
        {
            get => value;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(this.value, value))
                {
                    this.value = value;
                    OnChanged?.Invoke(value);
                }
            }
        }
    }
}