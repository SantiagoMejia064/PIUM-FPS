using UnityEngine;

public class PooledParticleEffect : MonoBehaviour
{
    public float duration = 0.3f;

    private float timer;
    private PooledObject pooledObject;

    private void Awake()
    {
        pooledObject = GetComponent<PooledObject>();
    }

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= duration)
        {
            gameObject.SetActive(false);
        }
    }
}
