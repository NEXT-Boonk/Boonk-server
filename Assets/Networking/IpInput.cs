using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System; 



public class IpInput : MonoBehaviour
{
    
    [SerializeField]private TMP_InputField Ip; // makes a field in the Unity editor for the Input field(text mesh pro)
    [SerializeField]private TMP_InputField Port;
    private string Input;
    UnityTransport UT;
    

    void Awake()
    {
        UT = GetComponent<UnityTransport>(); // finds the UnityTransport component
        Port.characterLimit = 4; // makes the character limit of the input field 4
        Port.contentType = TMP_InputField.ContentType.IntegerNumber; // makes the content of the input feild only allow Integer numbers
        Port.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric; // makes the port only accept Alphanumeric values
        

    }
        
    public void InputIp(){

        UT.ConnectionData.Address = Ip.text; //sets the Ip to the string in the input field
        
    }
    
    public void InputPort(){
    if(Port.text.Length > 0 && Port.text.Length <= 4){
        UT.ConnectionData.Port = UInt16.Parse(Port.text); //sets the Port to a 4 digit number        
    }
}
}
