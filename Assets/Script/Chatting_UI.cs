using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Chatting_UI : NetworkBehaviour
{
    [Header("ChattingUI")]
    [SerializeField] private Text _chatHistroy;
    [SerializeField] private Scrollbar _chatScrollbar;
    [SerializeField] private InputField _chatMessage;
    [SerializeField] private Button _sendButton;

    internal static Dictionary<NetworkConnectionToClient, string> _connectionUserNameDictionary = new Dictionary<NetworkConnectionToClient, string>();

    public override void OnStartServer()
    {
        _connectionUserNameDictionary.Clear();
    }

    public override void OnStartClient()
    {
        _chatHistroy.text = string.Empty;
    }

    public void OnClickSendMessage()
    {
        var chatMessage = _chatMessage.text;

        if (!string.IsNullOrWhiteSpace(chatMessage))
        {
            CommandChatMessage(chatMessage.Trim());
        }
    }

    [Command(requiresAuthority = false)] //requiresAuthority = false ���Ѽ���. 
    private void CommandChatMessage(string message, NetworkConnectionToClient sender = null)
    {
        //�Ϲ������� Command �޼���� ������ �ִ� ����ڸ� ����� �� �ִ�. ä���� ���Ѱ� ������� ��� ������ ���� �� �־�� �ϱ� ������ false�� �����Ͽ� ��� ������ ����� �� �ֵ��� ��.
        //NetworkConnectionToClient sender = null NetworkConnectionToClient -> Ŭ���̾�Ʈ - ���� ������ ��Ÿ���� Ŭ����. 
        //sender �Ű������� ȣ������ ��Ʈ��ũ ���� ������ �����Ѵ�. ��, �޽����� ���� Ŭ���̾�Ʈ�� �����ϰų�, �ش� Ŭ���̾�Ʈ�� �̸��� ã�� �޽����� ǥ���� �� �ִ�.
        //���� null�� ���������� ������ �̷��� �� ������ �ڵ����� ä���. ä���� ģ Ŭ���̾�Ʈ�� ��Ʈ��ũ ������ �̷��� �ڵ����� sender �Ű������� ��Ƽ� ������ �����Ѵ�.

        if(_connectionUserNameDictionary.ContainsKey(sender))
        {
            var user = sender.identity.GetComponent<User>();

            var userName = user.UserName;

            _connectionUserNameDictionary.Add(sender, userName);
        }

        if(!string.IsNullOrWhiteSpace(message))
        {
            var userName = _connectionUserNameDictionary[sender];

            OnResiveRPCMessage(userName, message);
        }
    }

    [ClientRpc]
    private void OnResiveRPCMessage(string senderName, string message)
    {

    }
}
