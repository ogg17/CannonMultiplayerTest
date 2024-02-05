using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VgGames.Core.StateEvents.InitState;

namespace VgGames.Game.UI.Buttons
{
    public abstract class BaseButton : MonoBehaviour, IInit
    {
        [SerializeField] private float clickDelay = 0.15f;

        private Button _button;

        public void Init()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(() => DelayClick().Forget());

            BaseInit();
        }

        protected virtual void BaseInit()
        {
        }

        private async UniTaskVoid DelayClick()
        {
            await UniTask.WaitForSeconds(clickDelay, cancellationToken: destroyCancellationToken);
            Action();
            EffectAction();
        }

        protected virtual void Action()
        {
        }

        protected virtual void EffectAction()
        {
        }
    }
}