using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login_PopUp : MonoBehaviour
{
    [Header("NetworkAddress")]
    [SerializeField] internal InputField _networkAddress;
    [SerializeField] internal InputField _userName;

    [SerializeField] internal Button _startHostButton;
    [SerializeField] internal Button _startClientButton;

    [SerializeField] internal Text _errorText;

    public static Login_PopUp Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
