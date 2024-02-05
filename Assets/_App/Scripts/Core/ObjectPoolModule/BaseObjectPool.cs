using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace VgGames.Core.ObjectPoolModule
{
    public abstract class BaseObjectPool<T> where T : class, IPooledObject
    {
        private readonly List<T> _pool = new();

        public List<T> GetObjects() 
            => _pool.ToList();
        
        public T GetObject()
        {
            var obj = _pool.FirstOrDefault(o => !o.GetIsEnabled());
            if (obj == null)
            {
                if (TryInstantiate(out var res))
                {
                    _pool.Add(res);
                    obj = res;
                }
                else throw new Exception($"Error: Failed to Instantiate object of {typeof(T)}!");
            }
            else obj.Enable();

            return obj;
        }
        
        public bool TryAdd(T obj)
        {
            if (obj == null)
                return false;
            Add(obj);
            return true;
        }

        public void Add(T obj)
        {
            _pool.Add(obj);
        }

        public async UniTask<T> GetObjectAsync()
        {
            var pool = _pool.ToList();
            if (pool.Count <= 0) return null;
            await UniTask.WaitUntil(() => pool.Any(o => !o.GetIsEnabled()));
            var obj = _pool.First(o => !o.GetIsEnabled());
            obj.Enable();
            return obj;
        }

        protected abstract T Instantiate();

        private bool TryInstantiate(out T res)
        {
            var obj = Instantiate();
            if (obj == null)
            {
                res = null;
                return false;
            }

            res = obj;
            return true;
        }
    }
}