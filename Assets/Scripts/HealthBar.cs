using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform ScalerTransform;
    public Transform Target;

    private Transform _camera;

    private void Start()
    {
        _camera = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.position = Target.position + Vector3.up * 2.5f;
        transform.rotation = _camera.rotation;
    }

    public void Setup(Transform target)
    {
        Target = target;
    }

    public void SetHealth(int health, int maxHealth)
    {
        float xScale = (float) health / (float)maxHealth;
        xScale = Mathf.Clamp01(xScale);
        ScalerTransform.localScale = new Vector3(xScale, 1, 1);
    }
}
