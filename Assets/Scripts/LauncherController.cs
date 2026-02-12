using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LauncherController : MonoBehaviour
{
    [SerializeField] private float _ammoPower;
    [SerializeField] private GameObject _ammoPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _firingCooldown;
    [SerializeField] private int _maxAmmo;
    [SerializeField] private float _reloadCooldown;
    [Header("UX Settings")]
    [SerializeField] private AudioClip _firingSound;
    [SerializeField] private AudioClip _reloadSound;
    [SerializeField] private GameObject _shootParticle;
    [Header("UI Settings")]
    [SerializeField] private Sprite _emptySprite;
    [SerializeField] private Sprite _fullSprite;
    [SerializeField] private GameObject _ammoUIGroup;
    [SerializeField] private Slider _ammoUISlider;
    
    private bool _canFire = true;
    private bool _reloading = false;
    private List<Image> _ammoUIImages = new List<Image>();
    private int _currentAmmo;


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
            else
            {
                StartCoroutine(Reload());
            }
        }
        
    }

    private void ResetFire()
    {
        _canFire = true;
    }

    private void RenderAmmo()
    {
        while (_ammoUIImages.Count < _maxAmmo)
        {
            _ammoUIImages.Add(InstantiateAmmoUI());
        }
        for (int i = 0; i < _ammoUIImages.Count; i++)
        {
            if (i > _currentAmmo - 1) _ammoUIImages[i].sprite = _emptySprite;
            else _ammoUIImages[i].sprite = _fullSprite;
        }
    }

    private Image InstantiateAmmoUI()
    {
        GameObject ammoInstance = new GameObject();
        ammoInstance.transform.SetParent(_ammoUIGroup.transform);
        ammoInstance.name = "Ammo" + (_ammoUIImages.Count + 1) ;
        ammoInstance.AddComponent<RectTransform>().localScale = Vector3.one;
        return ammoInstance.AddComponent<Image>();
    }

    private void OnReload()
    {
        if (!_reloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        _canFire = false;
        float time = 0;
        while (time < _reloadCooldown)
        {
            //edit slider value
            time += 0.1f;
            if (_currentAmmo < (int)(_maxAmmo * (time / _reloadCooldown)))
            {
                _currentAmmo = (int)(_maxAmmo * (time / _reloadCooldown));
                RenderAmmo();
            }
            yield return new WaitForSeconds(0.1f);
        }
        _currentAmmo = _maxAmmo;
        RenderAmmo();
        _canFire = true;
    }
}