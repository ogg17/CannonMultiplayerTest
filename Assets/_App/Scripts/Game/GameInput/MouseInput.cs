using UnityEngine;
using VgGames.Core.EventBusModule;
using VgGames.Core.InjectModule;
using VgGames.Game.RuntimeData;
using VgGames.Game.Signals;

namespace VgGames.Game.GameInput
{
    public class MouseInput
    {
        [Inject] private EventBus _eventBus;
        [Inject] private KickForceData _kickForceData;

        private Vector2 _lastPosition;
        private Vector2 _position;
        private bool _isClick;
        private float _clickTime;

        private readonly InputSignal _inputSignal = new();
        private readonly MouseClickSignal _mouseClickSignal = new();

        public MouseInput() => InjectSystem.Inject(this);

        public void Update()
        {
            SetClick();
            SetPosition();
        }

        private void SetClick()
        {
            if (!_isClick)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _clickTime = Time.timeSinceLevelLoad;
                    _isClick = true;
                }
                
                _kickForceData.Force.Value = 0;
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    _mouseClickSignal.Force = Time.timeSinceLevelLoad - _clickTime;
                    _eventBus.Invoke(_mouseClickSignal);
                    _isClick = false;
                }
                
                _kickForceData.Force.Value = Time.timeSinceLevelLoad - _clickTime;
            }
        }

        private void SetPosition()
        {
            _position.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            _inputSignal.Position = _position;
            _eventBus.Invoke(_inputSignal);
        }
    }
}