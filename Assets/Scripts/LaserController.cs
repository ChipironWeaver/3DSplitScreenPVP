using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private Transform _originTransform;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Debug.DrawRay(_originTransform.position, _originTransform.forward * 100, Color.red);
        
        if (Input.GetButtonDown("Fire1"))
        {
            _audioSource.PlayOneShot(_audioClip);
            
            Ray ray = new Ray(_originTransform.position, _originTransform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                HealthController health = hit.transform.GetComponent<HealthController>();
                if (health != null) health.TakeDamage(1);
            }
        }
    }
}