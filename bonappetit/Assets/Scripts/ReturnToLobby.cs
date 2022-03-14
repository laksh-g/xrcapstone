using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Photon.Realtime;

public class ReturnToLobby : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button inputHelpers = UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.MenuButton;
    public XRNode controller = XRNode.LeftHand;
    
    void Update()
    {
        UnityEngine.XR.Interaction.Toolkit.InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(controller), inputHelpers, out bool isPressed);

        if (isPressed)
        {
            //PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel(0);
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ExitGames.Client.Photon.Hashtable ht = otherPlayer.CustomProperties;
        if((string)ht["role"] == "HeadChefRole"){
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel(0);
            Debug.LogError("Master Client Disconnected");
        }
        base.OnPlayerLeftRoom(otherPlayer);
    }
}
