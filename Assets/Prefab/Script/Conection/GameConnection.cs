using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameConnection : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private bool test;
    [SerializeField]
    private MasterManager masterManager;
    [SerializeField]
    private GameObject roomsList, playersList, roomMenuLayout, nicknameField, roomLobyLayout;
    [SerializeField]
    private Text conectionLog, nickname, roomName, MaxPlayers, roomNameField;
    [SerializeField]
    private RoomData roomData;
    [SerializeField]
    private PlayerData playerData;

    private Hashtable customPropertie = new Hashtable();

    private List<RoomData> roomDataList = new List<RoomData>();
    private List<PlayerData> playerDataList = new List<PlayerData>();

    public void Awake()
    {
        conectionLog.text += "\nConecting...";

        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 5;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnEnable()

    {
        //EventManager.StartListening(Events.Connection.JoinLobby, OnJoinLobby);
        //EventManager.StartListening(Events.Connection.LeaveLobby, OnLeaveLobby);
        EventManager.StartListening(Events.Connection.CreateRoom, OnCreateRoom);
        EventManager.StartListening(Events.Connection.JoinRoom, OnJoinRoom);
        EventManager.StartListening(Events.Connection.LeaveRoom, OnLeaveRoom);
        EventManager.StartListening(Events.Connection.ReadyToPlay, OnReady);

        base.OnEnable();

    }

    public override void OnDisable()
    {
        //EventManager.StopListening(Events.Connection.JoinLobby, OnJoinLobby);
        //EventManager.StopListening(Events.Connection.LeaveLobby, OnLeaveLobby);
        EventManager.StopListening(Events.Connection.CreateRoom, OnCreateRoom);
        EventManager.StopListening(Events.Connection.JoinRoom, OnJoinRoom);
        EventManager.StopListening(Events.Connection.LeaveRoom, OnLeaveRoom);
        EventManager.StopListening(Events.Connection.ReadyToPlay, OnReady);

        base.OnDisable();
    }

    public override void OnConnectedToMaster()
    {
        conectionLog.text += "\nConected to server.";

        conectionLog.text += "\nJoining the lobby...";

        PhotonNetwork.JoinLobby();

        base.OnConnectedToMaster();
    }

    public override void OnJoinedLobby()
    {
        conectionLog.text += "\nJoined the lobby.";

        base.OnJoinedLobby();
    }

    public override void OnLeftLobby()
    {
        conectionLog.text += "\nLeft the lobby.";

        base.OnLeftLobby();
    }

    public override void OnJoinedRoom()
    {
        conectionLog.text += $"\nJoined the room: {PhotonNetwork.CurrentRoom.Name}.\nYour nickname: {PhotonNetwork.LocalPlayer.NickName}.";

        roomLobyLayout.SetActive(true);
        roomNameField.text = roomName.text;
        GetCurrentPlayersInRoom();

        base.OnJoinedRoom();
    }

    public override void OnLeftRoom()
    {
        conectionLog.text += "\nYou left the room.";

        ClearList(playersList.transform);
        playerDataList.Clear();

        base.OnLeftRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        conectionLog.text += $"\n{newPlayer.NickName} joined the room.";

        ListPlayer(newPlayer);

        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        conectionLog.text += $"\nPlayer {otherPlayer.NickName} leave the room.";

        DelistPlayer(otherPlayer);

        base.OnPlayerLeftRoom(otherPlayer);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        conectionLog.text += $"\nDisconnected: {cause}";
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList)
            {
                int index = roomDataList.FindIndex(x => x.RoomInfo.Name == room.Name);
                if (index != -1)
                {
                    Destroy(roomDataList[index].gameObject);
                    roomDataList.RemoveAt(index);
                }
            }
            else
            {
                int index = roomDataList.FindIndex(x => x.RoomInfo.Name == room.Name);
                if (index == -1)
                {
                    RoomData newRoomData = Instantiate(roomData, roomsList.transform);

                    newRoomData.SetInfo(room);
                    roomDataList.Add(newRoomData);

                    newRoomData.GetComponent<Button>().onClick.AddListener(delegate { OnRoomSelected(room.Name); });
                }
                else
                {
                    roomDataList[index].SetInfo(room);
                }
            }
        }

        base.OnRoomListUpdate(roomList);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        conectionLog.text += $"\nJoin the room failuer:\n{message}\nCODE: {returnCode}";

        nicknameField.SetActive(true);
        roomMenuLayout.SetActive(true);

        base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        conectionLog.text += $"\nCreate room failed:\n{message}\nCODE: {returnCode}";

        nicknameField.SetActive(true);
        roomMenuLayout.SetActive(true);

        base.OnCreateRoomFailed(returnCode, message);
    }

    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        conectionLog.text += $"\nError: {errorInfo.Info}.";

        nicknameField.SetActive(true);
        roomMenuLayout.SetActive(true);

        base.OnErrorInfo(errorInfo);
    }
    private void OnJoinLobby(object data)
    {
        conectionLog.text += "\nJoining the lobby...";

        PhotonNetwork.JoinLobby();
    }
    private void OnLeaveLobby(object data)
    {
        PhotonNetwork.LeaveLobby();
    }
    private void OnCreateRoom(object data)
    {
        conectionLog.text += $"\nCreating room...";

        try
        {
            AssignNickname();
            AssignReady();
            CreateRoom();
        }
        catch (Exception e)
        {
            nicknameField.SetActive(true);
            roomMenuLayout.SetActive(true);
            roomName.text = PhotonNetwork.CurrentRoom.Name;
            conectionLog.text += $"\nCreate room failed:\n{e.Message}";
        }
    }
    private void OnRoomSelected(object data)
    {
        roomName.text = (string)data;
    }
    private void OnJoinRoom(object data)
    {
        conectionLog.text += "\nJoining to room...";

        try
        {
            AssignNickname();
            AssignReady();
            JoinRoom();
        }
        catch (Exception e)
        {
            nicknameField.SetActive(true);
            roomMenuLayout.SetActive(true);
            roomName.text = PhotonNetwork.CurrentRoom.Name;
            conectionLog.text += $"\nJoin the room failuer:\n{e.Message}";

        }
    }
    private void OnLeaveRoom(object data)
    {
        PhotonNetwork.LeaveRoom(true);
    }
    private void OnReady(object data)
    {
        SetReady();
    }
    private void AssignNickname()
    {
        if (string.IsNullOrEmpty(nickname.text))
        {
            throw new Exception("\nInvalid nickname...");
        }
        PhotonNetwork.LocalPlayer.NickName = nickname.text;
    }
    private void AssignReady()
    {
        customPropertie["Ready"] = false;

        PhotonNetwork.LocalPlayer.CustomProperties = customPropertie;
    }
    private void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomName.text))
        {
            throw new Exception("\nInvalid room name.");
        }
        if (string.IsNullOrEmpty(MaxPlayers.text))
        {
            throw new Exception("\nInvalid max players value.");
        }

        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true;
        options.PublishUserId = true;
        options.MaxPlayers = Convert.ToByte(MaxPlayers.text);
        PhotonNetwork.CreateRoom(roomName.text, options, null);
    }
    private void JoinRoom()
    {
        if (string.IsNullOrEmpty(roomName.text))
        {
            throw new Exception("\nRoom not selected.");
        }
        PhotonNetwork.JoinRoom(roomName.text);
    }
    private void ClearList(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
    private void GetCurrentPlayersInRoom()
    {
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            ListPlayer(playerInfo.Value);
        }
    }
    private void ListPlayer(Player player)
    {
        PlayerData info = Instantiate(playerData, playersList.transform);
        if (info != null)
        {
            info.SetInfo(player);
            playerDataList.Add(info);
        }
    }
    private void DelistPlayer(Player player)
    {
        int index = playerDataList.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            Destroy(playerDataList[index].gameObject);
            playerDataList.RemoveAt(index);
        }
    }
    private void SetReady()
    {
        customPropertie["Ready"] = !(bool)customPropertie["Ready"];

        PhotonNetwork.SetPlayerCustomProperties(customPropertie);

        photonView.RPC("StartGame", RpcTarget.All);
    }

    [PunRPC]
    private void StartGame()
    {

        if (playerDataList.Count < 2 && !test)
        {
            return;
        }

        for (int i = 0; i < playerDataList.Count; i++)
        {
            if (!(bool)playerDataList[i].Player.CustomProperties["Ready"])
            {
                return;
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            PhotonNetwork.LoadLevel(1);
        }
    }
}
