using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	public const int MAX_SIZE = 10;

	private Celula[,] grid;
	private Vector3 playerPosition;
	private float playerRotation;
	private float cellSide = 2.0f;

	public GameObject baseCell;
	public GameObject entrancePlatform;
	public GameObject exitPlatform;
	public GameObject entranceBridge;
	public GameObject exitBridge;
	public GameObject corner;
	public GameObject side;

	public string colorChangeOrder = "RGB";

	public static Grid instance = null;

	/*public void Start() {
		DontDestroyOnLoad (gameObject);
	}*/

	public void Init () {
		grid = new Celula[MAX_SIZE, MAX_SIZE];
		playerPosition = new Vector3 ();
		playerRotation = 0;

		for (int i = 0; i < MAX_SIZE; i++) {
			for (int j = 0; j < MAX_SIZE; j++) {
				GameObject cell = Instantiate (
					baseCell, 
					new Vector3 (cellSide * i, -Mathf.Sqrt(3)/3.0f, cellSide * j), 
					Quaternion.identity) as GameObject;
				cell.transform.Rotate(new Vector3(270.0f, 0.0f, 0.0f));
				cell.transform.parent = this.transform;
				grid[i,j] = new Celula();
				grid[i,j].setGameObject(cell);
				
}
		}

		instance = this;
	}

	public void BuildScenario() {
		Utils.Pair<int, int> entrance = new Utils.Pair<int,int> (0, 0);
		Utils.Pair<int, int> exit 	  = new Utils.Pair<int,int> (0, 0);
		Utils.Pair<int, int> size	  = new Utils.Pair<int,int> (0, 0);

		Utils.Pair<int, int> tl = new Utils.Pair<int,int> (MAX_SIZE, MAX_SIZE);
		Utils.Pair<int, int> br	= new Utils.Pair<int,int> (0, 0);

		for (int i = 0; i < MAX_SIZE; i++) {
			for (int j = 0; j < MAX_SIZE; j++) {
				if (grid[i, j].isColored()) {
					tl.first  = Utils.min(tl.first, i);
					tl.second = Utils.min(tl.second, j);
					br.first  = Utils.max(br.first, i);
					br.second = Utils.max(br.second, j);
				}

				if (grid[i,j].getCellType() != Celula.CellType.Vazia) {
					size.first = Utils.max(size.first, i);
					size.second = Utils.max(size.second, j);
				}

				if (grid[i,j].getCellType() == Celula.CellType.Entrada) {
					entrance.first = i;
					entrance.second = j;
				}

				if (grid[i,j].getCellType() == Celula.CellType.Saida) {
					exit.first = i;
					exit.second = j;
				}
			}
		}

		/*
		 * Corners
		 */

		//Top-left
		GameObject tmp;
		tmp = Instantiate(corner, new Vector3 (cellSide * tl.first, 0.0f, cellSide * tl.second), 
		                   Quaternion.identity) as GameObject;
		tmp.transform.Rotate (Vector3.right * 270);
		tmp.transform.Rotate (Vector3.forward * 180);
		tmp.transform.parent = this.transform;
		//Top-right
		tmp = Instantiate (corner, new Vector3 (cellSide * br.first, 0.0f, cellSide * tl.second), 
		                  Quaternion.identity) as GameObject;
		tmp.transform.Rotate (Vector3.right * 270);
		tmp.transform.Rotate (Vector3.forward * 90);
		tmp.transform.parent = this.transform;
		//Bottom-left
		tmp = Instantiate (corner, new Vector3 (cellSide * tl.first, 0.0f, cellSide * br.second), 
		                  Quaternion.identity) as GameObject;
		tmp.transform.Rotate (Vector3.right * 270);
		tmp.transform.Rotate (Vector3.forward * 270);
		tmp.transform.parent = this.transform;
		//Bottom-right
		tmp = Instantiate (corner, new Vector3 (cellSide * br.first, 0.0f, cellSide * br.second), 
		                  Quaternion.identity) as GameObject;
		tmp.transform.Rotate (Vector3.right * 270);
		tmp.transform.parent = this.transform;
		/*
		 * Sides
		 */

		for (int i = tl.first+1; i < br.first; i++) {
			tmp = Instantiate (side, new Vector3(cellSide * i, 0.0f, cellSide * tl.second),
			                   Quaternion.identity) as GameObject;
			tmp.transform.Rotate (Vector3.right * 270);
			tmp.transform.Rotate (Vector3.forward * 180);
			tmp.transform.parent = this.transform;

			tmp = Instantiate (side, new Vector3(cellSide * i, 0.0f, cellSide * br.second),
			                   Quaternion.identity) as GameObject;
			tmp.transform.Rotate (Vector3.right * 270);
			tmp.transform.parent = this.transform;
		}

		for (int j = tl.second+1; j < br.second; j++) {
			tmp = Instantiate (side, new Vector3(cellSide * tl.first, 0.0f, cellSide * j),
			                   Quaternion.identity) as GameObject;
			tmp.transform.Rotate (Vector3.right * 270);
			tmp.transform.Rotate (Vector3.forward * 270);
			tmp.transform.parent = this.transform;

			tmp = Instantiate (side, new Vector3(cellSide * br.first, 0.0f, cellSide * j),
			                   Quaternion.identity) as GameObject;
			tmp.transform.Rotate (Vector3.right * 270);
			tmp.transform.Rotate (Vector3.forward * 90);
			tmp.transform.parent = this.transform;
		}

		/*
		 * Entrada
		 */
		Vector3 entranceBridgePosition;
		Vector3 entrancePlatformPosition;
		float entranceRotation;

		Transform ebt = entranceBridge.transform;
		Transform ept = entrancePlatform.transform;
		Bounds bridgeBounds = entranceBridge.GetComponent<MeshFilter> ().mesh.bounds;
		Bounds platformBounds = entrancePlatform.GetComponent<MeshFilter> ().mesh.bounds;

		float eX = cellSide * entrance.first;
		float eZ = cellSide * entrance.second;
		float x, z;

		// Esquerda
		if (entrance.second == 0) {
			x = eX;
			z = eZ - 0.3f - (bridgeBounds.size.y - 2.0f) / 2.0f;
			entranceBridgePosition = new Vector3(x, ebt.position.y, z);
			z -= (bridgeBounds.extents.y + platformBounds.extents.y) - 0.75f;
			entrancePlatformPosition = new Vector3(x, ept.position.y, z);
			entranceRotation = 0;
		} 
		// Direita
		else if (entrance.second == size.second) {
			x = eX;
			z = eZ + 0.3f + (bridgeBounds.size.y - 2.0f) / 2.0f;
			entranceBridgePosition = new Vector3(x, ebt.position.y, z);
			z += (bridgeBounds.extents.y + platformBounds.extents.y) - 0.75f;
			entrancePlatformPosition = new Vector3(x, ept.position.y, z);
			entranceRotation = 180;
		} 
		// Cima
		else if (entrance.first == 0) {
			x = eX - 0.3f - (bridgeBounds.size.y - 2.0f) / 2.0f;
			z = eZ;
			entranceBridgePosition = new Vector3(x, ebt.position.y, z);
			x -= (bridgeBounds.extents.y + platformBounds.extents.y) - 0.75f;
			entrancePlatformPosition = new Vector3(x, ept.position.y, z);
			entranceRotation = 90;
		}
		// Baixo
		else {
			x = eX + 0.3f + (bridgeBounds.size.y - 2.0f) / 2.0f;
			z = eZ;
			entranceBridgePosition = new Vector3(x, ebt.position.y, z);
			x += (bridgeBounds.extents.y + platformBounds.extents.y) - 0.75f;
			entrancePlatformPosition = new Vector3(x, ept.position.y, z);
			entranceRotation = 270;
		}

		entranceBridge.transform.position = entranceBridgePosition;
		entranceBridge.transform.Rotate(Vector3.forward * entranceRotation);

		entrancePlatform.transform.position = entrancePlatformPosition;
		entrancePlatform.transform.Rotate(Vector3.forward * entranceRotation);

		playerPosition = entrancePlatformPosition;
		playerRotation = entranceRotation;

		/*
		 * Saida
		 */
		Vector3 exitBridgePosition;
		Vector3 exitPlatformPosition;
		float exitRotation;
		
		Transform xbt = exitBridge.transform;
		Transform xpt = exitPlatform.transform;
		bridgeBounds = exitBridge.GetComponent<MeshFilter> ().mesh.bounds;
		platformBounds = exitPlatform.GetComponent<MeshFilter> ().mesh.bounds;
		
		eX = cellSide * exit.first;
		eZ = cellSide * exit.second;
		
		// Esquerda
		if (exit.second == 0) {
			x = eX;
			z = eZ - 0.3f - (bridgeBounds.size.y - 2.0f) / 2.0f;
			exitBridgePosition = new Vector3(x, xbt.position.y, z);
			z -= (bridgeBounds.extents.y + platformBounds.extents.y) - 0.5f;
			exitPlatformPosition = new Vector3(x, xpt.position.y, z);
			exitRotation = 0;
		} 
		// Direita
		else if (exit.second == size.second) {
			x = eX;
			z = eZ + 0.3f + (bridgeBounds.size.y - 2.0f) / 2.0f;
			exitBridgePosition = new Vector3(x, xbt.position.y, z);
			z += (bridgeBounds.extents.y + platformBounds.extents.y) - 0.5f;
			exitPlatformPosition = new Vector3(x, xpt.position.y, z);
			exitRotation = 180;
		} 
		// Cima
		else if (exit.first == 0) {
			x = eX - 0.3f - (bridgeBounds.size.y - 2.0f) / 2.0f;
			z = eZ;
			exitBridgePosition = new Vector3(x, xbt.position.y, z);
			x -= (bridgeBounds.extents.y + platformBounds.extents.y) - 0.5f;
			exitPlatformPosition = new Vector3(x, xpt.position.y, z);
			exitRotation = 90;
		}
		// Baixo
		else {
			x = eX + 0.3f + (bridgeBounds.size.y - 2.0f) / 2.0f;
			z = eZ;
			exitBridgePosition = new Vector3(x, xbt.position.y, z);
			x += (bridgeBounds.extents.y + platformBounds.extents.y) - 0.5f;
			exitPlatformPosition = new Vector3(x, xpt.position.y, z);
			exitRotation = 270;
		}
		
		exitBridge.transform.position = exitBridgePosition;
		exitBridge.transform.Rotate(Vector3.forward * exitRotation);
		
		exitPlatform.transform.position = exitPlatformPosition;
		exitPlatform.transform.Rotate(Vector3.forward * exitRotation);
	}

	public Utils.Pair<Vector3, float> getPlayerPositionAndRotation() {
		return new Utils.Pair<Vector3, float> (playerPosition, playerRotation);
	}

	public void setCelula (int i, int j, Celula.CellType tipo) {
		grid[i, j].setCellType(tipo);
	}

	public Celula getCelula (int i, int j) {
		return grid[i, j];
	}

	public Celula[,] getGrid() {
		return grid;
	}

	public Celula.CellType getCellType(int i, int j) {
		if (i < 0 || j < 0 || i >= MAX_SIZE || j >= MAX_SIZE)
			return Celula.CellType.Vazia;
		else
			return grid [i, j].getCellType ();
	}

	public Vector3 getGridCenter() {
		int I = 0, J = 0;
		for (int i = 0; i < MAX_SIZE; i++) {
			for (int j = 0; j < MAX_SIZE; j++) {
				if (grid[i,j].getCellType() != Celula.CellType.Vazia) {
					I = Utils.max(I, i);
					J = Utils.max(J, j);
				}
			}
		}

		return (grid [0, 0].getGameObject ().transform.position +
		        grid [I, J].getGameObject ().transform.position) * 0.5f;
	}

	public float getCellSide() {
		return cellSide;
	}

	void Update() {
		/*for (int i = 0; i < MAX_SIZE; i++) {
			for (int j = 0; j < MAX_SIZE; j++) {
				grid[i,j].cellUpdate();
			}
		}*/
	}
}
