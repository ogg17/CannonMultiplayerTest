using VgGames.Core.InjectModule;
using VgGames.Game.RuntimeData;

namespace VgGames.Game.UI.GameText
{
    public class PointsText : BaseText
    {
        [Inject] private PlayerPointsData _pointsData;

        protected override void BaseInit() => 
            _pointsData.Points.OnChange += i => SetValue($"Points: {i}");
    }
}