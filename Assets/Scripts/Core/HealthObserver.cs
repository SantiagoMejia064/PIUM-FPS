using UnityEngine;

public class HealthObserver : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        if (playerHealth == null)
        {
            playerHealth = GetComponent<PlayerHealth>();
        }

        if (playerHealth != null)
        {
            playerHealth.HealthChanged += OnHealthChanged;
            playerHealth.PlayerDied += OnPlayerDied;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.HealthChanged -= OnHealthChanged;
            playerHealth.PlayerDied -= OnPlayerDied;
        }
    }

    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        Debug.Log(name + " - Salud: " + currentHealth + "/" + maxHealth);
    }

    private void OnPlayerDied()
    {
        Debug.Log(name + " - Evento de jugador eliminado recibido.");
    }
}
