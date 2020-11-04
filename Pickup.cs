using System;
using UnityEngine;


public abstract class Pickup : MonoBehaviour, IPickup
{ 
protected bool player;

private bool thrown;

public float recoil;

private Transform outline;

public bool pickedUp
{ 
get;
set;
}

public bool readyToUse
{ 
get;
set;
}

private void Awake()
{ 
this.readyToUse = true;
this.outline = base.transform.GetChild(1);
}

private void Update()
{ 
bool arg_06_0 = this.pickedUp;
arg_06_0
}

public void PickupWeapon(bool player)
{ 
this.pickedUp = true;
this.player = player;
this.outline.gameObject.SetActive(false);
}

public void Drop()
{ 
this.readyToUse = true;
base.Invoke("DropWeapon", 0.5f);
this.thrown = true;
}

private void DropWeapon()
{ 
base.CancelInvoke();
this.pickedUp = false;
this.outline.gameObject.SetActive(true);
}

public abstract void Use(Vector3 attackDirection);

public abstract void OnAim();

public abstract void StopUse();

public bool IsPickedUp()
{ 
return this.pickedUp;
}

private void OnCollisionEnter(Collision other)
{ 
if (!this.thrown)
{ 
return;
}
if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
{ 
UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.enemyHitAudio, other.contacts[0].point, Quaternion.identity);
((RagdollController)other.transform.root.GetComponent(typeof(RagdollController))).MakeRagdoll(-base.transform.right * 60f);
Rigidbody component = other.gameObject.GetComponent<Rigidbody>();
if (component)
{ 
component.AddForce(-base.transform.right * 1500f);
}
((Enemy)other.transform.root.GetComponent(typeof(Enemy))).DropGun(Vector3.up);
}
this.thrown = false;
component
}
}
