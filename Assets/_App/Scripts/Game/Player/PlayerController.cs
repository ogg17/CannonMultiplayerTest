using Photon.Pun;
using UnityEngine;
using VgGames.Core.EventBusModule;
using VgGames.Core.InjectModule;
using VgGames.Game.Player.Balls;
using VgGames.Game.RuntimeData;
using VgGames.Game.RuntimeData.PlayerColorModule;
using VgGames.Game.Signals;

namespace VgGames.Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Inject] private PlayerCameraPositionData _playerCameraPositionData;
        [Inject] private ColorSelectorData _colorSelectorData;
        [Inject] private PlayerPointsData _playerPoints;
        [Inject] private PlayerSessionData _playerSessionData;
        [Inject] private EventBus _eventBus;

        [SerializeField] private Transform cameraPos;

        private PlayerInput _playerInput;
        private BallsManager _ballsManager;
        private PhotonView _view;

        private PlayerColor _playerColor;
        public void SetColor(int color) => _playerColor = (PlayerColor)color;
        
        public void Activate()
        {
            InjectSystem.Inject(this);

            _eventBus.Add<GoalCollisionSignal>(OnGoal);
            _playerInput = GetComponent<PlayerInput>();
            _ballsManager = GetComponent<BallsManager>();
            _view = GetComponent<PhotonView>();

            _playerInput.Activate();
            _ballsManager.Activate(_playerColor);
            
            if (!_view.IsMine) return;
            _playerCameraPositionData.Camera = cameraPos;
        }

        [PunRPC]
        private void OnNetGoal(int goalColor, int ballColor)
        {
            var playerSessionColor = (int)_playerSessionData.Color;
            
            if (goalColor == playerSessionColor && ballColor != playerSessionColor)
                _playerPoints.Points.Value--;
            else if(goalColor != playerSessionColor && ballColor == playerSessionColor)
                _playerPoints.Points.Value++;
        }

        private void OnGoal(GoalCollisionSignal signal)
        {
            if (!_view.IsMine) return;
            _view.RPC(nameof(OnNetGoal), RpcTarget.All, (int)signal.GoalColor, (int)signal.BallColor);
        }
        
        private void OnDestroy()
        {
            _eventBus.Remove<GoalCollisionSignal>(OnGoal);
            _playerInput.Deactivate();
            _ballsManager.Deactivate();
            _colorSelectorData.Selector.Pop(_playerColor);
        }
    }
}