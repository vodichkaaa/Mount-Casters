using DG.Tweening;
using UnityEngine;

public class CircularSaw : Obstacle
{
    private void Start()
    {
        if (this != null)
        {
            transform.DORotateQuaternion(Quaternion.Euler(new Vector3(90f, 0f, 90f)), .5f)
                .SetLoops(1000, LoopType.Restart).SetEase(Ease.Linear);
        }
    }
}
