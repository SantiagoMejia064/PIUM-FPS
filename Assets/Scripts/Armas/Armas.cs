using UnityEngine;

public class Armas : MonoBehaviour
{
    [Header("Configuracion provisional")]
    public int magazineSize = 12;
    public int currentAmmo = 12;
    public float fireRate = 0.2f;
    public float reloadTime = 1.2f;
    public float range = 100f;

    [Header("Punto de disparo")]
    public Transform firePoint;

    private ArmasState currentState;

    private void Awake()
    {
        currentAmmo = magazineSize;
        ChangeState(new ArmasReadyState(this));
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

    public void Fire()
    {
        currentState.Fire();
    }

    public void Reload()
    {
        currentState.Reload();
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

        RaycastHit hit;

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range))
        {
            Debug.Log("Impacto: " + hit.collider.name);
        }

        Debug.DrawRay(firePoint.position, firePoint.forward * range, Color.red, 1f);

        Debug.Log("Municion restante: " + currentAmmo);
    }
}
