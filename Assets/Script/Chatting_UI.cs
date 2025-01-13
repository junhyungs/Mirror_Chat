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

    [Command(requiresAuthority = false)] //requiresAuthority = false 권한설정. 
    private void CommandChatMessage(string message, NetworkConnectionToClient sender = null)
    {
        //일반적으로 Command 메서드는 권한이 있는 사용자만 사용할 수 있다. 채팅은 권한과 관계없이 모든 유저가 보낼 수 있어야 하기 때문에 false로 설정하여 모든 유저가 사용할 수 있도록 함.
        //NetworkConnectionToClient sender = null NetworkConnectionToClient -> 클라이언트 - 서버 연결을 나타내는 클래스. 
        //sender 매개변수는 호출자의 네트워크 연결 정보를 포함한다. 즉, 메시지를 보낸 클라이언트를 추적하거나, 해당 클라이언트의 이름을 찾아 메시지를 표시할 수 있다.
        //현재 null로 설정했지만 실제로 미러가 이 정보를 자동으로 채운다. 채팅을 친 클라이언트의 네트워크 정보를 미러가 자동으로 sender 매개변수에 담아서 서버로 전송한다.

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
