using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {
	public GameObject gameMenu;
	public GameObject strategicView;

	public void onClick(int action) {
		gameMenu.SetActive(action == 1);
		strategicView.SetActive(action == 2);
	}
}
