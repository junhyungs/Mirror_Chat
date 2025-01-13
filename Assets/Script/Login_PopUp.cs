using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Login_PopUp : MonoBehaviour
{
    [Header("NetworkAddress")]
    [SerializeField] internal InputField _networkAddress; //internal - ���� �����(ex - nameSpace)������ public�� ���� ����. ��, �ٸ� ����������� ������ �� ����.
    [SerializeField] internal InputField _userName;

    [SerializeField] internal Button _startHostButton;
    [SerializeField] internal Button _startClientButton;

    [SerializeField] internal Text _errorText;

    private string _originNetworkAddress;

    private void Awake()
    {
        _NetworkManager.Instance.Login_PopUp = this;
    }

    private void OnEnable()
    {
        _userName.onValueChanged.AddListener(OnValueChanged_ToggleButton);
    }

    private void OnDisable()
    {
        _userName.onValueChanged.RemoveListener(OnValueChanged_ToggleButton);
    }

    private void Start()
    {
        SetDefaultNetworkAddress();
    }

    private void Update()
    {
        CheckNetworkAddressOnUpdate();
    }

    private void SetDefaultNetworkAddress() //NetworkManager.singleton.networkAddress����.
    {
        if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress)) //���� ��Ʈ��ũ �ּҰ� null, �� ���ڿ�, �������� Ȯ��.
        {
            NetworkManager.singleton.networkAddress = "localhost"; //���ǿ� �ش�Ǹ� �ּҸ� "localHost"�� ����.
        }

        _originNetworkAddress = NetworkManager.singleton.networkAddress; //���� ��Ʈ��ũ �ּ� ����.
    }

    private void CheckNetworkAddressOnUpdate() //Network �ּ� ��ȭ ����.
    {
        if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress)) //�ּҰ� null, �� ���ڿ�, ������ �Ǹ� �����ص� ���� �ּҷ� ����.
        {
            NetworkManager.singleton.networkAddress = _originNetworkAddress;
        }

        if(_networkAddress.text != NetworkManager.singleton.networkAddress) //�ּ� �Է� Text�� ���� �ּҿ� ���� �ʴٸ� ���� �ּҷ� ����.
        {
            _networkAddress.text = NetworkManager.singleton.networkAddress;
        }
    }

    public void OnValueChanged_ToggleButton(string userName)
    {
        bool userNameValue = string.IsNullOrWhiteSpace(userName);

        _startHostButton.interactable = userNameValue;
        _startClientButton.interactable = userNameValue;
    }

    public void SetUIOnClientDisconnected()
    {
        this.gameObject.SetActive(true);
        _userName.text = string.Empty;
        _userName.ActivateInputField();
    }
}
