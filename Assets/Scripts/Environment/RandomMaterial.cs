using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    [SerializeField] private Material[] materials;
    [SerializeField] private Renderer renderer;

    private void OnEnable() {
        SetRandomMaterial();
    }

    private void SetRandomMaterial() {
        Material mat = ChooseRandomMaterial();
        if (mat != null) renderer.material = mat;
    }

    private Material ChooseRandomMaterial() {
        int rand = Random.Range(0, materials.Length);
        foreach (Material mat in materials) {
            if (mat == materials[rand]) return mat;
        }

        return null;
    }
}
