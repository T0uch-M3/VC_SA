using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[AddComponentMenu("")]
public class NewNetworkManager : NetworkManager
{
    public Transform leftPosition;
    public Transform rightPosition;
    //private GameObject player;

    // Start is called before the first frame update
    //void Start()
    //{
    //    //print("insideeeeeeeeeeeeeeeeeeeeeeeeeeeeee");

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        print("insideeeeeeeeeeeeeeeeeeeeeeeeeeeeee");

        //base.OnServerAddPlayer(conn);
        //print("insideeeeeeeeeeeeeeeeeeeeeeeeeeeeee");
        Transform start = numPlayers == 0 ? leftPosition : rightPosition;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);
    }

}
