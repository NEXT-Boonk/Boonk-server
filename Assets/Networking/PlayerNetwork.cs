using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNetwork : NetworkBehaviour
{

    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


//Dette er en struct, den minder om en class, men er det ikke
    public struct MyCustomData {
        public int _int;
        public bool _bool;

    }

        private NetworkVariable<MyCustomData> customNumber = new NetworkVariable<MyCustomData>(
        new MyCustomData{
            _int = 51,
            _bool = true,
        }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

/*Denne del af koden vil kun sende det tilfældige nummer når nummeret ændrer sig, i stedet for hele tiden
    public override void OnNetworkSpawn() {
        randomNumber.OnValueChanged += (int previousValue, int newValue) => {
            Debug.Log(OwnerClientId + "number: " + randomNumber.Value);
        };
    }
    */
    public override void OnNetworkSpawn() {
        customNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) => {
        Debug.Log(OwnerClientId + "; " + newValue._int + " and it's " + newValue._bool);

        };
    }

    private void Update() {
        if(!IsOwner) return;
        Debug.Log(OwnerClientId + "number: " + randomNumber.Value);


        if(Input.GetKeyDown(KeyCode.T)){
            randomNumber.Value = Random.Range(0,100);
        }
        if(Input.GetKeyDown(KeyCode.Y)){
            if(customNumber.Value._int == 51){
            customNumber.Value = new MyCustomData{
                _int = 10,
                _bool = false,
            };
            } else {
            customNumber.Value = new MyCustomData{
                _int = 51,
                _bool = true,
            };

            }
        }

        Vector3 moveDir = new Vector3(0,0,0);


        if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed *Time.deltaTime;
    }
}
