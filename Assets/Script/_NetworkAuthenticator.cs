using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class _NetworkAuthenticator : NetworkAuthenticator
{
    private readonly HashSet<NetworkConnection> _activeConnectionsSet = new HashSet<NetworkConnection>();
    internal static readonly HashSet<string> _userNames = new HashSet<string>();

    //readonly -> 필드가 한 번 초기화 된 이후에는 값이 변경되지 않도록 보장함.

    //NetworkConnection -> 클라와 서버 간의 연결을 추적, 데이터 송수신을 담당. 각 연결은 고유한 NetworkConnection을 가진다. 따라서 이걸 통해 메시지를 보내거나 연결된 클라이언트 식별이 가능.
    //NetworkConnectionToClient -> 서버 -> 클라이언트와 연결을 관리하는 클래스. NetworkConnection을 상속하며 서버 -> 클라 데이터 수신, 서버 -> 클라 연결관리, 특정 클라관리에 사용된다.
    //NetworkConnectionToServer -> 클라 -> 서버 연결을 관리하는 클래스. NetworkConnection을 상속하며 클라 -> 서버 데이터 수신, 클라 -> 서버 연결관리에 사용된다.

    public struct AuthRequestMessage : NetworkMessage //클라이언트가 서버에 인증을 요청할 때 사용되는 구조체(패킷). 
    {
        public string _authUserName; //클라이언트가 서버에 전달하는 유저 이름.
    }

    public struct AuthResiveMessage : NetworkMessage //서버가 클라이언트에게 인증 결과를 응답할 때 사용되는 구조체(패킷).
    {
        public byte _code; //인증 결과를 나타내는 변수. (Ex 성공 = 0, 실패 = 1)
        public string _message; //인증 결과 메시지.
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
        // 서버가 클라이언트로부터 AuthRequestMessage를 수신받았을 때 실행될 메서드(OnAuthRequestMessage)를 등록.
        // false -> 권한이 없어도 메시지 처리가 가능함을 의미.
        NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false); 
    }

    public override void OnStopServer()
    {
        NetworkServer.UnregisterHandler<AuthRequestMessage>();
    }

    public override void OnServerAuthenticate(NetworkConnectionToClient clientNetworkInformation)
    {
        
    }

    public void OnAuthRequestMessage(NetworkConnectionToClient clientNetworkInformation, AuthRequestMessage message)
    {
        if(_activeConnectionsSet.Contains(clientNetworkInformation)) //중복된 인증 방지. 이미 서버가 클라이언트의 인증 요청을 수행하고 있다면 리턴하여 다시 호출되는 것을 방지한다.
        {
            return;
        }

        if (!_userNames.Contains(message._authUserName)) //접속 유저 이름을 관리하는 해쉬셋에 요청자 이름이 없으면
        {
            _userNames.Add(message._authUserName); //해쉬셋에 유저 등록.

            clientNetworkInformation.authenticationData = message._authUserName; //authenticationData(인증자 데이터)에 유저 이름을 저장하여 이후 인증 상태를 추적할 수 있도록함.

            AuthResiveMessage authResiveMessage = new AuthResiveMessage() //인증 성공 구조체를 만들어서 클라이언트에게 인증 성공 메시지를 보낼 준비.
            {
                _code = 100,
                _message = "Auth Success"
            };

            clientNetworkInformation.Send(authResiveMessage); //NetworkConnectionToClient.Send( ) 메서드를 사용하여 클라이언트에게 인증 성공 메시지 전송.

            ServerAccept(clientNetworkInformation); //클라이언트 인증 성공 처리. 인증이 성공한 클라이언트를 서버가 인증된 사용자로 인정. 이 과정에서 권한이 없던 클라이언트에게 권한이 생김.
        }
        else
        {
            _activeConnectionsSet.Add(clientNetworkInformation); //연결 해제를 기다리는 클라이언트 관리 해쉬셋. 인증이 실패했기 때문에 이곳에 요청을 보낸 클라이언트 추가.

            AuthResiveMessage authResiveMessage = new AuthResiveMessage() //연결 실패 메시지 작성
            {
                _code = 200,
                _message = "User Name already is use! Try again"
            };

            clientNetworkInformation.Send(authResiveMessage); //실패 메시지 전송.

            clientNetworkInformation.isAuthenticated = false; //isAuthenticated를 false로 설정하여 실패처리.isAuthenticated -> 해당 클라이언트가 인증된 상태인지 여부를 나타내는 bool 값.

            StartCoroutine(DelayedDisconnect(clientNetworkInformation, 1.0f));
        }
    }

    private IEnumerator DelayedDisconnect(NetworkConnectionToClient clientNetworkInformation, float waitTime) //연결 해제 코루틴
    {
        yield return new WaitForSeconds(waitTime);

        ServerReject(clientNetworkInformation); //서버가 클라이언트의 요청을 거부하거나, 인증 실패 후 해당 클라이언트의 연결을 해제하기 위해 사용.

        yield return null;

        _activeConnectionsSet.Remove(clientNetworkInformation); //연결 해제를 대기하는 해쉬셋에서 삭제.
    }
    #endregion

    #region Client

    [Header("Login_PopUp")]
    [SerializeField] private Login_PopUp _loginPopUp;

    [Header("Client_UserName")]
    public string _clientUserName;

    public void OnInputValueChanged_SetUserName(string userName)
    {
        _clientUserName = userName;
        _loginPopUp.SetUIOnAuthValueChanged();
    }

    public override void OnStartClient()
    {
        // 클라이언트가 서버로부터 AuthResiveMessage를 수신받았을 때 실행될 메서드(OnAuthResponsMessage)를 등록.
        // false -> 권한이 없어도 메시지 처리가 가능함을 의미.
        NetworkClient.RegisterHandler<AuthResiveMessage>(OnAuthResponsMessage, false);
    }

    public override void OnStopClient()
    {
        NetworkClient.UnregisterHandler<AuthResiveMessage>();
    }

    public override void OnClientAuthenticate() //클라이언트에서 인증 요청 시 호출.
    {
        NetworkClient.Send(new AuthRequestMessage { _authUserName = _clientUserName });
    }

    public void OnAuthResponsMessage(AuthResiveMessage authResiveMessage)
    {
        if(authResiveMessage._code == 100)
        {
            ClientAccept(); //서버로부터 인증 승인 후 자신을 인증처리.
        }
        else
        {
            NetworkManager.singleton.StopHost(); //서버 호스트 중지.

            _loginPopUp.SetUIOnAuthError(authResiveMessage._message); //에러메시지 출력.
        }
    }
    #endregion
}
