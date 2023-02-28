using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRealUIManager : MonoBehaviour
{
    private static VRealUIManager _singleton;
    public static VRealUIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(VRealUIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    //Kullanýcý vr sahnesinden cizim sahnesine gecme metodu ancak calýsmadýgý yorum satýrýna alýndý.
    //public void BackRoomDrawingClicked()
    //{
    //    Player temp = Player.list[NetworkManager.Singleton.Client.Id];
    //    Player player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, temp.transform.position, temp.transform.rotation).GetComponent<Player>();
    //    temp.CopyPlayer(player);
    //    Player.MovePlayerToDestinationScene(NetworkManager.Singleton.Client.Id, "RoomDrawing");
    //}




    private void Awake()
    {
        Singleton = this;
    }
}
