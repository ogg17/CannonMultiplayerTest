using System.Collections.Generic;
using UnityEngine;
using VgGames.Core.InjectModule;
using VgGames.Game.RuntimeData;
using VgGames.Game.RuntimeData.PlayerColorModule;

namespace VgGames.Game.Goal
{
    public class GoalController : MonoBehaviour
    {
        [Inject] private PlayerSessionData _playerSessionData;

        [SerializeField] private List<Material> materials;

        private GoalMove _goalMove;
        private GoalCollision _goalCollision;

        private void Start()
        {
            _goalMove = GetComponent<GoalMove>();
            _goalCollision = GetComponent<GoalCollision>();

            GetComponent<MeshRenderer>().SetMaterials(new List<Material> { materials[(int)tag.ToColor()] });

            _goalMove.Activate();
            _goalCollision.Activate();
        }
    }
}