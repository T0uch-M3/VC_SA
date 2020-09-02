// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
using System.ComponentModel;
using Mirror;
using UnityEngine;


/// <summary>
/// An extension for the NetworkManager that displays a default HUD for controlling the network state of the game.
/// <para>This component also shows useful internal state for the networking system in the inspector window of the editor. It allows users to view connections, networked objects, message handlers, and packet statistics. This information can be helpful when debugging networked games.</para>
/// </summary>
[AddComponentMenu("Network/NetworkManagerHUD")]
[RequireComponent(typeof(NewNetworkManager))]
[EditorBrowsable(EditorBrowsableState.Never)]
[HelpURL("https://mirror-networking.com/docs/Components/NetworkManagerHUD.html")]
public class CustomNetworkHUD : MonoBehaviour
{
    NewNetworkManager manager;

    /// <summary>
    /// Whether to show the default control HUD at runtime.
    /// </summary>
    public bool showGUI = true;

    /// <summary>
    /// The horizontal offset in pixels to draw the HUD runtime GUI at.
    /// </summary>
    public int offsetX;

    /// <summary>
    /// The vertical offset in pixels to draw the HUD runtime GUI at.
    /// </summary>
    public int offsetY;

    public int width;
    public int hight;
    public int fontSize;

    void Awake()
    {
        manager = GetComponent<NewNetworkManager>();
    }

    void OnGUI()
    {
        if (!showGUI)
            return;
        GUIStyle guiStyle = GUI.skin.GetStyle("Button");
        guiStyle.fontSize = fontSize;
        guiStyle.normal.textColor = Color.white;
        GUIStyle guiStyleTF = GUI.skin.GetStyle("TextField");
        guiStyleTF.fontSize = fontSize;
        guiStyleTF.normal.textColor = Color.white;
        GUIStyle guiStyleL = GUI.skin.GetStyle("Label");
        guiStyleL.fontSize = fontSize;
        guiStyleL.normal.textColor = Color.white;
        GUILayout.BeginArea(new Rect(10 + offsetX, 40 + offsetY, width, 9999));

        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (!NetworkClient.active)
            {
                // LAN Host
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (GUILayout.Button("LAN Host", guiStyle, GUILayout.Height(hight)))
                    {
                        manager.StartHost();
                    }
                }

                // LAN Client + IP
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("LAN Client", guiStyle, GUILayout.Height(hight)))
                {
                    manager.StartClient();
                }
                manager.networkAddress = GUILayout.TextField(manager.networkAddress, guiStyleTF, GUILayout.Height(hight));
                GUILayout.EndHorizontal();

                // LAN Server Only
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    // cant be a server in webgl build
                    GUILayout.Box("(  WebGL cannot be server  )");
                }
                else
                {
                    if (GUILayout.Button("LAN Server Only", guiStyle, GUILayout.Height(hight))) manager.StartServer();
                }
            }
            else
            {
                // Connecting
                GUILayout.Label("Connecting to " + manager.networkAddress + "..", guiStyleL, GUILayout.Height(hight));
                if (GUILayout.Button("Cancel Connection Attempt", guiStyle, GUILayout.Height(hight)))
                {
                    manager.StopClient();
                }
            }
        }
        else
        {
            // server / client status message
            if (NetworkServer.active)
            {
                GUILayout.Label("Server: active. Transport: " + Transport.activeTransport, guiStyleL, GUILayout.Height(hight));
            }
            if (NetworkClient.isConnected)
            {
                GUILayout.Label("Client: address=" + manager.networkAddress, guiStyleL, GUILayout.Height(hight));
            }
        }

        // client ready
        if (NetworkClient.isConnected && !ClientScene.ready)
        {
            if (GUILayout.Button("Client Ready", guiStyle, GUILayout.Height(hight)))
            {
                ClientScene.Ready(NetworkClient.connection);

                if (ClientScene.localPlayer == null)
                {
                    ClientScene.AddPlayer();
                }
            }
        }

        // stop
        if (NetworkServer.active || NetworkClient.isConnected)
        {
            if (GUILayout.Button("Stop", guiStyle, GUILayout.Height(hight)))
            {
                manager.StopHost();
            }
        }

        GUILayout.EndArea();
    }
}

