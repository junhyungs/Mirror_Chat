using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class _NetworkManager : NetworkManager
{
    [Header("Login_PopUp")]
    [SerializeField] private Login_PopUp _login_PopUp;

    [Header("Chatting_UI")]
    [SerializeField] private Chatting_UI _chatting_UI; 

    public void OnValueChanged_SetHostName(string hostName)
    {
        this.networkAddress = hostName;
    }

    public override void OnServerDisconnect(NetworkConnectionToClient clientNetworkInformation)
    {
        _chatting_UI.RemoveNameOnServerDisconnected(clientNetworkInformation);

        base.OnServerDisconnect(clientNetworkInformation);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        _login_PopUp.SetUIOnClientDisconnected();
    }
}
