using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Mirror.Discovery;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class DontDestroyOnLoad : MonoBehaviour
{
    //will be used for things related to networking later
    private NewNetworkManager _manager;

    //Since dontdestroyonload will produce duplicate objects, a fix is needed
    //And this static instance var of this class will be used for that 
    public static DontDestroyOnLoad Instance;
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
    }
#if UNITY_EDITOR
    void OnValidate()
    {
        if (networkDiscovery == null)
        {
            networkDiscovery = GetComponent<NetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound,
                OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects(new Object[] {this, networkDiscovery}, "Set NetworkDiscovery");
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

        //timerIsRunning = true;
    }

    public void uniBtnClick()
    {
        //print("PRESSED");
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        // Note that you can check the versioning to decide if you can connect to the server or not using this method
        discoveredServers[info.serverId] = info;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                if (discoveredServers.Count > 0)
                {
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

        if (Input.GetMouseButtonDown(0) &&
            EventSystem.current.currentSelectedGameObject == uniBtn)
        {
            //if (/*isEditor || */isWindows)
            if (isServer)
            {
                discoveredServers.Clear();
                _manager.StartHost();
                networkDiscovery.AdvertiseServer();
                //this.gameObject.GetComponent<Canvas>().sortingOrder--;

            }
            else
            {
                if (!foundServer)
                {
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
                    _manager.StartClient(discoveredServers.Values.First().uri);
                    //SceneManager.LoadScene(1);
                }
            }

            
        }
        //if (NewNetworkManager.clientLoaded)
        //{

        //    unitText.text = "Start";
        //}

        if (Input.GetMouseButtonDown(1))
            //EventSystem.current.currentSelectedGameObject == uniBtn)
        {
            if (unitText.text == "Server")
            {
                unitText.text = "Client";
                isServer = false;
            }
            else
            {
                unitText.text = "Server";
                isServer = true;
            }
        }

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    SceneManager.LoadScene(0);
        //    this.gameObject.GetComponent<Canvas>().sortingOrder++;
        //}
    }

}