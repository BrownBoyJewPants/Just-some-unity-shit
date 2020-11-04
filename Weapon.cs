using System;
using UnityEngine;


public abstract class Weapon : Pickup
{ 
public float attackSpeed;

public float damage;

public TrailRenderer trailRenderer;

public float MultiplierDamage
{ 
get;
set;
}

public void Start()
{ 
this.MultiplierDamage = 1f;
}

protected void Cooldown()
{ 
base.readyToUse = true;
}

public float GetAttackSpeed()
{ 
return this.attackSpeed;
}
}
