using UnityEngine;

namespace VgGames.Core.Config
{
    public static class GameConfig
    {
        public static readonly bool IsDebug;
        public const string StorageMenuEntry = "VgGames/Storage/";
        public static readonly Vector3 StartBallPos;
        public static readonly byte InstantiationEventCode;
        public static readonly string PlayerPrefabName;
        public static readonly string GoalPrefabName;

        static GameConfig()
        {
            var load = Resources.Load<GameConfigStorage>(nameof(GameConfigStorage));
            IsDebug = load.IsDebug;
            StartBallPos = load.StartBallPos;
            InstantiationEventCode = load.InstantiationEventCode;
            PlayerPrefabName = load.PlayerPrefabName;
            GoalPrefabName = load.GoalPrefabName;
        }
    }
}