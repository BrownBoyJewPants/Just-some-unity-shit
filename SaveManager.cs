using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;


public class SaveManager : MonoBehaviour
{ 
public PlayerSave state;

public static SaveManager Instance
{ 
get;
set;
}

private void Awake()
{ 
UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
SaveManager.Instance = this;
this.Load();
}

public void Save()
{ 
PlayerPrefs.SetString("save", this.Serialize<PlayerSave>(this.state));
}

public void Load()
{ 
if (PlayerPrefs.HasKey("save"))
{ 
this.state = this.Deserialize<PlayerSave>(PlayerPrefs.GetString("save"));
return;
}
this.NewSave();
}

public void NewSave()
{ 
this.state = new PlayerSave();
this.Save();
MonoBehaviour.print("Creating new save file");
}

public string Serialize<T>(T toSerialize)
{ 
XmlSerializer arg_1C_0 = new XmlSerializer(typeof(T));
StringWriter stringWriter = new StringWriter();
arg_1C_0.Serialize(stringWriter, toSerialize);
return stringWriter.ToString();
arg_1C_0
stringWriter
}

public T Deserialize<T>(string toDeserialize)
{ 
XmlSerializer arg_17_0 = new XmlSerializer(typeof(T));
StringReader textReader = new StringReader(toDeserialize);
return (T)((object)arg_17_0.Deserialize(textReader));
arg_17_0
textReader
}
}
