using UnityEngine;

namespace VgGames.Game.UI.LoadingImage
{
    public class LoadingAnimation : MonoBehaviour
    {
        [SerializeField] private float speed;
        private void Update() => transform.Rotate(Vector3.forward, Time.deltaTime * speed);

        public void Enable() => gameObject.SetActive(true);
        public void Disable() => gameObject.SetActive(false);
    }
}