using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[AddComponentMenu("")]
public class NewNetworkManager : NetworkManager
{
    public Transform leftPosition;
    public Transform rightPosition;
    public PlayerScript playerScript;
    public static bool clientLoaded = false;

    
    //private GameObject player;

    // Start is called before the first frame update
    //void Start()
    //{
    //    //print("inside");

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        //avoiding wrong positioning when the client disconnect and connect again 
        Transform start = numPlayers == 0 ? leftPosition : rightPosition;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        clientLoaded = false;
        //playerScript.CloseOpenedChannels();
        print("***********************************OnClientDisconnect");
        base.OnClientDisconnect(conn);
        
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        //playerScript.stopHost();
        PlayerScript.triggerDisconnect = true;
        //playerScript.CloseOpenedChannels();
        print("************************************OnServerDisconnect");
        //base.OnServerDisconnect(conn);
    }

    
    public override void OnClientConnect(NetworkConnection conn)
    {
        print("CLIENT LOADED");
        clientLoaded = true;
        base.OnClientConnect(conn);
    }
}
