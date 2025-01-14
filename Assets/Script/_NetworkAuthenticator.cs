using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class _NetworkAuthenticator : NetworkAuthenticator
{
    public struct AuthRequestMessage : NetworkMessage //클라이언트가 서버에 인증을 요청할 때 사용되는 구조체. 
    {
        public string _authUserName; //클라이언트가 서버에 전달하는 유저 이름.
    }

    public struct AuthResiveMessage : NetworkMessage //서버가 클라이언트에게 인증 결과를 응답할 때 사용되는 구조체.
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
        //서버가 클라이언트로부터 AuthRequestMessage를 수신할 경우, OnAuthRequestMessage 메서드가 실행되도록 하는 핸들러 등록. false -> 권한설정. 인증이 없어도 메시지 처리가 가능함을 의미.
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
