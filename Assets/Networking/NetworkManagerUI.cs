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
        
        
        // https://gist.github.com/dantheman213/db3118bed76199186acf7be87af0c1c4
        // searches after an Ip from an avalibe Wi-fi eller Ethernet
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
                        //Debug.Log("IP Address:" + ip);
                        
=======
        // leder efter Ip'en for systemet for både Wi-fi eller Ethernet
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
>>>>>>> 935cf227f697d99d60b6580377b7504b95d36444

                    }
                }
            }
		foreach (NetworkInterface adapter in interfaces.Where(x => x.OperationalStatus == OperationalStatus.Up)) {
			if (adapter.Name.ToLower() == "ethernet" || adapter.Name.ToLower() == "wi-fi") {

			    IPInterfaceProperties props = adapter.GetIPProperties();
			    UnicastIPAddressInformation result = props.UnicastAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetwork);

			    if (result != null) {
				    ip = result.Address.ToString();
			    }
			}
	    }
    
        
        
        string[] args = System.Environment.GetCommandLineArgs();
        for(int i = 0; i < args.Length; i++) {
                NetworkManager.Singleton.StartClient();
            }
                UT.ConnectionData.Port = UInt16.Parse(port);
                UT.ConnectionData.Address = ip;
                NetworkManager.Singleton.StartServer();
            }
                UT.ConnectionData.Port = UInt16.Parse(port);
                UT.ConnectionData.Address = ip;
                NetworkManager.Singleton.StartHost();
            }
            
            
        }

        server.onClick.AddListener(() => {  // when cliced starts a server
            NetworkManager.Singleton.StartServer();
        });
        host.onClick.AddListener(() => { // when cliced starts a host 
            NetworkManager.Singleton.StartHost();
        });
        client.onClick.AddListener(() => { // when cliced starts a client
            NetworkManager.Singleton.StartClient();
        });
        disconnect.onClick.AddListener(() => { // when cliced starts shuts down server, host or client
           NetworkManager.Singleton.Shutdown();
        });

      
    }

    void Update(){
        // prints the Ip and Port in the console
        //Debug.Log("Ip:" + UT.ConnectionData.Address + " Port:" + UT.ConnectionData.Port);
    }

}


