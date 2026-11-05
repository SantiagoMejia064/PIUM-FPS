using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private ObjectPool objectPool;

    public void SetPool(ObjectPool pool)
    {
        objectPool = pool;
    }

    public void ReturnToPool()
    {
        if (objectPool == null)
        {
            gameObject.SetActive(false);
            return;
        }

        objectPool.ReturnObject(gameObject);
    }
}
