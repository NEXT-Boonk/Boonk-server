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
    [SerializeField]private Button Server;
    [SerializeField]private Button Host;
    [SerializeField]private Button Client;
    [SerializeField]private Button Disconnect;

    UnityTransport UT;
    string ip;
    
    //UnityTransport UT;

    private void Awake(){
        
        UT = FindObjectOfType<UnityTransport>();
        //UT.ConnectionData.Address = "127.0.0.1";
        
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
    
        //UT.ConnectionData.Address = ip;

        string[] args = System.Environment.GetCommandLineArgs();
        for(int i = 0; i < args.Length; i++) {
            if(args[i] == "-launch-as-client"){
                NetworkManager.Singleton.StartClient();
            }
            else if(args[i] == "-launch-as-server"){
                UT.ConnectionData.Address = ip;
                NetworkManager.Singleton.StartServer();
            }
            
        }

        Server.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
        });
        Host.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });
        Client.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
        Disconnect.onClick.AddListener(() => {
           NetworkManager.Singleton.Shutdown();
        });

      
    }

    void Update() {
            
      
    }

}
