#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;


public class PrefabPathBuilder : IPreprocessBuildWithReport
{
    public int callbackOrder
    {
        get
        {
            return 0;
        }
    }

    public void OnPreprocessBuild(BuildReport report)
    {
        MasterManager.PopulateNetworkedPrefabs();
    }
}
#endif