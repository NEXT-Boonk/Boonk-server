using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNetwork : NetworkBehaviour
{

/*This is a variable that is sent over the network, to change the type of variable, you can change the "int" to "float", "ensum", "bool", "struct". All value types, refrence type variables are not able to used with this.
https://www.youtube.com/watch?v=3yuBOB3VrCk&t=1487s&ab_channel=CodeMonkey
"NetworkVariableWritePermission.Owner" means that the client is able to change the variable, change this to server*/
    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


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

        /*This kode will send a random number when the value changes, not at all times, given the "OnValueChanged" part of the code
    public override void OnNetworkSpawn() {
        randomNumber.OnValueChanged += (int previousValue, int newValue) => {
            Debug.Log(OwnerClientId + "number: " + randomNumber.Value);
        };
    }
    */

    //This will send the struct defined above when one of it's values changes
    public override void OnNetworkSpawn() {
        customNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) => {
        Debug.Log(OwnerClientId + "; " + newValue._int + " and it's " + newValue._bool);

        };
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



        

        //The code below creates a simple movement system
        Vector3 moveDir = new Vector3(0,0,0);
        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed *Time.deltaTime;
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




}
