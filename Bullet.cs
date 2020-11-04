using Audio;
using System;
using UnityEngine;


public class Bullet : MonoBehaviour
{ 
public bool changeCol;

public bool player;

private float damage;

private float push;

private bool done;

private Color col;

public bool explosive;

private GameObject limbHit;

private Rigidbody rb;

private void Start()
{ 
this.rb = base.GetComponent<Rigidbody>();
}

private void OnCollisionEnter(Collision other)
{ 
if (this.done)
{ 
return;
}
this.done = true;
if (this.explosive)
{ 
UnityEngine.Object.Destroy(base.gameObject);
((Explosion)UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.explosion, other.contacts[0].point, Quaternion.identity).GetComponentInChildren(typeof(Explosion))).player = this.player;
return;
}
this.BulletExplosion(other.contacts[0]);
UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.bulletHitAudio, other.contacts[0].point, Quaternion.identity);
int layer = other.gameObject.layer;
if (layer == LayerMask.NameToLayer("Player"))
{ 
this.HitPlayer(other.gameObject);
UnityEngine.Object.Destroy(base.gameObject);
return;
}
if (layer == LayerMask.NameToLayer("Enemy"))
{ 
if (this.col == Color.blue)
{ 
AudioManager.Instance.Play("Hitmarker");
MonoBehaviour.print("HITMARKER");
}
UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.enemyHitAudio, other.contacts[0].point, Quaternion.identity);
((RagdollController)other.transform.root.GetComponent(typeof(RagdollController))).MakeRagdoll(-base.transform.right * 350f);
if (other.gameObject.GetComponent<Rigidbody>())
{ 
other.gameObject.GetComponent<Rigidbody>().AddForce(-base.transform.right * 1500f);
}
((Enemy)other.transform.root.GetComponent(typeof(Enemy))).DropGun(Vector3.up);
UnityEngine.Object.Destroy(base.gameObject);
return;
}
if (layer == LayerMask.NameToLayer("Bullet"))
{ 
if (other.gameObject.name == base.gameObject.name)
{ 
return;
}
UnityEngine.Object.Destroy(base.gameObject);
UnityEngine.Object.Destroy(other.gameObject);
this.BulletExplosion(other.contacts[0]);
}
UnityEngine.Object.Destroy(base.gameObject);
layer
}

private void HitPlayer(GameObject other)
{ 
PlayerMovement.Instance.KillPlayer();
}

private void Update()
{ 
if (!this.explosive)
{ 
return;
}
this.rb.AddForce(Vector3.up * Time.deltaTime * 1000f);
}

private void BulletExplosion(ContactPoint contact)
{ 
Vector3 point = contact.point;
Vector3 normal = contact.normal;
ParticleSystem expr_3A = UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.bulletDestroy, point + normal * 0.05f, Quaternion.identity).GetComponent<ParticleSystem>();
expr_3A.transform.rotation = Quaternion.LookRotation(normal);
expr_3A.startColor = Color.blue;
point
normal
expr_3A
}

public void SetBullet(float damage, float push, Color col)
{ 
this.damage = damage;
this.push = push;
this.col = col;
if (this.changeCol)
{ 
SpriteRenderer[] componentsInChildren = base.GetComponentsInChildren<SpriteRenderer>();
for (int i = 0; i < componentsInChildren.Length; i++)
{ 
componentsInChildren[i].color = col;
}
}
TrailRenderer componentInChildren = base.GetComponentInChildren<TrailRenderer>();
if (componentInChildren == null)
{ 
return;
}
componentInChildren.startColor = col;
componentInChildren.endColor = col;
componentsInChildren
i
componentInChildren
}
}
