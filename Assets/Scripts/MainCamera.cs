using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform target;    // Player 정보
    public float offsetZ;       // Player와 카메라 Z 거리
    public float smoothSpeed = 2.0f; // 카메라의 딜레이 속도
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // 로컬 x축 방향을 기준으로 이동 벡터 계산 (회전된 방향 반영)
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

        // 카메라 회전 = player 회전
        transform.rotation = target.rotation;

    }
}
