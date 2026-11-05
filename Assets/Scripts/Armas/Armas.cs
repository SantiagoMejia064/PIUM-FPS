using System;
using UnityEngine;

public class Armas : MonoBehaviour
{
    [Header("Configuracion provisional")]
    public int magazineSize = 12;
    public int currentAmmo = 12;
    public float fireRate = 0.2f;
    public float reloadTime = 1.2f;
    public float range = 100f;
    public float damage = 25f;

    [Header("Punto de disparo")]
    public Transform firePoint;

    [Header("Object Pool")]
    public ObjectPoolManager poolManager;
    public string muzzleFlashPoolName;

    private ArmasState currentState;
    private Transform aimCamera;

    public event Action<int, int> AmmoChanged;

    public int CurrentAmmo
    {
        get { return currentAmmo; }
    }

    private void Awake()
    {
        currentAmmo = magazineSize;
        ChangeState(new ArmasReadyState(this));
    }

    private void Start()
    {
        NotifyAmmoChanged();
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void ChangeState(ArmasState newState)
    {
        currentState = newState;
        currentState.Enter();
    }

    public void SetAimCamera(Transform cameraTransform)
    {
        aimCamera = cameraTransform;
    }

    public void Fire()
    {
        currentState.Fire();
    }

    public void Reload()
    {
        currentState.Reload();
    }

    public void NotifyAmmoChanged()
    {
        if (AmmoChanged != null)
        {
            AmmoChanged(currentAmmo, magazineSize);
        }
    }

    private void PlayMuzzleFlash()
    {
        if (poolManager == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(muzzleFlashPoolName))
        {
            return;
        }

        if (firePoint == null)
        {
            return;
        }

        poolManager.GetObject(
            muzzleFlashPoolName,
            firePoint.position,
            firePoint.rotation,
            firePoint
        );
    }

    public void PerformShot()
    {
        if (firePoint == null)
        {
            Debug.LogWarning("El arma no tiene un FirePoint asignado.");
            return;
        }

        if (currentAmmo <= 0)
        {
            return;
        }

        currentAmmo--;

        NotifyAmmoChanged();

        PlayMuzzleFlash();

        Vector3 aimPoint = firePoint.position + firePoint.forward * range;

        if (aimCamera != null)
        {
            Vector3 cameraDirection = aimCamera.forward;
            RaycastHit[] cameraHits = Physics.RaycastAll(aimCamera.position, cameraDirection, range);

            float closestDistance = range;

            for (int i = 0; i < cameraHits.Length; i++)
            {
                if (cameraHits[i].transform.root == transform.root)
                {
                    continue;
                }

                if (cameraHits[i].distance < closestDistance)
                {
                    closestDistance = cameraHits[i].distance;
                    aimPoint = cameraHits[i].point;
                }
            }
        }

        Vector3 shotDirection = (aimPoint - firePoint.position).normalized;
        RaycastHit[] shotHits = Physics.RaycastAll(firePoint.position, shotDirection, range);

        RaycastHit? closestHit = null;
        float closestShotDistance = range;

        for (int i = 0; i < shotHits.Length; i++)
        {
            if (shotHits[i].transform.root == transform.root)
            {
                continue;
            }

            if (shotHits[i].distance < closestShotDistance)
            {
                closestShotDistance = shotHits[i].distance;
                closestHit = shotHits[i];
            }
        }

        Debug.DrawRay(firePoint.position, shotDirection * range, Color.red, 1f);

        if (closestHit.HasValue)
        {
            Debug.Log("Impacto: " + closestHit.Value.collider.name);

            PlayerHealth targetHealth = closestHit.Value.collider.GetComponentInParent<PlayerHealth>();
            PlayerStats attackerStats = transform.root.GetComponent<PlayerStats>();

            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage, attackerStats);
            }
        }

        Debug.Log("Municion restante: " + currentAmmo);
    }
}
