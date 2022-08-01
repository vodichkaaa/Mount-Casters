using System;
using TMPro;
using UnityEngine;

public class StartAmountController : MonoBehaviour
{
    public static StartAmountController Instance;
    
    [HideInInspector]
    public int amount = 1;
    private int _price = 100;
    private int _money;
    
    [SerializeField]
    private TextMeshProUGUI _amountText = null;
    [SerializeField]
    private TextMeshProUGUI _priceText = null;
    [SerializeField]
    private TextMeshProUGUI _moneyText = null;
    
    private AudioSource _audioSource = null;
    
    private event Action OnPlayerPrefsUpdate = delegate {  };

    private void OnEnable()
    {
        OnPlayerPrefsUpdate += SetPlayerPrefs;
        OnPlayerPrefsUpdate += UpdateUI;
    }

    private void OnDisable()
    {
        OnPlayerPrefsUpdate -= SetPlayerPrefs;
        OnPlayerPrefsUpdate -= UpdateUI;
    }

    private void Start()
    {
        /*_price = 100;
        amount = 1;
        OnPlayerPrefsUpdate();*/
        
        Instance = this;
        
        _audioSource = FindObjectOfType<AudioSource>();
        
        _price = PlayerPrefs.GetInt("price");
        amount = PlayerPrefs.GetInt("amount");
        _money = PlayerPrefs.GetInt("money");
        
        if (_price == 0 || amount == 0)
        {
            _price = 100;
            amount = 1;
            
            PlayerPrefs.Save();
        }
        OnPlayerPrefsUpdate();
    }
    
    public void BuyAmount()
    {
        if (_price <= _money)
        {
            _money -= _price;
            _price += 100;
            amount++;

            OnPlayerPrefsUpdate();
            FacebookManager.Instance.AmountCollected(amount);
            
            _audioSource.Play();
        } 
    }

    private void SetPlayerPrefs()
    {
        PlayerPrefs.SetInt("price", _price);
        PlayerPrefs.SetInt("amount", amount);
        PlayerPrefs.SetInt("money", _money);
    }
    private void UpdateUI()
    {
        _priceText.text = $"{_price}";
        _amountText.text = $"{amount}";
        _moneyText.text = $"{_money}";
    }
}
