using System;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 3f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        HealthController health = other.GetComponent<HealthController>();
        if (health != null) 
        {
            health.TakeDamage(1);
        }
        Destroy(gameObject);
    }
}