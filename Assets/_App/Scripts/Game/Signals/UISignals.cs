using VgGames.Game.RuntimeData.PlayerColorModule;
using VgGames.Game.UI.Buttons;

namespace VgGames.Game.Signals
{
    public class ButtonSignal
    {
        public ButtonType Type = ButtonType.None;
    }

    public class SelectColorSignal
    {
        public PlayerColor PlayerColor;
    }
}