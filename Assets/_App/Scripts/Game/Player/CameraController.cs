using UnityEngine;
using VgGames.Core.EventBusModule;
using VgGames.Core.InjectModule;
using VgGames.Core.StateEvents.GameState;
using VgGames.Core.StateEvents.InitState;
using VgGames.Game.RuntimeData;

namespace VgGames.Game.Player
{
    public class CameraController : MonoBehaviour, IInit, IGameActivate, IGameDeactivate, IGameUpdate
    {
        [Inject] private EventBus _eventBus;
        [Inject] private PlayerCameraPositionData _playerCameraPositionData;

        [SerializeField] private float smooth;

        private bool _isEnable;
        private Transform _cameraTransform;

        public void Init() => _cameraTransform = transform;

        public void GameActivate()
        {
            _isEnable = true;
        }

        public void GameDeactivate()
        {
            _isEnable = false;
        }

        public void GameUpdate()
        {
            if (!_isEnable || _playerCameraPositionData.IsNull) return;
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position,
                _playerCameraPositionData.Camera.position, smooth * Time.deltaTime);
            _cameraTransform.rotation = Quaternion.Slerp(_cameraTransform.rotation,
                _playerCameraPositionData.Camera.rotation, smooth * Time.deltaTime);
        }
    }
}