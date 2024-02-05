using System;
using System.Collections.Generic;
using UnityEngine;
using VgGames.Core.InjectModule;
using VgGames.Core.StateEvents.InitState;
using VgGames.Game.RuntimeData;
using VgGames.Game.RuntimeData.PlayerColorModule;

namespace VgGames.Game.Player
{
    public class PlayerSetter : MonoBehaviour, IInit
    {
        [Inject] private PlayerSetterData _playerSetterData;

        [SerializeField] private List<Transform> playerPositions;
        [SerializeField] private List<Transform> goalPositions;


        private readonly Dictionary<PlayerColor, int> _positions = new();

        public void Init()
        {
            _positions.Add(PlayerColor.Red, 0);
            _positions.Add(PlayerColor.Green, 1);
            _positions.Add(PlayerColor.Blue, 2);
            _positions.Add(PlayerColor.Yellow, 3);
            _playerSetterData.Setter = this;
        }

        public Transform GetPlayerPosition(PlayerColor color)
        {
            if (!_positions.TryGetValue(color, out var res))
                throw new IndexOutOfRangeException("Slot not available!");
            return playerPositions[res];
        }

        public Transform GetGoalPosition(PlayerColor color)
        {
            if (!_positions.TryGetValue(color, out var res))
                throw new IndexOutOfRangeException("Slot not available!");
            return goalPositions[res];
        }
    }
}