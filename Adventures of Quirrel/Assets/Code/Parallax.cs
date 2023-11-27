using UnityEngine;

public class Parallax : MonoBehaviour 
{
	//Creates a LENGTH to the script 
	private float length, startPosition;
	//Creates a GAMEOBJECT that can be set in the UNITY IDE
	public GameObject camera;
	//Creates a FLOAT that can be set in the Unity IDE
	public float parallexEffect;

	//Function that runs at the start of the code
	//Sets the postion of the Gameobject that is hanivng parallax
	//Gets the same GameObjects X value
	void Start () 
	{
		startPosition = transform.position.x;
		length = GetComponent<SpriteRenderer>().size.x;
	}
	
	//Function that runs every frame
	//Sets the two values to the cameras location
	//Transforms the GameObject based on the second value 'distance'
	//Then if the transform goes past the temporary value changes it by the length value
	void Update () 
	{
		float temporary = (camera.transform.position.x * (1 - parallexEffect));
		float distance = (camera.transform.position.x * parallexEffect);

		transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

		if(temporary > startPosition + length) 
		{
			startPosition += length;
		}
		else if (temporary < startPosition - length)
		{
			startPosition -= length;
		}
	}

}
