using UnityEngine;
using VgGames.Core.InjectModule;
using VgGames.Game.RuntimeData;

namespace VgGames.Game.UI.GameText
{
    public class ForceText : BaseText
    {
        [Inject] private KickForceData _kickForceData;

        [SerializeField] private Vector2 range = new(0.1f, 1f);

        protected override void BaseInit() =>
            _kickForceData.Force.OnChange += f =>
                SetValue($"Force: {Mathf.Clamp(f, range.x, range.y):F1}");
    }
}