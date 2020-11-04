using System;
using UnityEngine;


public class PrefabManager : MonoBehaviour
{ 
public GameObject blood;

public GameObject bulletDestroy;

public GameObject muzzle;

public GameObject explosion;

public GameObject bulletHitAudio;

public GameObject enemyHitAudio;

public GameObject gunShotAudio;

public GameObject objectImpactAudio;

public GameObject thumpAudio;

public GameObject destructionAudio;

public static PrefabManager Instance
{ 
get;
private set;
}

private void Awake()
{ 
PrefabManager.Instance = this;
}
}
