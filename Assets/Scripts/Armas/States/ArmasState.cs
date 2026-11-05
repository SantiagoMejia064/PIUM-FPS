using UnityEngine;

public abstract class ArmasState
{
    protected Armas weapon;

    public ArmasState(Armas weapon)
    {
        this.weapon = weapon;
    }

    public virtual void Enter()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void Fire()
    {
    }

    public virtual void Reload()
    {
    }
}
