using Photon.Pun;
using UnityEngine;
using VgGames.Core.EventBusModule;
using VgGames.Core.InjectModule;
using VgGames.Game.RuntimeData;
using VgGames.Game.Signals;

namespace VgGames.Game.Player
{
    public class PlayerInput : MonoBehaviour
    {
        [Inject] private EventBus _eventBus;
        [Inject] private PlayerSessionData _playerSessionData;
        [Inject] private PlayerSetterData _playerSetter;

        [SerializeField] private Vector2 minAxis;
        [SerializeField] private Vector2 maxAxis;
        [SerializeField] [Range(1, 100)] private float sensitivity;

        private PhotonView _view;
        private Vector3 _pointer;
        private Vector2 _minAxis;
        private Vector2 _maxAxis;

        public void Activate()
        {
            InjectSystem.Inject(this);
            _view = GetComponent<PhotonView>();

            if (!_view.IsMine) return;
            SetPos();
            SetAxis();
            _eventBus.Add<InputSignal>(OnInput);
        }

        private void SetAxis()
        {
            _pointer = transform.localEulerAngles;
            _minAxis.x = _pointer.y + minAxis.x;
            _maxAxis.x = _pointer.y + maxAxis.x;
            _minAxis.y = minAxis.y;
            _maxAxis.y = maxAxis.y;
        }

        private void SetPos()
        {
            var pos = _playerSetter.Setter.GetPlayerPosition(_playerSessionData.Color);
            var playerTransform = transform;
            playerTransform.position = pos.position;
            playerTransform.rotation = pos.rotation;
        }

        private void OnInput(InputSignal signal)
        {
            var deltaX = signal.Position.y * Time.deltaTime * sensitivity;
            var deltaY = signal.Position.x * Time.deltaTime * sensitivity;

            _pointer.x = Mathf.Clamp(_pointer.x - deltaX, _minAxis.y, _maxAxis.y);
            _pointer.y = Mathf.Clamp(_pointer.y + deltaY, _minAxis.x, _maxAxis.x);

            transform.localEulerAngles = _pointer;
        }

        public void Deactivate()
        {
            if (!_view.IsMine) return;
            _eventBus.Remove<InputSignal>(OnInput);
        }
    }
}