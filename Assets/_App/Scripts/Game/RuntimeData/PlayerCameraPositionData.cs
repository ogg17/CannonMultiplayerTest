using UnityEngine;
using VgGames.Core.InjectModule;

namespace VgGames.Game.RuntimeData
{
    public class PlayerCameraPositionData : IInjectable
    {
        private Transform _camera;

        public Transform Camera
        {
            get => _camera;
            set
            {
                if (value != null) IsNull = false;
                _camera = value;
            }
        }

        public bool IsNull { get; private set; } = true;
    }
}