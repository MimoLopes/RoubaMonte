using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Manager/MaterialList")]
public class MaterialList : ScriptableObject
{
    [SerializeField]
    private List<Material> materials = new List<Material>();
    public List<Material> Materials
    {
        get
        {
            return materials;
        }
    }

    public Material Find(string materialName)
    {
        foreach (Material material in materials)
        {
            if(materialName == material.name)
            {
                return material;
            }
        }
        return null;
    }
}
