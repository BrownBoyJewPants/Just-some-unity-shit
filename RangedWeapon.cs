using Audio;
using System;
using System.Collections.Generic;
using UnityEngine;


public class RangedWeapon : Weapon
{ 
public GameObject projectile;

public float pushBackForce;

public float force;

public float accuracy;

public int bullets;

public float boostRecoil;

private Transform guntip;

private Rigidbody rb;

private Collider[] projectileColliders;

private new void Start()
{ 
base.Start();
this.rb = base.GetComponent<Rigidbody>();
this.guntip = base.transform.GetChild(0);
}

public override void Use(Vector3 attackDirection)
{ 
if (!base.readyToUse || !base.pickedUp)
{ 
return;
}
this.SpawnProjectile(attackDirection);
this.Recoil();
base.readyToUse = false;
base.Invoke("GetReady", this.attackSpeed);
}

public override void OnAim()
{ 
}

public override void StopUse()
{ 
}

private void SpawnProjectile(Vector3 attackDirection)
{ 
Vector3 vector = this.guntip.position - this.guntip.transform.right / 4f;
Vector3 normalized = (attackDirection - vector).normalized;
List<Collider> list = new List<Collider>();
if (this.player)
{ 
PlayerMovement.Instance.GetRb().AddForce(base.transform.right * this.boostRecoil, ForceMode.Impulse);
}
for (int i = 0; i < this.bullets; i++)
{ 
UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.muzzle, vector, Quaternion.identity);
GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.projectile, vector, base.transform.rotation);
Rigidbody componentInChildren = gameObject.GetComponentInChildren<Rigidbody>();
this.projectileColliders = gameObject.GetComponentsInChildren<Collider>();
this.RemoveCollisionWithPlayer();
componentInChildren.transform.rotation = base.transform.rotation;
Vector3 a = normalized + (this.guntip.transform.up * UnityEngine.Random.Range(-this.accuracy, this.accuracy) + this.guntip.transform.forward * UnityEngine.Random.Range(-this.accuracy, this.accuracy));
componentInChildren.AddForce(componentInChildren.mass * this.force * a);
Bullet bullet = (Bullet)gameObject.GetComponent(typeof(Bullet));
if (bullet != null)
{ 
Color col = Color.red;
if (this.player)
{ 
col = Color.blue;
Gun.Instance.Shoot();
if (bullet.explosive)
{ 
UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.thumpAudio, base.transform.position, Quaternion.identity);
}
else
{ 
AudioManager.Instance.PlayPitched("GunBass", 0.3f);
AudioManager.Instance.PlayPitched("GunHigh", 0.3f);
AudioManager.Instance.PlayPitched("GunLow", 0.3f);
}
componentInChildren.AddForce(componentInChildren.mass * this.force * a);
}
else
{ 
UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.gunShotAudio, base.transform.position, Quaternion.identity);
}
bullet.SetBullet(this.damage, this.pushBackForce, col);
bullet.player = this.player;
}
using (List<Collider>.Enumerator enumerator = list.GetEnumerator())
{ 
while (enumerator.MoveNext())
{ 
Physics.IgnoreCollision(enumerator.Current, this.projectileColliders[0]);
}
}
list.Add(this.projectileColliders[0]);
}
vector
normalized
list
i
gameObject
componentInChildren
a
bullet
col
enumerator
}

private void GetReady()
{ 
base.readyToUse = true;
}

private void Recoil()
{ 
}

private void RemoveCollisionWithPlayer()
{ 
Collider[] array;
if (this.player)
{ 
array = new Collider[]
{ 
PlayerMovement.Instance.GetPlayerCollider()
};
}
else
{ 
array = base.transform.root.GetComponentsInChildren<Collider>();
}
for (int i = 0; i < array.Length; i++)
{ 
for (int j = 0; j < this.projectileColliders.Length; j++)
{ 
Physics.IgnoreCollision(array[i], this.projectileColliders[j], true);
}
}
array
i
j
}
}
