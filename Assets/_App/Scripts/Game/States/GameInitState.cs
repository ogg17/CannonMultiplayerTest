using Cysharp.Threading.Tasks;
using VgGames.Core.EventBusModule;
using VgGames.Core.StateMachineModule;
using VgGames.Game.Photon;
using VgGames.Game.RuntimeData;
using VgGames.Game.Signals;
using VgGames.Game.UI.Menus;

namespace VgGames.Game.States
{
    public class GameInitState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly EventBus _eventBus;
        private readonly MenusStateMachine _menusStateMachine;
        private readonly PlayerSessionData _playerSessionData;

        private readonly PhotonRoomHandler _photonRoom = new();

        public GameInitState(StateMachine stateMachine, EventBus eventBus, MenusStateMachine menusStateMachine,
            PlayerSessionData playerSessionData)
        {
            _stateMachine = stateMachine;
            _eventBus = eventBus;
            _menusStateMachine = menusStateMachine;
            _playerSessionData = playerSessionData;
        }

        public async UniTask Enter()
        {
            await _photonRoom.Init();
            await _menusStateMachine.SetMenu(MenuType.Lobby);
            _eventBus.Add<SelectColorSignal>(OnSelect);
        }

        private void OnSelect(SelectColorSignal signal)
        {
            _playerSessionData.Color = signal.PlayerColor;
            _stateMachine.SetState<GameState>().Forget();
        }

        public async UniTask Exit()
        {
            _eventBus.Remove<SelectColorSignal>(OnSelect);
            await UniTask.CompletedTask;
        }
        
        public void OnDestroy()
        {
        }
    }
}