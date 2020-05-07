using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _speed;

    private Vector3 _nextPosition;
    private Vector3 _nextRotation;

    private void Start()
    {
        _nextPosition = _camera.transform.position;
        _nextRotation = _camera.transform.eulerAngles;
    }

    private void Update()
    {
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, _nextPosition, _speed * Time.deltaTime);
        _camera.transform.eulerAngles = Vector3.Lerp(_camera.transform.eulerAngles, _nextRotation, _speed * Time.deltaTime);
    }

    public void SetNextTransform(Transform nextPoint)
    {
        _nextPosition = nextPoint.position;
        _nextRotation = nextPoint.eulerAngles;
    }
}
