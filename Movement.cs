using Audio;
using EZCameraShake;
using System;
using UnityEngine;


public class Movement : MonoBehaviour
{ 
public GameObject spawnWeapon;

private float sensitivity = 50f;

private float sensMultiplier = 1f;

private bool dead;

public PhysicMaterial deadMat;

public Transform playerCam;

public Transform orientation;

public Transform gun;

private float xRotation;

public Rigidbody rb;

private float moveSpeed = 4500f;

private float walkSpeed = 20f;

private float runSpeed = 10f;

public bool grounded;

public Transform groundChecker;

public LayerMask whatIsGround;

private bool readyToJump;

private float jumpCooldown = 0.2f;

private float jumpForce = 550f;

private float x;

private float y;

private bool jumping;

private bool sprinting;

private bool crouching;

public LineRenderer lr;

private Vector3 grapplePoint;

private SpringJoint joint;

private Vector3 normalVector;

private Vector3 wallNormalVector;

private bool wallRunning;

private Vector3 wallRunPos;

private DetectWeapons detectWeapons;

public ParticleSystem ps;

private ParticleSystem.EmissionModule psEmission;

private Collider playerCollider;

public bool paused;

public LayerMask whatIsGrabbable;

private Rigidbody objectGrabbing;

private Vector3 previousLookdir;

private Vector3 grabPoint;

private float dragForce = 700000f;

private SpringJoint grabJoint;

private LineRenderer grabLr;

private Vector3 myGrabPoint;

private Vector3 myHandPoint;

private Vector3 endPoint;

private Vector3 grappleVel;

private float offsetMultiplier;

private float offsetVel;

private float distance;

private float actualWallRotation;

private float wallRotationVel;

private float desiredX;

private float wallRunRotation;

private bool airborne;

private bool touchingGround;

public LayerMask whatIsHittable;

private float desiredTimeScale = 1f;

private float timeScaleVel;

public static Movement Instance
{ 
get;
private set;
}

private void Awake()
{ 
Movement.Instance = this;
this.rb = base.GetComponent<Rigidbody>();
MonoBehaviour.print("rb: " + this.rb);
}

private void Start()
{ 
this.psEmission = this.ps.emission;
this.playerCollider = base.GetComponent<Collider>();
this.detectWeapons = (DetectWeapons)base.GetComponentInChildren(typeof(DetectWeapons));
Cursor.lockState = CursorLockMode.Locked;
Cursor.visible = false;
this.readyToJump = true;
this.wallNormalVector = Vector3.up;
this.CameraShake();
if (this.spawnWeapon != null)
{ 
GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.spawnWeapon, base.transform.position, Quaternion.identity);
this.detectWeapons.ForcePickup(gameObject);
}
if (GameState.Instance)
{ 
this.sensMultiplier = GameState.Instance.GetSensitivity();
}
gameObject
}

private void LateUpdate()
{ 
if (this.dead || this.paused)
{ 
return;
}
this.DrawGrapple();
this.DrawGrabbing();
this.WallRunning();
}

private void FixedUpdate()
{ 
if (this.dead || Game.Instance.done || this.paused)
{ 
return;
}
this.Move();
}

private void Update()
{ 
this.MyInput();
if (this.dead || Game.Instance.done || this.paused)
{ 
return;
}
this.Look();
this.DrawGrabbing();
this.UpdateTimescale();
if (base.transform.position.y < -200f)
{ 
this.KillPlayer();
}
}

private void MyInput()
{ 
if (this.dead || Game.Instance.done)
{ 
return;
}
this.x = Input.GetAxisRaw("Horizontal");
this.y = Input.GetAxisRaw("Vertical");
this.jumping = Input.GetButton("Jump");
this.sprinting = Input.GetKey(KeyCode.LeftShift);
this.crouching = Input.GetKey(KeyCode.LeftControl);
if (Input.GetKeyDown(KeyCode.LeftControl))
{ 
this.StartCrouch();
}
if (Input.GetKeyUp(KeyCode.LeftControl))
{ 
this.StopCrouch();
}
if (Input.GetKey(KeyCode.Mouse0))
{ 
if (this.detectWeapons.HasGun())
{ 
this.detectWeapons.Shoot(this.HitPoint());
}
else
{ 
this.GrabObject();
}
}
if (Input.GetKeyUp(KeyCode.Mouse0))
{ 
this.detectWeapons.StopUse();
if (this.objectGrabbing)
{ 
this.StopGrab();
}
}
if (Input.GetKeyDown(KeyCode.E))
{ 
this.detectWeapons.Pickup();
}
if (Input.GetKeyDown(KeyCode.Q))
{ 
this.detectWeapons.Throw((this.HitPoint() - this.detectWeapons.weaponPos.position).normalized);
}
if (Input.GetKeyDown(KeyCode.Escape))
{ 
this.Pause();
}
}

private void Pause()
{ 
if (this.dead)
{ 
return;
}
if (this.paused)
{ 
Time.timeScale = 1f;
UIManger.Instance.DeadUI(false);
Cursor.lockState = CursorLockMode.Locked;
Cursor.visible = false;
this.paused = false;
return;
}
this.paused = true;
Time.timeScale = 0f;
UIManger.Instance.DeadUI(true);
Cursor.lockState = CursorLockMode.None;
Cursor.visible = true;
}

private void UpdateTimescale()
{ 
if (Game.Instance.done || this.paused || this.dead)
{ 
return;
}
Time.timeScale = Mathf.SmoothDamp(Time.timeScale, this.desiredTimeScale, ref this.timeScaleVel, 0.15f);
}

private void GrabObject()
{ 
if (this.objectGrabbing == null)
{ 
this.StartGrab();
return;
}
this.HoldGrab();
}

private void DrawGrabbing()
{ 
if (!this.objectGrabbing)
{ 
return;
}
this.myGrabPoint = Vector3.Lerp(this.myGrabPoint, this.objectGrabbing.position, Time.deltaTime * 45f);
this.myHandPoint = Vector3.Lerp(this.myHandPoint, this.grabJoint.connectedAnchor, Time.deltaTime * 45f);
this.grabLr.SetPosition(0, this.myGrabPoint);
this.grabLr.SetPosition(1, this.myHandPoint);
}

private void StartGrab()
{ 
RaycastHit[] array = Physics.RaycastAll(this.playerCam.transform.position, this.playerCam.transform.forward, 8f, this.whatIsGrabbable);
if (array.Length < 1)
{ 
return;
}
for (int i = 0; i < array.Length; i++)
{ 
MonoBehaviour.print("testing on: " + array[i].collider.gameObject.layer);
if (array[i].transform.GetComponent<Rigidbody>())
{ 
this.objectGrabbing = array[i].transform.GetComponent<Rigidbody>();
this.grabPoint = array[i].point;
this.grabJoint = this.objectGrabbing.gameObject.AddComponent<SpringJoint>();
this.grabJoint.autoConfigureConnectedAnchor = false;
this.grabJoint.minDistance = 0f;
this.grabJoint.maxDistance = 0f;
this.grabJoint.damper = 4f;
this.grabJoint.spring = 40f;
this.grabJoint.massScale = 5f;
this.objectGrabbing.angularDrag = 5f;
this.objectGrabbing.drag = 1f;
this.previousLookdir = this.playerCam.transform.forward;
this.grabLr = this.objectGrabbing.gameObject.AddComponent<LineRenderer>();
this.grabLr.positionCount = 2;
this.grabLr.startWidth = 0.05f;
this.grabLr.material = new Material(Shader.Find("Sprites/Default"));
this.grabLr.numCapVertices = 10;
this.grabLr.numCornerVertices = 10;
return;
}
}
array
i
}

private void HoldGrab()
{ 
this.grabJoint.connectedAnchor = this.playerCam.transform.position + this.playerCam.transform.forward * 5.5f;
this.grabLr.startWidth = 0f;
this.grabLr.endWidth = 0.0075f * this.objectGrabbing.velocity.magnitude;
this.previousLookdir = this.playerCam.transform.forward;
}

private void StopGrab()
{ 
UnityEngine.Object.Destroy(this.grabJoint);
UnityEngine.Object.Destroy(this.grabLr);
this.objectGrabbing.angularDrag = 0.05f;
this.objectGrabbing.drag = 0f;
this.objectGrabbing = null;
}

private void StartCrouch()
{ 
float d = 400f;
base.transform.localScale = new Vector3(1f, 0.5f, 1f);
base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 0.5f, base.transform.position.z);
if (this.rb.velocity.magnitude > 0.1f && this.grounded)
{ 
this.rb.AddForce(this.orientation.transform.forward * d);
AudioManager.Instance.Play("StartSlide");
AudioManager.Instance.Play("Slide");
}
d
}

private void StopCrouch()
{ 
base.transform.localScale = new Vector3(1f, 1.5f, 1f);
base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.5f, base.transform.position.z);
}

private void StopGrapple()
{ 
UnityEngine.Object.Destroy(this.joint);
this.grapplePoint = Vector3.zero;
}

private void StartGrapple()
{ 
RaycastHit[] array = Physics.RaycastAll(this.playerCam.transform.position, this.playerCam.transform.forward, 70f, this.whatIsGround);
if (array.Length < 1)
{ 
return;
}
this.grapplePoint = array[0].point;
this.joint = base.gameObject.AddComponent<SpringJoint>();
this.joint.autoConfigureConnectedAnchor = false;
this.joint.connectedAnchor = this.grapplePoint;
this.joint.spring = 6.5f;
this.joint.damper = 2f;
this.joint.maxDistance = Vector2.Distance(this.grapplePoint, base.transform.position) * 0.8f;
this.joint.minDistance = Vector2.Distance(this.grapplePoint, base.transform.position) * 0.25f;
this.joint.spring = 4.5f;
this.joint.damper = 7f;
this.joint.massScale = 4.5f;
this.endPoint = this.gun.transform.GetChild(0).position;
this.offsetMultiplier = 2f;
array
}

private void DrawGrapple()
{ 
if (this.grapplePoint == Vector3.zero || this.joint == null)
{ 
this.lr.positionCount = 0;
return;
}
this.lr.positionCount = 2;
this.endPoint = Vector3.Lerp(this.endPoint, this.grapplePoint, Time.deltaTime * 15f);
this.offsetMultiplier = Mathf.SmoothDamp(this.offsetMultiplier, 0f, ref this.offsetVel, 0.1f);
int num = 100;
this.lr.positionCount = num;
Vector3 position = this.gun.transform.GetChild(0).position;
float arg_CF_0 = Vector3.Distance(this.endPoint, position);
this.lr.SetPosition(0, position);
this.lr.SetPosition(num - 1, this.endPoint);
float num2 = arg_CF_0;
float num3 = 1f;
for (int i = 1; i < num - 1; i++)
{ 
float expr_E4 = (float)i / (float)num;
float num4 = expr_E4 * this.offsetMultiplier;
float num5 = (Mathf.Sin(num4 * num2) - 0.5f) * num3 * (num4 * 2f);
Vector3 normalized = (this.endPoint - position).normalized;
float num6 = Mathf.Sin(expr_E4 * 180f * 0.0174532924f);
float num7 = Mathf.Cos(this.offsetMultiplier * 90f * 0.0174532924f);
Vector3 position2 = position + (this.endPoint - position) / (float)num * (float)i + (num7 * num5 * Vector2.Perpendicular(normalized) + this.offsetMultiplier * num6 * Vector3.down);
this.lr.SetPosition(i, position2);
}
num
position
arg_CF_0
num2
num3
i
expr_E4
num4
num5
normalized
num6
num7
position2
}

private void WallRunning()
{ 
if (this.wallRunning)
{ 
this.rb.AddForce(-this.wallNormalVector * Time.deltaTime * this.moveSpeed);
}
}

private void FootSteps()
{ 
if (this.crouching || this.dead)
{ 
return;
}
if (this.grounded || this.wallRunning)
{ 
float num = 1.2f;
float num2 = this.rb.velocity.magnitude;
if (num2 > 20f)
{ 
num2 = 20f;
}
this.distance += num2;
if (this.distance > 300f / num)
{ 
AudioManager.Instance.PlayFootStep();
this.distance = 0f;
}
}
num
num2
}

private void Move()
{ 
if (this.dead)
{ 
return;
}
this.grounded = (Physics.OverlapSphere(this.groundChecker.position, 0.1f, this.whatIsGround).Length != 0);
if (!this.touchingGround)
{ 
this.grounded = false;
}
Vector2 vector = this.FindVelRelativeToLook();
float num = vector.x;
float num2 = vector.y;
this.FootSteps();
this.CounterMovement(this.x, this.y, vector);
if (this.readyToJump && this.jumping)
{ 
this.Jump();
}
float num3 = this.walkSpeed;
if (this.sprinting)
{ 
num3 = this.runSpeed;
}
if (this.crouching && this.grounded && this.readyToJump)
{ 
this.rb.AddForce(Vector3.down * Time.deltaTime * 2000f);
return;
}
if (this.x > 0f && num > num3)
{ 
this.x = 0f;
}
if (this.x < 0f && num < -num3)
{ 
this.x = 0f;
}
if (this.y > 0f && num2 > num3)
{ 
this.y = 0f;
}
if (this.y < 0f && num2 < -num3)
{ 
this.y = 0f;
}
float d = 1f;
float d2 = 1f;
if (!this.grounded)
{ 
d = 0.5f;
}
if (this.grounded && this.crouching)
{ 
d2 = 0f;
}
this.rb.AddForce(this.orientation.transform.forward * this.y * this.moveSpeed * Time.deltaTime * d * d2);
this.rb.AddForce(this.orientation.transform.right * this.x * this.moveSpeed * Time.deltaTime * d);
this.SpeedLines();
vector
num
num2
num3
d
d2
}

private void SpeedLines()
{ 
float num = Vector3.Angle(this.rb.velocity, this.playerCam.transform.forward) * 0.15f;
if (num < 1f)
{ 
num = 1f;
}
float rateOverTimeMultiplier = this.rb.velocity.magnitude / num;
if (this.grounded && !this.wallRunning)
{ 
rateOverTimeMultiplier = 0f;
}
this.psEmission.rateOverTimeMultiplier = rateOverTimeMultiplier;
num
rateOverTimeMultiplier
}

private void CameraShake()
{ 
float num = this.rb.velocity.magnitude / 9f;
CameraShaker.Instance.ShakeOnce(num, 0.1f * num, 0.25f, 0.2f);
base.Invoke("CameraShake", 0.2f);
num
}

private void ResetJump()
{ 
this.readyToJump = true;
MonoBehaviour.print("reset");
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
if (this.wallRunning)
{ 
this.rb.AddForce(this.wallNormalVector * this.jumpForce * 1.5f);
}
base.Invoke("ResetJump", this.jumpCooldown);
if (this.wallRunning)
{ 
this.wallRunning = false;
}
AudioManager.Instance.PlayJump();
}
velocity
}

private void Look()
{ 
float num = Input.GetAxis("Mouse X") * this.sensitivity * Time.fixedDeltaTime * this.sensMultiplier;
float num2 = Input.GetAxis("Mouse Y") * this.sensitivity * Time.fixedDeltaTime * this.sensMultiplier;
Vector3 eulerAngles = this.playerCam.transform.localRotation.eulerAngles;
this.desiredX = eulerAngles.y + num;
this.xRotation -= num2;
this.xRotation = Mathf.Clamp(this.xRotation, -90f, 90f);
this.FindWallRunRotation();
this.actualWallRotation = Mathf.SmoothDamp(this.actualWallRotation, this.wallRunRotation, ref this.wallRotationVel, 0.2f);
this.playerCam.transform.localRotation = Quaternion.Euler(this.xRotation, this.desiredX, this.actualWallRotation);
this.orientation.transform.localRotation = Quaternion.Euler(0f, this.desiredX, 0f);
num
num2
eulerAngles
}

private void FindWallRunRotation()
{ 
if (!this.wallRunning)
{ 
this.wallRunRotation = 0f;
return;
}
Vector3 arg_40_0 = new Vector3(0f, this.playerCam.transform.rotation.y, 0f).normalized;
new Vector3(0f, 0f, 1f);
float arg_131_0 = this.playerCam.transform.rotation.eulerAngles.y;
if (Math.Abs(this.wallNormalVector.x - 1f) >= 0.1f)
{ 
if (Math.Abs(this.wallNormalVector.x - -1f) >= 0.1f)
{ 
if (Math.Abs(this.wallNormalVector.z - 1f) >= 0.1f)
{ 
if (Math.Abs(this.wallNormalVector.z - -1f) < 0.1f)
{ 
}
}
}
}
float target = Vector3.SignedAngle(new Vector3(0f, 0f, 1f), this.wallNormalVector, Vector3.up);
float num = Mathf.DeltaAngle(arg_131_0, target);
this.wallRunRotation = -(num / 90f) * 15f;
arg_40_0
arg_131_0
target
num
}

private void CounterMovement(float x, float y, Vector2 mag)
{ 
if (!this.grounded)
{ 
return;
}
float d = 0.2f;
if (this.crouching)
{ 
this.rb.AddForce(this.moveSpeed * Time.deltaTime * -this.rb.velocity.normalized * 0.045f);
return;
}
if (Math.Abs(x) < 0.05f || (mag.x < 0f && x > 0f) || (mag.x > 0f && x < 0f))
{ 
this.rb.AddForce(this.moveSpeed * this.orientation.transform.right * Time.deltaTime * -mag.x * d);
}
if (Math.Abs(y) < 0.05f || (mag.y < 0f && y > 0f) || (mag.y > 0f && y < 0f))
{ 
this.rb.AddForce(this.moveSpeed * this.orientation.transform.forward * Time.deltaTime * -mag.y * d);
}
if (Mathf.Sqrt(Mathf.Pow(this.rb.velocity.x, 2f) + Mathf.Pow(this.rb.velocity.z, 2f)) > 20f)
{ 
float num = this.rb.velocity.y;
Vector3 vector = this.rb.velocity.normalized * 20f;
this.rb.velocity = new Vector3(vector.x, num, vector.z);
}
d
num
vector
}

public Vector2 FindVelRelativeToLook()
{ 
float arg_42_0 = this.orientation.transform.eulerAngles.y;
float target = Mathf.Atan2(this.rb.velocity.x, this.rb.velocity.z) * 57.29578f;
float num = Mathf.DeltaAngle(arg_42_0, target);
float num2 = 90f - num;
float expr_64 = this.rb.velocity.magnitude;
float num3 = expr_64 * Mathf.Cos(num * 0.0174532924f);
return new Vector2(expr_64 * Mathf.Cos(num2 * 0.0174532924f), num3);
arg_42_0
target
num
num2
expr_64
num3
}

private void OnCollisionEnter(Collision other)
{ 
int layer = other.gameObject.layer;
if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
{ 
CameraShaker.Instance.ShakeOnce(5.5f * GameState.Instance.cameraShake, 1.2f, 0.2f, 0.3f);
if (this.wallRunning && other.contacts[0].normal.y == -1f)
{ 
MonoBehaviour.print("ROOF");
return;
}
this.wallNormalVector = other.contacts[0].normal;
MonoBehaviour.print("nv: " + this.wallNormalVector);
AudioManager.Instance.PlayLanding();
if (Math.Abs(this.wallNormalVector.y) < 0.1f)
{ 
this.StartWallRun();
}
this.airborne = false;
}
if (layer == LayerMask.NameToLayer("Enemy"))
{ 
if (this.grounded && !this.crouching)
{ 
return;
}
if (this.rb.velocity.magnitude < 3f)
{ 
return;
}
Enemy enemy = (Enemy)other.transform.root.GetComponent(typeof(Enemy));
if (!enemy)
{ 
return;
}
if (enemy.IsDead())
{ 
return;
}
UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.enemyHitAudio, other.contacts[0].point, Quaternion.identity);
RagdollController ragdollController = (RagdollController)other.transform.root.GetComponent(typeof(RagdollController));
if (this.grounded && this.crouching)
{ 
ragdollController.MakeRagdoll(this.rb.velocity * 1.2f * 34f);
}
else
{ 
ragdollController.MakeRagdoll(this.rb.velocity.normalized * 250f);
}
this.rb.AddForce(this.rb.velocity.normalized * 2f, ForceMode.Impulse);
enemy.DropGun(this.rb.velocity.normalized * 2f);
}
layer
enemy
ragdollController
}

private void StartWallRun()
{ 
if (this.wallRunning)
{ 
MonoBehaviour.print("stopping since wallrunning");
return;
}
if (this.touchingGround)
{ 
MonoBehaviour.print("stopping since grounded");
return;
}
MonoBehaviour.print("got through");
float d = 20f;
this.wallRunning = true;
this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
this.rb.AddForce(Vector3.up * d, ForceMode.Impulse);
d
}

private void OnCollisionExit(Collision other)
{ 
if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
{ 
if (Math.Abs(this.wallNormalVector.y) < 0.1f)
{ 
MonoBehaviour.print("oof");
this.wallRunning = false;
this.wallNormalVector = Vector3.up;
}
else
{ 
this.touchingGround = false;
}
this.airborne = true;
}
if (other.gameObject.layer == LayerMask.NameToLayer("Object"))
{ 
this.touchingGround = false;
}
}

private void OnCollisionStay(Collision other)
{ 
if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && Math.Abs(other.contacts[0].normal.y) > 0.1f)
{ 
this.touchingGround = true;
this.wallRunning = false;
}
if (other.gameObject.layer == LayerMask.NameToLayer("Object"))
{ 
this.touchingGround = true;
}
}

public Vector3 GetVelocity()
{ 
return this.rb.velocity;
}

public float GetFallSpeed()
{ 
return this.rb.velocity.y;
}

public Vector3 GetGrapplePoint()
{ 
return this.detectWeapons.GetGrapplerPoint();
}

public Collider GetPlayerCollider()
{ 
return this.playerCollider;
}

public Transform GetPlayerCamTransform()
{ 
return this.playerCam.transform;
}

public Vector3 HitPoint()
{ 
RaycastHit[] array = Physics.RaycastAll(this.playerCam.transform.position, this.playerCam.transform.forward, (float)this.whatIsHittable);
if (array.Length < 1)
{ 
return this.playerCam.transform.position + this.playerCam.transform.forward * 100f;
}
if (array.Length > 1)
{ 
for (int i = 0; i < array.Length; i++)
{ 
if (array[i].transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
{ 
return array[i].point;
}
}
}
return array[0].point;
array
i
}

public float GetRecoil()
{ 
return this.detectWeapons.GetRecoil();
}

public void KillPlayer()
{ 
if (Game.Instance.done)
{ 
return;
}
CameraShaker.Instance.ShakeOnce(3f * GameState.Instance.cameraShake, 2f, 0.1f, 0.6f);
Cursor.lockState = CursorLockMode.None;
Cursor.visible = true;
UIManger.Instance.DeadUI(true);
Timer.Instance.Stop();
this.dead = true;
this.rb.freezeRotation = false;
this.playerCollider.material = this.deadMat;
this.detectWeapons.Throw(Vector3.zero);
this.paused = false;
this.ResetSlowmo();
}

public void Respawn()
{ 
this.detectWeapons.StopUse();
}

public void Slowmo(float timescale, float length)
{ 
if (!GameState.Instance.shake)
{ 
return;
}
base.CancelInvoke("Slowmo");
this.desiredTimeScale = timescale;
base.Invoke("ResetSlowmo", length);
AudioManager.Instance.Play("SlowmoStart");
}

private void ResetSlowmo()
{ 
this.desiredTimeScale = 1f;
AudioManager.Instance.Play("SlowmoEnd");
}

public bool IsCrouching()
{ 
return this.crouching;
}

public bool HasGun()
{ 
return this.detectWeapons.HasGun();
}

public bool IsDead()
{ 
return this.dead;
}

public Rigidbody GetRb()
{ 
return this.rb;
}
}
