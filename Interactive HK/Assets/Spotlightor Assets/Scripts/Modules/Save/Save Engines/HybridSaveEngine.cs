using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HybridSaveEngine : SaveEngine
{
	[System.Serializable]
	public class SaveEngineSetting
	{
		public SaveEngine engine;
		public List<RuntimePlatform> platforms;
		public bool autoInstall = true;
		public bool autoLoadAfterInstall = true;
	}

	private static HybridSaveEngine instance;

	public static HybridSaveEngine Instance {
		get {
			if (instance == null) {
				instance = Resources.Load<HybridSaveEngine> ("Save/hybrid_save_engine");
				if (instance == null) {
					instance = ScriptableObject.CreateInstance<HybridSaveEngine> ();
					Debug.Log ("Cannot load hybrid save engine, a default hybrid save engine created");
				} else
					Debug.Log ("Hybrid save engine loaded");
			}
			return instance;
		}
	}

	[SerializeField]
	private List<SaveEngineSetting> engineSettings;
	[System.NonSerialized]
	private SaveEngine activeSaveEngine = null;

	//TODO: Not save to set active save engine directly outside, install & uninstall not triggered
	public SaveEngine ActiveSaveEngine {
		set {
			if (value != null && value != activeSaveEngine) {
				if (activeSaveEngine != null) {
					activeSaveEngine.StateChanged -= HandleStateChanged;
					activeSaveEngine.SaveCompleted -= HandleSaveCompleted;
					activeSaveEngine.LoadCompleted -= HandleLoadCompleted;
					activeSaveEngine.DeleteCompleted -= HandleDeleteCompleted;

					if (activeSaveEngine.State == StateTypes.Installed) {
						this.Log ("Change activeSaveEngine while old engine not uninstalled. Uninstall old engine: {0}", activeSaveEngine.name);
						activeSaveEngine.Uninstall ();
					}
				}

				activeSaveEngine = value;
				this.State = activeSaveEngine.State;

				activeSaveEngine.StateChanged += HandleStateChanged;
				activeSaveEngine.SaveCompleted += HandleSaveCompleted;
				activeSaveEngine.LoadCompleted += HandleLoadCompleted;
				activeSaveEngine.DeleteCompleted += HandleDeleteCompleted;

				this.Log ("Active engine changed to {0}", activeSaveEngine);

				SaveEngineSetting engineSetting = engineSettings.Find (es => es.engine == activeSaveEngine);
				if (engineSetting != null) {
					if (engineSetting.autoInstall && activeSaveEngine.State != StateTypes.Installed) {
						this.Log ("Auto install SaveEngine: {0}", activeSaveEngine.name);
						activeSaveEngine.Install ();
					}
					if (engineSetting.autoLoadAfterInstall) {
						this.Log ("SaveEngine {0} Auto Load", activeSaveEngine.name);
						SaveSlotsStorage.ActiveSaveSlot.Load ();
					}
				}
			}
		}
		get {
			if (activeSaveEngine == null) {
				ActiveSaveEngine = AutoSelectSaveEngine ();
				this.Log ("Active engine auto selected to {0}", activeSaveEngine);
			} 
			
			return activeSaveEngine;
		}
	}

	[RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void Initialize ()
	{
		Debug.Log ("Initialize HybridSaveEngine");
		Debug.Log ("Active Save Engine = " + HybridSaveEngine.Instance.ActiveSaveEngine.name);
	}

	private SaveEngine AutoSelectSaveEngine ()
	{
		SaveEngine engine = null;
        if (engineSettings == null)
            engineSettings = new List<SaveEngineSetting>();
        SaveEngineSetting settingByPlatform = engineSettings.Find (e => e.platforms.Contains (Application.platform));
        if (settingByPlatform != null)
			engine = settingByPlatform.engine;

		if (engine == null && engineSettings.Count > 0)
			engine = engineSettings [0].engine;

		if (engine == null) {
			engine = ScriptableObject.CreateInstance<PlayerPrefsSaveEngine> ();
			SaveEngineSetting setting = new SaveEngineSetting ();
			setting.engine = engine;
			setting.autoInstall = true;
			setting.autoLoadAfterInstall = true;
			engineSettings.Add (setting);
		}

		return engine;
	}

	void HandleStateChanged (SaveEngine engine, StateTypes currentState, StateTypes prevState)
	{
		if (prevState == StateTypes.Installing) {
			OnInstallationCompleted (currentState == StateTypes.Installed);
		} else if (prevState == StateTypes.Uninstalling) {
			OnUninstallationCompleted (currentState == StateTypes.NotInstalled);
		}
		this.State = engine.State;
	}

	void HandleLoadCompleted (SaveEngine engine, SlotOperationResult result, string loadedDataString)
	{
		OnLoadCompleted (result, loadedDataString);
	}

	void HandleSaveCompleted (SaveEngine engine, SlotOperationResult result)
	{
		OnSaveCompleted (result);
	}

	void HandleDeleteCompleted (SaveEngine engine, SlotOperationResult result)
	{
		OnDeleteCompleted (result);
	}

	public override void Save (string slotName, string saveDataString)
	{
		OnSaveStarted ();
		ActiveSaveEngine.Save (slotName, saveDataString);
	}

	public override void Load (string slotName)
	{
		ActiveSaveEngine.Load (slotName);
	}

	public override void Delete (string slotName)
	{
		ActiveSaveEngine.Delete (slotName);
	}

	protected override void DoInstall ()
	{
		ActiveSaveEngine.Install ();
	}

	protected override void DoUninstall ()
	{
		ActiveSaveEngine.Uninstall ();
	}
}
