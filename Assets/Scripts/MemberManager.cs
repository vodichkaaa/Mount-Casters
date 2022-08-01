using UnityEngine;
using Random = UnityEngine.Random;

public class MemberManager : MonoBehaviour
{
    private Animator _anim = null;
    private Transform _boss = null;
    private Rigidbody _rb = null;
    
    [SerializeField]
    private GameObject _deathParticle = null;

    [SerializeField]
    private int _health = 0;
    
    [SerializeField]
    private float _minDistance = 0f;
    [SerializeField]
    private float _maxDistance = 0f;
    [SerializeField]
    private float _speed = 0f;
    
    private bool _isFighting = false;
    
    [HideInInspector]
    public bool isMember = false;

    private AudioSource _audioSource;
    
    private void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();

        _boss = FindObjectOfType<BossManager>().transform;

        _health = 5;
    }

    private void Update()
    {
        var bossDistance = _boss.position - transform.position;

        var damages = GetComponentsInChildren<DamageBoss>(true);

        if (isMember)
        {
            _anim.SetFloat(PlayerMovement.Run, 1f);
        }
        
        if (_isFighting)
        {
            foreach (var damage in damages)
            {
                damage.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var damage in damages)
            {
                damage.gameObject.SetActive(false);
            }
            if (bossDistance.sqrMagnitude <= _maxDistance * _maxDistance)
            {
                PlayerMovement.Instance.gameState = PlayerMovement.GameState.Attacking;
            }

            if (PlayerMovement.Instance.gameState == PlayerMovement.GameState.Attacking && isMember)
            {
                transform.position = Vector3.MoveTowards(transform.position, _boss.position, _speed * Time.deltaTime);
                
                var rotation = new Vector3(_boss.position.x, transform.position.y, _boss.position.z) - transform.position;
                transform.rotation = 
                    Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotation, Vector3.up), 10f * Time.deltaTime);

                _rb.velocity = Vector3.zero;
            }
        }

        if (bossDistance.sqrMagnitude <= _minDistance * _minDistance)
        {
            _isFighting = true;

            var rotation = new Vector3(_boss.position.x, transform.position.y, _boss.position.z) - transform.position;
            transform.rotation =
                Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotation, Vector3.up), 10f * Time.deltaTime);

            _anim.SetBool(PlayerMovement.Fighting, true);
            

            _minDistance = _maxDistance;
            _rb.velocity = Vector3.zero;
        }
        else _isFighting = false;
    }

    public void AttackTypeChange()
    {
        _anim.SetFloat( PlayerMovement.AttackType, Random.Range(0, 3));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out DamageRecruit damageRecruit))
        {
            _health--;

            if (_health <= 0)
            {
                Death();

                BossManager.Instance.isLockedOnTarget = false;
            }
        }

        if (other.gameObject.TryGetComponent(out Obstacle obstacle))
        {
            Death();
        }
    }

    private void Death()
    {
        var particle = Instantiate(_deathParticle, transform.position, Quaternion.identity);
        particle.transform.SetParent(PlayerMovement.Instance.road);
        
        transform.parent = null;
        isMember = false;
        
        if (PlayerMovement.Instance.memberList.Contains(gameObject))
        {
            PlayerMovement.Instance.memberList.Remove(gameObject);
        }
        
        AudioSource.PlayClipAtPoint(_audioSource.clip, transform.position);
        
        gameObject.SetActive(false);
    }
}
