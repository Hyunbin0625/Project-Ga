using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform player;    // player

    public float offsetZ;       // Player와 카메라 Z 거리
    public float smoothSpeed = 2f; // 카메라의 딜레이 속도
    public float maxPosY = 5.0f;    // Camera의 최대 Y

    public bool isBossView = false;    // boss view에 따라 시점 변경

    // 마우스 상하 각도 제어 변수
    public float rotationSpeed = 2.0f;  // 마우스 감도
    // Boss zone 일때
    public float B_MinYAngle = -10.0f;    // 카메라 최소 각도
    public float B_MaxYAngle = 10.0f;     // 카메라 최대 각도

    private float currentXRotation = 0.0f;  // 현재 카메라의 X축 회전값

    // 각도에 따른 Y 위치 변경 변수
    public float minYOffset = 0f;   // 최소 Y 오프셋
    public float maxYOffset = 4.0f;   // 최대 Y 오프셋

    public LayerMask cameraCollision;   // 카메라가 통과하지 못할 오브젝트의 오브젝트

    // Start is called before the first frame update
    void Start()
    {
        // 초기 회전값 설정
        currentXRotation = transform.localEulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        // 로컬 x축 방향을 기준으로 이동 벡터 계산 (회전된 방향 반영)
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

            // 마우스 입력으로 상하 회전
            float mouseY = Input.GetAxis("Mouse Y");
            currentXRotation -= mouseY * rotationSpeed;  // 마우스 이동 반대로 각도 조정
            currentXRotation = Mathf.Clamp(currentXRotation, B_MinYAngle, B_MaxYAngle);  // 최소, 최대 각도 제한

            // 각도에 따른 Y 위치 변경
            float normalizedAngle = (currentXRotation - B_MinYAngle) / (B_MaxYAngle - B_MinYAngle); // 0~1로 정규화
            float dynamicYOffset = Mathf.Lerp(minYOffset, maxYOffset, normalizedAngle); // Y 오프셋 계산

            // 카메라 위치와 회전 업데이트
            transform.localEulerAngles = new Vector3(currentXRotation, transform.localEulerAngles.y, transform.localEulerAngles.z);
            transform.position = new Vector3(transform.position.x, transform.parent.position.y + dynamicYOffset, transform.position.z);


            // 플레이어에서부터 카메라까지의 방향 구하기
            //Vector3 rayDir = transform.position - player.position;
            Vector3 rayDir = (transform.position - player.position).normalized;

            // Raycast 최대 거리 설정
            float rayMaxDistance = Vector3.Distance(player.position, transform.position);

            // 플레이어에서 카메라 방향으로 Ray 발사
            if (Physics.Raycast(player.position, rayDir, out RaycastHit hit, rayMaxDistance, cameraCollision))
            {
                // 맞은 부위보다 더 안쪽으로 위치 이동
                //transform.position = hit.point - rayDir.normalized;

                // 맞은 지점 바로 앞에 위치하도록 이동
                float safeDistance = 0.2f; // 벽이나 땅과 약간의 여유 거리
                transform.position = hit.point - rayDir * safeDistance;
            }
        }
    }
}
