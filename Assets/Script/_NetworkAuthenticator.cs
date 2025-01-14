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
    //UnityEngin�� ����Ǵ� ������ �ڵ����� �޼��带 ȣ���Ѵ�.
    //RunTime���� ���ʷ� ����Ǵ� �޼���.
    //�� ��Ʈ����Ʈ�� ������ �޼���� �ݵ�� ����(static)�̿��� �ϰ�, �ν��Ͻ� �޼��忡�� ����� �� ����.
    //ȣ�� �������� �Ű������� ���� �� ������ �ݵ�� ��ȯ���� void�����Ѵ�.
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
