using System;
using UnityEngine;


public abstract class Actor : MonoBehaviour
{ 
public Transform gunPosition;

public Transform orientation;

private float xRotation;

private Rigidbody rb;

private float accelerationSpeed = 4500f;

private float maxSpeed = 20f;

private bool crouching;

private bool jumping;

private bool wallRunning;

protected float x;

protected float y;

private Vector3 wallNormalVector = Vector3.up;

private bool grounded;

public Transform groundChecker;

public LayerMask whatIsGround;

private bool readyToJump;

private float jumpCooldown = 0.2f;

private float jumpForce = 500f;

private void Awake()
{ 
this.rb = base.GetComponent<Rigidbody>();
this.OnStart();
}

private void FixedUpdate()
{ 
this.Movement();
this.RotateBody();
}

private void LateUpdate()
{ 
this.Look();
}

private void Update()
{ 
this.Logic();
}

protected abstract void OnStart();

protected abstract void Logic();

protected abstract void RotateBody();

protected abstract void Look();

private void Movement()
{ 
this.grounded = (Physics.OverlapSphere(this.groundChecker.position, 0.1f, this.whatIsGround).Length != 0);
Vector2 vector = this.FindVelRelativeToLook();
float num = vector.x;
float num2 = vector.y;
this.CounterMovement(this.x, this.y, vector);
if (this.readyToJump && this.jumping)
{ 
this.Jump();
}
if (this.crouching && this.grounded && this.readyToJump)
{ 
this.rb.AddForce(Vector3.down * Time.deltaTime * 2000f);
return;
}
if (this.x > 0f && num > this.maxSpeed)
{ 
this.x = 0f;
}
if (this.x < 0f && num < -this.maxSpeed)
{ 
this.x = 0f;
}
if (this.y > 0f && num2 > this.maxSpeed)
{ 
this.y = 0f;
}
if (this.y < 0f && num2 < -this.maxSpeed)
{ 
this.y = 0f;
}
this.rb.AddForce(Time.deltaTime * this.y * this.accelerationSpeed * this.orientation.transform.forward);
this.rb.AddForce(Time.deltaTime * this.x * this.accelerationSpeed * this.orientation.transform.right);
vector
num
num2
}

private void Jump()
{ 
if (this.grounded || this.wallRunning)
{ 
Vector3 velocity = this.rb.velocity;
this.rb.velocity = new Vector3(velocity.x, 0f, velocity.z);
this.readyToJump = false;
this.rb.AddForce(Vector2.up * this.jumpForce * 1.5f);
this.rb.AddForce(this.wallNormalVector * this.jumpForce * 0.5f);
base.Invoke("ResetJump", this.jumpCooldown);
if (this.wallRunning)
{ 
this.wallRunning = false;
}
}
velocity
}

private void ResetJump()
{ 
this.readyToJump = true;
}

protected void CounterMovement(float x, float y, Vector2 mag)
{ 
if (!this.grounded || this.crouching)
{ 
return;
}
float num = 0.2f;
if (x == 0f || (mag.x < 0f && x > 0f) || (mag.x > 0f && x < 0f))
{ 
this.rb.AddForce(this.accelerationSpeed * num * Time.deltaTime * -mag.x * this.orientation.transform.right);
}
if (y == 0f || (mag.y < 0f && y > 0f) || (mag.y > 0f && y < 0f))
{ 
this.rb.AddForce(this.accelerationSpeed * num * Time.deltaTime * -mag.y * this.orientation.transform.forward);
}
if (Mathf.Sqrt(Mathf.Pow(this.rb.velocity.x, 2f) + Mathf.Pow(this.rb.velocity.z, 2f)) > 20f)
{ 
float num2 = this.rb.velocity.y;
Vector3 vector = this.rb.velocity.normalized * 20f;
this.rb.velocity = new Vector3(vector.x, num2, vector.z);
}
num
num2
vector
}

protected Vector2 FindVelRelativeToLook()
{ 
float arg_3A_0 = this.orientation.transform.eulerAngles.y;
Vector3 velocity = this.rb.velocity;
float target = Mathf.Atan2(velocity.x, velocity.z) * 57.29578f;
float num = Mathf.DeltaAngle(arg_3A_0, target);
float num2 = 90f - num;
float expr_5C = this.rb.velocity.magnitude;
float num3 = expr_5C * Mathf.Cos(num * 0.0174532924f);
return new Vector2(expr_5C * Mathf.Cos(num2 * 0.0174532924f), num3);
arg_3A_0
velocity
target
num
num2
expr_5C
num3
}
}
