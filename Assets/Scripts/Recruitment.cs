using System.Linq;
using UnityEngine;

public class Recruitment : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Recruit recruit) && !other.gameObject.TryGetComponent(out Recruitment recruitment))
        {
            if(!PlayerMovement.Instance.memberList.Contains(other.gameObject))
                PlayerMovement.Instance.memberList.Add(other.gameObject);
            
            other.transform.parent = PlayerMovement.Instance.transform;

            other.gameObject.GetComponent<MemberManager>().isMember = true;

            if (!other.gameObject.GetComponent<Recruitment>())
            {
                other.gameObject.AddComponent<Recruitment>();
            }

            other.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials = PlayerMovement.Instance.memberList
                .ElementAt(0).transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().materials;
        }
    }
}
