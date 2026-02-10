using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Image = UnityEngine.UI.Image;

public class HealthController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;
    [Header("Invincibility Frame Settings")]
    [SerializeField] private float _invicibilityTime;
    [SerializeField] private bool  _invicible;
    [Header("Render Settings")]
    [SerializeField] private Sprite _heartSprite;
    [SerializeField] private GameObject _group;
    [Header("Respawn Settings")]
    [SerializeField] public bool canRespawn;
    [SerializeField] public float respawnTime;
    [SerializeField] public Transform respawnPoint;
    [Header("UX Settings")]
    [SerializeField] private AudioClip _hurtSound;
    [SerializeField] private List<AudioClip> _deathSound;
    [SerializeField] private List<GameObject> _deathParticles;
        
    
    private List<GameObject> _heartGroup = new List<GameObject>();
    private Transform _transform;
    private MovementController _movementController;
    private CharacterController _characterController;


    public void Start()
    {
        _maxHealth = _currentHealth;
        RenderHealth(); 
        _transform = GetComponent<Transform>();
        _movementController = GetComponent<MovementController>();
        _characterController = GetComponent<CharacterController>();
        Respawn();
    }

    public void TakeDamage(int damage)
    {
        if (!_invicible)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Death();
            }
            _invicible = true;
            Invoke(nameof(RemoveInvincibility), _invicibilityTime);
            AudioSource.PlayClipAtPoint(_hurtSound, Vector3.zero);
        }
        RenderHealth();
    }

    public void Heal(int heal)
    {
        _currentHealth += heal;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        RenderHealth();
    }

    public void Death()
    {
        _movementController.isActive =  true;
        if (_deathSound != null)
        {
            foreach (AudioClip clip in _deathSound)
            {
                AudioSource.PlayClipAtPoint(clip, Vector3.zero);
            }
        }

        if (_deathParticles != null)
        {
            foreach (GameObject particle in _deathParticles)
            {
                Instantiate(particle,_transform.position,Quaternion.identity);
            }
            
        }
        if (canRespawn)
        {
            Invoke(nameof(Respawn), respawnTime);
            
        }
    }

    private void RemoveInvincibility()
    {
        _invicible = false;
    }

    private void RenderHealth()
    {
        while (_heartGroup.Count < _maxHealth)
        {
            _heartGroup.Add(InstantiateHeart());
        }

        for (int i = 0; i < _heartGroup.Count; i++)
        {
            if (i > _currentHealth - 1) _heartGroup[i].gameObject.SetActive(false);
            else _heartGroup[i].gameObject.SetActive(true);
        }
    }

    private GameObject InstantiateHeart()
    {
        GameObject Heart = new GameObject();
        Heart.transform.SetParent(_group.transform);
        Heart.name = "Heart" + (_heartGroup.Count + 1) ;
        Heart.AddComponent<Image>().sprite = _heartSprite;
        Heart.GetComponent<RectTransform>().localScale = Vector3.one;
        return Heart;
    }

    public void Respawn()
    {
        if (respawnPoint != null)
        {
            _characterController.enabled = false;
            _transform.position = respawnPoint.position;
            _characterController.enabled = true;
            Heal(_maxHealth);
            _movementController.isActive =  true;
        }
    }
}