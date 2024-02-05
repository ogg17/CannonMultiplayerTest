using UnityEngine;
using VgGames.Core.Config;
using VgGames.Core.ObjectPoolModule;

namespace VgGames.Game.Player.Balls
{
    public class BallScript : MonoBehaviour, IPooledObject
    {
        private Rigidbody _rigidbody;
        private Transform _transform;

        public float Time { get; set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _transform = transform;
        }

        public bool GetIsEnabled() => isActiveAndEnabled;

        public void Enable() => gameObject.SetActive(true);

        public void Disable()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            _transform.position = GameConfig.StartBallPos;
            _transform.rotation = Quaternion.identity;
            
            gameObject.SetActive(false);
        }

        public void Kick(float force, Transform spawn)
        {
            _rigidbody.position = spawn.position;
            _rigidbody.rotation = spawn.rotation;
            _rigidbody.AddForce(spawn.up * force);
        }
    }
}