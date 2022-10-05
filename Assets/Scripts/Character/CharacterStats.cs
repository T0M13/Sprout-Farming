using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int currentHealth = 20;
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private int minHealth = 10;

    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int MinHealth { get => minHealth; set => minHealth = value; }

    private void Start()
    {
        SetHealth(minHealth);
    }

    public void AddHealth(int value)
    {
        currentHealth += value;
        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
    }

    public void RemoveHealth(int value)
    {
        currentHealth -= value;
        if (currentHealth <= 0)
        {
            Debug.Log("Player 'Died'");
            //Kill Player - Respawn - Die
        }
    }

    public void SetHealth(int value)
    {
        currentHealth = value;
        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
    }
}
