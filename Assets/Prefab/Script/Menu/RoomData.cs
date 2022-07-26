using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;

public class RoomData : MonoBehaviour
{
    [SerializeField]
    private Text roomName, maxPlayers;

    public RoomInfo RoomInfo { get; private set; }

    public void SetInfo(RoomInfo roomInfo)
    {
        this.RoomInfo = roomInfo;
        this.roomName.text = roomInfo.Name;
        this.maxPlayers.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
    }

}
