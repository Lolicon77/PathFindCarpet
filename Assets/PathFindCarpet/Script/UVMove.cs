using UnityEngine;
using System.Collections;

namespace Test {
	public class UVMove : MonoBehaviour {

		public float speed;
		public Material mat;

		public static void AddUVMoveComponent(GameObject go, float speed, Material mat = null) {
			var component = go.AddComponent<UVMove>();
			component.speed = speed;
			component.mat = mat;
		}

		void Start() {
			if (!mat) {
				mat = GetComponent<Renderer>().sharedMaterial;
			}
		}

		void Update() {
			mat.mainTextureOffset = new Vector2(1 - Time.time % 1 * speed, 0);
		}
	}
}