using UnityEngine;

public class GunPosition : MonoBehaviour
{
    [SerializeField] private Transform _cameraRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       //_cameraRotation=FindObjectOfType<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = _cameraRotation.rotation;
    }
}
