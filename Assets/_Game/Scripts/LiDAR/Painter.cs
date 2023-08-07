using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace _Game.Scripts.LiDAR
{
    [Serializable]
    public class Painter : RayGunMode
    {
        // Paint angles are measured from center line to edge of paint volume (half of the "point" angle of a cone)
        [Serializable]
        private struct Angles
        {
            public float min;
            public float max;
            public float initial;
        }
        
        [SerializeField] private Transform rayContainer;
        [SerializeField] private int raysPerLayer;
        [SerializeField] private int numOfLayers;
        [SerializeField] private Angles angles;
        [SerializeField] private float angleAdjustSensitivity;

        private float _paintAngle;
        private GameObject[,] _paintRays;
        private bool _painting;

        public bool Painting
        {
            get => _painting;
            set
            {
                _painting = value;
                ActivateRays(_paintRays, _painting);
            }
            
        }
        public override void InitRays(GameObject rayPrefab)
        {
            _paintRays = new GameObject[raysPerLayer, numOfLayers];
            _paintAngle = angles.initial;

            for (var i = 0; i < numOfLayers; i++)
            {
                for (int j = 0; j < raysPerLayer; j++)
                {
                    _paintRays[i, j] = Object.Instantiate(rayPrefab, rayContainer);
                }
            }

            Painting = false;
        }

        public void AdjustAngle(float scrollDelta)
        {
            // Return if not painting so rays don't awkwardly jump to the new angle after adjusting while painting is off
            // Could also call AdjustRays() right before enabling them, similar to scanner
            if(!_painting)
                return;

            _paintAngle = Mathf.Clamp(_paintAngle + scrollDelta * angleAdjustSensitivity, angles.min, angles.max);
        }
        
        // Paint dots on the geometry in a scattered circle pattern
        public void Paint()
        {
            if (!_painting)
                return;

            AdjustRays();
            Painting = false;
        }
        
        // Randomizes the orientation of all paint rays, resizes them to the distance from the surface they hit, and paints
        // a dot for the first ray in each ray layer

        private void AdjustRays()
        {
            var hit = new RaycastHit();

            for (var i = 0; i < numOfLayers; i++)
            {
                var angleFromCenter = Random.Range(
                    _paintAngle * (i / (float)numOfLayers),
                    _paintAngle * ((i + 1) / (float)numOfLayers));

                var radianOffset = Random.Range(0, 2 * Mathf.PI);

                for (var j = 0; j < raysPerLayer; j++)
                {
                    var radians = 2 * Mathf.PI * (j / (float)raysPerLayer) + radianOffset;
                    if(AdjustRayFromRaycast(_paintRays[i,j].transform, angleFromCenter, radians, ref hit))
                        CreateDotFromRaycast(hit);
                }
            }
        }
        
        // Tries to adjust the paint ray from a raycast, returning whether the raycast hit was successful
        private bool AdjustRayFromRaycast(Transform ray, float angleFromCenter, float radians, ref RaycastHit hit)
        {
            ray.localEulerAngles = angleFromCenter * new Vector3(Mathf.Sin(radians), Mathf.Cos(radians), 0);

            var successfulHit = Raycast(ray.position, ray.forward, out hit);
            ResizeRay(ray, hit.distance);

            return successfulHit;
        }

    }
}