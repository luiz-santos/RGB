using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {
	public Grid grid;
	public Parser parser;

	void Awake () {
		grid.Init ();
		parser.BuildLevel (grid, "level00");
		grid.BuildScenario ();

	}

	// Use this for initialization
	void Start () {
	
	}
}
