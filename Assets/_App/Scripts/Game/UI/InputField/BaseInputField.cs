using TMPro;
using UnityEngine;
using VgGames.Core.StateEvents.InitState;

namespace VgGames.Game.UI.InputField
{
    public abstract class BaseInputField : MonoBehaviour, IInit
    {
        private TMP_InputField _field;

        public void Init()
        {
            _field = GetComponent<TMP_InputField>();
            _field.onEndEdit.AddListener(OnEndEdit);
            BaseInit();
        }
        
        protected virtual void BaseInit(){}

        protected virtual void OnEndEdit(string value)
        {
        }
    }
}