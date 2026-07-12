using UnityEngine;

namespace Camera
{
    public class CameraFollow2D : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float smoothTime = 0.15f;
        [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
        [SerializeField] private string playerTag = "Player";

        private Vector3 _velocity;

        private void LateUpdate()
        {
            if (target == null)
            {
                TryFindPlayer();
            }

            if (target == null)
                return;

            Vector3 targetPosition = target.position + offset;

            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref _velocity,
                smoothTime
            );
        }

        private void TryFindPlayer()
        {
            var player = GameObject.FindGameObjectWithTag(playerTag);

            if (player != null)
                target = player.transform;
        }
    }
}
