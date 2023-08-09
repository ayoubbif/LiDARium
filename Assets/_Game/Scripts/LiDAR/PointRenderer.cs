using System.Collections.Generic;
using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.VFX;

namespace _Game.Scripts.LiDAR
{
    public class PointRenderer : MonoBehaviour
    {
        private const string PositionTextureName = "Positions";
        private const string CapacityParamName = "Capacity";
        private const string ReferencePosParamName = "ReferencePosition";
        private const int Texture2DMaxHeight = 16384;
        private static readonly Color ClearColor = new(0,0,0,0);

        [SerializeField] private VisualEffect effectPrefab;
        [SerializeField] private Transform effectContainer;

        private VisualEffect _currentEffect;
        private Queue<VisualEffect> _effects;
        private Texture2D _texture2D;
        private Color[] _points;
        private int _particleCount;

        private void Awake()
        {
            _effects = new Queue<VisualEffect>();
            _points = new Color[Texture2DMaxHeight];
        }

        private void Start()
        {
            CreateNewEffect();
            ApplyPoints();
        }

        private void FixedUpdate()
        {
            ApplyPoints();
        }

        private void CreateNewEffect()
        {
            _currentEffect = Instantiate(effectPrefab, effectContainer);
            _currentEffect.SetUInt(CapacityParamName, Texture2DMaxHeight);
            _effects.Enqueue(_currentEffect);
            _texture2D = new Texture2D(Texture2DMaxHeight, 1, TextureFormat.RGBAFloat, false);
            ClearPoints();
            _particleCount = 0;
        }

        private void ClearPoints()
        {
            // Reset all points to clear color
            _points.Default(ClearColor);
        }

        private void ApplyPoints()
        {
            // Update the texture with the points
            _texture2D.SetPixels(_points);
            _texture2D.Apply();
        
            // Update the current effect with the updated texture
            _currentEffect.SetTexture(PositionTextureName, _texture2D);
            _currentEffect.Reinit();
        }
    
        public void SetRefPosition(Vector3 pos)
        {
            // Set reference position for all the effects
            foreach (var effect in _effects)
            {
                effect.SetVector3(ReferencePosParamName, pos);
            }
        }

        public void ClearAllPoints()
        {
            // Clear all effects and their associated game objects
            while (_effects.Count > 0)
            {
                Destroy(_effects.Dequeue().gameObject);
            }
            CreateNewEffect();
        }

        public void CachePoint(Vector3 pos)
        {
            // Cache the point as a color in points array
            _points[_particleCount++] = new Color(pos.x, pos.y, pos.z);

            // Create new effect if capacity is reached
            if (_particleCount >= Texture2DMaxHeight)
            {
                CreateNewEffect();
            }
        }
    }
}