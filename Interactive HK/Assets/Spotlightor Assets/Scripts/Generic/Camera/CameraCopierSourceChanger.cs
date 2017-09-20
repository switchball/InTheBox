using UnityEngine;
using System.Collections;

public class CameraCopierSourceChanger : FunctionalMonoBehaviour
{
	public CameraCopier cameraPropertiesCopier;
	public CameraCopier.SmoothChangeCopySourceSetting sourceSetting;
	public bool useSmoothChange = true;

	protected override void OnBecameFunctional (bool forTheFirstTime)
	{
		if (useSmoothChange)
			cameraPropertiesCopier.SmoothChangeCopySource (sourceSetting);
		else
			cameraPropertiesCopier.SourceCamera = sourceSetting.sourceCamera;
	}

	protected override void OnBecameUnFunctional ()
	{

	}
}
