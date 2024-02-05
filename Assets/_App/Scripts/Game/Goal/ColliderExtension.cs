using UnityEngine;
using VgGames.Game.Player.Balls;

namespace VgGames.Game.Goal
{
    public static class ColliderExtension
    {
        public static void DisableBall(this Collider col)
        {
            if (col.TryGetComponent<BallScript>(out var ball)) ball.Disable();
        }
    }
}