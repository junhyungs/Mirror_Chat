using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Login_PopUp : MonoBehaviour
{
    [Header("NetworkAddress")]
    [SerializeField] internal InputField _networkAddress; //internal - 같은 어셈블리(ex - nameSpace)내에서 public과 같은 동작. 단, 다른 어셈블리에서는 접근할 수 없다.
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

    private void SetDefaultNetworkAddress() //NetworkManager.singleton.networkAddress설정.
    {
        if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress)) //현재 네트워크 주소가 null, 빈 문자열, 공백인지 확인.
        {
            NetworkManager.singleton.networkAddress = "localhost"; //조건에 해당되면 주소를 "localHost"로 설정.
        }

        _originNetworkAddress = NetworkManager.singleton.networkAddress; //원본 네트워크 주소 저장.
    }

    private void CheckNetworkAddressOnUpdate() //Network 주소 변화 감지.
    {
        if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress)) //주소가 null, 빈 문자열, 공백이 되면 저장해둔 원본 주소로 설정.
        {
            NetworkManager.singleton.networkAddress = _originNetworkAddress;
        }

        if(_networkAddress.text != NetworkManager.singleton.networkAddress) //주소 입력 Text가 현재 주소와 같지 않다면 현재 주소로 설정.
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
