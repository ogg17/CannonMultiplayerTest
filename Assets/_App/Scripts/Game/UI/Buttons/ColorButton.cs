using UnityEngine;
using UnityEngine.UI;
using VgGames.Core.StateEvents.InitState;
using VgGames.Game.RuntimeData.PlayerColorModule;

namespace VgGames.Game.UI.Buttons
{
    public class ColorButton: MonoBehaviour, IInit
    {
        [SerializeField] private PlayerColor color;

        public Button Button { get; private set; }
        public PlayerColor Color => color;
        
        public void Init()
        {
            Button = GetComponent<Button>();
        }
    }
}