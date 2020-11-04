using System;


public class PlayerSave
{ 
public float[] times = new float[100];

public bool cameraShake
{ 
get;
set;
}

public bool motionBlur
{ 
get;
set;
}

public bool slowmo
{ 
get;
set;
}

public bool graphics
{ 
get;
set;
}

public bool muted
{ 
get;
set;
}

public float sensitivity
{ 
get;
set;
}

public float fov
{ 
get;
set;
}

public float volume
{ 
get;
set;
}

public float music
{ 
get;
set;
}

public PlayerSave()
{ 
this.<cameraShake>k__BackingField = true;
this.<motionBlur>k__BackingField = true;
this.<slowmo>k__BackingField = true;
this.<graphics>k__BackingField = true;
this.<sensitivity>k__BackingField = 1f;
this.<fov>k__BackingField = 80f;
this.<volume>k__BackingField = 0.75f;
this.<music>k__BackingField = 0.5f;
base..ctor();
}
}
