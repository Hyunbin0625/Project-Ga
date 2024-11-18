using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlatformTrigger : MonoBehaviour
{
    public PlatformSpawner spawner; // 플랫폼 스포너 참조
    public bool canSpawn = true; // 플랫폼을 생성할 수 있는지 여부

    private Vector3 initialPlayerRotation; // 플레이어의 초기 회전 값을 저장할 변수
    private float platformRotationY;    // 플랫폼의 회전 y값
    private Vector3 initialPlayerPosition; // 플레이어의 초기 위치 값을 저장할 변수
    private float platformLeftEndZ;     // 플랫폼의 왼쪽 끝 z값

    private float triggerStartX; // 트리거 시작 지점의 x 좌표
    private float triggerEndX; // 트리거 끝 지점의 x 좌표

    void Start()
    {
        canSpawn = true;

        // 트리거의 시작과 끝 지점 x 좌표 계산
        triggerStartX = transform.position.x - transform.lossyScale.x / 2;
        triggerEndX = transform.position.x + transform.lossyScale.x / 2;

        // 플랫폼의 왼쪽 끝 z축 값
        // 오브젝트의 중심 위치
        Vector3 centerPosition = transform.parent.position;

        // 오브젝트의 회전 각도 (라디안으로 변환)
        float angleRad = transform.parent.eulerAngles.y * Mathf.Deg2Rad;

        // 오브젝트의 절반 너비 (왼쪽 끝까지 거리)
        float halfWidth = transform.parent.lossyScale.x / 2;

        // 왼쪽 끝의 x, z 좌표 계산
        platformLeftEndZ = centerPosition.z + halfWidth * Mathf.Sin(angleRad);

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && canSpawn)
        {
            // 플레이어가 이 트리거 존에 진입하면 다음 플랫폼 생성
            spawner.TrySpawnPlatform();

            // 플레이어의 초기 회전 값 저장
            //initialPlayerRotation = collision.transform.rotation.eulerAngles;
            // 플레이어의 초기 위치 값 저장
            initialPlayerPosition = collision.transform.position;
            
            // 한 번만 생성하도록 canSpawn을 false로 설정
            canSpawn = false;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Player") && !canSpawn)
        {
            // 트리거 범위 내에서 플레이어의 위치를 -1 ~ 1 사이의 값으로 정규화
            float normalizedPosition = (collision.transform.position.x - triggerStartX) / (triggerEndX - triggerStartX);

            // 정규화 값을 사용해 해당 각도 구하기
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

            // 플레이어의 회전 값에 반영
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
