using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlatformTrigger : MonoBehaviour
{
    public PlatformSpawner spawner; // �÷��� ������ ����
    public bool canSpawn = true; // �÷����� ������ �� �ִ��� ����

    private Vector3 initialPlayerRotation; // �÷��̾��� �ʱ� ȸ�� ���� ������ ����
    private float platformRotationY;    // �÷����� ȸ�� y��
    private Vector3 initialPlayerPosition; // �÷��̾��� �ʱ� ��ġ ���� ������ ����
    private float platformLeftEndZ;     // �÷����� ���� �� z��

    private float triggerStartX; // Ʈ���� ���� ������ x ��ǥ
    private float triggerEndX; // Ʈ���� �� ������ x ��ǥ

    void Start()
    {
        canSpawn = true;

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
        if (collision.CompareTag("Player") && canSpawn)
        {
            // �÷��̾ �� Ʈ���� ���� �����ϸ� ���� �÷��� ����
            spawner.TrySpawnPlatform();

            // �÷��̾��� �ʱ� ȸ�� �� ����
            //initialPlayerRotation = collision.transform.rotation.eulerAngles;
            // �÷��̾��� �ʱ� ��ġ �� ����
            initialPlayerPosition = collision.transform.position;
            
            // �� ���� �����ϵ��� canSpawn�� false�� ����
            canSpawn = false;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Player") && !canSpawn)
        {
            // Ʈ���� ���� ������ �÷��̾��� ��ġ�� -1 ~ 1 ������ ������ ����ȭ
            float normalizedPosition = (collision.transform.position.x - triggerStartX) / (triggerEndX - triggerStartX);

            // ����ȭ ���� ����� �ش� ���� ���ϱ�
            float targetYRotate;
            float targetZPosition;
            if (normalizedPosition < 0)
            {
                targetYRotate = initialPlayerRotation.y;
                targetZPosition = initialPlayerPosition.z;
            }
            else if (normalizedPosition > 1)
            {
                targetYRotate = platformRotationY;
                targetZPosition = platformLeftEndZ;
            }
            else
            {
                targetYRotate = normalizedPosition * (platformRotationY - initialPlayerRotation.y) + initialPlayerRotation.y;
                targetZPosition = normalizedPosition * (platformLeftEndZ - initialPlayerPosition.z) + initialPlayerPosition.z;
            }

            Quaternion targetRotation = Quaternion.Euler(initialPlayerRotation.x, targetYRotate, collision.transform.rotation.z);
            Vector3 targetPosition = new Vector3(collision.transform.position.x, collision.transform.position.y, targetZPosition);

            // �÷��̾��� ȸ�� ���� �ݿ�
            collision.transform.rotation = targetRotation;
            collision.transform.position = targetPosition;
        }
    }

    public void SetInitialPlayerRotation(float rotationY)
    {
        initialPlayerRotation.x = 12f;
        initialPlayerRotation.y = rotationY;
    }

    public float GetPlatformRotation()
    {
        return platformRotationY;
    }

    public void SetPlatformRotation(float rotationY)
    {
        platformRotationY = rotationY;
    }
}
