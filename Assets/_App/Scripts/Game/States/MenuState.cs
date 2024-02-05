using Cysharp.Threading.Tasks;
using Photon.Realtime;
using VgGames.Core.EventBusModule;
using VgGames.Core.MonoBehaviours;
using VgGames.Core.StateMachineModule;
using VgGames.Game.Photon;
using VgGames.Game.RuntimeData;
using VgGames.Game.Signals;
using VgGames.Game.UI.Buttons;
using VgGames.Game.UI.LoadingImage;
using VgGames.Game.UI.Menus;
using WebSocketSharp;
using UnityEngine;

namespace VgGames.Game.States
{
    public class MenuState : IState
    {
        private readonly MonoBehavioursContainer _monoBehaviours;
        private readonly StateMachine _stateMachine;
        private readonly MenusStateMachine _menusStateMachine;
        private readonly EventBus _eventBus;
        private readonly PlayerSessionData _playerSessionData;

        private bool _buttonBlock;
        private readonly PhotonInitialisationHandler _photonInitialisation = new();

        public MenuState(StateMachine stateMachine, MenusStateMachine menusStateMachine,
            MonoBehavioursContainer monoBehavioursContainer, EventBus eventBus, PlayerSessionData playerSessionData)
        {
            _stateMachine = stateMachine;
            _menusStateMachine = menusStateMachine;
            _monoBehaviours = monoBehavioursContainer;
            _eventBus = eventBus;
            _playerSessionData = playerSessionData;

            _photonInitialisation.OnDisconnectedAction += OnDisconnected;
        }

        public async UniTask Enter()
        {
            await _menusStateMachine.SetMenu(MenuType.StartMenu);
            _eventBus.Add<ButtonSignal>(OnButton);
        }

        private async UniTaskVoid OnStartButton()
        {
            _buttonBlock = true;
            _monoBehaviours.Get<LoadingAnimation>().Enable();

            if (_playerSessionData.NickName.IsNullOrEmpty())
                _playerSessionData.NickName = $"Player_{Random.Range(0, 10000)}";
            
            await _photonInitialisation.Init(_playerSessionData.NickName);

            _buttonBlock = false;
            _monoBehaviours.Get<LoadingAnimation>().Disable();

            if (_photonInitialisation.IsConnected) _stateMachine.SetState<GameInitState>().Forget();
        }

        private void OnDisconnected(DisconnectCause cause)
        {
            if(cause == DisconnectCause.ApplicationQuit) return;
            _stateMachine.SetState<MenuState>().Forget();
        }


        private void OnButton(ButtonSignal signal)
        {
            if (!_buttonBlock && signal.Type == ButtonType.Start) OnStartButton().Forget();
        }

        public async UniTask Exit()
        {
            _eventBus.Remove<ButtonSignal>(OnButton);
            await UniTask.CompletedTask;
        }
        
        public void OnDestroy()
        {
            _photonInitialisation.OnDisconnectedAction -= OnDisconnected;
        }
    }
}