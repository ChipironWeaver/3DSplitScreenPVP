using UnityEngine;
using UnityEngine.InputSystem;

public class LauncherController : MonoBehaviour
{
    [SerializeField] private float _ammoPower;
    [SerializeField] private GameObject _ammoPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _firingCooldown;
    [Header("UX Settings")]
    [SerializeField] private AudioClip _firingSound;
    [SerializeField] private AudioClip _reloadSound;
    
    private bool _canFire = true;

    private void OnFire(InputValue value)
    {
        if (_canFire)
        {
            GameObject instantiate = Instantiate(_ammoPrefab, _spawnPoint.position, Quaternion.identity);
            Rigidbody rb = instantiate.GetComponent<Rigidbody>();
            rb.AddForce(_spawnPoint.forward * _ammoPower);
            _canFire = false;
            Invoke(nameof(ResetFire), _firingCooldown);
            if (_firingSound != null)
            {
                AudioSource.PlayClipAtPoint(_firingSound, Vector3.zero);
            }
        }
        
    }

    private void ResetFire()
    {
        _canFire = true;
    }
}