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
    internal static string _localPlayerName;

    public override void OnStartServer()
    {
        this.gameObject.SetActive(true);
        _connectionUserNameDictionary.Clear();
    }

    public override void OnStartClient()
    {
        this.gameObject.SetActive(true);
        _chatHistroy.text = string.Empty;
    }

    public void SetLocalUserName(string userName)
    {
        _localPlayerName = userName; //로컬 플레이어 이름 저장.
    }

    public void OnClickSendMessage()
    {
        var chatMessage = _chatMessage.text;

        if (!string.IsNullOrWhiteSpace(chatMessage))
        {
            CommandChatMessage(chatMessage.Trim());
        }
    }

    public void RemoveNameOnServerDisconnected(NetworkConnectionToClient clientNetworkInformation)
    {
        if (_connectionUserNameDictionary.ContainsKey(clientNetworkInformation))
        {
            _connectionUserNameDictionary.Remove(clientNetworkInformation);
        }
    }

    [Command(requiresAuthority = false)] //requiresAuthority = false 권한설정. 
    private void CommandChatMessage(string message, NetworkConnectionToClient sender = null)
    {
        //일반적으로 Command 메서드는 권한이 있는 사용자만 사용할 수 있다. 채팅은 권한과 관계없이 모든 유저가 보낼 수 있어야 하기 때문에 false로 설정하여 모든 유저가 사용할 수 있도록 함.
        //NetworkConnectionToClient sender = null NetworkConnectionToClient -> 서버 -> 클라이언트 연결을 나타내는 클래스. 
        //sender 매개변수는 호출자의 네트워크 연결 정보를 포함한다. 즉, 메시지를 보낸 클라이언트를 추적하거나, 해당 클라이언트의 이름을 찾아 메시지를 표시할 수 있다.
        //현재 null로 설정했지만 실제로 미러가 이 정보를 자동으로 채운다. 채팅을 친 클라이언트의 네트워크 정보를 미러가 자동으로 sender 매개변수에 담아서 서버로 전송한다.

        if(_connectionUserNameDictionary.ContainsKey(sender))
        {
            var user = sender.identity.GetComponent<User>();

            var userName = user._userName;

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
        string formatedMessage = (senderName == _localPlayerName) ?
            $"<color=red>{senderName}:</color>{message}" :
            $"<color=blue>{senderName}:</color>{message}";

        AppendMessage(formatedMessage); //AppendMessage 메서드도 빼고 딱 여기까지 서버에서 호출. 어트리뷰트가 사용된 메서드만 특정 처리가 되고 나머지 이어지는 메서드는 전부 로컬에서 실행.
    }

    private void AppendMessage(string message)
    {
        StartCoroutine(AppendAndScroll(message));
    }

    private IEnumerator AppendAndScroll(string message)
    {
        _chatHistroy.text += message + "\n";

        yield return null; // -> UI가 텍스트를 변경하고 업데이트 하는 시간을 보장. 
        yield return null; // -> 스크롤바의 상태가 완전히 반영되지 않는 경우를 대비. UI는 LateUpdate에서 업데이트 되기 때문에 한 프레임 더 대기하여 완전히 반영되도록 보장한다.
                           // -> 비동기로 처리되는 특성 때문에 발생되는 타이밍 문제를 해결하기 위해 프레임 대기를 2번.
        _chatScrollbar.value = 0; //스크롤 맨 아래로 이동.
    }

    public void OnClickExit()
    {
        NetworkManager.singleton.StopHost();
    }

    public void OnValueChanged_ToggleButton(string input)
    {
        _sendButton.interactable = !string.IsNullOrWhiteSpace(input);
    }

    public void OnReturnSendMessage(string input)
    {
        bool getKey = Input.GetKeyDown(KeyCode.Return)||
            Input.GetKeyDown(KeyCode.KeypadEnter)||
            Input.GetButtonDown("Submit");

        if (getKey)
        {
            OnClickSendMessage();
        }
    }
}
