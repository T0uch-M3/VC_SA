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
        DontDestroyOnload.disconnected = true;
        print("***********************************OnClientDisconnect");
        base.OnClientDisconnect(conn);
        
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        //since this get triggered when client leave server
        //we want to opt the server out from the comm interface
        //this way, both client and server will go back to their initial UI
        //so the roles can be switched if necessary 
        PlayerScript.triggerDisconnect = true;

        //DontDestroyOnload.disconnected = true;
        //DontDestroyOnload.disconnected = true;
        //playerScript.CloseOpenedChannels();
        print("************************************OnServerDisconnect");
        //base.OnServerDisconnect(conn);
    }

    public override void OnApplicationQuit()
    {
        print("************************************OnApplicationQuit");
        base.OnApplicationQuit();
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, NetworkIdentity player)
    {
        print("************************************OnServerRemovePlayer");
        base.OnServerRemovePlayer(conn, player);
    }

    public override void OnServerChangeScene(string newSceneName)
    {
        print("************************************OnServerChangeScene");
        //DontDestroyOnload.disconnected = true;
        base.OnServerChangeScene(newSceneName);
    }
    public override void OnClientChangeScene(string newSceneName)
    {
        //DontDestroyOnload.disconnected = true;
        print("************************************OnClientChangeScene");
        base.OnClientChangeScene(newSceneName);
    }

    public override void Awake()
    {
        DontDestroyOnload.disconnected = true;
        print("************************************Awake");
        base.Awake();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        print("CLIENT LOADED");
        clientLoaded = true;
        base.OnClientConnect(conn);
    }
}
