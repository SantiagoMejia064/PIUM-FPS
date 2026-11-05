public class ArmasReadyState : ArmasState
{
    public ArmasReadyState(Armas weapon) : base(weapon)
    {
    }

    public override void Fire()
    {
        if (weapon.currentAmmo <= 0)
        {
            weapon.ChangeState(new ArmasEmptyState(weapon));
            return;
        }

        weapon.ChangeState(new ArmasFiringState(weapon));
    }

    public override void Reload()
    {
        if (weapon.currentAmmo < weapon.magazineSize)
        {
            weapon.ChangeState(new ArmasReloadingState(weapon));
        }
    }
}
