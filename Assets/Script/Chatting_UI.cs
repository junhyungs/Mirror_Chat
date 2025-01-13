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
        if (!string.IsNullOrWhiteSpace(_chatMessage.text))
        {

        }
    }
}
