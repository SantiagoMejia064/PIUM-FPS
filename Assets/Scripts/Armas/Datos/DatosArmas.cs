using UnityEngine;

[CreateAssetMenu(fileName = "NewArmaData", menuName = "Arena UAO/Data Arma")]
public class DatosArmas : ScriptableObject
{
    [Header("Identificacion")]
    public string weaponName;
    public GameObject weaponPrefab;

    [Header("Efectos")]
    public GameObject muzzleFlashPrefab;
    public string muzzleFlashPoolName;

    public GameObject impactPrefab;
    public string impactPoolName;

    public GameObject projectilePrefab;
    public string projectilePoolName;

    [Header("Configuracion")]
    public int magazineSize;
    public float fireRate;
    public float reloadTime;
    public float range;
    public float damage;
}
