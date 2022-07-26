using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text countText;
    [SerializeField]
    private Text winnerText;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private List<Transform> spawnPoints = new List<Transform>();
    [SerializeField]
    private MaterialList materialList;

    private List<PlayerManager> playersObject = new List<PlayerManager>();

    private List<Deck> playersDecks = new List<Deck>();

    private List<CardManager> playedCards = new List<CardManager>();

    private Card playedCard = new Card();

    private CustomSerialization customSerialization = new CustomSerialization();

    private int turnNumber = 0;
    private int countNumber = 0;
    private int steallerNumber = -1;

    public void Awake()
    {
        if (photonView.IsMine)
        {
            PhotonPeer.RegisterType(typeof(CustomSerialization), (byte)'M', CustomSerialization.Serialize, CustomSerialization.Deserialize);
        }
    }

    public override void OnEnable()
    {
        EventManager.StartListening(Events.Player.Draw, OnDraw);
        EventManager.StartListening(Events.Player.Steal, OnSteal);

        base.OnEnable();
    }

    public override void OnDisable()
    {
        EventManager.StopListening(Events.Player.Draw, OnDraw);
        EventManager.StartListening(Events.Player.Steal, OnSteal);

        base.OnDisable();
    }

    public void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Deck newDeck = new Deck();

            foreach (Material material in materialList.Materials)
            {
                newDeck.Add(new Card(material.name));
            }

            newDeck.Shuffler();

            customSerialization.SetValues(PhotonNetwork.PlayerList.Length, newDeck.ToString());

            photonView.RPC("SendDeck", RpcTarget.All, CustomSerialization.Serialize(customSerialization));

            Player[] players = PhotonNetwork.PlayerList;

            for (int i = 0; i < players.Length; i++)
            {
                photonView.RPC("RPC_CreatePlayer", players[i], i, spawnPoints[i].position, spawnPoints[i].rotation);
            }
        }
    }

    public void Update()
    {
        countText.text = countNumber.ToString();
    }
    [PunRPC]
    private void RPC_CreatePlayer(int playerNumber, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        GameObject newPlayer = MasterManager.NetworkInstantiate(playerPrefab, spawnPosition, spawnRotation);

        playersObject.Add(newPlayer.GetComponent<PlayerManager>());

        playersObject[playersObject.Count - 1].Number = playerNumber;
    }

    [PunRPC]
    private void SendDeck(byte[] data)
    {
        CustomSerialization customSerialization = (CustomSerialization)CustomSerialization.Deserialize(data);

        int number = customSerialization.NumberValue;
        string cards = customSerialization.StringValue;

        Deck newDeck = new Deck(cards);
        playersDecks = newDeck.Divide(number);
    }

    private void OnDraw(object data)
    {
        if (photonView.IsMine)
        {
            int playerNumber = (int)data;

            if (playerNumber == turnNumber)
            {
                if (playersDecks[playerNumber].Count > 0)
                {
                    playedCard = playersDecks[playerNumber].Remove();

                    customSerialization.SetValues(playerNumber, playedCard.ToString());
                    photonView.RPC("RPC_OnDraw", RpcTarget.All, CustomSerialization.Serialize(customSerialization));

                    if(playedCards.Count >= 52)
                    {
                        customSerialization.SetValues(playerNumber);
                        photonView.RPC("RPC_SetWinner", RpcTarget.All, CustomSerialization.Serialize(customSerialization));
                        photonView.RPC("RPC_OnSteal", RpcTarget.All, CustomSerialization.Serialize(customSerialization));
                    }

                    ContValue();
                    TurnValue();
                }
            }
        }
    }
    [PunRPC]
    private void RPC_OnDraw(byte[] data)
    {
        CustomSerialization customSerialization = (CustomSerialization)CustomSerialization.Deserialize(data);

        int playerNumber = customSerialization.NumberValue;
        string materialName = customSerialization.StringValue;

        GameObject newCard = Instantiate(cardPrefab, spawnPoints[playerNumber].position, spawnPoints[playerNumber].rotation);

        playedCards.Add(newCard.GetComponent<CardManager>());

        playedCards[playedCards.Count - 1].SetMaterial(materialName);
        playedCards[playedCards.Count - 1].Center = new Vector3(UnityEngine.Random.Range(-0.25f, 0.25f), 0.001f * playedCards.Count, UnityEngine.Random.Range(-0.25f, 0.25f));
    }
    [PunRPC]
    private void RPC_SetWinner(byte[] data)
    {
        CustomSerialization customSerialization = (CustomSerialization)CustomSerialization.Deserialize(data);

        int playerNumber = customSerialization.NumberValue;

        winnerText.text = $"{PhotonNetwork.PlayerList[playerNumber].NickName} Win!!!";

        Invoke("Restart", 5f);
    }

    private void OnSteal(object data)
    {
        if (photonView.IsMine)
        {
            int playerNumber = (int)data;

            if (steallerNumber == -1)
            {
                if (playedCard.Value == countNumber)
                {
                    customSerialization.SetValues(playerNumber);


                    if (playersDecks[playerNumber].Count >= 52)
                    {
                        photonView.RPC("RPC_SetWinner", RpcTarget.All, CustomSerialization.Serialize(customSerialization));
                    }

                    photonView.RPC("RPC_OnSteal", RpcTarget.All, CustomSerialization.Serialize(customSerialization));
                }
            }
        }
    }

    [PunRPC]
    private void RPC_OnSteal(byte[] data)
    {
        CustomSerialization customSerialization = (CustomSerialization)CustomSerialization.Deserialize(data);
        int playerNumber = customSerialization.NumberValue;

        steallerNumber = playerNumber;

        foreach (CardManager card in playedCards)
        {
            playersDecks[playerNumber].Add(new Card(card.Name));

            card.stealler = spawnPoints[playerNumber];
        }

        playedCards.Clear();

        turnNumber = playerNumber;
        countNumber = 0;

        steallerNumber = -1;

    }

    private void ContValue()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (photonView.IsMine)
            {
                if (countNumber < 13)
                {
                    countNumber++;
                }
                else
                {
                    countNumber = 1;
                }
            }

            customSerialization.SetValues(countNumber);
            photonView.RPC("RPC_SendContValue", RpcTarget.Others, CustomSerialization.Serialize(customSerialization));
        }
    }

    private void TurnValue()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (photonView.IsMine)
            {
                if (turnNumber < PhotonNetwork.CurrentRoom.PlayerCount - 1)
                {
                    turnNumber++;
                }
                else
                {
                    turnNumber = 0;
                }
            }

            customSerialization.SetValues(turnNumber);
            photonView.RPC("RPC_SendTurnValue", RpcTarget.Others, CustomSerialization.Serialize(customSerialization));
        }
    }

    [PunRPC]
    private void RPC_SendContValue(byte[] data)
    {
        CustomSerialization customSerialization = (CustomSerialization)CustomSerialization.Deserialize(data);
        int number = customSerialization.NumberValue;

        countNumber = number;
    }
    [PunRPC]
    private void RPC_SendTurnValue(byte[] data)
    {
        CustomSerialization customSerialization = (CustomSerialization)CustomSerialization.Deserialize(data);
        int number = customSerialization.NumberValue;

        turnNumber = number;
    }

    private void Restart()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
