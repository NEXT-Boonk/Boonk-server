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
    
    //public UnityTransport placeholderName;
    [SerializeField]private TMP_InputField Ip; // laver et felt i Scriptet for et Input field(text mesh pro)
    [SerializeField]private TMP_InputField Port;
    private string Input;
    UnityTransport UT;
    

    // Start is called before the first frame update
    void Awake()
    {
        UT = GetComponent<UnityTransport>(); // finder UnityTransport
        Port.characterLimit = 5; // gør at input feltet "Port" har en char limit på 5 
        Port.contentType = TMP_InputField.ContentType.IntegerNumber; // gør at input feltet "Port" kun kan bruge Integers
        Port.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric; // gør at input feltet "Port" kun kan accepterer Integers
        

    }
        
    public void InputIp(){

        UT.ConnectionData.Address = Ip.text; //sætter stringen i Inputfeltet "Ip" til Ip adressen programmet bruger
        
    }
    
    public void InputPort(){
    if(Port.text.Length > 0 && Port.text.Length <= 4){
        UT.ConnectionData.Port = UInt16.Parse(Port.text); //sætter stringen i Inputfeltet "Port" og parser den til en UInt16 som programmet bruger
        
    }
}
}
