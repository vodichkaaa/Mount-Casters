using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BossManager: MonoBehaviour
{
    public static BossManager Instance = null;
    
    private List<GameObject> _enemies = new List<GameObject>();
    
    [SerializeField]
    private GameObject _deathParticle = null;
    
    private Animator _anim = null;
    private Transform _target = null;
    
    private float _attackType = 0f;
    
    [SerializeField]
    private float _minDistance = 0f;
    [SerializeField]
    private float _maxDistance = 0f;

    public int health = 0;

    [HideInInspector]
    public bool isLockedOnTarget = false;
    [HideInInspector]
    public bool isBossAlive = false;

    [Header("UI")]
    public Slider healthBar = null;
    public TextMeshProUGUI healthText = null;

    private static readonly int Fighting = Animator.StringToHash("isFighting");
    private static readonly int AttackType = Animator.StringToHash("attackType");
    
    private void Start()
    {
        Instance = this;
        
        _anim = GetComponent<Animator>();

        var enemiesManager = FindObjectsOfType<MemberManager>(true);
        foreach (var enemyManager in enemiesManager)
        {
            _enemies.Add(enemyManager.gameObject);
        }
        
        isBossAlive = true;
        
        healthBar.value = healthBar.maxValue = health;
        healthText.text = health.ToString();
    }

    private void Update()
    {
        healthBar.transform.rotation = Quaternion.Euler(healthBar.transform.rotation.x, 0f, healthBar.transform.rotation.z);
        
        foreach (var enemy in _enemies.ToList())
        {
            var enemyDistance = enemy.transform.position - transform.position;
            
            if (enemyDistance.sqrMagnitude <= _maxDistance * _maxDistance && !isLockedOnTarget)
            {
                for (int i = 0; i < _enemies.Count; i++)
                {
                    if(!_enemies.ElementAt(i).GetComponent<MemberManager>().isMember)
                        _enemies.RemoveAt(i);
                }
                
                isLockedOnTarget = false;
                _target = enemy.transform;
                _anim.SetBool(Fighting, true);

                transform.position = Vector3.MoveTowards(transform.position, _target.position, 5f * Time.deltaTime);
            }

            if (enemyDistance.sqrMagnitude <= _minDistance * _minDistance && enemy.GetComponent<MemberManager>().isMember)
                isLockedOnTarget = true;
        }

        if (isLockedOnTarget)
        {
            var rotation = new Vector3(_target.position.x, transform.position.y, _target.position.z) - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotation, Vector3.up), 10f * Time.deltaTime);
        }

        if (_enemies.Count == 0)
        {
            _anim.SetFloat(AttackType, 4f);
            _anim.SetBool(Fighting, false);
        }

        if (health <= 0 && isBossAlive)
        {
            Instantiate(_deathParticle, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            isBossAlive = false;
        }
    }

    public void ChangeBossAttackType()
    {
        _anim.SetFloat(AttackType, Random.Range(2, 3));
    }
}
