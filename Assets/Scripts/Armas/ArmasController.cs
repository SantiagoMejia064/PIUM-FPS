using UnityEngine;

public class ArmasController : MonoBehaviour
{
    [Header("Configuracion del arma")]
    public GameObject startingWeaponPrefab;
    public Transform weaponHolder;

    private Armas currentWeapon;

    private void Start()
    {
        if (startingWeaponPrefab != null && weaponHolder != null)
        {
            EquipWeapon(startingWeaponPrefab);
        }
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        currentWeapon = ArmasFactory.CreateWeapon(weaponPrefab, weaponHolder);
    }

    public void Fire()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Fire();
        }
    }

    public void Reload()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Reload();
        }
    }
}
