using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float horSens = 1;
    [SerializeField] private float verSens = 1;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        float rotX = Input.GetAxis("Mouse Y");
        float rotZ = Input.GetAxis("Mouse X");

        transform.Rotate(Vector3.up * rotZ * horSens);
        //transform.Rotate(Camera.main.transform.right * -rotX * verSens);

        //transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.y);
    }
}
