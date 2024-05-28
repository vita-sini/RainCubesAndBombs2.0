using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour, IColorable
{
    private float _lifeTime = 5f;
    private SpawnerCube _spawner;
    private Rigidbody _rigidbody;
    private Renderer _renderer;

    private bool _colorChanged = false;
    private bool _isDeactivated = false;
    private bool _countdownStarted = false;

    private Vector3 _lastPosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_colorChanged && !_countdownStarted)
        {
            SetColor();
            StartCountdown();
        }
    }

    public void Disable()
    {
        _isDeactivated = false;
    }

    public void Initialize(SpawnerCube spawner, Vector3 zero)
    {
        SetVelocity(zero);
        SetStartColor();
        _spawner = spawner;
    }

    public void SetColor()
    {
        if (_colorChanged == false)
        {
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            _renderer.material.color = randomColor;
            _colorChanged = true;
        }
    }

    public void SetStartColor()
    {
        _renderer.material.color = Color.white;
    }

    public void SetVelocity(Vector3 velocity)
    {
        _rigidbody.velocity = velocity;
    }

    public void StartCountdown()
    {
        _countdownStarted = true;
        Invoke(nameof(Deactivate), _lifeTime);
    }

    public void Deactivate()
    {
        if (!_isDeactivated)
        {
            _colorChanged = false;
            _countdownStarted = false;
            _spawner.ReturnItem(this);
            _isDeactivated = true;
        }
    }
}
