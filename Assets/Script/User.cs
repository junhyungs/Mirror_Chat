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


}
