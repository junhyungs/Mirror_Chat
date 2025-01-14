using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class User : NetworkBehaviour
{
    [SyncVar]
    public string _userName;
    //SyncVar -> 서버에서만 변경되는 값. 클라이언트가 임의로 변경 시 무시되거나 예기치 못한 상황이 나타날 수 있음.
    //서버에서 SyncVar 값을 변경하면 자동으로 모든 클라이언트에게 동기화 됨으로 모든 클라는 항상 최신 정보를 유지할 수 있음.
    //Hook -> [SyncVar(hook = nameof(OnUserNameChanged))] SyncVar 값이 변경되면 nameof안에 있는 메서드를 자동으로 호출함.
    //UI 반영 같은 값 변경 이후 추가처리 같은 부분에 사용.

    //SyncDirection -> ServerToClient (기본) 클라이언트에서 어떤 데이터를 처리하고 서버로 동기화하여 서버가 처리하도록 하고 싶을 때는 ClientToServer
    //SyncMode
    //Owner -> 소유자. 해당 NetworkIdentity를 가진 객체의 클라이언트만 해당 데이터를 변경하거나 동기화 할 수 있다. 주로 클라이언트의 소유권을 나타내는 값에 사용된다.
    //서버에서 이 값을 변경할 수는 있으나, 해당 값을 소유한 클라이언트에서만 동기화가 일어난다. Observers 설정은 모든 클라에게 동기화지만 Owner는 해당 클라에게만 동기화라는 차이점이 있다.
    //Observers -> 관찰자. 값이 서버에서 변경되었을 때 모든 클라이언트에게 동기화된다. 모두가 알 수 있음.

    public override void OnStartServer() //서버가 시작될 때.
    {
        _userName = (string)connectionToClient.authenticationData;
        //아까 _NetworkAuthenticator에서 클라이언트가 서버로 인증 요청을 보내고 OnAuthRequestMessage 메서드가 실행될 때
        //인증 데이터(사용자 이름)를 NetworkConnectionToClient.authenticationData에 저장했음. 그걸 가져와서 SyncVar를 통해 모든 클라에게 현재 유저 이름 동기화.
    }

    public override void OnStartLocalPlayer() //로컬 플레이어가 시작될 때 (로컬 클라이언트에서 현재 이 오브젝트가 활성화 되었을 때)
    {
        var objChattingUI = GameObject.Find("ChattingUI_Canvas");

        if(objChattingUI != null)
        {
            var chattingUIComponent = objChattingUI.GetComponent<Chatting_UI>();

            if(chattingUIComponent != null)
            {
                chattingUIComponent.SetLocalUserName(_userName);
            }
        }
    }
}
