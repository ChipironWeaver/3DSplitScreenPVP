using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Image = UnityEngine.UI.Image;

public class LauncherController : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float _ammoPower;
    [SerializeField] private GameObject _ammoPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _firingCooldown;
    [SerializeField] private int _maxAmmo;
    [SerializeField] private int _currentAmmo;
    [SerializeField] private float _reloadTime;
    [Header("UX Settings")]
    [SerializeField] private AudioClip _firingSound;
    [SerializeField] private AudioClip _reloadSound;
    [SerializeField] private GameObject _shootParticle;
    [Header("UI Settings")]
    [SerializeField] private GameObject _ui;
    [SerializeField] private Sprite fullAmmoSprite;
    [SerializeField] private Sprite emptyAmmoSprite;
    
    private bool _canFire = true;
    private List<Image> _ammoImages = new List<Image>();

    private void Start()
    {
        _currentAmmo = _maxAmmo;
        RenderAmmo();
    }

    private void OnFire(InputValue value)
    {
        if (_canFire)
        {
            if (_currentAmmo > 0)
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

                if (_shootParticle != null)
                {
                    Instantiate(_shootParticle, _spawnPoint.position, Quaternion.identity);
                }

                _currentAmmo--;
                RenderAmmo();
            }
        }
    }
    
    private void RenderAmmo()
    {
        while (_ammoImages.Count < _maxAmmo)
        {
            _ammoImages.Add(InstantiateBulletRender());
        }

        for (int i = 0; i < _ammoImages.Count; i++)
        {
            if (i > _currentAmmo - 1) _ammoImages[i].sprite = emptyAmmoSprite;
            else _ammoImages[i].sprite = fullAmmoSprite;
        }
    }

    private Image InstantiateBulletRender()
    {
        GameObject AmmoTemp = new GameObject();
        AmmoTemp.transform.SetParent(_ui.transform);
        AmmoTemp.name = "bullet" + (_ammoImages.Count + 1) ;
        AmmoTemp.AddComponent<RectTransform>().localScale = Vector3.one;
        return AmmoTemp.AddComponent<Image>();
    }
    
    private void ResetFire()
    {
        _canFire = true;
    }
}