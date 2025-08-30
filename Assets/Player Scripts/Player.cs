using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 2, -5);
    public float mouseSensitivity = 150f;
    public float smoothTime = 0.05f;

    private Rigidbody rb;
    private float xRotation = 0f;
    private Vector3 currentVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();
        UpdateCamera();
    }

    void MovePlayer()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = transform.right * h + transform.forward * v;
        Vector3 velocity = move * moveSpeed;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;

        if (Input.GetButtonDown("Jump") && IsGrounded())
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);        // �¿� ȸ��
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30f, 60f); // ���� ����
    }

    void UpdateCamera()
    {
        // ��ǥ ��ġ: �÷��̾� �� + offset
        Vector3 targetPos = transform.position + Quaternion.Euler(xRotation, transform.eulerAngles.y, 0) * cameraOffset;

        // ������ �̵�
        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, targetPos, ref currentVelocity, smoothTime);

        // �׻� �÷��̾ ���� ���� �ٶ󺸱�
        cameraTransform.LookAt(transform.position + Vector3.up * 1.5f);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}