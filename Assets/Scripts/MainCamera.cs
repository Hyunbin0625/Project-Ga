using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform player;    // player

    public float offsetZ;       // Player�� ī�޶� Z �Ÿ�
    public float smoothSpeed = 2f; // ī�޶��� ������ �ӵ�
    public float maxPosY = 5.0f;    // Camera�� �ִ� Y

    public bool isBossView = false;    // boss view�� ���� ���� ����

    // ���콺 ���� ���� ���� ����
    public float rotationSpeed = 2.0f;  // ���콺 ����
    // Boss zone �϶�
    public float B_MinYAngle = -10.0f;    // ī�޶� �ּ� ����
    public float B_MaxYAngle = 10.0f;     // ī�޶� �ִ� ����

    private float currentXRotation = 0.0f;  // ���� ī�޶��� X�� ȸ����

    // ������ ���� Y ��ġ ���� ����
    public float minYOffset = 0f;   // �ּ� Y ������
    public float maxYOffset = 4.0f;   // �ִ� Y ������

    public LayerMask cameraCollision;   // ī�޶� ������� ���� ������Ʈ�� ������Ʈ

    // Start is called before the first frame update
    void Start()
    {
        // �ʱ� ȸ���� ����
        currentXRotation = transform.localEulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        // ���� x�� ������ �������� �̵� ���� ��� (ȸ���� ���� �ݿ�)
        Vector3 moveDirection = new Vector3(transform.right.z * offsetZ, transform.right.y, transform.right.x * offsetZ);

        Vector3 targetPosition;
        if (transform.parent.position.y + 2.5f >= 4.0f)
        {
            targetPosition = new Vector3(transform.parent.position.x - moveDirection.x, maxPosY, transform.parent.position.z + moveDirection.z);
        }
        else
        {
            targetPosition = new Vector3(transform.parent.position.x - moveDirection.x, transform.parent.position.y + 3f, transform.parent.position.z + moveDirection.z);
        }

        if (!isBossView)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = targetPosition;

            // ���콺 �Է����� ���� ȸ��
            float mouseY = Input.GetAxis("Mouse Y");
            currentXRotation -= mouseY * rotationSpeed;  // ���콺 �̵� �ݴ�� ���� ����
            currentXRotation = Mathf.Clamp(currentXRotation, B_MinYAngle, B_MaxYAngle);  // �ּ�, �ִ� ���� ����

            // ������ ���� Y ��ġ ����
            float normalizedAngle = (currentXRotation - B_MinYAngle) / (B_MaxYAngle - B_MinYAngle); // 0~1�� ����ȭ
            float dynamicYOffset = Mathf.Lerp(minYOffset, maxYOffset, normalizedAngle); // Y ������ ���

            // ī�޶� ��ġ�� ȸ�� ������Ʈ
            transform.localEulerAngles = new Vector3(currentXRotation, transform.localEulerAngles.y, transform.localEulerAngles.z);
            transform.position = new Vector3(transform.position.x, transform.parent.position.y + dynamicYOffset, transform.position.z);


            // �÷��̾������ ī�޶������ ���� ���ϱ�
            //Vector3 rayDir = transform.position - player.position;
            Vector3 rayDir = (transform.position - player.position).normalized;

            // Raycast �ִ� �Ÿ� ����
            float rayMaxDistance = Vector3.Distance(player.position, transform.position);

            // �÷��̾�� ī�޶� �������� Ray �߻�
            if (Physics.Raycast(player.position, rayDir, out RaycastHit hit, rayMaxDistance, cameraCollision))
            {
                // ���� �������� �� �������� ��ġ �̵�
                //transform.position = hit.point - rayDir.normalized;

                // ���� ���� �ٷ� �տ� ��ġ�ϵ��� �̵�
                float safeDistance = 0.2f; // ���̳� ���� �ణ�� ���� �Ÿ�
                transform.position = hit.point - rayDir * safeDistance;
            }
        }
    }
}
