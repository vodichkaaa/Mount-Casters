using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float SpawnOffset = 0.05f;
    
    private bool _isPaid = false;
    private bool _isAudioPlayed = false;
    
    [SerializeField]
    private int _memberPrice = 0;
    
    [Header("UI")]
    [SerializeField]
    private GameObject _loseScreen = null;
    [SerializeField]
    private GameObject _victoryScreen = null;

    [SerializeField]
    private TextMeshProUGUI _moneyText = null;
    
    [Header("Sounds")]
    [SerializeField] 
    private AudioClip _loseSound;
    [SerializeField] 
    private AudioClip _victorySound;

    private AudioSource _audioSource;
    private void Awake()
    {
        _moneyText.text = $"{PlayerPrefs.GetInt("money")}";
        
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        for (var i = 0; i < StartAmountController.Instance.amount - 1; i++)
        {
            var member = ObjectPool.Instance.GetPooledObject(); 
                
            if (member != null) 
            {
                member.transform.position = PlayerMovement.Instance.transform.position + new Vector3(0f, 0f, i * SpawnOffset);
                member.transform.rotation = PlayerMovement.Instance.transform.rotation;
                member.transform.parent = PlayerMovement.Instance.transform;
                        
                member.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (PlayerMovement.Instance.memberList.Count <= 0)
        {
            PlayerMovement.Instance.gameState = PlayerMovement.GameState.Dead;
            _loseScreen.transform.parent.gameObject.SetActive(true);
            _loseScreen.SetActive(true);
            
            _audioSource.clip = _loseSound;
            AudioPlay();
        }

        if (BossManager.Instance.health <= 0)
        {
            _victoryScreen.transform.parent.gameObject.SetActive(true);
            _victoryScreen.SetActive(true);
            
            _audioSource.clip = _victorySound;
            AudioPlay();
            
            if (!_isPaid)
            {
                var money = PlayerPrefs.GetInt("money");
                money += PlayerMovement.Instance.memberList.Count * _memberPrice;

                PlayerPrefs.SetInt("money", money);
                PlayerPrefs.Save();
                
                _moneyText.text = $"{PlayerPrefs.GetInt("money")}";
                Debug.Log(money);
            
                _isPaid = true;
            }
        }
    }

    private void AudioPlay()
    {
        if (!_isAudioPlayed)
        {
            _audioSource.Play();
            _isAudioPlayed = true;
        }
    }
}
