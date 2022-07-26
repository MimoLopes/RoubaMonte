using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject maxPlayersValuePlaceHolder;

    [SerializeField]
    private Text maxPlayersValue;

    public void JoinLobby()
    {
        EventManager.TriggerEvent(Events.Connection.JoinLobby);
    }
    public void LeaveLobby()
    {
        EventManager.TriggerEvent(Events.Connection.LeaveLobby);
    }    
    public void CreateRoom()
    {
        EventManager.TriggerEvent(Events.Connection.CreateRoom);
    }
    public void JoinRoom()
    {
        EventManager.TriggerEvent(Events.Connection.JoinRoom);
    }
    public void LeaveRoom()
    {
        EventManager.TriggerEvent(Events.Connection.LeaveRoom);
    }
    public void GetRoom()
    {
        EventManager.TriggerEvent(Events.Connection.SetRoomName);
    }
    public void MaxPlayerEncrease()
    {
        int value;

        if (string.IsNullOrEmpty(maxPlayersValue.text))
        {
            value = 2;
        }
        else
        {
            value = int.Parse(maxPlayersValue.text);

            if (value < 4)
            {
                value++;
            }
        }

        maxPlayersValuePlaceHolder.SetActive(false);
        maxPlayersValue.text = value.ToString();
    }
    public void MaxPlayerDecrease()
    {
        int value;

        if (string.IsNullOrEmpty(maxPlayersValue.text))
        {
            value = 4;
        }
        else
        {
            value = int.Parse(maxPlayersValue.text);

            if(value > 2)
            {
                value--;
            }
        }

        maxPlayersValuePlaceHolder.SetActive(false);
        maxPlayersValue.text = value.ToString();
    }
    public void ReadyToPlay()
    {
        EventManager.TriggerEvent(Events.Connection.ReadyToPlay);
    }
}
