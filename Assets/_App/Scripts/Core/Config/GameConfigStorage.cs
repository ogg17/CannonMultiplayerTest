using UnityEngine;
using UnityEngine.Serialization;

namespace VgGames.Core.Config
{
    [CreateAssetMenu(fileName = nameof(GameConfigStorage),
        menuName = GameConfig.StorageMenuEntry + nameof(GameConfigStorage), order = 0)]
    public class GameConfigStorage : ScriptableObject
    {
        [Tooltip("Enable/Disable Custom Debug")] [SerializeField]
        private bool isDebug;

        [Tooltip("Balls Default Spawn Point")] [SerializeField]
        private Vector3 startBallPos;
        
        [Tooltip("Instantiate Event Code")] [SerializeField]
        private byte instantiationEventCode = 90;
        
        [Tooltip("Player Prefab Name")] [SerializeField]
        private string playerPrefabName;
        
        [Tooltip("Goal Prefab Name")] [SerializeField]
        private string goalPrefabName;

        public bool IsDebug => isDebug;
        public Vector3 StartBallPos => startBallPos;
        public byte InstantiationEventCode => instantiationEventCode;
        public string PlayerPrefabName => playerPrefabName;
        public string GoalPrefabName => goalPrefabName;
    }
}