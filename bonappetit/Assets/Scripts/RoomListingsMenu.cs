using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingsMenu : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private RoomListing _roomListing;

    private List<RoomListing> _listings = new List<RoomListing>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Searching for rooms");
        Debug.Log(roomList.Count);

        // RoomListing listing2 = Instantiate(_roomListing, _content);
        // RoomListing listing3 = Instantiate(_roomListing, _content);
        // RoomListing listing4 = Instantiate(_roomListing, _content);
        // RoomListing listing5 = Instantiate(_roomListing, _content);
        foreach (RoomInfo info in roomList)
        {
            Debug.Log(info.Name);
            //Removed from rooms list
            if(info.RemovedFromList)
            {
                int index = _listings.FindIndex( x => x.RoomInfo.Name == info.Name);
                if(index != -1)
                {
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            //Added to roomsList
            else{
                RoomListing listing = Instantiate(_roomListing, _content);
                if (listing != null)
                {
                    listing.SetRoomInfo(info);
                    _listings.Add(listing);
                }
            }
        }
    }
}
