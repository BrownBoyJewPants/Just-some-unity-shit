using System;
using UnityEngine;


public class Barrel : MonoBehaviour
{ 
private bool done;

private void OnCollisionEnter(Collision other)
{ 
if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
{ 
Explosion explosion = (Explosion)UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.explosion, base.transform.position, Quaternion.identity).GetComponentInChildren(typeof(Explosion));
UnityEngine.Object.Destroy(base.gameObject);
base.CancelInvoke();
this.done = true;
Bullet bullet = (Bullet)other.gameObject.GetComponent(typeof(Bullet));
if (bullet && bullet.player)
{ 
explosion.player = bullet.player;
}
}
explosion
bullet
}

private void OnTriggerEnter(Collider other)
{ 
if (other.gameObject.layer != LayerMask.NameToLayer("Bullet"))
{ 
return;
}
this.done = true;
base.Invoke("Explode", 0.2f);
}

private void Explode()
{ 
UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.explosion, base.transform.position, Quaternion.identity);
UnityEngine.Object.Destroy(base.gameObject);
}
}
