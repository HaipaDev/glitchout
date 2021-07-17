using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Encoders;
using BayatGames.SaveGameFree.Serializers;

public class SaveSerial : MonoBehaviour{
	public static SaveSerial instance;
	void Awake(){if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}}
	//void OnValidate(){Photon.Pun.PhotonNetwork.GameVersion=settingsData.gameVersion;}
	[SerializeField] string filename = "playerData";
	bool dataEncode=true;
	[SerializeField] string filenameSettings = "gameSettings.cfg";
	bool settingsEncode=false;
	//[HeaderAttribute("PlayerData")]
	
	#region //Settings Data
	public SettingsData settingsData=new SettingsData();
	[System.Serializable]public class SettingsData{
		public string gameVersion;
		public bool fullscreen;
		public bool pprocessing;
		//public bool scbuttons;
		//public int quality;
		public float masterVolume;
		public float soundVolume;
		public float musicVolume;
	}

	public void SaveSettings(){
		SaveGame.Encode = settingsEncode;
		SaveGame.Serializer = new SaveGameJsonSerializer();
		SaveGame.Save(filenameSettings, settingsData);
		Debug.Log("Settings saved");
	}
	public void LoadSettings(){
		if (File.Exists(Application.persistentDataPath + "/"+filenameSettings)){
			SettingsData data = new SettingsData();
			SaveGame.Encode = settingsEncode;
			SaveGame.Serializer = new SaveGameJsonSerializer();
			settingsData = SaveGame.Load<SettingsData>(filenameSettings);
			Debug.Log("Settings loaded");
		}
		else Debug.Log("Settings file not found in " + Application.persistentDataPath + "/" + filenameSettings);
	}
	public void ResetSettings(){
		settingsData=new SettingsData();
		GC.Collect();
		if (File.Exists(Application.persistentDataPath + "/"+filenameSettings)){
			File.Delete(Application.persistentDataPath + "/"+filenameSettings);
			Debug.Log("Settings Data deleted");
		}else Debug.Log("Settings file not found in "+Application.persistentDataPath+"/"+filenameSettings);
	}
	#endregion
}