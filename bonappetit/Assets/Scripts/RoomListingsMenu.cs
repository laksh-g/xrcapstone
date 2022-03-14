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

    private HashSet<RoomInfo> _info = new HashSet<RoomInfo>();

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
                    _info.Remove(_listings[index].RoomInfo);
                    Destroy(_listings[index].gameObject);
                    _listings.RemoveAt(index);
                }
            }
            //Added to roomsList
            else if (!_info.Contains(info)) {
                RoomListing listing = Instantiate(_roomListing, _content);
                if (listing != null)
                {
                    _info.Add(info);
                    listing.SetRoomInfo(info);
                    _listings.Add(listing);
                }
            }
        }
        HashSet<RoomListing> needToBeRemoved = new HashSet<RoomListing>();
        foreach (RoomListing listing in _listings) {
            if (!roomList.Contains(listing.RoomInfo)) {
                _info.Remove(listing.RoomInfo);
                needToBeRemoved.Add(listing);
            }
        }

        foreach (RoomListing o in needToBeRemoved) {
            _listings.Remove(o);
            Destroy(o.gameObject);
        }
    }
}
