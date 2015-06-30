using UnityEngine;
using System.Collections;

public class Celula {

	public enum CellType {
		R, G, B, Entrada, Saida, Vazia
	}
	private CellType tipo;
	private GameObject cell;

	private int lastRotation;

	// Construtor
	public Celula() {
		tipo = CellType.Vazia;
		lastRotation = 0;
	}

	// Metodos
	public void setCellType(CellType tipo) {
		this.tipo = tipo;
		switch (tipo) {
		case CellType.R:
			cell.transform.Rotate(Vector3.back * lastRotation);
			lastRotation = 0;
			cell.SetActive(true);
			break;
		case CellType.G:
			cell.transform.Rotate(Vector3.down * lastRotation);
			cell.transform.Rotate(Vector3.up * 120);
			lastRotation = 120;
			cell.SetActive(true);
			break;
		case CellType.B:
			cell.transform.Rotate(Vector3.down * lastRotation);
			cell.transform.Rotate(Vector3.up * 240);
			lastRotation = 240;
			cell.SetActive(true);
			break;
		}
	}

	public CellType getCellType() {
		return tipo;
	}

	public bool isColored() {
		return tipo == CellType.R || tipo == CellType.G || tipo == CellType.B;
	}

	public bool isEndpoint() {
		return tipo == CellType.Entrada || tipo == CellType.Saida;
	}

	public void setGameObject(GameObject cell) {
		this.cell = cell;
		this.cell.SetActive (false);
	}

	public GameObject getGameObject() {
		return cell;
	}

	public void nextColor() {
		switch (tipo) {
		case CellType.R:
			setCellType(CellType.G);
			break;
		case CellType.G:
			setCellType(CellType.B);
			break;
		case CellType.B:
			setCellType(CellType.R);
			break;
		}
	}

	/* public void cellUpdate() {
		switch (tipo) {
		case CellType.R:
			cell.GetComponent<Renderer> ().material.color = Color.red;
			break;
		case CellType.G:
			cell.GetComponent<Renderer> ().material.color = Color.green;
			break;
		case CellType.B:
			cell.GetComponent<Renderer> ().material.color = Color.blue;
			break;
		case CellType.Entrada:
		case CellType.Saida:
			cell.GetComponent<Renderer> ().material.color = Color.gray;
			break;
		case CellType.Vazia:
			cell.GetComponent<Renderer> ().material.color = Color.clear;
			break;
		}
	} */
}
