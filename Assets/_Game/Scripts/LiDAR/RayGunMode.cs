using System;
using UnityEngine;

namespace _Game.Scripts.LiDAR
{
    public abstract class RayGunMode
    {
        private PointRenderer _pointRenderer;
        private float _rayDistance;
        private LayerMask _layerMask;

        public abstract void InitRays(GameObject rayPrefab);

        public void Setup(PointRenderer pointRenderer, float rayDistance, LayerMask layerMask, GameObject rayPrefab)
        {
            _pointRenderer = pointRenderer;
            _rayDistance = rayDistance;
            _layerMask = layerMask;
            
            InitRays(rayPrefab);
        }
        
        //Attempt a raycast, defaulting the hit distance to 0 if it fails
        public bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo)
        {
            if (!Physics.Raycast(origin, direction, out hitInfo, _rayDistance, _layerMask))
            {
                hitInfo.distance = 0;
                return false;
            }
            return true;
        }

        public void ResizeRay(Transform ray, float length)
        {
            ray.localScale = new Vector3(1, 1, length);
        }

        public void CreateDotFromRaycast(RaycastHit hit)
        {
            _pointRenderer.CachePoint(hit.point);
        }

        public void ActivateRays(Array rays, bool active)
        {
            foreach (GameObject ray in rays)
            {
                ray.SetActive(active);
            }
        }
    }
}