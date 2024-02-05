using System.ComponentModel;
using UnityEngine;
using VgGames.Core.EventBusModule;
using VgGames.Core.InjectModule;
using VgGames.Game.RuntimeData;
using VgGames.Game.RuntimeData.PlayerColorModule;
using VgGames.Game.Signals;

namespace VgGames.Game.Goal
{
    public class GoalCollision : MonoBehaviour
    {
        [Inject] private EventBus _eventBus;
        [Inject] private ColorSelectorData _colorSelectorData;

        private GoalCollisionSignal _goalCollisionSignal;

        public void Activate()
        {
            InjectSystem.Inject(this);

            if (tag.TryToColor(out var color))
                _goalCollisionSignal = new GoalCollisionSignal { GoalColor = color };
            else
                throw new InvalidEnumArgumentException("Tag is not a PlayerColor!");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.TryToColor(out var color)
                && !_colorSelectorData.Selector.CheckAvailable(tag.ToColor()))
            {
                _goalCollisionSignal.BallColor = color;
                _eventBus.Invoke(_goalCollisionSignal);
                other.DisableBall();
            }
        }
    }
}