using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class Parser : MonoBehaviour {

	public bool BuildLevel(Grid grid, string level) {
		try {
			string line;
			string filename = "Assets/level/" + level + ".txt";
			StreamReader reader = new StreamReader (filename, Encoding.Default);

			using (reader) {
				int row = 0, col = 0;

				while (true) {
					line = reader.ReadLine();

					if (line == null)
						break;

					for (col = 0; col < line.Length; col++) {
						Celula.CellType tipo = Celula.CellType.Vazia;
						if (line[col] == 'R') tipo = Celula.CellType.R;
						if (line[col] == 'G') tipo = Celula.CellType.G;
						if (line[col] == 'B') tipo = Celula.CellType.B;
						if (line[col] == 'E') tipo = Celula.CellType.Entrada;
						if (line[col] == 'S') tipo = Celula.CellType.Saida;
						if (line[col] == 'X') tipo = Celula.CellType.Vazia;
						grid.setCelula(row, col, tipo);
					}

					row++;
				}
			}

		} catch (System.Exception e) {
			Debug.LogError("Erro ao abrir arquivo level" + level + ".txt");
			Debug.LogError(e.Message);
			return false;
		}
		return true;
	}
}
