using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PlayerData : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text nickname, ready;

    public Player Player { get; private set; }

    public void SetInfo(Player player)
    {
        this.Player = player;
        this.nickname.text = player.NickName;

        SetReady((bool)player.CustomProperties["Ready"]);
    }

    private void SetReady(bool ready)
    {
        this.ready.text = ready ? "X" : "O";
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(targetPlayer != null && targetPlayer == this.Player)
        {
            SetInfo(targetPlayer);
        }

        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    }


}
