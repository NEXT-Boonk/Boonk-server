using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class PlayerNetwork : NetworkBehaviour
{

Material teamColor;
[SerializeField]private Material team1;
[SerializeField]private Material team2;

public GameObject[] players;

public List<GameObject> playersList = new List<GameObject>();

/*This is a variable that is sent over the network, to change the type of variable, you can change the "int" to "float", "ensum", "bool", "struct". All value types, refrence type variables are not able to used with this.
https://www.youtube.com/watch?v=3yuBOB3VrCk&t=1487s&ab_channel=CodeMonkey
"NetworkVariableWritePermission.Owner" means that the client is able to change the variable, change this to server*/
    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> team = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    
    

//This is a struct, a refrence variable, not definable using the method above
    public struct MyCustomData: INetworkSerializable {
        public int _int;
        public bool _bool;

        public void NetworkSerialize<T>(BufferSerializer<T>serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
        }
    }


        /*This method can be used to define refrence variables, refrence variables are variables like "class", "Object", "array" and "string" 
        among others. To refrence one of these, replace MyCustomData with the name of the refrence type one has already defined above.
        */
        private NetworkVariable<MyCustomData> customNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData{
            _int = 51,
            _bool = true,
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        /*This code will send a random number when the value changes, not at all times, given the "OnValueChanged" part of the code
        public override void OnNetworkSpawn() {
        randomNumber.OnValueChanged += (int previousValue, int newValue) => {
            Debug.Log(OwnerClientId + "number: " + randomNumber.Value);
        };
    }
    */

    //This will send the struct defined above when one of it's values changes
    public override void OnNetworkSpawn() {
        if(IsOwner){ playerAddServerRpc(new ServerRpcParams()); }

        if(team.Value){
            GetComponentInChildren<MeshRenderer>().material = team1;
        }
        else{
           GetComponentInChildren<MeshRenderer>().material = team2; 
        }
        customNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) => {
        Debug.Log(OwnerClientId + "; " + newValue._int + " and it's " + newValue._bool);
        };
    }

    public override void OnNetworkDespawn() {
        if(IsOwner){ playerRemoveServerRpc(new ServerRpcParams()); }

    }


    private void Update() {
        if(!IsOwner) return; //This checks if the code is not run by the player, if so it does nothing.
        //Debug.Log(OwnerClientId + "number: " + randomNumber.Value); //this code sends the command of the random number, which is sent at all times

        

        if(Input.GetKeyDown(KeyCode.T)){
            randomNumber.Value = Random.Range(0,100); //changes the random number
        }
        if(Input.GetKeyDown(KeyCode.Y)){
            if(customNumber.Value._int == 51){
            customNumber.Value = new MyCustomData{
                _int = 10,
                _bool = false,
            }; //sets a new struct
            } else {
            customNumber.Value = new MyCustomData{
                _int = 51,
                _bool = true,
            };

            }
        }
        //This code is connected to the code under the line "[ServerRpc]" further down
        if(Input.GetKeyDown(KeyCode.U)){
            testServerRpc(new ServerRpcParams());
        }

        //This is connected to the clientrpc further down
        if(Input.GetKeyDown(KeyCode.O)){
            //thanks to the parameter we only run the function on the client with the id of 1
            TestClientRpc(new ClientRpcParams {Send = new ClientRpcSendParams { TargetClientIds = new List<ulong>{1}}});
        }



        

        //The code below creates a simple movement system
        Vector3 moveDir = new Vector3(0,0,0);
        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed *Time.deltaTime;

          if(Input.GetKeyDown(KeyCode.N)){
            //changeTeamServerRpc(new ServerRpcParams());
            GetComponentInChildren<MeshRenderer>().material = team1;
             }
        else if(Input.GetKeyDown(KeyCode.M)){
            GetComponentInChildren<MeshRenderer>().material = team2;
        }

    }

   


    /*This is how to create a funktion that is run on the server, a serverRPC
    Note that it won't be run on the local client, but instead be run on the server
    If you wish to add parameters you will need to have them as value types, not refrence types

    You can track which client sent the code to the server, by putting a parameter of "serverRpcParams parameter name", and calling 
    Receive.SenderClientId, this gives you the id of the player sending the funktion, which could be used to identify where the effect should occur
    Note that one has to put "[ServerRpc]" right above the code
    */
    [ServerRpc]
    private void testServerRpc(ServerRpcParams Rpc){
        Debug.Log("server rpc working" + Rpc.Receive.SenderClientId);
    }

    [ServerRpc]
    private void playerAddServerRpc(ServerRpcParams Rpc) {
        players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++) {
            if(players[i].GetComponent<NetworkObject>().OwnerClientId == Rpc.Receive.SenderClientId){
                Debug.Log(playersList.Count);
                playersList.Add(players[i]);
            }      
        }
    }

    [ServerRpc]
    private void playerRemoveServerRpc(ServerRpcParams Rpc) {
        players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; i++) {
            if(players[i].GetComponent<NetworkObject>().OwnerClientId == Rpc.Receive.SenderClientId){
                Debug.Log(playersList.Count);
                playersList.Remove(players[i]);
            }      
        }
    }
/*
        players = GameObject.FindGameObjectsWithTag("Player");
        //Debug.Log(players.Length);
        Debug.Log(players[0].GetComponent<NetworkObject>().OwnerClientId);
        Debug.Log(GetComponentInChildren<NetworkObject>().OwnerClientId);
        Debug.Log("id lenght" + activePlayerIds.Length + ", player lenght" + players.Length);
        */
        //Rpc.Receive.SenderClientId
    /*
    A clientRpc is a function that the server activates that is then run on the clients instead of the server, opposite of a serverRpc.
    The parameter ClientRpcParams can be used to specifi a specific client to run the function on.
    This would f.eks. allow the server to tell a player that they have died and run the death command on it.
    */

    [ClientRpc]
    private void TestClientRpc(ClientRpcParams ClientRpcParams) {
        Debug.Log("ClientRPC");
    }

    [ClientRpc]
    private void showTeamClientRpc() {
        Debug.Log("ClientRPC");
    }
}
