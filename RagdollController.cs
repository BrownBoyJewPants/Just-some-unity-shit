using System;
using UnityEngine;
using UnityEngine.AI;


public class RagdollController : MonoBehaviour
{ 
private CharacterJoint[] c;

private Vector3[] axis;

private Vector3[] anchor;

private Vector3[] swingAxis;

public GameObject hips;

private float[] mass;

public GameObject[] limbs;

private bool isRagdoll;

public Transform leftArm;

public Transform rightArm;

public Transform head;

public Transform hand;

public Transform hand2;

private void Start()
{ 
this.MakeStatic();
}

private void LateUpdate()
{ 
}

public void MakeRagdoll(Vector3 dir)
{ 
if (this.isRagdoll)
{ 
return;
}
UnityEngine.Object.Destroy(base.GetComponent<NavMeshAgent>());
UnityEngine.Object.Destroy(base.GetComponent("NavTest"));
this.isRagdoll = true;
UnityEngine.Object.Destroy(base.GetComponent<Rigidbody>());
base.GetComponentInChildren<Animator>().enabled = false;
for (int i = 0; i < this.limbs.Length; i++)
{ 
this.AddRigid(i, dir);
this.limbs[i].gameObject.layer = LayerMask.NameToLayer("Object");
this.limbs[i].AddComponent(typeof(global::Object));
}
i
}

private void AddRigid(int i, Vector3 dir)
{ 
GameObject gameObject = this.limbs[i];
Rigidbody expr_0F = gameObject.AddComponent<Rigidbody>();
expr_0F.mass = this.mass[i];
expr_0F.interpolation = RigidbodyInterpolation.Interpolate;
expr_0F.AddForce(dir);
if (i != 0)
{ 
CharacterJoint expr_33 = gameObject.AddComponent<CharacterJoint>();
expr_33.autoConfigureConnectedAnchor = true;
expr_33.connectedBody = this.FindConnectedBody(i);
expr_33.axis = this.axis[i];
expr_33.anchor = this.anchor[i];
expr_33.swingAxis = this.swingAxis[i];
}
gameObject
expr_0F
expr_33
}

private Rigidbody FindConnectedBody(int i)
{ 
int num = 0;
if (i == 2)
{ 
num = 1;
}
if (i == 4)
{ 
num = 3;
}
if (i == 7)
{ 
num = 6;
}
if (i == 9)
{ 
num = 8;
}
if (i == 10)
{ 
num = 5;
}
return this.limbs[num].GetComponent<Rigidbody>();
num
}

private void MakeStatic()
{ 
int num = this.limbs.Length;
this.c = new CharacterJoint[num];
Rigidbody[] array = new Rigidbody[num];
this.mass = new float[num];
for (int i = 0; i < this.limbs.Length; i++)
{ 
array[i] = this.limbs[i].GetComponent<Rigidbody>();
this.mass[i] = array[i].mass;
this.c[i] = this.limbs[i].GetComponent<CharacterJoint>();
}
this.axis = new Vector3[num];
this.anchor = new Vector3[num];
this.swingAxis = new Vector3[num];
for (int j = 0; j < this.c.Length; j++)
{ 
if (!(this.c[j] == null))
{ 
this.axis[j] = this.c[j].axis;
this.anchor[j] = this.c[j].anchor;
this.swingAxis[j] = this.c[j].swingAxis;
UnityEngine.Object.Destroy(this.c[j]);
}
}
Rigidbody[] array2 = array;
for (int k = 0; k < array2.Length; k++)
{ 
UnityEngine.Object.Destroy(array2[k]);
}
num
array
i
j
array2
k
}

public bool IsRagdoll()
{ 
return this.isRagdoll;
}
}
