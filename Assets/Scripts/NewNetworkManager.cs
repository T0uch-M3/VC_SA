using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//public delegate void OnWindowResize();

[AddComponentMenu("")]
public class NewNetworkManager : NetworkManager
{
	public Transform leftPosition;
	public Transform rightPosition;
	public PlayerScript playerScript;
	public static bool clientLoaded = false;
	private GameObject tip;

	//private GameObject player;

	//Start is private called before private the first private frame update

	new private void Start()

	{
		///manually setting the resolution because of a bug in the Toolbarhider class
		///which is every time the program get launched (win) it's resolution get bigger
		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
		{
			//running the 3 lines bellow because OnSceneChange wont get triggered on game launch
			GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
			canvas.AddComponent<DragScript>();
			canvas.GetComponent<Outline>().enabled = true;
		}

		//on scene change call OnSceneChange
		SceneManager.sceneLoaded += OnSceneChange;
	}

	private void OnSceneChange(Scene scene, LoadSceneMode mode)
	{
		GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");

		canvas.AddComponent<DragScript>();

		canvas.GetComponent<Outline>().enabled = true;
	}

	// Update is called once per frame
	//private void Update()
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
		//print("***********************************OnClientDisconnect");
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
		//print("************************************OnServerDisconnect");
		//base.OnServerDisconnect(conn);
	}

	public override void OnApplicationQuit()
	{
		//print("************************************OnApplicationQuit");
		base.OnApplicationQuit();
	}

	public override void OnServerRemovePlayer(NetworkConnection conn, NetworkIdentity player)
	{
		//print("************************************OnServerRemovePlayer");
		base.OnServerRemovePlayer(conn, player);
	}

	public override void OnServerChangeScene(string newSceneName)
	{
		//print("************************************OnServerChangeScene");
		//DontDestroyOnload.disconnected = true;
		base.OnServerChangeScene(newSceneName);
	}

	public override void OnClientChangeScene(string newSceneName)
	{
		//DontDestroyOnload.disconnected = true;
		//print("************************************OnClientChangeScene");
		base.OnClientChangeScene(newSceneName);
	}

	public override void Awake()
	{
		DontDestroyOnload.disconnected = true;
		//print("************************************Awake");
		base.Awake();
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		//print("CLIENT LOADED");
		clientLoaded = true;
		base.OnClientConnect(conn);
	}
}