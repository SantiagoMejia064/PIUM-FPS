using System;
using UnityEngine;

public class ArmasController : MonoBehaviour
{
    [Header("Configuracion del arma")]
    public GameObject startingWeaponPrefab;
    public Transform weaponHolder;

    private Armas currentWeapon;
    private ObjectPoolManager poolManager;

    public event Action<Armas> WeaponChanged;

    public Armas CurrentWeapon
    {
        get { return currentWeapon; }
    }

    public int CurrentAmmo
    {
        get
        {
            if (currentWeapon != null)
            {
                return currentWeapon.CurrentAmmo;
            }

            return 0;
        }
    }

    private void Start()
    {
        poolManager = FindFirstObjectByType<ObjectPoolManager>();

        if (poolManager == null)
        {
            Debug.LogWarning("No se encontro un ObjectPoolManager en la escena.");
        }

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

        if (currentWeapon != null)
        {
            if (poolManager == null)
            {
                poolManager = FindFirstObjectByType<ObjectPoolManager>();
            }

            currentWeapon.poolManager = poolManager;
        }

        if (WeaponChanged != null)
        {
            WeaponChanged(currentWeapon);
        }
    }

    public void Fire()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Fire();
        }
    }

    public void Fire(Transform aimCamera)
    {
        if (currentWeapon != null)
        {
            currentWeapon.SetAimCamera(aimCamera);
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
