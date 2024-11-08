using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public PlatformSpawner spawner; // 플랫폼 스포너 참조

    void Start()
    {
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어가 이 트리거 존에 진입하면 다음 플랫폼 생성
            spawner.TrySpawnPlatform();

            // 한 번만 작동하도록 트리거 존 비활성화
            gameObject.SetActive(false);
        }
    }
}
