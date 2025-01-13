using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class _NetworkManager : NetworkManager
{
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
    }
}
