using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerListingsMenu : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerListings _playerListing;

    private List<PlayerListings> _listings = new List<PlayerListings>();
    private List<Player> playerList = new List<Player>();


    //Work in Progress
    //public override void OnPlayerEnteredRoom(Player newPlayer)
    void Update()
    {
        PhotonNetwork.NickName = "Work in Progress"; //"Player No. " + PhotonNetwork.LocalPlayer.ActorNumber.ToString();

        if (PhotonNetwork.InRoom)
        {
            // foreach (PlayerListings item in _listings){
            //     if(!PhotonNetwork.CurrentRoom.Players.ContainsValue(item.Player)){
            //         Destroy(item.gameObject);
            //         playerList.Remove(item.Player);
            //     }
            // }

            foreach (var info in PhotonNetwork.CurrentRoom.Players)
            {
                Player pl = info.Value;
                // Removed from player list
                
                if(!playerList.Contains(pl))
                {
                    PlayerListings listing = Instantiate(_playerListing, _content);
                    if (listing != null)
                    {
                        playerList.Add(pl);
                        listing.SetPlayerInfo(pl);
                        _listings.Add(listing);
                    }
                    
                    // int index = _listings.FindIndex( x => x.Player.ActorNumber == pl.ActorNumber);
                    // if(index != -1)
                    // {
                    //     Destroy(_listings[index].gameObject);
                    //     _listings.RemoveAt(index);
                    // }
                }
                // Added to roomsList
                // else{
                //     PlayerListings listing = Instantiate(_playerListing, _content);
                //     if (listing != null)
                //     {
                //         listing.SetPlayerInfo(pl);
                //         _listings.Add(listing);
                //     }
                // }
            }

        }
    }
}