using System;
using UnityEngine;


public class Gun : MonoBehaviour
{ 
private Vector3 rotationVel;

private float speed = 8f;

private float posSpeed = 0.075f;

private float posOffset = 0.004f;

private Vector3 defaultPos;

private Vector3 posVel;

private Rigidbody rb;

private float rotationOffset;

private float rotationOffsetZ;

private float rotVelY;

private float rotVelZ;

private Vector3 prevRotation;

private Vector3 desiredBob;

private float xBob = 0.12f;

private float yBob = 0.08f;

private float zBob = 0.1f;

private float bobSpeed = 0.45f;

public static Gun Instance
{ 
get;
set;
}

private void Start()
{ 
Gun.Instance = this;
this.defaultPos = base.transform.localPosition;
this.rb = PlayerMovement.Instance.GetRb();
}

private void Update()
{ 
if (PlayerMovement.Instance && !PlayerMovement.Instance.HasGun())
{ 
return;
}
this.MoveGun();
Vector3 arg_FB_0 = PlayerMovement.Instance.GetGrapplePoint();
Quaternion b = Quaternion.LookRotation((PlayerMovement.Instance.GetGrapplePoint() - base.transform.position).normalized);
this.rotationOffset += Mathf.DeltaAngle(base.transform.parent.rotation.eulerAngles.y, this.prevRotation.y);
if (this.rotationOffset > 90f)
{ 
this.rotationOffset = 90f;
}
if (this.rotationOffset < -90f)
{ 
this.rotationOffset = -90f;
}
this.rotationOffset = Mathf.SmoothDamp(this.rotationOffset, 0f, ref this.rotVelY, 0.025f);
Vector3 b2 = new Vector3(0f, this.rotationOffset, this.rotationOffset);
if (arg_FB_0 == Vector3.zero)
{ 
b = Quaternion.Euler(base.transform.parent.rotation.eulerAngles - b2);
}
base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * this.speed);
Vector3 vector = PlayerMovement.Instance.FindVelRelativeToLook() * this.posOffset;
float num = PlayerMovement.Instance.GetFallSpeed() * this.posOffset;
if (num < -0.08f)
{ 
num = -0.08f;
}
Vector3 a = this.defaultPos - new Vector3(vector.x, num, vector.y);
base.transform.localPosition = Vector3.SmoothDamp(base.transform.localPosition, a + this.desiredBob, ref this.posVel, this.posSpeed);
this.prevRotation = base.transform.parent.rotation.eulerAngles;
arg_FB_0
b
b2
vector
num
a
}

private void MoveGun()
{ 
if (!this.rb || !PlayerMovement.Instance.grounded)
{ 
return;
}
if (Mathf.Abs(this.rb.velocity.magnitude) < 4f)
{ 
this.desiredBob = Vector3.zero;
return;
}
float x = Mathf.PingPong(Time.time * this.bobSpeed, this.xBob) - this.xBob / 2f;
float y = Mathf.PingPong(Time.time * this.bobSpeed, this.yBob) - this.yBob / 2f;
float z = Mathf.PingPong(Time.time * this.bobSpeed, this.zBob) - this.zBob / 2f;
this.desiredBob = new Vector3(x, y, z);
x
y
z
}

public void Shoot()
{ 
float recoil = PlayerMovement.Instance.GetRecoil();
Vector3 b = (Vector3.up / 4f + Vector3.back / 1.5f) * recoil;
base.transform.localPosition = base.transform.localPosition + b;
Quaternion localRotation = Quaternion.Euler(-60f * recoil, 0f, 0f);
base.transform.localRotation = localRotation;
recoil
b
localRotation
}
}
