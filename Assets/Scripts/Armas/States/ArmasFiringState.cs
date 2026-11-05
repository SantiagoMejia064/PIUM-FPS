using UnityEngine;

public class ArmasFiringState : ArmasState
{
    private float timer;

    public ArmasFiringState(Armas weapon) : base(weapon)
    {
    }

    public override void Enter()
    {
        timer = weapon.fireRate;
        weapon.PerformShot();
    }

    public override void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (weapon.currentAmmo <= 0)
            {
                weapon.ChangeState(new ArmasEmptyState(weapon));
            }
            else
            {
                weapon.ChangeState(new ArmasReadyState(weapon));
            }
        }
    }

    public override void Reload()
    {
        if (weapon.currentAmmo < weapon.magazineSize)
        {
            weapon.ChangeState(new ArmasReloadingState(weapon));
        }
    }
}
