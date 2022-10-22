using UnityEngine;

public class MaterialSetter : MonoBehaviour
{
    [SerializeField] private int[] _indices;
    [SerializeField] private Renderer _meshRenderer;

    public void SetMaterial(Material mat)
    {
        var materials = _meshRenderer.sharedMaterials;
        foreach (var index in _indices)
        {
            materials[index] = mat;
        }

        _meshRenderer.sharedMaterials = materials;
    }
}