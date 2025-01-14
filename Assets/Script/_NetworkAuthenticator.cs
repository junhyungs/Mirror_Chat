using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class _NetworkAuthenticator : NetworkAuthenticator
{
    public struct AuthRequestMessage : NetworkMessage
    {
        public string _authUserName;
    }

    public struct AuthResiveMessage : NetworkMessage
    {
        public byte _code;
        public string _message;
    }

    #region Server
    //UnityEngin이 실행되는 시점에 자동으로 메서드를 호출한다.
    //RunTime에서 최초로 실행되는 메서드.
    //이 어트리뷰트를 적용한 메서드는 반드시 정적(static)이여야 하고, 인스턴스 메서드에는 사용할 수 없다.
    //호출 시점에서 매개변수를 받을 수 없으며 반드시 반환값이 void여야한다.
    [UnityEngine.RuntimeInitializeOnLoadMethod]
    private static void ResetStatics() { }

    public override void OnStartServer()
    {
        
    }

    public override void OnStopServer()
    {

    }

    public override void OnServerAuthenticate(NetworkConnectionToClient clientNetworkInformation)
    {
        
    }

    public void OnAuthRequestMessage(NetworkConnectionToClient clientNetworkInformation)
    {

    }

    private IEnumerator DelayedDisconnect(NetworkConnectionToClient clientNetworkInformation, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        yield return null;
    }
    #endregion
}
