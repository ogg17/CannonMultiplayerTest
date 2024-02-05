using Cysharp.Threading.Tasks;
using UnityEngine;
using VgGames.Core.EventBusModule;
using VgGames.Core.InjectModule;
using VgGames.Core.MonoBehaviours;
using VgGames.Core.StateMachineModule;
using VgGames.Game.Player;
using VgGames.Game.Player.Colors;
using VgGames.Game.RuntimeData;
using VgGames.Game.States;
using VgGames.Game.UI.Buttons;
using VgGames.Game.UI.GameText;
using VgGames.Game.UI.InputField;
using VgGames.Game.UI.LoadingImage;
using VgGames.Game.UI.Menus;

namespace VgGames.Game
{
    public class GameBootstrap : MonoBehaviour
    {
        private readonly StateMachine _stateMachine = new();
        private readonly EventBus _eventBus = new();
        private readonly PlayerSessionData _playerSessionData = new();
        private readonly PlayerPointsData _playerPointsData = new();
        private readonly PlayerCameraPositionData _playerCameraPositionData = new();
        private readonly PlayerSetterData _playerSetterData = new();
        private readonly KickForceData _kickForceData = new();
        private readonly ColorSelectorData _colorSelectorData = new();
        private readonly MonoBehavioursContainer _monoBehaviours = new();
        private readonly MenusStateMachine _menusStateMachine = new();

        private void Start()
        {
            Application.targetFrameRate = 60;
            AddMonoBehaviours();
            AddInjects();
            CreateStates();
        }

        private void CreateStates()
        {
            _stateMachine.Add(new InitState(_stateMachine, _monoBehaviours));
            _stateMachine.Add(new MenuState(_stateMachine, _menusStateMachine, _monoBehaviours, _eventBus,
                _playerSessionData));
            _stateMachine.Add(new GameInitState(_stateMachine, _eventBus, _menusStateMachine, _playerSessionData));
            _stateMachine.Add(new GameState(_stateMachine, _monoBehaviours, _menusStateMachine, _playerSessionData));

            _stateMachine.SetState<InitState>().Forget();
        }

        private void AddInjects()
        {
            InjectSystem.Init();

            InjectSystem.Add(_eventBus);
            InjectSystem.Add(_menusStateMachine);
            InjectSystem.Add(_playerSessionData);
            InjectSystem.Add(_playerPointsData);
            InjectSystem.Add(_playerCameraPositionData);
            InjectSystem.Add(_playerSetterData);
            InjectSystem.Add(_kickForceData);
            InjectSystem.Add(_colorSelectorData);
        }

        private void AddMonoBehaviours()
        {
            // --------UI-----------
            _monoBehaviours.Add<Menu>();
            _monoBehaviours.Add<BaseText>();
            _monoBehaviours.Add<BaseInputField>();
            _monoBehaviours.Add<BaseButton>();
            _monoBehaviours.Add<ColorButton>();
            
            // --------Player-----------
            _monoBehaviours.Add<PlayerSetter>();
            _monoBehaviours.Add<ColorSelector>();
            
            // --------Other-----------
            _monoBehaviours.Add<LoadingAnimation>();
            _monoBehaviours.Add<CameraController>();
            //_monoBehaviours.Add<SoundPlayer>();
        }

        private void LateUpdate() => _stateMachine.Tick();

        private void OnDestroy() => _stateMachine.OnDestroy();
    }
}