using VgGames.Core.InjectModule;

namespace VgGames.Game.RuntimeData
{
    public class PlayerPointsData : IInjectable
    {
        public readonly ReactiveData<int> Points = 0;
    }
}