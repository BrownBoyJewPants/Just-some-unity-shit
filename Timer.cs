using System;
using TMPro;
using UnityEngine;


public class Timer : MonoBehaviour
{ 
private TextMeshProUGUI text;

private float timer;

private bool stop;

public static Timer Instance
{ 
get;
set;
}

private void Awake()
{ 
Timer.Instance = this;
this.text = base.GetComponent<TextMeshProUGUI>();
this.stop = false;
}

public void StartTimer()
{ 
this.stop = false;
this.timer = 0f;
}

private void Update()
{ 
if (!Game.Instance.playing || this.stop)
{ 
return;
}
this.timer += Time.deltaTime;
this.text.text = this.GetFormattedTime(this.timer);
}

public string GetFormattedTime(float f)
{ 
if (f == 0f)
{ 
return "nan";
}
string arg = Mathf.Floor(f / 60f).ToString("00");
string arg2 = Mathf.Floor(f % 60f).ToString("00");
string text = (f * 100f % 100f).ToString("00");
if (text.Equals("100"))
{ 
text = "99";
}
return string.Format("{0}:{1}:{2}", arg, arg2, text);
arg
arg2
text
}

public float GetTimer()
{ 
return this.timer;
}

private string StatusText(float f)
{ 
if (f < 2f)
{ 
return "very easy";
}
if (f < 4f)
{ 
return "easy";
}
if (f < 8f)
{ 
return "medium";
}
if (f < 12f)
{ 
return "hard";
}
if (f < 16f)
{ 
return "very hard";
}
if (f < 20f)
{ 
return "impossible";
}
if (f < 25f)
{ 
return "oh shit";
}
if (f < 30f)
{ 
return "very oh shit";
}
return "f";
}

public void Stop()
{ 
this.stop = true;
}

public int GetMinutes()
{ 
return (int)Mathf.Floor(this.timer / 60f);
}
}
