using UnityEngine;
using VgGames.Core.Config;

namespace VgGames.Core.ObjectPoolModule
{
    public class ObjectPool<T> : BaseObjectPool<T> where T : class, IPooledObject
    {
        private readonly GameObject _prefab;
        
        public ObjectPool(GameObject prefab) => _prefab = prefab;

        protected override T Instantiate() => Object.Instantiate(_prefab, GameConfig.StartBallPos, Quaternion.identity).GetComponent<T>();
    }
}