using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class _NetworkAuthenticator : NetworkAuthenticator
{
    public struct AuthRequestMessage : NetworkMessage //Ŭ���̾�Ʈ�� ������ ������ ��û�� �� ���Ǵ� ����ü. 
    {
        public string _authUserName; //Ŭ���̾�Ʈ�� ������ �����ϴ� ���� �̸�.
    }

    public struct AuthResiveMessage : NetworkMessage //������ Ŭ���̾�Ʈ���� ���� ����� ������ �� ���Ǵ� ����ü.
    {
        public byte _code; //���� ����� ��Ÿ���� ����. (Ex ���� = 0, ���� = 1)
        public string _message; //���� ��� �޽���.
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
        //������ Ŭ���̾�Ʈ�κ��� AuthRequestMessage�� ������ ���, OnAuthRequestMessage �޼��尡 ����ǵ��� �ϴ� �ڵ鷯 ���. false -> ���Ѽ���. ������ ��� �޽��� ó���� �������� �ǹ�.
        NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false); 
    }

    public override void OnStopServer()
    {

    }

    public override void OnServerAuthenticate(NetworkConnectionToClient clientNetworkInformation)
    {
        
    }

    public void OnAuthRequestMessage(NetworkConnectionToClient clientNetworkInformation, AuthRequestMessage message)
    {

    }

    private IEnumerator DelayedDisconnect(NetworkConnectionToClient clientNetworkInformation, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        yield return null;
    }
    #endregion
}
