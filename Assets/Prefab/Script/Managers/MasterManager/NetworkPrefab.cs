using UnityEngine;

[System.Serializable]
public class NetworkPrefab 
{
    public GameObject prefab;
    public string path;

    public NetworkPrefab(GameObject prefab, string path)
    {
        this.prefab = prefab;
        SetPath(path);
    }

    private void SetPath(string path)
    {
        int extensionLength = System.IO.Path.GetExtension(path).Length;
        int additionalLegth = 10;
        int startIndex = path.ToLower().IndexOf("resources");

        if(startIndex == -1)
        {
            this.path = string.Empty;
        }
        else
        {
            this.path = path.Substring(startIndex + additionalLegth, path.Length - (additionalLegth + startIndex + extensionLength));
        }
    }
}
