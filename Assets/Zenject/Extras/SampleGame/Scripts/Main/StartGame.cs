using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour 
{
	private void Start () 
	{
		Debug.Log("Start Scene");
		Application.LoadLevel(1);
	}
}
