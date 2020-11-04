using System;
using UnityEngine;


public class Grappling : MonoBehaviour
{ 
private LineRenderer lr;

public LayerMask whatIsSickoMode;

private Transform connectedTransform;

private SpringJoint joint;

private Vector3 offsetPoint;

private Vector3 endPoint;

private Vector3 ropeVel;

private Vector3 desiredPos;

private float offsetMultiplier;

private float offsetVel;

private bool readyToUse = true;

public static Grappling Instance
{ 
get;
set;
}

private void Start()
{ 
Grappling.Instance = this;
this.lr = base.GetComponentInChildren<LineRenderer>();
this.lr.positionCount = 0;
}

private void Update()
{ 
this.DrawLine();
if (this.connectedTransform == null)
{ 
return;
}
Vector2 vector = (this.connectedTransform.position - base.transform.position).normalized;
Mathf.Atan2(vector.y, vector.x);
this.joint == null;
vector
}

private void DrawLine()
{ 
if (this.connectedTransform == null || this.joint == null)
{ 
this.ClearLine();
return;
}
this.desiredPos = this.connectedTransform.position + this.offsetPoint;
this.endPoint = Vector3.SmoothDamp(this.endPoint, this.desiredPos, ref this.ropeVel, 0.03f);
this.offsetMultiplier = Mathf.SmoothDamp(this.offsetMultiplier, 0f, ref this.offsetVel, 0.12f);
int num = 100;
this.lr.positionCount = num;
Vector3 position = base.transform.position;
this.lr.SetPosition(0, position);
this.lr.SetPosition(num - 1, this.endPoint);
float num2 = 15f;
float num3 = 0.5f;
for (int i = 1; i < num - 1; i++)
{ 
float expr_D8 = (float)i / (float)num;
float num4 = expr_D8 * this.offsetMultiplier;
float num5 = (Mathf.Sin(num4 * num2) - 0.5f) * num3 * (num4 * 2f);
Vector3 normalized = (this.endPoint - position).normalized;
float num6 = Mathf.Sin(expr_D8 * 180f * 0.0174532924f);
float num7 = Mathf.Cos(this.offsetMultiplier * 90f * 0.0174532924f);
Vector3 position2 = position + (this.endPoint - position) / (float)num * (float)i + (num7 * num5 * Vector2.Perpendicular(normalized) + this.offsetMultiplier * num6 * Vector2.down);
this.lr.SetPosition(i, position2);
}
num
position
num2
num3
i
expr_D8
num4
num5
normalized
num6
num7
position2
}

private void ClearLine()
{ 
this.lr.positionCount = 0;
}

public void Use(Vector3 attackDirection)
{ 
if (!this.readyToUse)
{ 
return;
}
this.ShootRope(attackDirection);
this.readyToUse = false;
}

public void StopUse()
{ 
if (this.joint == null)
{ 
return;
}
MonoBehaviour.print("STOPPING");
this.connectedTransform = null;
this.readyToUse = true;
}

private void ShootRope(Vector3 dir)
{ 
RaycastHit[] arg_2B_0 = Physics.RaycastAll(base.transform.position, dir, 10f, this.whatIsSickoMode);
GameObject gameObject = null;
RaycastHit raycastHit = default(RaycastHit);
RaycastHit[] array = arg_2B_0;
for (int i = 0; i < array.Length; i++)
{ 
RaycastHit raycastHit2 = array[i];
gameObject = raycastHit2.collider.gameObject;
if (gameObject.layer != LayerMask.NameToLayer("Player"))
{ 
raycastHit = raycastHit2;
break;
}
gameObject = null;
}
if (gameObject == null || raycastHit.collider == null)
{ 
return;
}
this.connectedTransform = raycastHit.collider.transform;
this.joint = base.gameObject.AddComponent<SpringJoint>();
UnityEngine.Object arg_E2_0 = gameObject.GetComponent<Rigidbody>();
this.offsetPoint = raycastHit.point - raycastHit.collider.transform.position;
this.joint.connectedBody = gameObject.GetComponent<Rigidbody>();
if (arg_E2_0 == null)
{ 
this.joint.connectedAnchor = raycastHit.point;
}
else
{ 
this.joint.connectedAnchor = this.offsetPoint;
}
this.joint.autoConfigureConnectedAnchor = false;
this.endPoint = base.transform.position;
this.offsetMultiplier = 1f;
arg_2B_0
gameObject
raycastHit
array
i
raycastHit2
arg_E2_0
}
}
