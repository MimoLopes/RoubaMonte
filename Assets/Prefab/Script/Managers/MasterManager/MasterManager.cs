using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using Photon.Pun;


[CreateAssetMenu(menuName = "Singletons/MasterManager")]
public class MasterManager : SingletonScriptableObject<MasterManager>
{
    [SerializeField]
    private GameSettings gameSettings;
    public static GameSettings GameSettings
    {
        get
        {
            return Instance.gameSettings;
        }
    }

    [SerializeField]
    private List<NetworkPrefab> networkPrefabs = new List<NetworkPrefab>();

    public static GameObject NetworkInstantiate(GameObject prefab, Vector3 position, Quaternion rotationl)
    {
        foreach (NetworkPrefab networkPrefab in Instance.networkPrefabs)
        {
            if (networkPrefab.prefab == (GameObject)prefab)
            {
                if (networkPrefab.path != string.Empty)
                {
                    GameObject gameObject = PhotonNetwork.Instantiate(networkPrefab.path, position, rotationl);
                    return gameObject;
                }
                else
                {
                    Debug.LogError($"Path is empty for gameobject name {networkPrefab.prefab}");
                    return null;
                }
            }
        }
        return null;
    }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void PopulateNetworkedPrefabs()
    {
#if UNITY_EDITOR
        Instance.networkPrefabs.Clear();

        GameObject[] results = Resources.LoadAll<GameObject>("");

        for (int i = 0; i < results.Length; i++)
        {
            if (results[i].GetComponent<PhotonView>() != null)
            {
                string path = AssetDatabase.GetAssetPath(results[i]);
                Instance.networkPrefabs.Add(new NetworkPrefab(results[i], path));
            }
        }
#endif
    }

}

