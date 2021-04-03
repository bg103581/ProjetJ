using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMesh : MonoBehaviour
{
    [SerializeField] private Mesh[] meshes;
    [SerializeField] private MeshFilter meshFilter;

    private void OnEnable() {
        SetRandomMesh();
    }

    private void SetRandomMesh() {
        Mesh randMesh = ChooseRandomMesh();
        if (randMesh != null) meshFilter.mesh = randMesh;
    }

    private Mesh ChooseRandomMesh() {
        int rand = Random.Range(0, meshes.Length);
        foreach (Mesh mesh in meshes) {
            if (mesh == meshes[rand]) return mesh;
        }

        return null;
    }
}
