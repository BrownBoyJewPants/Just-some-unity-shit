using Audio;
using System;
using System.Collections.Generic;
using UnityEngine;


public class DetectWeapons : MonoBehaviour
{ 
public Transform weaponPos;

private List<GameObject> weapons;

private bool hasGun;

private GameObject gun;

private Pickup gunScript;

private float speed = 10f;

private Quaternion desiredRot = Quaternion.Euler(0f, 90f, 0f);

private Vector3 desiredPos = Vector3.zero;

private Vector3 posVel;

private float throwForce = 1000f;

private Vector3 scale;

public void Pickup()
{ 
if (this.hasGun || !this.HasWeapons())
{ 
return;
}
this.gun = this.GetWeapon();
this.gunScript = (Pickup)this.gun.GetComponent(typeof(Pickup));
if (this.gunScript.pickedUp)
{ 
this.gun = null;
this.gunScript = null;
return;
}
UnityEngine.Object.Destroy(this.gun.GetComponent<Rigidbody>());
this.scale = this.gun.transform.localScale;
this.gun.transform.parent = this.weaponPos;
this.gun.transform.localScale = this.scale;
this.hasGun = true;
this.gunScript.PickupWeapon(true);
AudioManager.Instance.Play("GunPickup");
this.gun.layer = LayerMask.NameToLayer("Equipable");
}

public void Shoot(Vector3 dir)
{ 
if (!this.hasGun)
{ 
return;
}
this.gunScript.Use(dir);
}

public void StopUse()
{ 
if (!this.hasGun)
{ 
return;
}
this.gunScript.StopUse();
}

public void Throw(Vector3 throwDir)
{ 
if (!this.hasGun)
{ 
return;
}
if (this.gun.GetComponent<Rigidbody>())
{ 
return;
}
this.gunScript.StopUse();
this.hasGun = false;
Rigidbody expr_39 = this.gun.AddComponent<Rigidbody>();
expr_39.interpolation = RigidbodyInterpolation.Interpolate;
expr_39.maxAngularVelocity = 20f;
expr_39.AddForce(throwDir * this.throwForce);
expr_39.AddRelativeTorque(new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f) * 0.4f), ForceMode.Impulse);
this.gun.layer = LayerMask.NameToLayer("Gun");
this.gunScript.Drop();
this.gun.transform.parent = null;
this.gun.transform.localScale = this.scale;
this.gun = null;
this.gunScript = null;
expr_39
}

public void Fire(Vector3 dir)
{ 
this.gunScript.Use(dir);
}

private void Update()
{ 
if (!this.hasGun)
{ 
return;
}
this.gun.transform.localRotation = Quaternion.Slerp(this.gun.transform.localRotation, this.desiredRot, Time.deltaTime * this.speed);
this.gun.transform.localPosition = Vector3.SmoothDamp(this.gun.transform.localPosition, this.desiredPos, ref this.posVel, 1f / this.speed);
this.gunScript.OnAim();
}

private void Start()
{ 
this.weapons = new List<GameObject>();
}

private void OnTriggerEnter(Collider other)
{ 
if (other.CompareTag("Gun") && !this.weapons.Contains(other.gameObject))
{ 
this.weapons.Add(other.gameObject);
}
}

private void OnTriggerExit(Collider other)
{ 
if (other.CompareTag("Gun") && this.weapons.Contains(other.gameObject))
{ 
this.weapons.Remove(other.gameObject);
}
}

public GameObject GetWeapon()
{ 
if (this.weapons.Count == 1)
{ 
return this.weapons[0];
}
GameObject result = null;
float num = float.PositiveInfinity;
foreach (GameObject current in this.weapons)
{ 
float num2 = Vector3.Distance(base.transform.position, current.transform.position);
if (num2 < num)
{ 
num = num2;
result = current;
}
}
return result;
result
num
enumerator
current
num2
}

public void ForcePickup(GameObject gun)
{ 
this.gunScript = (Pickup)gun.GetComponent(typeof(Pickup));
this.gun = gun;
if (this.gunScript.pickedUp)
{ 
gun = null;
this.gunScript = null;
return;
}
UnityEngine.Object.Destroy(gun.GetComponent<Rigidbody>());
this.scale = gun.transform.localScale;
gun.transform.parent = this.weaponPos;
gun.transform.localScale = this.scale;
this.hasGun = true;
this.gunScript.PickupWeapon(true);
gun.layer = LayerMask.NameToLayer("Equipable");
}

public float GetRecoil()
{ 
return this.gunScript.recoil;
}

public bool HasWeapons()
{ 
return this.weapons.Count > 0;
}

public bool IsGrappler()
{ 
return this.gun && this.gun.GetComponent(typeof(Grappler));
}

public Vector3 GetGrapplerPoint()
{ 
if (this.IsGrappler())
{ 
return ((Grappler)this.gun.GetComponent(typeof(Grappler))).GetGrapplePoint();
}
return Vector3.zero;
}

public Pickup GetWeaponScript()
{ 
return this.gunScript;
}

public bool HasGun()
{ 
return this.hasGun;
}
}
