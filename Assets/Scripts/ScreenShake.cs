using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{

    private Vector3 _originalPosition;
    [SerializeField]
    private float _shakeDuration = 0f;
    [SerializeField]
    private float _shakeAmount = 0.7f;
    [SerializeField]
    private float _decreaseFactor = 1f;

    void Start()
    {
        _originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shakeDuration > 0)
        {
            transform.position = _originalPosition + Random.insideUnitSphere * _shakeAmount;
            _shakeDuration -= Time.deltaTime * _decreaseFactor;
        }
        else
        {
            _shakeDuration = 0f;
            transform.position = _originalPosition;
        }
    }

    public void StartShake()
    {
        _shakeDuration = 1f;
    }
}
