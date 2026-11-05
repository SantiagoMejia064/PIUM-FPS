using UnityEngine;

public static class ArmasFactory
{
    public static Armas CreateWeapon(GameObject weaponPrefab, Transform weaponHolder)
    {
        if (weaponPrefab == null || weaponHolder == null)
        {
            Debug.LogError("Falta el prefab del arma o el WeaponHolder.");
            return null;
        }

        GameObject weaponObject = Object.Instantiate(weaponPrefab, weaponHolder);

        weaponObject.transform.localPosition = Vector3.zero;
        weaponObject.transform.localRotation = Quaternion.identity;
        weaponObject.transform.localScale = Vector3.one;

        Armas weapon = weaponObject.GetComponent<Armas>();

        if (weapon == null)
        {
            Debug.LogError("El prefab no tiene el componente Armas.");
            Object.Destroy(weaponObject);
            return null;
        }

        return weapon;
    }
}
