using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [Header("Armas")]
    public DatosArmas[] weaponDataList;

    [Header("Configuracion")]
    public int initialPoolSize = 5;

    private Dictionary<string, ObjectPool> poolDictionary = new Dictionary<string, ObjectPool>();

    private void Awake()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        for (int i = 0; i < weaponDataList.Length; i++)
        {
            DatosArmas weaponData = weaponDataList[i];

            if (weaponData == null)
            {
                continue;
            }

            RegisterPool(weaponData.muzzleFlashPoolName, weaponData.muzzleFlashPrefab);
            RegisterPool(weaponData.impactPoolName, weaponData.impactPrefab);
            RegisterPool(weaponData.projectilePoolName, weaponData.projectilePrefab);
        }
    }

    private void RegisterPool(string poolName, GameObject prefab)
    {
        if (string.IsNullOrEmpty(poolName) || prefab == null)
        {
            return;
        }

        if (poolDictionary.ContainsKey(poolName))
        {
            return;
        }

        GameObject poolObject = new GameObject("Pool_" + poolName);

        poolObject.transform.SetParent(transform);

        ObjectPool objectPool = poolObject.AddComponent<ObjectPool>();

        objectPool.prefab = prefab;
        objectPool.initialSize = initialPoolSize;

        objectPool.InitializePool();

        poolDictionary.Add(poolName, objectPool);
    }

    public GameObject GetObject(string poolName, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (string.IsNullOrEmpty(poolName))
        {
            return null;
        }

        if (!poolDictionary.ContainsKey(poolName))
        {
            Debug.LogWarning("No existe el Pool: " + poolName);
            return null;
        }

        return poolDictionary[poolName].GetObject(position, rotation, parent);
    }


}
