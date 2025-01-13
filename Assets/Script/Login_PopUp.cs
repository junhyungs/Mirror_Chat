using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Login_PopUp : MonoBehaviour
{
    [Header("NetworkAddress")]
    [SerializeField] internal InputField _networkAddress;
    [SerializeField] internal InputField _userName;

    [SerializeField] internal Button _startHostButton;
    [SerializeField] internal Button _startClientButton;

    [SerializeField] internal Text _errorText;

    public static Login_PopUp Instance { get; private set; }

    private string _originNetworkAddress;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetDefaultNetworkAddress();
    }

    private void SetDefaultNetworkAddress()
    {
        if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress))
        {
            NetworkManager.singleton.networkAddress = "localHost";
        }

        _originNetworkAddress = NetworkManager.singleton.networkAddress;
    }
}
