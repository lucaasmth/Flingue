using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public float distance = 10f;

    private Vector3 _cameraOffsetVector;

    void Start()
    {
        _cameraOffsetVector = new Vector3(0f, distance, 0f);
    }
   
    void Update()
    {
        transform.position = playerTransform.position + _cameraOffsetVector;
    }
}
