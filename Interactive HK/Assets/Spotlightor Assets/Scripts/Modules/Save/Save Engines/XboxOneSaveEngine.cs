using UnityEngine;
using System.Collections;

#if UNITY_XBOXONE
using Users;
using Storage;
#endif

public class XboxOneSaveEngine : DummySaveEngine
{
	#if UNITY_XBOXONE
	public class PluginsInitializer : SingletonMonoBehaviour<PluginsInitializer>
	{
		public delegate void FullyInitializedEventHandler (PluginsInitializer initializer);

		public event FullyInitializedEventHandler FullyInitialized;

		public void WaitForFullyInitialized ()
		{
			StopCoroutine ("DoWaitForFullyInitialized");
			StartCoroutine ("DoWaitForFullyInitialized");
		}

		private IEnumerator DoWaitForFullyInitialized ()
		{
			while (StorageManager.AmFullyInitialized () == false)
				yield return null;
			
			if (FullyInitialized != null)
				FullyInitialized (this);
		}
	}
	#endif

	public string containerName = "save";
	public int bufferSize = 1024 * 1024;
	[System.NonSerialized]
	private int userId = -1;

	#if UNITY_XBOXONE
	private ConnectedStorage storage;
	#endif

	public int UserId {
		get { return userId; }
		set { userId = value; }
	}

	#if UNITY_XBOXONE
	protected ConnectedStorage Storage {
		get { return storage; }
		set {
			if (value != storage) {
				if (storage != null)
					storage.OnUserSignedOut -= HandleUserSignedOut;
				
				storage = value;

				if (storage != null)
					storage.OnUserSignedOut += HandleUserSignedOut;
			}
		}
	}

	protected override void DoInstall ()
	{
		storage = null;

		if (StorageManager.AmFullyInitialized ()) {
			this.Log ("StorageManager has initialized");
			CreateConnectedStorage ();
		} else {
			this.Log ("Wait plugins fully initialized");
			PluginsInitializer.Instance.FullyInitialized += HandleFullyInitialized;
			DontDestroyOnLoad (PluginsInitializer.Instance.gameObject);
			PluginsInitializer.Instance.WaitForFullyInitialized ();
		}
	}

	void HandleFullyInitialized (PluginsInitializer initializer)
	{
		initializer.FullyInitialized -= HandleFullyInitialized;

		if (StorageManager.AmFullyInitialized ()) {
			this.Log ("StorageManager initialized successfully.");
			CreateConnectedStorage ();
		} else {
			this.Log ("Plugins failed to intialize. Installation failed.");
			OnInstallationCompleted (false);
		}
	}

	private void CreateConnectedStorage ()
	{
		bool hasValidUserId = true;
		if (userId != -1) {
			User user = UsersManager.FindUserById (userId);
			hasValidUserId = user != null && user.IsSignedIn;
		}

		if (hasValidUserId) {
			this.Log ("Create ConnectedStorage with userId {0}, time = {1}", userId, Time.time);
			ConnectedStorage.CreateAsync (userId, containerName, OnConnectedStorageCreated, 0, null);
		} else {
			this.Log ("Don't have a valid user id: {0}. Installation failed", userId);
			OnInstallationCompleted (false);
		}
	}

	private void OnConnectedStorageCreated (ConnectedStorage storage, CreateConnectedStorageOp op)
	{
		if (op.Success) {
			this.Storage = storage;

			if (this.Storage.IsMachineStorage)
				this.Log ("Machine storage created, time = {0}", Time.time);
			else
				this.Log ("User {0} Connected storage created, time = {1}", this.Storage.UserId, Time.time);

			this.Log ("Installation success.");
		} else {
			this.Log ("ConnectedStorage failed to create! RESULT: [0x" + op.Result.ToString ("X") + "]\n");
		}
		OnInstallationCompleted (op.Success);
	}

	void HandleUserSignedOut (uint userId, ConnectedStorage self)
	{
		this.Log ("Storage user signed out, userId = {0}. Uninstall to keep connected storage working correctly.", userId);
		this.Uninstall ();
	}

	protected override void DoUninstall ()
	{
		if (this.Storage != null)
			this.Storage.Dispose ();

		this.Storage = null;
		
		OnUninstallationCompleted (true);
	}

	public override void Save (string slotName, string saveDataString)
	{
		OnSaveStarted ();
		if (Storage != null) {
			DataMap dataMap = DataMap.Create ();
			byte[] data = System.Text.Encoding.UTF8.GetBytes (saveDataString);

			dataMap.AddOrReplaceBuffer (slotName, data);
			this.Log ("Submit Update Async with bytes array size: {0}, length: {1}", data.Length, dataMap.Length (slotName));
			Storage.SubmitUpdatesAsync (dataMap, null, (storage, op) => OnSubmitDone (storage, op, slotName));
		} else {
			SlotOperationResult result = new SlotOperationResult (slotName, false);
			result.message = "ConnectedStorage not created yet! Cannot save.";
			OnSaveCompleted (result);
		}
	}

	void OnSubmitDone (ContainerContext storage, SubmitDataMapUpdatesAsyncOp op, string slotName)
	{
		bool success = op.Success && op.Status == ConnectedStorageStatus.SUCCESS;
		SlotOperationResult result = new SlotOperationResult (storage.Name, success);
		if (!success) {
			result.message = op.Status.ToString ();
			this.Log ("Save submit failed! Result = {0}", result.message);
		} else
			this.Log ("Save submit success.");

		OnSaveCompleted (result);
	}

	public override void Load (string slotName)
	{
		if (Storage != null) {
			DataMap dataMap = DataMap.Create ();
			dataMap.AddNewBuffer (slotName, bufferSize);
			this.Log ("Read Async with bufferSize: {0}, length: {1}", bufferSize, dataMap.Length (slotName));
			Storage.ReadAsync (dataMap, (storage, op) => ReadAsyncReturns (storage, op, slotName));
		} else {
			SlotOperationResult result = new SlotOperationResult (slotName, false);
			result.message = "ConnectedStorage not created yet!";
			OnLoadCompleted (result, "{}");
		}
	}

	void ReadAsyncReturns (ContainerContext storage, ReadDataMapAsyncOp op, string slotName)
	{
		SlotOperationResult result = new SlotOperationResult (slotName, op.Success);
		string loadedString = "{}";
		if (op.Success) {
			byte[] buffer = op.Map.GetBuffer (slotName);
			loadedString = System.Text.Encoding.UTF8.GetString (buffer);
			result.message = "Success. Result = " + op.Result.ToString ();
			this.Log ("Read async success. loaded buffer length: {0}, string length: {1}", buffer.Length, loadedString.Length);
		} else {
			result.message = "Read async failed";
			this.Log ("Read async failed. Result = {0}", op.Result);
		}

		OnLoadCompleted (result, loadedString);
	}

	public override void Delete (string slotName)
	{
		throw new System.NotImplementedException ();
	}
	#endif
}
