using UnityEngine;

public class FollowFirePosition : MonoBehaviour
{
    [SerializeField] private Transform _firePosition;
    [SerializeField] private Transform _cameraRotation;
    // Update is called once per frame
    void Update()
    {
        transform.position=_firePosition.position;
        transform.rotation= _cameraRotation.rotation;
    }
}
