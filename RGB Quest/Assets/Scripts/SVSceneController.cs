using UnityEngine;
using System.Collections;

public class SVSceneController : MonoBehaviour {
	public StrategicViewGrid grid;

	void Awake() {
		grid.Init ();
	}
}
