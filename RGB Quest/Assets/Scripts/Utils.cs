using UnityEngine;
using System.Collections;

public class Utils {

	public class Pair<T, U> {
		public T first;
		public U second;

		public Pair() {}

		public Pair(T first, U second) {
			this.first = first;
			this.second = second;
		}
	}

	public static int max (int a, int b) {
		if (a > b)
			return a;
		else
			return b;
	}

	public static float max (float a, float b) {
		if (a > b)
			return a;
		else
			return b;
	}

	public static int min (int a, int b) {
		if (a < b)
			return a;
		else
			return b;
	}
	
	public static float min (float a, float b) {
		if (a < b)
			return a;
		else
			return b;
	}

	public static float dist(Vector3 a, Vector3 b) {
		float X = a.x - b.x, Y = a.y - b.y, Z = a.z-b.z;
		return Mathf.Sqrt (X * X + Y * Y + Z * Z);
	}
}
