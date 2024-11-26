using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_PlatformTrigger : MonoBehaviour
{
    public PlatformSpawner spawner; // �÷��� ������ ����

    private Vector3 initialPlayerRotation; // �÷��̾��� �ʱ� ȸ�� ���� ������ ����
    private float platformRotationX = 0;    // �÷����� ȸ�� x��
    private float platformRotationY = 0;    // �÷����� ȸ�� x��
    private Vector3 initialPlayerPosition; // �÷��̾��� �ʱ� ��ġ ���� ������ ����
    private float platformLeftEndZ;     // �÷����� ���� �� z��

    private float triggerStartX; // Ʈ���� ���� ������ x ��ǥ
    private float triggerEndX; // Ʈ���� �� ������ x ��ǥ

    // Start is called before the first frame update
    void Start()
    {
        // Ʈ������ ���۰� �� ���� x ��ǥ ���
        triggerStartX = transform.position.x - transform.lossyScale.x / 2;
        triggerEndX = transform.position.x + transform.lossyScale.x / 2;

        // �÷����� ���� �� z�� ��
        // ������Ʈ�� �߽� ��ġ
        Vector3 centerPosition = transform.parent.position;

        // ������Ʈ�� ȸ�� ���� (�������� ��ȯ)
        float angleRad = transform.parent.eulerAngles.y * Mathf.Deg2Rad;

        // ������Ʈ�� ���� �ʺ� (���� ������ �Ÿ�)
        float halfWidth = transform.parent.lossyScale.x / 2;

        // ���� ���� x, z ��ǥ ���
        platformLeftEndZ = centerPosition.z + halfWidth * Mathf.Sin(angleRad);
    }

    private void OnTriggerEnter(Collider collision)
    {
        // �÷��̾��� �ʱ� ȸ�� �� ����
        //initialPlayerRotation = collision.transform.rotation.eulerAngles;
        // �÷��̾��� �ʱ� ��ġ �� ����
        //initialPlayerPosition = collision.transform.position;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Ʈ���� ���� ������ �÷��̾��� ��ġ�� -1 ~ 1 ������ ������ ����ȭ
            float normalizedPosition = (collision.transform.position.x - triggerStartX) / (triggerEndX - triggerStartX);

            // ����ȭ ���� ����� �ش� ���� ���ϱ�
            float targetXRotate;
            float targetYRotate;
            if (normalizedPosition < 0)
            {
                targetXRotate = initialPlayerRotation.x;
                targetYRotate = initialPlayerRotation.y;
            }
            else if (normalizedPosition > 1)
            {
                targetXRotate = platformRotationX;
                targetYRotate = platformRotationY;
            }
            else
            {
                targetXRotate = normalizedPosition * (platformRotationX - initialPlayerRotation.x) + initialPlayerRotation.x;
                targetYRotate = normalizedPosition * (platformRotationY - initialPlayerRotation.y) + initialPlayerRotation.y;
            }

            Quaternion targetRotation = Quaternion.Euler(targetXRotate, targetYRotate, collision.transform.rotation.z);

            // �÷��̾��� ȸ�� ���� �ݿ�
            collision.transform.rotation = targetRotation;
            collision.transform.position += new Vector3(3f, 0, 0) * Time.deltaTime; // �ּ� �ӵ��� �̵�

        }
    }

    public void SetInitialPlayerRotation(float rotationY)
    {
        initialPlayerRotation.x = 12f;
        initialPlayerRotation.y = rotationY;
    }

}
