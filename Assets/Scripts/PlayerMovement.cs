using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance = null;
    
    public enum GameState
    {
        Running,
        Attacking,
        Dead
    }

    public GameState gameState;
    
    [HideInInspector]
    public List<GameObject> memberList = null;
    
    public Transform road = null;
    private AudioSource _audioSource = null;
    
    private bool _isMoving = false;
    
    private Vector3 _direction = Vector3.zero;
    
    [SerializeField] 
    private float _speed = 0f;
    [SerializeField] 
    private float _velocity = 0f;
    [SerializeField] 
    private float _swipeSpeed = 0f;
    [SerializeField] 
    private float _roadSpeed = 0f;

    public static readonly int Run = Animator.StringToHash("run");
    public static readonly int Fighting = Animator.StringToHash("isFighting");
    public static readonly int AttackType = Animator.StringToHash("attackType");

    private void Start()
    {
        Instance = this;

        _audioSource = GetComponent<AudioSource>();
        memberList.Add(transform.GetChild(0).gameObject);
        
        gameState = GameState.Running;
    }

    private void Update()
    {
        if (gameState == GameState.Running)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isMoving = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                _isMoving = false;
            }
        
            if (_isMoving)
            {
                _direction = new Vector3(Mathf.Lerp(_direction.x, Input.GetAxis("Mouse X"), _speed * Time.deltaTime), 0f);
                _direction = Vector3.ClampMagnitude(_direction, 1f);
            }
            
            foreach (var member in memberList)
            {
                var memberRb = member.GetComponent<Rigidbody>();
                member.GetComponent<Animator>().SetFloat(Run, 1f);
                
                if (memberRb.velocity.magnitude > 0.5f)
                {
                    memberRb.rotation = 
                        Quaternion.Slerp(memberRb.rotation, Quaternion.LookRotation(memberRb.velocity, Vector3.up), _velocity * Time.deltaTime);
                }
                else memberRb.rotation = Quaternion.Slerp(memberRb.rotation, Quaternion.identity, _velocity * Time.deltaTime);
            }
        }
        else
        {
            _audioSource.mute = true;
            
            if (!BossManager.Instance.isBossAlive)
            {
                foreach (var member in memberList)
                {
                    member.GetComponent<Animator>().SetFloat(AttackType, 4f);
                }
            }
        }
       
    }

    private void FixedUpdate()
    {
        if (gameState == GameState.Running)
        {
            road.transform.Translate(Vector3.back * Time.deltaTime * _roadSpeed);
        }
        
        if (gameState == GameState.Running)
        {
            foreach(var memberRb in memberList.Select(member => member.GetComponent<Rigidbody>()))
            {
                if (_isMoving)
                {
                    var offset = new Vector3(_direction.x, 0f, 0f) * Time.fixedDeltaTime;
                    memberRb.velocity = new Vector3(_direction.x * _swipeSpeed * Time.fixedDeltaTime, 0f, 0f) + offset;
                }
                else memberRb.velocity = Vector3.zero;
            }
        }
    }
}
