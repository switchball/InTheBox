using UnityEngine;
using System.Collections;

#if UNITY_XBOXONE
using Users;
#endif

public class XboxUser
{
	#if UNITY_XBOXONE
	private User user;
	#endif

	public string DisplayName {
		get {
			string displayName = "Unkown";
			#if UNITY_XBOXONE
			if (user != null)
				displayName = user.GameDisplayName;
			#endif
			return displayName;
		}
	}

	public int Index {
		get {
			#if UNITY_XBOXONE
			return user != null ? user.Index : -1; 
			#else
			return -1;
			#endif
		}
	}

	public int Id {
		get { 
			#if UNITY_XBOXONE
			return user != null ? user.Id : -1; 
			#else
			return -1;
			#endif
		}
	}

	public string UID {
		get { 
			#if UNITY_XBOXONE
			return user != null ? user.UID : ""; 
			#else
			return "";
			#endif
		}
	}

	#if UNITY_XBOXONE
	public XboxUser (User user)
	{
		this.user = user;
	}
	#endif

	public XboxUser ()
	{
	}
}
