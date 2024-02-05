using VgGames.Core.InjectModule;
using VgGames.Game.Player;

namespace VgGames.Game.RuntimeData
{
    public class PlayerSetterData : IInjectable
    {
        public PlayerSetter Setter { get; set; }
    }
}