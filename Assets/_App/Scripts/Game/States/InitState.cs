using System.Linq;
using Cysharp.Threading.Tasks;
using VgGames.Core.InjectModule;
using VgGames.Core.MonoBehaviours;
using VgGames.Core.StateEvents.InitState;
using VgGames.Core.StateMachineModule;
using VgGames.Game.UI.LoadingImage;
using VgGames.Game.UI.Menus;

namespace VgGames.Game.States
{
    public class InitState : IState
    {
        private readonly MonoBehavioursContainer _monoBehaviours;
        private readonly StateMachine _stateMachine;

        public InitState(StateMachine stateMachine, MonoBehavioursContainer monoBehavioursContainer)
        {
            _stateMachine = stateMachine;
            _monoBehaviours = monoBehavioursContainer;
        }

        public async UniTask Enter()
        {
            InjectSystem.InjectAllMono();
            CallInit();
            DisableMenus();

            _stateMachine.SetState<MenuState>().Forget();
            await UniTask.CompletedTask;
        }

        private void DisableMenus()
        {
            _monoBehaviours.GetAll<Menu>().ForEach(m => m.ForceDisable());
            _monoBehaviours.Get<LoadingAnimation>().Disable();
        }

        private void CallInit()
        {
            foreach (var init in _monoBehaviours.GetAll().OfType<IInit>()) init.Init();
        }

        public void OnDestroy()
        {
        }
    }
}