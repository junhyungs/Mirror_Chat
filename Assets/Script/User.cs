using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class User : NetworkBehaviour
{
    [SyncVar]
    public string _userName;
    //SyncVar -> ���������� ����Ǵ� ��. Ŭ���̾�Ʈ�� ���Ƿ� ���� �� ���õǰų� ����ġ ���� ��Ȳ�� ��Ÿ�� �� ����.
    //�������� SyncVar ���� �����ϸ� �ڵ����� ��� Ŭ���̾�Ʈ���� ����ȭ ������ ��� Ŭ��� �׻� �ֽ� ������ ������ �� ����.
    //Hook -> [SyncVar(hook = nameof(OnUserNameChanged))] SyncVar ���� ����Ǹ� nameof�ȿ� �ִ� �޼��带 �ڵ����� ȣ����.
    //UI �ݿ� ���� �� ���� ���� �߰�ó�� ���� �κп� ���.


}
