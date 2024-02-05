using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using VgGames.Core.EventBusModule;
using VgGames.Core.InjectModule;
using VgGames.Core.ObjectPoolModule;
using VgGames.Game.RuntimeData.PlayerColorModule;
using VgGames.Game.Signals;

namespace VgGames.Game.Player.Balls
{
    public class BallsManager : MonoBehaviour, IOnEventCallback
    {
        [Inject] private EventBus _eventBus;

        [SerializeField] private Transform spawnPoint;
        [SerializeField] private GameObject prefab;
        [SerializeField] private float despawnTime = 30;
        [SerializeField] private float kickForce = 100;
        [SerializeField] private Vector2 forceRange;

        private PhotonView _view;
        private BaseObjectPool<BallScript> _balls;
        private BallsTimer _timer;
        private PlayerColor _playerColor;

        private const byte KickBallEventCode = 1;

        public void Activate(PlayerColor color)
        {
            InjectSystem.Inject(this);
            PhotonNetwork.AddCallbackTarget(this);
            _balls = new ObjectPool<BallScript>(prefab);
            _timer = new BallsTimer(despawnTime);
            _view = GetComponent<PhotonView>();
            _playerColor = color;
            if (_view.IsMine)
                _eventBus.Add<MouseClickSignal>(OnClick);
        }

        private void OnClick(MouseClickSignal signal)
        {
            var force = Mathf.Clamp(signal.Force, forceRange.x, forceRange.y);
            KickBall(force, _playerColor);
            SendKickEvent(force);
        }

        private void KickBall(float force, PlayerColor color)
        {
            var ball = _balls.GetObject();
            ball.Kick(kickForce * force, spawnPoint);
            ball.tag = color.ToTag();
        }

        private void SendKickEvent(float force)
        {
            object[] data = { _view.ViewID, force, (int)_playerColor };
            var raiseEventOptions = new RaiseEventOptions
                { Receivers = ReceiverGroup.Others, CachingOption = EventCaching.DoNotCache };
            var sendOptions = new SendOptions { Reliability = true };

            PhotonNetwork.RaiseEvent(KickBallEventCode, data, raiseEventOptions, sendOptions);
        }

        public void Update() => _timer.UpdateTimer(_balls);

        public void Deactivate()
        {
            if (_view.IsMine)
                _eventBus.Remove<MouseClickSignal>(OnClick);
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code != KickBallEventCode) return;

            var data = (object[])photonEvent.CustomData;
            if ((int)data[0] != _view.ViewID) return;

            KickBall((float)data[1], (PlayerColor)(int)data[2]);
        }
    }
}