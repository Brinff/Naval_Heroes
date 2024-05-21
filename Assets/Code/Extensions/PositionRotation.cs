using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class PositionRotation
{
    [SerializeField] private Vector3 _position;
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _duration;
    [SerializeField] private Ease _ease;

    public Vector3 Position => _position;
    public Vector3 EulerAngles => _rotation;
    public Quaternion Rotation => Quaternion.Euler(_rotation);
    public float Duration => _duration;
    public Ease Ease => _ease;

    public PositionRotation(Vector3 position, Vector3 rotation)
    {
        _position = position;
        _rotation = rotation;
    }

    public PositionRotation(Vector3 position, Quaternion rotation)
    {
        _position = position;
        _rotation = rotation.eulerAngles;
    }
}
