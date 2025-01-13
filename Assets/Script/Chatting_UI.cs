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

    public override void OnStartServer()
    {
        
    }

    public override void OnStartClient()
    {
        
    }
}
