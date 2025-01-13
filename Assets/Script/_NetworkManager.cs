using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class _NetworkManager : NetworkManager
{
    public _NetworkManager Instance { get; private set; }
    public Chatting_UI Chatting_UI { get; set; }

    public override void Awake()
    {
        Instance = this;
    }

    public void OnValueChanged_SetHostName(string hostName)
    {
        this.networkAddress = hostName;
    }

    public override void OnServerDisconnect(NetworkConnectionToClient clientNetworkInformation)
    {
        base.OnServerDisconnect(clientNetworkInformation);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        Login_PopUp.Instance.SetUIOnClientDisconnected();
    }
}
