using EZCameraShake;
using System;
using UnityEngine;


public class Explosion : MonoBehaviour
{ 
public bool player;

private void Start()
{ 
float num = Vector3.Distance(base.transform.position, PlayerMovement.Instance.gameObject.transform.position);
MonoBehaviour.print(num);
float num2 = 10f / num;
if (num2 < 0.1f)
{ 
num2 = 0f;
}
CameraShaker.Instance.ShakeOnce(20f * num2 * GameState.Instance.cameraShake, 2f, 0.4f, 0.5f);
MonoBehaviour.print("ratio: " + num2);
num
num2
}

private void OnTriggerEnter(Collider other)
{ 
int layer = other.gameObject.layer;
Vector3 normalized = (other.transform.position - base.transform.position).normalized;
float num = Vector3.Distance(other.transform.position, base.transform.position);
if (other.gameObject.CompareTag("Enemy"))
{ 
if (other.gameObject.name != "Torso")
{ 
return;
}
RagdollController ragdollController = (RagdollController)other.transform.root.GetComponent(typeof(RagdollController));
if (!ragdollController || ragdollController.IsRagdoll())
{ 
return;
}
ragdollController.MakeRagdoll(normalized * 1100f);
if (this.player)
{ 
PlayerMovement.Instance.Slowmo(0.35f, 0.5f);
}
Enemy enemy = (Enemy)other.transform.root.GetComponent(typeof(Enemy));
if (enemy)
{ 
enemy.DropGun(Vector3.up);
}
return;
}
else
{ 
Rigidbody component = other.gameObject.GetComponent<Rigidbody>();
if (!component)
{ 
return;
}
if (num < 5f)
{ 
num = 5f;
}
component.AddForce(normalized * 450f / num, ForceMode.Impulse);
component.AddTorque(new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * 10f);
if (layer == LayerMask.NameToLayer("Player"))
{ 
((PlayerMovement)other.transform.root.GetComponent(typeof(PlayerMovement))).Explode();
}
return;
}
layer
normalized
num
ragdollController
enemy
component
}
}
