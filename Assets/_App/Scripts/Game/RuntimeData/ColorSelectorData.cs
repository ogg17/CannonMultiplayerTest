using VgGames.Core.InjectModule;
using VgGames.Game.Player.Colors;

namespace VgGames.Game.RuntimeData
{
    public class ColorSelectorData : IInjectable
    {
        public ColorSelector Selector { get; set; }
    }
}