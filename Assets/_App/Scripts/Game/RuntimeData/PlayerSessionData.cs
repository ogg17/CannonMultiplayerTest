using VgGames.Core.InjectModule;
using VgGames.Game.RuntimeData.PlayerColorModule;
using WebSocketSharp;

namespace VgGames.Game.RuntimeData
{
    public class PlayerSessionData : IInjectable
    {
        private string _nickName;

        public string NickName
        {
            get => _nickName;
            set
            {
                if (value.IsNullOrEmpty()) return;
                _nickName = value;
            }
        }

        public PlayerColor Color { get; set; }
    }
}