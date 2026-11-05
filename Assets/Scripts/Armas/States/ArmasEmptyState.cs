using UnityEngine;

public class ArmasEmptyState : ArmasState
{
    public ArmasEmptyState(Armas weapon) : base(weapon)
    {
    }

    public override void Enter()
    {
        Debug.Log("Sin municion. Recarga el arma.");
    }

    public override void Reload()
    {
        weapon.ChangeState(new ArmasReloadingState(weapon));
    }
}
