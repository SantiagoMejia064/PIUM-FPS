using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuracion de salud")]
    public float maxHealth = 100f;

    private float currentHealth;
    private bool isDead;

    private PlayerStats playerStats;

    public event Action<float, float> HealthChanged;
    public event Action PlayerDied;

    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    public bool IsDead
    {
        get { return isDead; }
    }

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();

        currentHealth = maxHealth;
        isDead = false;
    }

    public void TakeDamage(float damage)
    {
        TakeDamage(damage, null);
    }

    public void TakeDamage(float damage, PlayerStats attacker)
    {
        if (isDead || damage <= 0f)
        {
            return;
        }

        currentHealth -= damage;

        if (currentHealth < 0f)
        {
            currentHealth = 0f;
        }

        if (HealthChanged != null)
        {
            HealthChanged(currentHealth, maxHealth);
        }

        Debug.Log(name + " recibio " + damage + " de dano. Salud: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die(attacker);
        }
    }

    private void Die(PlayerStats attacker)
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        if (playerStats != null)
        {
            playerStats.AddDeath();
        }

        if (attacker != null && attacker != playerStats)
        {
            attacker.AddKill();
        }

        Debug.Log(name + " ha sido eliminado.");

        if (PlayerDied != null)
        {
            PlayerDied();
        }
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        isDead = false;

        if (HealthChanged != null)
        {
            HealthChanged(currentHealth, maxHealth);
        }
    }
}
