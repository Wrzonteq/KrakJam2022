using System;

namespace PartTimeKamikaze.KrakJam2022.Utils {
    public class Observable <T> {
        public event Action<T> ChangedValue;
        public event Action Changed;

        T value;
        readonly bool isValueType;

        public T Value {
            get => value;
            set => Set(value);
        }

        public Observable(T value = default) {
            this.value = value;
            isValueType = typeof(T).IsValueType;
        }

        public void Set(T newValue, bool forceUpdate = false) {
            var equals = isValueType ? value.Equals(newValue) : ReferenceEquals(value, newValue);
            if (equals && !forceUpdate)
                return;
            value = newValue;
            Changed?.Invoke();
            ChangedValue?.Invoke(value);
        }
    }
}
