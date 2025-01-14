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

    //SyncDirection -> ServerToClient (�⺻) Ŭ���̾�Ʈ���� � �����͸� ó���ϰ� ������ ����ȭ�Ͽ� ������ ó���ϵ��� �ϰ� ���� ���� ClientToServer
    //SyncMode
    //Owner -> ������. �ش� NetworkIdentity�� ���� ��ü�� Ŭ���̾�Ʈ�� �ش� �����͸� �����ϰų� ����ȭ �� �� �ִ�. �ַ� Ŭ���̾�Ʈ�� �������� ��Ÿ���� ���� ���ȴ�.
    //�������� �� ���� ������ ���� ������, �ش� ���� ������ Ŭ���̾�Ʈ������ ����ȭ�� �Ͼ��. Observers ������ ��� Ŭ�󿡰� ����ȭ���� Owner�� �ش� Ŭ�󿡰Ը� ����ȭ��� �������� �ִ�.
    //Observers -> ������. ���� �������� ����Ǿ��� �� ��� Ŭ���̾�Ʈ���� ����ȭ�ȴ�. ��ΰ� �� �� ����.

    public override void OnStartServer() //������ ���۵� ��.
    {
        _userName = (string)connectionToClient.authenticationData;
        //�Ʊ� _NetworkAuthenticator���� Ŭ���̾�Ʈ�� ������ ���� ��û�� ������ OnAuthRequestMessage �޼��尡 ����� ��
        //���� ������(����� �̸�)�� NetworkConnectionToClient.authenticationData�� ��������. �װ� �����ͼ� SyncVar�� ���� ��� Ŭ�󿡰� ���� ���� �̸� ����ȭ.
    }

    public override void OnStartLocalPlayer() //���� �÷��̾ ���۵� �� (���� Ŭ���̾�Ʈ���� ���� �� ������Ʈ�� Ȱ��ȭ �Ǿ��� ��)
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
