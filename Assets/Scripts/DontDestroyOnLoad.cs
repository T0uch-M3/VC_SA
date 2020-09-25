using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.Discovery;
using Mirror.Websocket;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class DontDestroyOnload : MonoBehaviour
{
    //will be used for things related to networking later
    private NewNetworkManager _manager;

    //Since dontdestroyonload will produce duplicate objects, a fix is needed
    //And this static instance var of this class will be used for that 
    public static DontDestroyOnload Instance;
    public GameObject uniBtn;
    public GameObject eventSystem;
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    public NetworkDiscovery networkDiscovery;

    private bool isEditor = Application.platform == RuntimePlatform.WindowsEditor;
    private bool isAndroid = Application.platform == RuntimePlatform.Android;
    private bool isWindows = Application.platform == RuntimePlatform.WindowsPlayer;

    public float timeRemaining = 5;
    public bool timerIsRunning = false;
    public bool foundServer = false;
    private TextMeshProUGUI unitText;
    public bool isServer = true;
    public static bool disconnected = false;
    public UnityEvent uEvent;

    bool pointerDown = false;
    float clickTimer = 0.6f;
    bool clickChanged = false;
    bool advance = false;
    bool secondClick = false;
    void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(this.gameObject);
        //}
        //else if (Instance != this)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}
        _manager = GameObject.Find("NetworkManager").GetComponent<NewNetworkManager>();
        networkDiscovery = _manager.GetComponent<NetworkDiscovery>();

    }
#if UNITY_EDITOR
    void OnValidate()
    {
        if (networkDiscovery == null)
        {
            networkDiscovery = GetComponent<NetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound,
                OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif
    // Start is called before the first frame update
    void Start()
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
            unitText.text = "Client";
        }
        else
        {
            unitText.text = "Server";
        }


        networkDiscovery.OnServerFound.AddListener(OnDiscoveredServer);

        //timerIsRunning = true;
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        print("INSIDE OnDiscoveredServer");
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        discoveredServers[info.serverId] = info;
    }

    // Update is called once per frame
    void Update()
    {
        if (disconnected)
        {
            print("STooooooooooooooooooooooooooooooooOP DISCOVERY");
            networkDiscovery.StopDiscovery();
            disconnected = false;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            //print("SERVERS::: " + discoveredServers.Count);
            //networkDiscovery.StopDiscovery();
            //Destroy(GameObject.Find("NetworkManager"));
            //networkDiscovery.StopDiscovery();
            //_manager.gameObject.AddComponent<NetworkDiscovery>();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //print("SERVERS::: " + discoveredServers.Count);
            networkDiscovery.StartDiscovery();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            //print("SERVERS::: " + discoveredServers.Count);
            networkDiscovery.StopDiscovery();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //print("SERVERS::: " + discoveredServers.Count);
            //networkDiscovery.AdvertiseServer();
            //OnDiscoveredServer();
        }

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                if (discoveredServers.Count > 0)
                {
                    print("SERVERS::: " + discoveredServers.Count);
                    timerIsRunning = false;
                    uniBtn.GetComponent<Button>().interactable = true;
                    //unitText.text = discoveredServers.Count.ToString();
                    unitText.text = "Connect?";
                    foundServer = true;
                }
            }
            else
            {
                print("Found no servers");
                //print("Servers found = " + discoveredServers.Count);
                timerIsRunning = false;
                uniBtn.GetComponent<Button>().interactable = true;
                unitText.text = "Try Again";
            }
        }

        if (advance)
        {
            //if (/*isEditor || */isWindows)
            if (isServer)
            {
                print("SERVVVVVVVVVVVVER");
                discoveredServers.Clear();
                _manager.StartHost();
                networkDiscovery.AdvertiseServer();
                //this.gameObject.GetComponent<Canvas>().sortingOrder--;

            }
            else
            {
                if (!foundServer)
                {
                    print("CLIIIIIIIIIIIIIIENT");
                    discoveredServers.Clear();
                    networkDiscovery.StartDiscovery();
                    timerIsRunning = true;
                    timeRemaining = 5;
                    unitText.text = "Wait..";
                    uniBtn.GetComponent<Button>().interactable = false;
                    //uniBtn.SetActive(false);}
                }
                else
                {
                    print("INNNNNNNNNNSIDEEE");
                    _manager.StartClient(discoveredServers.Values.First().uri);
                    //networkDiscovery.StopDiscovery();
                    foundServer = false;
                    //discoveredServers.Clear();
                    //SceneManager.LoadScene(1);
                }
            }


        }

        if (pointerDown)
        {
            //Debug.Log("Right Clicked on " + this.name);
            clickTimer -= Time.deltaTime;
            //print(clickTimer);
            if (clickTimer < 0)
            {
                pointerDown = false;
                clickTimer = 0.7f;
                clickChanged = true;
                secondClick = false;
                if (unitText.text == "Server")
                {
                    unitText.text = "Client";
                    isServer = false;
                    print("switch");
                }
                else
                {
                    unitText.text = "Server";
                    isServer = true;
                    print("switch");
                }
            }

        }
        if (advance)
        {
            print("ADVANCE " + unitText.text);
            advance = false;
        }
    }

    public void OnPointerUp()
    {
        pointerDown = false;
        if (!clickChanged)
        {
            clickTimer = 0.7f;
            advance = true;
        }
        if (clickChanged && secondClick)
        {
            advance = true;
            clickChanged = false;
            secondClick = false;
        }
    }
    public void OnPointerDown()
    {
        pointerDown = true;
        if (clickChanged)
            secondClick = true;

    }

}