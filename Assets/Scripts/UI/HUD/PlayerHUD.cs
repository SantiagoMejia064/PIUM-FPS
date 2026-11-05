
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Referencias UI")]
    public Text healthText;
    public Text killsText;
    public Text deathsText;
    public Text ammoText;

    private PlayerHealth playerHealth;
    private PlayerStats playerStats;
    private ArmasController armasController;
    private Armas currentWeapon;

    private void Awake()
    {
        playerHealth = GetComponentInParent<PlayerHealth>();
        playerStats = GetComponentInParent<PlayerStats>();
        armasController = GetComponentInParent<ArmasController>();
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.HealthChanged += UpdateHealth;
        }

        if (playerStats != null)
        {
            playerStats.StatsChanged += UpdateStats;
        }

        if (armasController != null)
        {
            armasController.WeaponChanged += OnWeaponChanged;
        }
    }

    private void Start()
    {
        if (playerHealth != null)
        {
            UpdateHealth(playerHealth.CurrentHealth, playerHealth.maxHealth);
        }

        if (playerStats != null)
        {
            UpdateStats(playerStats.Kills, playerStats.Deaths);
        }

        if (armasController != null)
        {
            OnWeaponChanged(armasController.CurrentWeapon);
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.HealthChanged -= UpdateHealth;
        }

        if (playerStats != null)
        {
            playerStats.StatsChanged -= UpdateStats;
        }

        if (armasController != null)
        {
            armasController.WeaponChanged -= OnWeaponChanged;
        }

        SubscribeToWeapon(null);
    }

    private void OnWeaponChanged(Armas weapon)
    {
        SubscribeToWeapon(weapon);

        if (weapon != null)
        {
            UpdateAmmo(weapon.CurrentAmmo, weapon.magazineSize);
        }
        else if (ammoText != null)
        {
            ammoText.text = "Municion: --";
        }
    }

    private void SubscribeToWeapon(Armas weapon)
    {
        if (currentWeapon != null)
        {
            currentWeapon.AmmoChanged -= UpdateAmmo;
        }

        currentWeapon = weapon;

        if (currentWeapon != null)
        {
            currentWeapon.AmmoChanged += UpdateAmmo;
        }
    }

    private void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = Mathf.CeilToInt(currentHealth) + " / " + Mathf.CeilToInt(maxHealth);
        }
    }

    private void UpdateStats(int kills, int deaths)
    {
        if (killsText != null)
        {
            killsText.text = kills + " K";
        }

        if (deathsText != null)
        {
            deathsText.text = deaths + " D";
        }
    }

    private void UpdateAmmo(int currentAmmo, int magazineSize)
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + magazineSize;
        }
    }
}
