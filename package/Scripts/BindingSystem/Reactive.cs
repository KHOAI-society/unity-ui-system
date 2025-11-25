using System;
using System.Collections.Generic;

namespace Khoai
{
    public class Reactive<T>
    {
        private T value;
        private event Action<T> OnChanged;

        public T Value
        {
            get => value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(this.value, value)) return;
                this.value = value;
                OnChanged?.Invoke(value);
            }
        }

        public Reactive(T newValue) => Value = newValue;

        public void RegisterListerner(Action<T> func)
        {
            OnChanged += func;
            func?.Invoke(value);
        }

        public void DeregisterListener(Action<T> func)
        {
            OnChanged -= func;
        }
    }
}