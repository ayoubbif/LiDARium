using UnityEngine;

namespace _Game.Scripts.LiDAR
{
    public class PointAim : MonoBehaviour
    {
        [SerializeField] private Transform pointer;
        [SerializeField] private Transform rayOrigin;

        [SerializeField] private bool smooth;
        [SerializeField] [Range(0.01f, 1)] private float smoothRate = 0.1f;

        private float AdjustSmoothRate => smoothRate / Time.fixedDeltaTime;

        private void FixedUpdate()
        {
            Aim(Time.fixedDeltaTime, pointer);
        }

        private void Aim(float deltaTime, Transform t)
        {
            if(!Physics.Raycast(rayOrigin.position, rayOrigin.forward, out var hit))
                return;

            if (smooth)
            {
                var relativePos = hit.point - t.position;
                var toRotation = Quaternion.LookRotation(relativePos);
                t.rotation = Quaternion.Lerp(t.rotation, toRotation, AdjustSmoothRate * deltaTime);
            }
            else
            {
                t.LookAt(hit.point);
            }
        }
    }
}