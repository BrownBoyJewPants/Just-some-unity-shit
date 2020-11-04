using System;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{ 
private float hipSpeed = 3f;

private float headAndHandSpeed = 4f;

private Transform target;

public LayerMask objectsAndPlayer;

private NavMeshAgent agent;

private bool spottedPlayer;

private Animator animator;

public GameObject startGun;

public Transform gunPosition;

private Weapon gunScript;

public GameObject currentGun;

private float attackSpeed;

private bool readyToShoot;

private RagdollController ragdoll;

public Transform leftArm;

public Transform rightArm;

public Transform head;

public Transform hips;

public Transform player;

private bool takingAim;

private void Start()
{ 
this.ragdoll = (RagdollController)base.GetComponent(typeof(RagdollController));
this.animator = base.GetComponentInChildren<Animator>();
this.agent = base.GetComponent<NavMeshAgent>();
this.GiveGun();
}

private void LateUpdate()
{ 
this.FindPlayer();
this.Aim();
}

private void Aim()
{ 
if (this.currentGun == null)
{ 
return;
}
if (this.ragdoll.IsRagdoll())
{ 
return;
}
if (!this.animator.GetBool("Aiming"))
{ 
return;
}
Vector3 vector = this.target.transform.position - base.transform.position;
if (Vector3.Angle(base.transform.forward, vector) > 70f)
{ 
base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(vector), Time.deltaTime * this.hipSpeed);
}
this.head.transform.rotation = Quaternion.Slerp(this.head.transform.rotation, Quaternion.LookRotation(vector), Time.deltaTime * this.headAndHandSpeed);
this.rightArm.transform.rotation = Quaternion.Slerp(this.head.transform.rotation, Quaternion.LookRotation(vector), Time.deltaTime * this.headAndHandSpeed);
this.leftArm.transform.rotation = Quaternion.Slerp(this.head.transform.rotation, Quaternion.LookRotation(vector), Time.deltaTime * this.headAndHandSpeed);
if (this.readyToShoot)
{ 
this.gunScript.Use(this.target.position);
this.readyToShoot = false;
base.Invoke("Cooldown", this.attackSpeed + UnityEngine.Random.Range(this.attackSpeed, this.attackSpeed * 5f));
}
vector
}

private void FindPlayer()
{ 
this.FindTarget();
if (!this.agent || !this.target)
{ 
return;
}
Vector3 normalized = (this.target.position - base.transform.position).normalized;
RaycastHit[] array = Physics.RaycastAll(base.transform.position + normalized, normalized, (float)this.objectsAndPlayer);
if (array.Length < 1)
{ 
return;
}
bool flag = false;
float num = 1001f;
float num2 = 1000f;
for (int i = 0; i < array.Length; i++)
{ 
int layer = array[i].collider.gameObject.layer;
if (!(array[i].collider.transform.root.gameObject.name == base.gameObject.name) && layer != LayerMask.NameToLayer("TransparentFX"))
{ 
if (layer == LayerMask.NameToLayer("Player"))
{ 
num = array[i].distance;
flag = true;
}
else if (array[i].distance < num2)
{ 
num2 = array[i].distance;
}
}
}
if (!flag)
{ 
return;
}
if (num2 < num && num != 1001f)
{ 
this.readyToShoot = false;
if (this.animator.GetBool("Running") && this.agent.remainingDistance < 0.2f)
{ 
this.animator.SetBool("Running", false);
this.spottedPlayer = false;
}
if (!this.spottedPlayer || !this.agent.isOnNavMesh || this.animator.GetBool("Running"))
{ 
return;
}
MonoBehaviour.print("oof");
this.takingAim = false;
this.agent.destination = this.target.transform.position;
this.animator.SetBool("Running", true);
this.animator.SetBool("Aiming", false);
this.readyToShoot = false;
return;
}
else
{ 
if (this.takingAim || this.animator.GetBool("Aiming"))
{ 
return;
}
if (!this.spottedPlayer)
{ 
this.spottedPlayer = true;
}
base.Invoke("TakeAim", UnityEngine.Random.Range(0.3f, 1f));
this.takingAim = true;
return;
}
normalized
array
flag
num
num2
i
layer
}

private void TakeAim()
{ 
this.animator.SetBool("Running", false);
this.animator.SetBool("Aiming", true);
base.CancelInvoke();
base.Invoke("Cooldown", UnityEngine.Random.Range(0.3f, 1f));
if (this.agent && this.agent.isOnNavMesh)
{ 
this.agent.destination = base.transform.position;
}
}

private void GiveGun()
{ 
if (this.startGun == null)
{ 
return;
}
GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.startGun);
UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
this.gunScript = (Weapon)gameObject.GetComponent(typeof(Weapon));
this.gunScript.PickupWeapon(false);
gameObject.transform.parent = this.gunPosition;
gameObject.transform.localPosition = Vector3.zero;
gameObject.transform.localRotation = Quaternion.identity;
this.currentGun = gameObject;
this.attackSpeed = this.gunScript.GetAttackSpeed();
gameObject
}

private void Cooldown()
{ 
this.readyToShoot = true;
}

private void FindTarget()
{ 
if (this.target != null)
{ 
return;
}
if (!PlayerMovement.Instance)
{ 
return;
}
this.target = PlayerMovement.Instance.playerCam;
}

public void DropGun(Vector3 dir)
{ 
if (this.gunScript == null)
{ 
return;
}
this.gunScript.Drop();
Rigidbody expr_25 = this.currentGun.AddComponent<Rigidbody>();
expr_25.interpolation = RigidbodyInterpolation.Interpolate;
this.currentGun.transform.parent = null;
expr_25.AddForce(dir, ForceMode.Impulse);
float d = 10f;
expr_25.AddTorque(new Vector3((float)UnityEngine.Random.Range(-1, 1), (float)UnityEngine.Random.Range(-1, 1), (float)UnityEngine.Random.Range(-1, 1)) * d);
this.gunScript = null;
expr_25
d
}

public bool IsDead()
{ 
return this.ragdoll.IsRagdoll();
}
}
