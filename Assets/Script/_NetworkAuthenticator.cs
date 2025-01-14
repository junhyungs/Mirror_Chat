using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class _NetworkAuthenticator : NetworkAuthenticator
{
    private readonly HashSet<NetworkConnection> _activeConnectionsSet = new HashSet<NetworkConnection>();
    internal static readonly HashSet<string> _userNames = new HashSet<string>();

    //readonly -> �ʵ尡 �� �� �ʱ�ȭ �� ���Ŀ��� ���� ������� �ʵ��� ������.

    //NetworkConnection -> Ŭ��� ���� ���� ������ ����, ������ �ۼ����� ���. �� ������ ������ NetworkConnection�� ������. ���� �̰� ���� �޽����� �����ų� ����� Ŭ���̾�Ʈ �ĺ��� ����.
    //NetworkConnectionToClient -> ���� -> Ŭ���̾�Ʈ�� ������ �����ϴ� Ŭ����. NetworkConnection�� ����ϸ� ���� -> Ŭ�� ������ ����, ���� -> Ŭ�� �������, Ư�� Ŭ������� ���ȴ�.
    //NetworkConnectionToServer -> Ŭ�� -> ���� ������ �����ϴ� Ŭ����. NetworkConnection�� ����ϸ� Ŭ�� -> ���� ������ ����, Ŭ�� -> ���� ��������� ���ȴ�.

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
        if(_activeConnectionsSet.Contains(clientNetworkInformation)) //�ߺ��� ���� ����. �̹� ������ Ŭ���̾�Ʈ�� ���� ��û�� �����ϰ� �ִٸ� �����Ͽ� �ٽ� ȣ��Ǵ� ���� �����Ѵ�.
        {
            return;
        }

        if (!_userNames.Contains(message._authUserName)) //���� ���� �̸��� �����ϴ� �ؽ��¿� ��û�� �̸��� ������
        {
            _userNames.Add(message._authUserName); //�ؽ��¿� ���� ���.

            clientNetworkInformation.authenticationData = message._authUserName; //authenticationData(������ ������)�� ���� �̸��� �����Ͽ� ���� ���� ���¸� ������ �� �ֵ�����.

            AuthResiveMessage authResiveMessage = new AuthResiveMessage() //���� ���� ����ü�� ���� Ŭ���̾�Ʈ���� ���� ���� �޽����� ���� �غ�.
            {
                _code = 100,
                _message = "Auth Success"
            };

            clientNetworkInformation.Send(authResiveMessage); //NetworkConnectionToClient.Send( ) �޼��带 ����Ͽ� Ŭ���̾�Ʈ���� ���� ���� �޽��� ����.

            ServerAccept(clientNetworkInformation); //Ŭ���̾�Ʈ ���� ���� ó��. ������ ������ Ŭ���̾�Ʈ�� ������ ������ ����ڷ� ����. �� �������� ������ ���� Ŭ���̾�Ʈ���� ������ ����.
        }
        else
        {

        }
    }

    private IEnumerator DelayedDisconnect(NetworkConnectionToClient clientNetworkInformation, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        yield return null;
    }
    #endregion
}
