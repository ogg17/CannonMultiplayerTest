using UnityEngine;
using VgGames.Core.ObjectPoolModule;

namespace VgGames.Game.Player.Balls
{
    public class BallsTimer
    {
        private readonly float _despawnTime;

        public BallsTimer(float despawnTime)
        {
            _despawnTime = despawnTime;
        }
        
        public void UpdateTimer(BaseObjectPool<BallScript> pool)
        {
            foreach (var obj in pool.GetObjects())
            {
                if (!obj.GetIsEnabled()) continue;
                obj.Time += Time.deltaTime;

                if (obj.Time < _despawnTime) continue;
                obj.Time = 0;
                obj.Disable();
            }
        }
    }
}