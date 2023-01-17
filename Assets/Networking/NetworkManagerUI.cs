using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;



public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField]private Button Server;
    [SerializeField]private Button Host;
    [SerializeField]private Button Client;
    [SerializeField]private Button Disconnect;
    
    //UnityTransport UT;

    private void Awake(){

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

}
