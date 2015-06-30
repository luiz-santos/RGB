using UnityEngine;
using System.Collections;

public class SVCameraController : MonoBehaviour {

	public float speed = 1.5f;
	
	private Vector3 focus;
	private Vector3 center;
	
	public StrategicViewGrid grid;
	
	void Start() {
		focus = center = grid.getGridCenter ();
		transform.position = center + new Vector3 (0.0f, 8.0f, 0.0f);
		
		//transform.parent.position = transform.position;
		//transform.parent.rotation = transform.rotation;
		transform.LookAt (center);
	}

}
