using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SolarSystemController : MonoBehaviour
{
    [SerializeField] private List<SolarElement> _planets;

    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;

    private void Awake()
    {
        float minSpeed = _planets.Min(p => p._realSpeed);
        float maxSpeed = _planets.Max(p => p._realSpeed);
        float speedRange = maxSpeed - minSpeed;
        float newSpeedRange = _maxSpeed - minSpeed;

        foreach (var planet in _planets)
        {
            var rotateAround = planet._planet.GetComponent<RotateAround>();

            float newSpeed = ((planet._realSpeed - minSpeed) / speedRange) * newSpeedRange + _minSpeed;

            rotateAround.SetSpeed(newSpeed);
        }
    }

    [Serializable]
    private class SolarElement
    {
        public Transform _planet;
        public float _realSpeed;
    }
}
