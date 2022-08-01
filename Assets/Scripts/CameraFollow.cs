using System.Linq;
using Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera = null;

    private Transform _target = null;
    
    private void Start()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    
    private void Update()
    {
        if (!_cinemachineVirtualCamera.Follow.gameObject.activeSelf && PlayerMovement.Instance.memberList.Count > 0)
        {
            _target = PlayerMovement.Instance.memberList.ElementAt(0).transform;
            _target.rotation = Quaternion.Euler(0f,0f,0f);
            
            _cinemachineVirtualCamera.m_Follow = _target;
            _cinemachineVirtualCamera.m_LookAt = _target;
        }
    }
}
