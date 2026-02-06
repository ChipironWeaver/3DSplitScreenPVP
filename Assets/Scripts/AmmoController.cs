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
        TargetController target = other.GetComponent<TargetController>();
        if (target != null) 
        {
            target.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}