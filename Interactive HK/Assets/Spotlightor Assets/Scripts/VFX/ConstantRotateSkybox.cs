using UnityEngine;
using System.Collections;

// Inspired by:http://answers.unity3d.com/questions/47885/easiest-way-to-rotate-a-cubemap.html

public class ConstantRotateSkybox : MonoBehaviour
{
	public Vector3 rotationSpeed = new Vector3 (0, 1, 0);
	private Vector3 currentRotation = Vector3.zero;
	private Material skyboxMaterialInstance;

	void Start ()
	{
		skyboxMaterialInstance = new Material (RenderSettings.skybox);
		skyboxMaterialInstance.name = RenderSettings.skybox.name + " Instance";
		RenderSettings.skybox = skyboxMaterialInstance;
	}

	void Update ()
	{
		currentRotation += rotationSpeed * Time.deltaTime;
		Matrix4x4 m = Matrix4x4.TRS (Vector3.zero, Quaternion.Euler (currentRotation), new Vector3 (1, 1, 1));
		skyboxMaterialInstance.SetMatrix ("_Rotation", m);
	}
}
