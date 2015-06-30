using UnityEngine;
using System.Collections;

public class StrategicViewGrid : MonoBehaviour {

	public GameObject baseSVCell;
	private GameObject[,] grid;

	private int MAX_SIZE = Grid.MAX_SIZE;
	
	public void Init () {

		float side = 1.0f;
		float offsetX = 10.0f;
		float offsetY = 10.0f;

		Grid g = Grid.instance;
		if (g == null) {
			Debug.Log("ERRO: grid e nulo");
			return;
		}

		grid = new GameObject[MAX_SIZE, MAX_SIZE];
		
		for (int i = 0; i < MAX_SIZE; i++) {
			for (int j = 0; j < MAX_SIZE; j++) {

				Celula c = g.getCelula(i, j);
				if (c.isColored() || c.isEndpoint()) {
					grid[i,j] = Instantiate (
						baseSVCell, 
						new Vector3 ((float)i * side + offsetX, 0.0f, (float)j * side + offsetY), 
						Quaternion.identity) as GameObject;

					switch (c.getCellType()) {
					case Celula.CellType.R:
						grid[i,j].GetComponent<Renderer> ().material.color = Color.red;
						break;
					case Celula.CellType.G:
						grid[i,j].GetComponent<Renderer> ().material.color = Color.green;
						break;
					case Celula.CellType.B:
						grid[i,j].GetComponent<Renderer> ().material.color = Color.blue;
						break;
					case Celula.CellType.Entrada:
						grid[i,j].GetComponent<Renderer> ().material.color = Color.black;
						break;
					case Celula.CellType.Saida:
						grid[i,j].GetComponent<Renderer> ().material.color = Color.gray;
						break;
					}
				}
			}
		}
	}

	public Vector3 getGridCenter() {
		int I = 0, J = 0, _I = MAX_SIZE, _J = MAX_SIZE;
		for (int i = 0; i < MAX_SIZE; i++) {
			for (int j = 0; j < MAX_SIZE; j++) {
				if (grid[i,j] != null) {
					_I = Utils.min (_I, i);
					_J = Utils.min (_J, j);

					I = Utils.max(I, i);
					J = Utils.max(J, j);
				}
			}
		}
		
		return (grid [_I, _J].transform.position +
		        grid [I, J].transform.position) * 0.5f;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
