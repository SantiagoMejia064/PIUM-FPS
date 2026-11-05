using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Configuracion del Pool")]
    public GameObject prefab;
    public int initialSize = 5;

    private List<GameObject> poolObjects = new List<GameObject>();
    private bool initialized = false;

    public void InitializePool()
    {
        if (initialized)
        {
            return;
        }

        if (prefab == null)
        {
            Debug.LogError("El Pool no tiene un Prefab asignado: " + gameObject.name);
            return;
        }

        for (int i = 0; i < initialSize; i++)
        {
            CreateObject();
        }

        initialized = true;
    }

    private GameObject CreateObject()
    {
        GameObject newObject = Instantiate(prefab, transform);

        PooledObject pooledObject = newObject.GetComponent<PooledObject>();

        if (pooledObject == null)
        {
            pooledObject = newObject.AddComponent<PooledObject>();
        }

        pooledObject.SetPool(this);

        newObject.SetActive(false);

        poolObjects.Add(newObject);

        return newObject;
    }

    public GameObject GetObject(Vector3 position, Quaternion rotation, Transform parent)
    {
        if (!initialized)
        {
            InitializePool();
        }

        for (int i = 0; i < poolObjects.Count; i++)
        {
            if (!poolObjects[i].activeSelf)
            {
                GameObject objectToReturn = poolObjects[i];

                objectToReturn.transform.SetParent(parent, false);
                objectToReturn.transform.SetPositionAndRotation(position, rotation);

                objectToReturn.SetActive(true);

                return objectToReturn;
            }
        }

        Debug.LogWarning("No hay objetos disponibles en el Pool: " + gameObject.name);

        return null;
    }

    public void ReturnObject(GameObject objectToReturn)
    {
        if (objectToReturn == null)
        {
            return;
        }

        if (poolObjects.Contains(objectToReturn))
        {
            objectToReturn.transform.SetParent(transform, true);
            objectToReturn.SetActive(false);
        }
    }
}
