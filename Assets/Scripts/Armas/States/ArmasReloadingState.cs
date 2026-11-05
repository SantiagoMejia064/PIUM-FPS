using UnityEngine;

public class ArmasReloadingState : ArmasState
{
    private float timer;

    public ArmasReloadingState(Armas weapon) : base(weapon)
    {
    }

    public override void Enter()
    {
        timer = weapon.reloadTime;
        Debug.Log("Recargando arma...");
    }

    public override void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            weapon.currentAmmo = weapon.magazineSize;
            weapon.NotifyAmmoChanged();
            weapon.ChangeState(new ArmasReadyState(weapon));

            Debug.Log("Recarga completada.");
        }
    }
}
