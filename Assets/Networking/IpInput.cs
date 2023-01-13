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
    [SerializeField]private TMP_InputField Ip;
    [SerializeField]private TMP_InputField Port;
    private string Input;
    UnityTransport UT;
    TMP_InputField Ip1;
    TMP_InputField Port1;
    

    // Start is called before the first frame update
    void Awake()
    {
        UT = GetComponent<UnityTransport>();
        Port.characterLimit = 4;
        Port.contentType = TMP_InputField.ContentType.IntegerNumber;
        Port.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric;
        
        //Ip1 =  GameObject.Find("Ip").GetComponent<TMP_InputField>();
        //Port1 =  GameObject.Find("Port").GetComponent<TMP_InputField>();
        //textField = FindObjectOfType<TMP_InputField>();

    }

    // Update is called once per frame
    void Update()
    {

        UT.ConnectionData.Address = Ip.text;
        if(Port.text.Length > 0 && Port.text.Length <= 4){
        UT.ConnectionData.Port = UInt16.Parse(Port.text);
       }
       // UT.ConnectionData.Port = Port.text;
      //if (Input.GetKey(KeyCode.G)) UT.connectionData.Address = "10.78.64.202";
        
    }

   
}
