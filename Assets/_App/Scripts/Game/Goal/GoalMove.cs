using System.ComponentModel;
using UnityEngine;
using VgGames.Core.InjectModule;
using VgGames.Game.RuntimeData;
using VgGames.Game.RuntimeData.PlayerColorModule;

namespace VgGames.Game.Goal
{
    public class GoalMove : MonoBehaviour
    {
        [Inject] private PlayerSessionData _playerSessionData;
        [Inject] private PlayerSetterData _playerSetter;

        [SerializeField] private bool xAxis;
        [SerializeField] private bool yAxis;
        [SerializeField] private float speed = 1;
        [SerializeField] private float size = 1;

        private Vector3 _pos;
        private Vector3 _startPos;
        private float _offset;
        private bool _isEnabled;

        public void Activate()
        {
            InjectSystem.Inject(this);

            _startPos = transform.localPosition;
            _offset = Random.Range(-1, 2);
            SetPos();
            _isEnabled = true;
        }

        private void SetPos()
        {
            if (tag.TryToColor(out var t))
            {
                var pos = _playerSetter.Setter.GetGoalPosition(tag.ToColor());
                var goalTransform = transform;
                goalTransform.position = pos.position;
                goalTransform.rotation = pos.rotation;
                goalTransform.SetParent(pos);
            }
            else
                throw new InvalidEnumArgumentException("Tag is not a PlayerColor!");
        }

        private void Update()
        {
            if (!_isEnabled) return;
            if (_playerSessionData.Color != tag.ToColor()) return;

            _pos.Set(xAxis ? Mathf.Sin(Time.timeSinceLevelLoad * speed + _offset) * size : 0,
                yAxis ? Mathf.Cos(Time.timeSinceLevelLoad * speed + _offset) * size : 0, 0);
            transform.localPosition = _startPos + _pos;
        }
    }
}