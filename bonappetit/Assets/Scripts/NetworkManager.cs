using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public List<DefaultRoom> defaultRooms;

    public GameObject connectGameMenu;

    public GameObject connectingScreen;

    public TextMeshProUGUI createdRoomCode;

    public GameObject joinInput;

    public GameObject JoinRoomScreen;
    public GameObject RoomSettings;
    public GameObject FoodDisplay;

    void Awake()
    {
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Update()
    {

    }

    void Start()
    {
        // ConnectToServer();
        // DontDestroyOnLoad(this.gameObject);
    }

    public void StartGame()
    {
        if (PhotonNetwork.CurrentRoom != null) {
            ExitGames.Client.Photon.Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;
            Debug.Log((string) ht["difficulty"]);
            if((string) ht["difficulty"] == "Cafe Kitchen"){
                PhotonNetwork.LoadLevel("CafeKitchen");
            }else if((string) ht["difficulty"] == "Restaurant Kitchen"){
                PhotonNetwork.LoadLevel("RestaurantKitchen");
            }else if((string) ht["difficulty"] == "Fine Dining Kitchen"){
                PhotonNetwork.LoadLevel("FineDiningKitchen");
            }
        }
    }

    public void ConnectedToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Trying to connect to Server...");
        connectingScreen.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Server.");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Lobby Joined");
        //selectRoles.SetActive(true);
        connectGameMenu.SetActive(true);
        connectingScreen.SetActive(false);
    }

    public DefaultRoom getRoomSettings(int idx){
        return defaultRooms[idx];
    }

    public void InitializeRoom(int defaultRoomIndex)
    {
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        // Loading the rooms
        PhotonNetwork.LoadLevel(roomSettings.sceneIndex);
        // Creating the rooms
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)roomSettings.maxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        //set correct game objects
        //connectGameMenu.SetActive(false);
        JoinRoomScreen.SetActive(false);
        RoomSettings.SetActive(true);
        FoodDisplay.SetActive(true);
        createdRoomCode.text = "<b>Room Code:</b> " + PhotonNetwork.CurrentRoom.Name;
        base.OnJoinedRoom();
    }

    // public override void OnPlayerEnteredRoom(Player newPlayer)
    // {
    //     Debug.Log("A new player joined the room.");
    //     base.OnPlayerEnteredRoom(newPlayer);
    // }

    public void CreateRoom()
    {
        // Generate a random 4 letter room name
        string roomName = "";
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        for (int i = 0; i < 4; i ++)
        {
            roomName += chars[Random.Range(0, chars.Length)];
        }

        //################################
        // Debug statements : remember to remove
        //roomName = "BBBB";
        //################################
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsVisible = true;  // possible to join randomly
        roomOptions.IsOpen = true;  // let others join

        // Set text
        //createdRoomCode.text = "<b>Room Code:</b> " + roomName;

        // Create and join room
        Debug.Log("Creating and joining room " + roomName);
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    //REDUNDANT ++++++ REMOVE
    public void JoinRoom()
    {
        //string roomName = joinInput.GetComponent<TMP_InputField>().text;
        //################################
        // Debug statements : remember to remove
        Debug.Log("Pressed Join room button");
        string roomName = "BBBB";
        //################################
        Debug.Log("Trying to join room " + roomName);
        PhotonNetwork.JoinRoom(roomName.ToUpper());
    }
    
    public void LeaveRoom()
    {
        Debug.Log("Leaving room.");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public void DisconnectClient()
    {
        Debug.Log("Disconnecting Photon.");
        PhotonNetwork.Disconnect();
    }
    
}
