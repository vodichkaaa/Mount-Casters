using UnityEngine;
using DG.Tweening;

public class Hammer : Obstacle
{
    [SerializeField]
    private bool _rotateRight = false;
    [SerializeField]
    private bool _rotateLeft = false;
    
    private void Start()
    {
        if (this != null)
        {
            if(_rotateRight)
                transform.DORotate(new Vector3(0f, 0f, 90f), 1f).SetLoops(1000, LoopType.Yoyo).SetEase(Ease.InOutSine);
            if(_rotateLeft)
                transform.DORotate(new Vector3(0f, 0f, -90f), 1f).SetLoops(1000, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
    }
}
