using UnityEngine;
using UnityEngine.UI;
using VgGames.Core.EventBusModule;
using VgGames.Core.InjectModule;
using VgGames.Game.Signals;

namespace VgGames.Game.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public class GameButton : BaseButton
    {
        [Inject] private EventBus _eventBus;

        [SerializeField] private ButtonType type;
        
        private ButtonSignal _signal;

        protected override void Action() => _eventBus.Invoke(_signal);
        protected override void BaseInit() => _signal = new ButtonSignal { Type = type };
    }
}