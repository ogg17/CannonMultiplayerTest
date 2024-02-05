using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VgGames.Core.MonoBehaviours;
using VgGames.Core.StateEvents.GameState;
using VgGames.Core.StateMachineModule;
using VgGames.Game.GameInput;
using VgGames.Game.Photon;
using VgGames.Game.RuntimeData;
using VgGames.Game.UI.Menus;

namespace VgGames.Game.States
{
    public class GameState : IUpdateState
    {
        private readonly MonoBehavioursContainer _monoBehaviours;
        private readonly MenusStateMachine _menusStateMachine;
        private readonly PlayerSessionData _sessionData;

        private readonly MouseInput _mouseInput;
        private readonly PhotonGameHandler _photonGame;

        public GameState(StateMachine stateMachine, MonoBehavioursContainer monoBehavioursContainer,
            MenusStateMachine menusStateMachine, PlayerSessionData sessionData)
        {
            _monoBehaviours = monoBehavioursContainer;
            _menusStateMachine = menusStateMachine;
            _sessionData = sessionData;

            _mouseInput = new MouseInput();
            _photonGame = new PhotonGameHandler();
        }

        public async UniTask Enter()
        {
            CallActivate();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _photonGame.Spawn((int)_sessionData.Color);

            await _menusStateMachine.SetMenu(MenuType.GameUI);
        }

        private void CallActivate()
        {
            foreach (var init in _monoBehaviours.GetAll().OfType<IGameActivate>()) init.GameActivate();
        }

        private void CallDeactivate()
        {
            foreach (var init in _monoBehaviours.GetAll().OfType<IGameDeactivate>()) init.GameDeactivate();
        }

        public async UniTask Exit()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            CallDeactivate();
            await UniTask.CompletedTask;
        }

        public void StateUpdate()
        {
            _mouseInput.Update();
            foreach (var init in _monoBehaviours.GetAll().OfType<IGameUpdate>()) init.GameUpdate();
        }

        public void OnDestroy()
        {
        }
    }
}