using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform target;    // Player 정보
    public float offsetZ;       // Player와 카메라 Z 거리
    public float smoothSpeed = 2.0f; // 카메라의 딜레이 속도
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

        // 카메라 회전 = player 회전
        transform.rotation = target.rotation;

    }
}
