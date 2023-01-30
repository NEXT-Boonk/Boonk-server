using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Net.NetworkInformation;
using Unity.Netcode.Transports.UTP;
using System; 
using System.Linq;
using System.Net.Sockets;





public class NetworkManagerUI : MonoBehaviour
{
    // makes a field in the Unity editor for the Buttons
    [SerializeField]private Button server;
    [SerializeField]private Button host;
    [SerializeField]private Button client;
    [SerializeField]private Button disconnect;

    // Deifining Unity transport
    UnityTransport UT;
    string ip;
    string port = "9999";
    
    //UnityTransport UT;

    private void Awake(){
        
        UT = FindObjectOfType<UnityTransport>(); // finder Unity Transport 
        //UT.ConnectionData.Address = "127.0.0.1";
        
        // Lavet af dantheman213
        //https://gist.github.com/dantheman213/db3118bed76199186acf7be87af0c1c4
        // leder efter Ip'en for systemet for både Wi-fi eller Ethernet
        var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var adapter in interfaces.Where(x => x.OperationalStatus == OperationalStatus.Up))
            {
                if (adapter.Name.ToLower() == "ethernet" || adapter.Name.ToLower() == "wi-fi")
                {
                    var props = adapter.GetIPProperties();
                    var result = props.UnicastAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetwork);
                    if (result != null)
                    {
                       
                        ip = result.Address.ToString();
                        Debug.Log("IP Address:" + ip);
                        

                    }
                }
            }
    
        
        //gør at programmet kan køres med et argument som -launch-as-server
        string[] args = System.Environment.GetCommandLineArgs();
        for(int i = 0; i < args.Length; i++) {
            if(args[i] == "-launch-as-client"){ //køre programmet som en client
                NetworkManager.Singleton.StartClient();
            }
            else if(args[i] == "-launch-as-server"){ //køre programmet som en server med local Ip'en af det netværk systemet er forbundet til med porten 60000
                UT.ConnectionData.Port = UInt16.Parse(port);
                UT.ConnectionData.Address = ip;
                NetworkManager.Singleton.StartServer();
            }
            else if(args[i] == "-launch-as-host"){ //køre programmet som en host med local Ip'en af det netværk systemet er forbundet til med porten 60000
                UT.ConnectionData.Port = UInt16.Parse(port);
                UT.ConnectionData.Address = ip;
                NetworkManager.Singleton.StartHost();
            }
            
            
        }

        server.onClick.AddListener(() => {  // gør at når man trykker på knappen "Server" starter programet en server 
            NetworkManager.Singleton.StartServer();
        });
        host.onClick.AddListener(() => { // gør at når man trykker på knappen "Host" starter programet en host 
            NetworkManager.Singleton.StartHost();
        });
        client.onClick.AddListener(() => { // gør at når man trykker på knappen "Client" starter programet en client 
            NetworkManager.Singleton.StartClient();
        });
        disconnect.onClick.AddListener(() => { // gør at når man trykker på knappen "Disconnect" lukker programmet for sin instance af en client, host eller server
           NetworkManager.Singleton.Shutdown();
        });

      
    }

    void Update(){
        // sender Ip'en og porten til console logen og error logen i dev-buildet
        Debug.Log("Ip:" + UT.ConnectionData.Address + " Port:" + UT.ConnectionData.Port);
        Debug.LogError("Ip:" + UT.ConnectionData.Address + " Port:" + UT.ConnectionData.Port);

    }

}


