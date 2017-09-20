using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Runtime.InteropServices;

#if UNITY_XBOXONE
using Users;
#endif
public class XboxUserGamerPictureDisplayer : ContentDisplayerChildBehavior<XboxUser>
{
	public Texture2D defaultPicture;
	#if UNITY_XBOXONE
	public Picture.Size pictureSize = Picture.Size.Small;
	#endif
	private Texture2D gamerPicture;

	public override void UpdateDisplay (XboxUser xboxUser)
	{
		GetComponent<TextureDisplayer> ().Display (defaultPicture);

		#if UNITY_XBOXONE
		int pictureDimension = Picture.GetDimension (pictureSize);
		gamerPicture = new Texture2D (pictureDimension, pictureDimension, TextureFormat.BGRA32, false);

		UsersManager.GetGamerPictureAsync (xboxUser.Index, (int)pictureSize, gamerPicture.GetNativeTexturePtr (), OnAsyncImageLoaded);
		#endif
	}
	#if UNITY_XBOXONE
	void OnAsyncImageLoaded (uint hresult, IntPtr texture, int userId)
	{
		this.Log ("Gamer Picture for User {0} loaded", userId);
		GetComponent<TextureDisplayer> ().Display (gamerPicture);
	}
	#endif
}
