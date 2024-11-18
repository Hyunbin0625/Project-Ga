using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform target;    // Player ����
    public float offsetZ;       // Player�� ī�޶� Z �Ÿ�
    public float smoothSpeed = 2.0f; // ī�޶��� ������ �ӵ�
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // ���� x�� ������ �������� �̵� ���� ��� (ȸ���� ���� �ݿ�)
        Vector3 moveDirection = new Vector3(transform.right.z * offsetZ, transform.right.y, transform.right.x * offsetZ);

        Vector3 targetPosition;
        if (target.position.y + 2.5f >= 4.0f)
        {

            targetPosition = new Vector3(target.position.x - moveDirection.x, 4.0f, target.position.z + moveDirection.z);
        }
        else
        {
            targetPosition = new Vector3(target.position.x - moveDirection.x, target.position.y + 2.5f, target.position.z + moveDirection.z);
        }
        

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // ī�޶� ȸ�� = player ȸ��
        transform.rotation = target.rotation;

    }
}
