using System.Collections.Generic;
using _Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.VFX;

public class PointRenderer : MonoBehaviour
{
    private const string PositionTextureName = "Positions";
    private const string CapacityParamName = "Capacity";
    private const string ReferencePosParamName = "ReferencePosition";

    private const int Texture2DMaxHeigth = 16384;

    [SerializeField] private VisualEffect effectPrefab;
    [SerializeField] private Transform effectContainer;

    private VisualEffect _currentEffect;
    private Queue<VisualEffect> _effects;
    private Texture2D _texture2D;
    private Color[] _points;
    private int _particleCount;

    private void Awake()
    {
        _points = new Color[Texture2DMaxHeigth];
        _effects = new Queue<VisualEffect>();
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
        _currentEffect.SetUInt(CapacityParamName, Texture2DMaxHeigth);
        _effects.Enqueue(_currentEffect);
        _texture2D = new Texture2D(Texture2DMaxHeigth, 1, TextureFormat.RGBAFloat, false);
        _points.Default(new Color(0,0,0,0));
        _particleCount = 0;
    }

    private void ApplyPoints()
    {
        // Update the texture
        _texture2D.SetPixels(_points);
        _texture2D.Apply();
        
        // Update the current effect with the updated texture
        _currentEffect.SetTexture(PositionTextureName, _texture2D);
        _currentEffect.Reinit();
    }
    
    public void SetRefPosition(Vector3 pos)
    {
        foreach (var effect in _effects)
        {
            effect.SetVector3(ReferencePosParamName, pos);
        }
    }

    public void ClearAllPoints()
    {
        while (_effects.Count > 0)
        {
            Destroy(_effects.Dequeue().gameObject);
        }
        CreateNewEffect();
    }

    public void CachePoint(Vector3 pos)
    {
        _points[_particleCount] = new Color(pos.x, pos.y, pos.z);
        _particleCount++;

        if (_particleCount >= Texture2DMaxHeigth)
        {
            CreateNewEffect();
        }
    }
}
