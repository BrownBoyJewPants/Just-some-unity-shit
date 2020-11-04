using System;
using UnityEngine;


public class ExplosiveBullet : MonoBehaviour
{ 
private Rigidbody rb;

private void Start()
{ 
this.rb = base.GetComponent<Rigidbody>();
UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.thumpAudio, base.transform.position, Quaternion.identity);
}

private void OnCollisionEnter(Collision other)
{ 
UnityEngine.Object.Destroy(base.gameObject);
UnityEngine.Object.Instantiate<GameObject>(PrefabManager.Instance.explosion, base.transform.position, Quaternion.identity);
}

private void Update()
{ 
this.rb.AddForce(Vector3.up * Time.deltaTime * 1000f);
}
}
