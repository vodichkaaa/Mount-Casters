using TMPro;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    private TextMeshPro _amountText;

    private const float SpawnOffset = 0.05f;
    
    [Header("Math")]
    [SerializeField]
    private int _amount;

    [SerializeField]
    private bool _isAdd = false;
    [SerializeField]
    private bool _isSub = false;
    
    [Header("Sounds")] 
    [SerializeField]
    private AudioClip _addClip = null;
    [SerializeField]
    private AudioClip _subClip = null;
    
    private AudioSource _audioSource = null;

    private void Start()
    {
        _amountText = GetComponentInChildren<TextMeshPro>();
        _audioSource = GetComponent<AudioSource>();

        if (_isAdd)
        {
            _amountText.text = $"+{_amount}";
            _audioSource.clip = _addClip;
        }
        if (_isSub)
        {
            _amountText.text = $"-{_amount}";
            _audioSource.clip = _subClip;
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out MemberManager memberManager))
        {
            if (_isAdd)
            {
                for (var i = 0; i < _amount; i++)
                {
                    var member = ObjectPool.Instance.GetPooledObject(); 
                
                    if (member != null)
                    {
                        member.transform.parent = null;
                        member.transform.position = other.transform.position + new Vector3(0f, 0f, i * SpawnOffset);
                        member.transform.rotation = other.transform.rotation;
                        member.transform.parent = PlayerMovement.Instance.transform;
                        
                        PlayerMovement.Instance.memberList.Add(member.gameObject);
                        member.GetComponent<MemberManager>().isMember = true;
                        
                        member.SetActive(true);
                        
                        AudioSource.PlayClipAtPoint(_addClip, other.transform.position);
                    }
                }
            }

            if (_isSub)
            {
                if (_amount > PlayerMovement.Instance.memberList.Count)
                    _amount = PlayerMovement.Instance.memberList.Count;
                
                for (var i = 0; i < _amount; i++)
                {
                    Destroy(PlayerMovement.Instance.memberList[i].GetComponent<Recruitment>());
                    PlayerMovement.Instance.memberList[i].GetComponent<MemberManager>().isMember = false;
                    PlayerMovement.Instance.memberList[i].transform.parent = null;
                    PlayerMovement.Instance.memberList[i].SetActive(false);
                    
                    PlayerMovement.Instance.memberList.RemoveAt(i);
                    
                    AudioSource.PlayClipAtPoint(_subClip, other.transform.position);
                }
            }
            transform.parent.gameObject.SetActive(false);
        }
    }
}
