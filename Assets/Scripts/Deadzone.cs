using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadzone : MonoBehaviour
{
    public Transform target;    // Player ����
 
    // Update is called once per frame
    void Update()
    {
        // �������� ��ġ = �÷����� ��ġ�� x, z��, �ڱ��ڽ��� ��ġ�� y��
        transform.position = new Vector3(target.position.x,transform.position.y,target.position.z);
       
        // ������ ȸ�� = player ȸ��
        transform.rotation = target.rotation;
    }
}
