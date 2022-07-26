using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private string gameVersion = "0.0.0";
    public string GameVersion
    {
        get
        {
            return gameVersion;
        }
    }

    [SerializeField]
    private string nickName = "NotANickName";
    public string NickName
    {
        get
        {
            return nickName;
        }
    }
}
