using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    private void Update()
    {
        transform.position = new Vector3(_playerTransform.transform.position.x, transform.position.y, _playerTransform.transform.position.z);
    }
}
