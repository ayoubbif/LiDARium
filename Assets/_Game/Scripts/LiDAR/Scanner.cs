using UnityEngine;
using System;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Game.Scripts.LiDAR
{
    [Serializable]
    public class Scanner : RayGunMode
    {
        [SerializeField] private Transform rayContainer;
        [SerializeField] private int numOfRays;
        [SerializeField] private float verticalScanAngle;
        [SerializeField] private float horizontalScanAngle;
        [SerializeField] private float scanRate;

        private GameObject[] _rays;
        private float _scanAngle;

        private bool _scanning;

        public bool Scanning
        {
            get => _scanning;
            set
            {
                if(_scanAngle < verticalScanAngle)
                    return;

                _scanning = value;
                _scanAngle = _scanning ? -verticalScanAngle : verticalScanAngle;
                ActivateRays(_rays, _scanning);
            }
        }
        public override void InitRays(GameObject rayPrefab)
        {
            _rays = new GameObject[numOfRays];
            _scanAngle = verticalScanAngle;

            for (int i = 0; i < numOfRays; i++)
            {
                _rays[i] = Object.Instantiate(rayPrefab, rayContainer);
            }

            Scanning = false;
        }

        public void Scan(float deltaTime)
        {
            if(!_scanning)
                return;
            _scanAngle += scanRate * deltaTime;
            AdjustRays();

            Scanning = false;
        }

        private void AdjustRays()
        {
            var hit = new RaycastHit();

            for (var i = 0; i < numOfRays; i++)
            {
                if (AdjustRayFromRaycast(
                        _rays[i].transform,
                        horizontalScanAngle,
                        _scanAngle,
                        Mathf.PI * Random.Range(i/(float) numOfRays, i+1/(float)numOfRays),
                        ref hit))
                {
                    CreateDotFromRaycast(hit);
                }
            }
        }

        private bool AdjustRayFromRaycast(Transform ray, float horizontalAngle,
            float verticalAngle, float horizontalRadians, ref RaycastHit hit)
        {
            // Orient the ray
            ray.localEulerAngles = new Vector3(verticalAngle, Mathf.Cos(horizontalRadians) * horizontalAngle, 0);

            var successfulHit = Raycast(ray.position, ray.forward, out hit);
            ResizeRay(ray, hit.distance);

            return successfulHit;
        }

        public new void ActivateRays(Array rays, bool active)
        {
            if(active)
                AdjustRays();
            
            base.ActivateRays(rays, active);
        }
    }
}