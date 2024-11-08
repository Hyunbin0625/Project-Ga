using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public PlatformSpawner spawner; // �÷��� ������ ����

    void Start()
    {
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �÷��̾ �� Ʈ���� ���� �����ϸ� ���� �÷��� ����
            spawner.TrySpawnPlatform();

            // �� ���� �۵��ϵ��� Ʈ���� �� ��Ȱ��ȭ
            gameObject.SetActive(false);
        }
    }
}
