using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;
using TMPro;


public class IpInput : MonoBehaviour
{
    
    //public UnityTransport placeholderName;
    private string Input;
    UnityTransport UT;
    TMP_InputField textField;
    

    // Start is called before the first frame update
    void Awake()
    {
        UT = GetComponent<UnityTransport>();
        textField = FindObjectOfType<TMP_InputField>();

    }

    // Update is called once per frame
    void Update()
    {

        UT.ConnectionData.Address = textField.text;
      //if (Input.GetKey(KeyCode.G)) UT.connectionData.Address = "10.78.64.202";
        
    }

   
}
