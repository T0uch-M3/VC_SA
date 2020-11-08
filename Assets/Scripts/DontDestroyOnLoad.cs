using Mirror.Discovery;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class DontDestroyOnload : MonoBehaviour
{
	//will be used for things related to networking later
	private NewNetworkManager _manager;

	//Since dontdestroyonload will produce duplicate objects, a fix is needed
	//And this static instance var of this class will be used for that
	public static DontDestroyOnload Instance;

	private static bool runOnce = true;

	public GameObject uniBtn;

	public GameObject eventSystem;
	private readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
	public NetworkDiscovery networkDiscovery;

	private bool isEditor = Application.platform == RuntimePlatform.WindowsEditor;
	private bool isAndroid = Application.platform == RuntimePlatform.Android;
	private bool isWindows = Application.platform == RuntimePlatform.WindowsPlayer;

	public float timeRemaining = 4;
	public bool timerIsRunning = false;
	public bool foundServer = false;
	private TextMeshProUGUI unitText;
	public bool isServer = true;
	public static bool disconnected = false;
	public UnityEvent uEvent;

	/// <summary>
	/// Vars for dealing with holding button
	/// </summary>
	public bool pointerDown = false;

	public bool pointerUp = false;
	public float clickTimer = 0.6f;
	private bool clickChanged = false;
	private bool advance = false;
	public bool secondClick = false;
	private bool touched = false;//whether the button got touched while on "wait..." or not

	private void Awake()
	{
		///just for preventing a deletion
		//if (Instance != null)
		//{
		//    Destroy(gameObject);
		//    return;
		//}
		//DontDestroyOnLoad(gameObject);
		//Instance = this;

		_manager = GameObject.Find("NetworkManager").GetComponent<NewNetworkManager>();
		networkDiscovery = _manager.GetComponent<NetworkDiscovery>();

		//moved these 2 lines to NewNetWorkManager for code cleanup and things started bugging, so here they are
		Screen.SetResolution(250, 300, false);
		TitlebarHider.showWindowsBorder();
	}

#if UNITY_EDITOR

	private void OnValidate()
	{
		if (networkDiscovery == null)
		{
			networkDiscovery = GetComponent<NetworkDiscovery>();
			UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound,
				OnDiscoveredServer);
			UnityEditor.Undo.RecordObjects(new UnityEngine.Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
		}
	}

#endif

	// Start is called before the first frame update
	private void Start()
	{
		eventSystem = GameObject.Find("EventSystem");
		DontDestroyOnLoad(eventSystem);
		unitText = uniBtn.GetComponentInChildren<TextMeshProUGUI>();
		if (isEditor)
		{
			unitText.text = "Server";
		}
		else if (isAndroid)
		{
			unitText.text = "Server";
		}
		else
		{
			unitText.text = "Server";
		}

		networkDiscovery.OnServerFound.AddListener(OnDiscoveredServer);
	}

	public void OnDiscoveredServer(ServerResponse info)
	{
		// Note that you can check the versioning to decide if you can connect to the server or not using this method
		discoveredServers[info.serverId] = info;
	}

	// Update is called once per frame
	private void Update()
	{
		if (disconnected)
		{
			//print("STOP DISCOVERY");
			networkDiscovery.StopDiscovery();
			disconnected = false;
		}
		//if (Input.GetKeyDown(KeyCode.V))
		//{
		//}
		//if (Input.GetKeyDown(KeyCode.B))
		//{
		//    networkDiscovery.StartDiscovery();
		//}
		//if (Input.GetKeyDown(KeyCode.N))
		//{
		//    networkDiscovery.StopDiscovery();
		//}
		//if (Input.GetKeyDown(KeyCode.W))
		//{
		//}

		if (timerIsRunning)
		{
			if (timeRemaining > 0)
			{
				timeRemaining -= Time.deltaTime;
				if (discoveredServers.Count > 0)
				{
					//print("SERVERS::: " + discoveredServers.Count);
					timerIsRunning = false;
					uniBtn.GetComponent<Button>().interactable = true;
					//unitText.text = discoveredServers.Count.ToString();
					unitText.text = "Connect?";
					foundServer = true;
					if (clickTimer == 0.7f)
						uniBtn.GetComponent<UnityEngine.UI.Image>().fillAmount = 1;
				}
			}
			else
			{
				print("Found no servers");
				//print("Servers found = " + discoveredServers.Count);
				timerIsRunning = false;
				uniBtn.GetComponent<Button>().interactable = true;
				unitText.text = "Try Again";
				timeRemaining = 4;
				if (clickTimer == 0.7f)//to prevent the color fill from stucking while transitioning
					uniBtn.GetComponent<UnityEngine.UI.Image>().fillAmount = 1;
			}
		}

		if (advance)//=true, means that we going next scene, whether as server or client
		{
			//if (/*isEditor || */isWindows)
			if (isServer)
			{
				//print("SERVER");
				discoveredServers.Clear();
				_manager.StartHost();
				networkDiscovery.AdvertiseServer();
				//this.gameObject.GetComponent<Canvas>().sortingOrder--;
			}
			else
			{
				if (!foundServer)
				{
					//print("CLIENT");
					discoveredServers.Clear();
					networkDiscovery.StartDiscovery();
					timerIsRunning = true;
					timeRemaining = 4;
					unitText.text = "Wait..";
					uniBtn.GetComponent<Button>().interactable = false;
					if (clickTimer == 0.7f)
						uniBtn.GetComponent<UnityEngine.UI.Image>().fillAmount = 1;
				}
				else
				{
					//print("INSIDE");
					_manager.StartClient(discoveredServers.Values.First().uri);
					foundServer = false;
				}
			}

			advance = false;
		}

		///button color animation here
		if (pointerDown)
		{
			//Debug.Log("Right Clicked on " + this.name);
			clickTimer -= Time.deltaTime;
			uniBtn.GetComponent<UnityEngine.UI.Image>().fillAmount -=
				Time.deltaTime / clickTimer;//image fill transition while holding

			//print(clickTimer);
			if (clickTimer < 0)
			{
				uniBtn.GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
				//while (clickTimer < 0.7f)
				//{
				//    uniBtn.GetComponent<UnityEngine.UI.Image>().fillAmount +=
				//Time.deltaTime / Time.deltaTime;
				//}

				pointerDown = false;
				//clickTimer = 0.7f;
				clickChanged = true;//the button got held till the text changed
				secondClick = false;
				if (unitText.text == "Server")
				{
					unitText.text = "Client";
					isServer = false;
					pointerUp = true;
					return;
					//print("switch");
				}
				else
				{
					unitText.text = "Server";
					isServer = true;
					pointerUp = true;
					return;
					//print("switch");
				}
			}
		}
		if (pointerUp)
		{
			clickTimer += Time.deltaTime;
			uniBtn.GetComponent<UnityEngine.UI.Image>().fillAmount +=
				clickTimer;
			//uniBtn.GetComponent<Button>().interactable = false;
			if (clickTimer > 0.4)
			{
				//uniBtn.GetComponent<Button>().interactable = true;
				pointerUp = false;
				clickTimer = 0.7f;
			}
		}
	}

	public void OnPointerUp()
	{
		if (touched)//if button down on wait..., so button up won't get triggered
		{
			touched = false;
			return;
		}
		if (timerIsRunning)//to avoid changing the button while in "Wait.."
			return;
		print("upppppp");
		pointerDown = false;
		pointerUp = true;

		if (!clickChanged)//if buttonUp and button text didn't change => reset vars (normal click)
		{
			clickTimer = 0.7f;
			advance = true;
		}
		if (clickChanged && secondClick)//buttonUp + text changed then 2nd click => advance = true
		{
			advance = true;
			clickChanged = false;
			secondClick = false;
		}
	}

	public void OnPointerDown()
	{
		if (pointerUp)
			return;
		if (timerIsRunning)
		{
			touched = true;//button down while on wait...
			return;
		}

		print("downnnnn");
		pointerDown = true;
		if (clickChanged)
			secondClick = true;
	}
}