using System.Collections.Generic;
using Test;
using UnityEngine;

public class CreateCarpetTest : MonoBehaviour {

	public Material material;
	public Vector3[] verts;


	void OnGUI() {
		if (GUILayout.Button("test")) {
			CreateCarpetByVerts(0.2f);
		}
	}

	void CreateCarpetByVerts(float width) {
		List<Mesh> meshList = new List<Mesh>();
		for (int i = 0; i < verts.Length - 1; i++) {
			meshList.Add(CreateMeshByTwoPoint(verts[i], verts[i + 1], width));
		}
		Mesh mesh = new Mesh();
		CombineInstance[] combine = new CombineInstance[meshList.Count];
		for (int i = 0; i < meshList.Count; i++) {
			combine[i].mesh = meshList[i];
			combine[i].transform = Matrix4x4.identity;
		}
		mesh.CombineMeshes(combine);
		gameObject.AddComponent<MeshFilter>().mesh = mesh;
		gameObject.AddComponent<MeshRenderer>().material = material;
		UVMove.AddUVMoveComponent(gameObject, 3);
		//		CombineMesh();
	}

	Mesh CreateMeshByTwoPoint(Vector3 start, Vector3 end, float width) {
		float halfWidth = width * 0.5f;
		var deltaVert = end - start;
		float length = Vector3.Magnitude(deltaVert);
		float lengthWidthRatio = length / width;
		var dir = Vector3.Cross(Vector3.Normalize(deltaVert), Vector3.up);
		Mesh mesh = new Mesh {
			vertices = new Vector3[]
			{
				start + dir*halfWidth,
				start - dir*halfWidth,
				end + dir*halfWidth,
				end - dir*halfWidth,
			},
			triangles = new int[]
			{
				0, 2, 1, 1, 2, 3
			},
			uv = new[]
			{
				Vector2.zero,
				Vector2.up,
				Vector2.right *lengthWidthRatio,
				Vector2.right *lengthWidthRatio +Vector2.up,
			}
		};
		return mesh;
		//		InstantiateNewChildMesh(mesh);
	}

	void InstantiateNewChildMesh(Mesh mesh) {
		var child = new GameObject("test");
		child.AddComponent<MeshRenderer>().material = material;
		child.AddComponent<MeshFilter>().mesh = mesh;
		child.transform.SetParent(transform);
		child.transform.localPosition = Vector3.zero;
		child.transform.localRotation = Quaternion.identity;
		child.transform.localScale = Vector3.one;
	}

	void CombineMesh() {
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		int i = 0;
		while (i < meshFilters.Length) {
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			meshFilters[i].gameObject.SetActive(false);
			i++;
		}
		gameObject.AddComponent<MeshFilter>().mesh = new Mesh();
		transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		gameObject.AddComponent<MeshRenderer>().material = material;
		gameObject.SetActive(true);
	}

}
