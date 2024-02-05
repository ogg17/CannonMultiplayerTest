using VgGames.Core.InjectModule;

namespace VgGames.Game.RuntimeData
{
    public class KickForceData : IInjectable
    {
        public readonly ReactiveData<float> Force = 0.1f;
    }
}