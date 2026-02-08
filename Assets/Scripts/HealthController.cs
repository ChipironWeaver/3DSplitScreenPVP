using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
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
    
    private List<GameObject> _heartGroup = new List<GameObject>();


    public void Start()
    {
        _maxHealth = _currentHealth;
        RenderHealth();
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
        Debug.Log("Death");
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
}