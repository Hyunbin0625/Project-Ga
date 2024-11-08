using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform target;    // Player ����
    public float offsetZ;       // Player�� ī�޶� Z �Ÿ�
    public float smoothSpeed = 2.0f; // ī�޶��� ������ �ӵ�
    public float Speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // ī�޶� ȸ�� = player ȸ��
        transform.rotation = target.rotation;

    }
}
