using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public Grid grid;
	public Celula.CellType playerColor;
	bool canMove = false;

	//public Animator anim;

	public int i, j;

	private int[] shift_i = {-1, 0, 1, 0};
	private int[] shift_j = {0, 1, 0, -1};

	private Utils.Pair<int, int>[] movement;
	private bool[,] marked;
	private int totalMovements, currentMov;

	private Vector3 nextPosition = Vector3.zero;
	private Utils.Pair<int, int> cellToChange;
	private bool canChangeColor = false;

	void Awake(){
		
		//anim = GetComponent <Animator> ();
	}

	void Start () {
		playerColor = Celula.CellType.R;
		Utils.Pair<Vector3, float> aux = grid.getPlayerPositionAndRotation ();
		transform.position = aux.first;
		transform.Rotate (Vector3.up * aux.second);

		bool found = false;
		for (i = 0; i < Grid.MAX_SIZE; i++) {
			for (j = 0; j < Grid.MAX_SIZE; j++) {
				if (grid.getCelula(i, j).getCellType() == Celula.CellType.Entrada) {
					found = true;
					break;
				}
			}
			if (found) break;
		}

		marked = new bool[Grid.MAX_SIZE, Grid.MAX_SIZE];
		movement = new Utils.Pair<int, int>[Grid.MAX_SIZE * Grid.MAX_SIZE];
		totalMovements = 0;
		currentMov = 0;
	}



	//Ativado pelo botao Start
	public void releaseMovement() {
		canMove = true;

		if (totalMovements > 0)
			return;

		// Pre-computa todos os movimentos possiveis
		while (grid.getCelula(i,j).getCellType() != Celula.CellType.Saida) {
			for (int k = 0; k < 4; k++) {
				int I = i + shift_i[k], J = j + shift_j[k];

				if (I >= 0 && I < Grid.MAX_SIZE 
				    && J >= 0 && J < Grid.MAX_SIZE 
				    && !marked[I,J] 
				    && (grid.getCelula(I,J).getCellType() == playerColor 
				    || grid.getCelula(I,J).getCellType() == Celula.CellType.Saida)) {

					marked[I,J] = true;
					movement[totalMovements] = new Utils.Pair<int, int>(I, J);
					totalMovements++;
					i = I;
					j = J;
					break;
				}

			}
		}
	}

	void rotatePlayer(int x_p, int y_p, int x, int y){
		
		if (y > y_p){
			//Debug.Log ("TRAS");
			transform.rotation = Quaternion.Euler (0, 0, 0);
		}
		else if (x > x_p){
			//Debug.Log ("ESQUERDA");
			transform.rotation = Quaternion.Euler(0, 90, 0);
		}
		else if (x < x_p){
			//Debug.Log ("DIREITA");
			transform.rotation = Quaternion.Euler(0, 270, 0);
		}
		else if(y < y_p){
			//Debug.Log ("FRENTE");
			transform.rotation = Quaternion.Euler(0, 180, 0);
		}
	}

	// Update is called once per frame

	/*void Update () {
		if (!canMove || currentMov == totalMovements) {
			anim.SetBool ("IsWalking", false);
			//Debug.Log ("idle");
			return;
		} else {
			anim.SetBool ("IsWalking", true);
			//Debug.Log("walking");
		}
		
		Transform nextTransform;
		
		if (currentMov == totalMovements - 1)
			nextTransform = grid.exitPlatform.transform;
		else {
			Utils.Pair<int, int> p = movement[currentMov];
			nextTransform = grid.getCelula(p.first, p.second).getGameObject().transform;
		}
		
		if (nextTransform == null)
			return;
		
		// Quando chega no pivo da celula
		if (transform.position == nextTransform.position) {
			
			// rotacionar o personagem
			int p_currentMov;
			Utils.Pair<int, int> p = movement[currentMov];
			
			if(currentMov == totalMovements - 1)
				rotatePlayer(0, 1, 0, 0);
			else{
				p_currentMov = currentMov + 1;
				Utils.Pair<int, int> p_p = movement[p_currentMov];
				rotatePlayer(p.first, p.second, p_p.first, p_p.second);
			}
			// fim rotacao
			
			grid.getCelula (p.first, p.second).nextColor ();
			currentMov++;
			
		}
		
		//"Animacao" entre duas celulas
		transform.position = Vector3.MoveTowards(transform.position, nextTransform.position, Time.deltaTime * 3);
		
	} */

	void Update () {
		if (!canMove || currentMov == totalMovements)
			return;

		if (currentMov == 0) {
			Utils.Pair<int, int> p = movement [0];
			Vector3 tmp = grid.getCelula (p.first, p.second).getGameObject ().transform.position;
			nextPosition = new Vector3 (tmp.x, 0.0f, tmp.z);
		}
		else if (canChangeColor) {
			if (currentMov == totalMovements - 1)
				nextPosition = grid.exitPlatform.transform.position;
			else {
				Utils.Pair<int, int> p = movement [currentMov];
				Vector3 tmp = grid.getCelula (p.first, p.second).getGameObject ().transform.position;
				nextPosition = new Vector3 (tmp.x, 0.0f, tmp.z);
			}

			Vector3 prevPosition = grid.getCelula (cellToChange.first, cellToChange.second).getGameObject().transform.position;
			// ATIVAR AQUI DENTRO A ANIMACAO DA MUDANCA DE COR
			if (currentMov > 0 && Utils.dist (transform.position, prevPosition) > grid.getCellSide () / 1.5f ) {
				//Debug.Log ("COLOR CHANGE: " + transform.position.ToString() + " " + nextPosition.ToString());
				grid.getCelula (cellToChange.first, cellToChange.second).nextColor ();
				canChangeColor = false;
				return;
			}
		}

		// Quando chega no pivo da celula
		if (transform.position == nextPosition) {
			cellToChange = movement[currentMov];
			canChangeColor = true;
			currentMov++;

			if (currentMov == totalMovements) return;
			Celula c = grid.getCelula(movement[currentMov].first, movement[currentMov].second);
			//Debug.Log ("NEXT CELL: " + c.getGameObject().transform.position.ToString());
			//Debug.Log ("CURR TRANSFORM: " + transform.position.ToString());
		}

		//Interpolaçao do movimento entre duas celulas
		transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.deltaTime * 3);
	}
}