using UnityEngine;

using Photon.Pun;


public class PlayerManager : MonoBehaviourPunCallbacks
{

    public int Number{ get; set; }

    public int Penality { get; set; }

    private CustomSerialization customSerialization = new CustomSerialization();

    public void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                string eventName = Events.Player.Draw;

                customSerialization.SetValues(Number, eventName);

                photonView.RPC("RCP_EventTrigger", RpcTarget.All, CustomSerialization.Serialize(customSerialization));
            }
            if (Input.GetMouseButtonDown(1))
            {
                string eventName = Events.Player.Steal;

                customSerialization.SetValues(Number, eventName);

                photonView.RPC("RCP_EventTrigger", RpcTarget.All, CustomSerialization.Serialize(customSerialization));
            }
        }
    }

    [PunRPC]
    private void RCP_EventTrigger(byte[] data)
    {
        CustomSerialization customSerialization = (CustomSerialization)CustomSerialization.Deserialize(data);

        string eventName = customSerialization.StringValue;
        int number = customSerialization.NumberValue;

        EventManager.TriggerEvent(eventName, number);
    }
}