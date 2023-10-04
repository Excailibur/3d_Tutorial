using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] Vector3 offset;
    [SerializeField] float downAngle;
    [SerializeField] float power;
    private float horizonalInput;

    Transform cueball;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            if (ball.GetComponent<Ball>().IsCueBall())
            {
                cueball = ball.transform;
                break;
            }
        }

        ResetCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (cueball != null)
        {
            horizonalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.RotateAround(cueball.position, Vector3.up, horizonalInput);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetCamera();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 hitDirection = transform.forward;
            hitDirection = new Vector3(hitDirection.x, 0, hitDirection.z).normalized;

            cueball.gameObject.GetComponent<Rigidbody>().AddForce(hitDirection * power, ForceMode.Impulse);
        }
    }

    public void ResetCamera()
    {
        transform.position = cueball.position + offset;
        transform.LookAt(cueball.position);
        transform.localEulerAngles = new Vector3 (downAngle, transform.localEulerAngles.y, 0);
    }
}
