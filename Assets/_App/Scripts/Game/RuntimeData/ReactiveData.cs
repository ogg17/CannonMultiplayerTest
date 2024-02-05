using System;
using Debug = VgGames.Core.CustomDebug.Debug;

namespace VgGames.Game.RuntimeData
{
    public class ReactiveData<T>
    {
        public Action<T> OnChange;

        private T _value;
        private readonly Predicate<T> _predicate;

        public T Value
        {
            get => _value;
            set
            {
                if (_predicate != null && !_predicate.Invoke(value))
                {
                    Debug.Log($"{typeof(T)} Predicate block!");
                    return;
                }

                _value = value;
                OnChange?.Invoke(_value);
            }
        }

        public ReactiveData(T value, Predicate<T> predicate = null)
        {
            _predicate = predicate;
            Value = value;
        }

        public static implicit operator T(ReactiveData<T> data) => data.Value;
        public static implicit operator ReactiveData<T>(T data) => new(data);
    }
}