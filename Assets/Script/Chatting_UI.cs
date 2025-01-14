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
        _localPlayerName = userName; //���� �÷��̾� �̸� ����.
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

    [Command(requiresAuthority = false)] //requiresAuthority = false ���Ѽ���. 
    private void CommandChatMessage(string message, NetworkConnectionToClient sender = null)
    {
        //�Ϲ������� Command �޼���� ������ �ִ� ����ڸ� ����� �� �ִ�. ä���� ���Ѱ� ������� ��� ������ ���� �� �־�� �ϱ� ������ false�� �����Ͽ� ��� ������ ����� �� �ֵ��� ��.
        //NetworkConnectionToClient sender = null NetworkConnectionToClient -> ���� -> Ŭ���̾�Ʈ ������ ��Ÿ���� Ŭ����. 
        //sender �Ű������� ȣ������ ��Ʈ��ũ ���� ������ �����Ѵ�. ��, �޽����� ���� Ŭ���̾�Ʈ�� �����ϰų�, �ش� Ŭ���̾�Ʈ�� �̸��� ã�� �޽����� ǥ���� �� �ִ�.
        //���� null�� ���������� ������ �̷��� �� ������ �ڵ����� ä���. ä���� ģ Ŭ���̾�Ʈ�� ��Ʈ��ũ ������ �̷��� �ڵ����� sender �Ű������� ��Ƽ� ������ �����Ѵ�.

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

        AppendMessage(formatedMessage); //AppendMessage �޼��嵵 ���� �� ������� �������� ȣ��. ��Ʈ����Ʈ�� ���� �޼��常 Ư�� ó���� �ǰ� ������ �̾����� �޼���� ���� ���ÿ��� ����.
    }

    private void AppendMessage(string message)
    {
        StartCoroutine(AppendAndScroll(message));
    }

    private IEnumerator AppendAndScroll(string message)
    {
        _chatHistroy.text += message + "\n";

        yield return null; // -> UI�� �ؽ�Ʈ�� �����ϰ� ������Ʈ �ϴ� �ð��� ����. 
        yield return null; // -> ��ũ�ѹ��� ���°� ������ �ݿ����� �ʴ� ��츦 ���. UI�� LateUpdate���� ������Ʈ �Ǳ� ������ �� ������ �� ����Ͽ� ������ �ݿ��ǵ��� �����Ѵ�.
                           // -> �񵿱�� ó���Ǵ� Ư�� ������ �߻��Ǵ� Ÿ�̹� ������ �ذ��ϱ� ���� ������ ��⸦ 2��.
        _chatScrollbar.value = 0; //��ũ�� �� �Ʒ��� �̵�.
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
