using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    public int maxHP = 3; // �÷��̾��� �ִ� HP
    public int currentHP; // ���� HP

    public GameObject heartPrefab; // ��Ʈ ������
    public Vector3 offset = new Vector3(0, 0.9f, 0);

    private PlayerController player; // �÷��̾�

    private GameObject heartObject;
    private bool showingHearts = false; // ��Ʈ�� ǥ�� ������ ����

    // Start is called before the first frame update
    void Start()
    {
        // �÷��̾��� ���� HP�� �ִ� HP�� ����
        currentHP = maxHP;

        // �÷��̾ �����ɴϴ�
        player = GetComponent<PlayerController>();

        // ��Ʈ ��ġ ���
        Vector3 heartPosition = player.transform.position + offset;
        heartObject = Instantiate(heartPrefab, heartPosition, player.transform.rotation);

        // �ʱ� ��Ʈ UI ����
        UpdateHP();
    }

    // Update is called once per frame
    void Update()
    {
        // ��Ʈ ��ġ ���
        Vector3 heartPosition = player.transform.position + offset;
        heartObject.transform.position = heartPosition;
        heartObject.transform.rotation = player.transform.rotation;

        // ���� ������ ���� ��Ʈ�� ȭ�鿡 ǥ��
        if (player.GetIsInvincible() && !showingHearts)
        {
            ShowHearts();
        }
        else if (!player.GetIsInvincible() && showingHearts)
        {
            HideHearts();
        }
    }

    // ��Ʈ UI ������Ʈ
    private void UpdateHP()
    {
        // �ڽ� ��Ʈ ������Ʈ�� ������ Ȯ��
        int childCount = heartObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = heartObject.transform.GetChild(i); // i��° �ڽ� ��������

            if (i < currentHP)
            {
                // ���� HP���� ���� �ε����� ��Ʈ�� Ȱ��ȭ
                child.gameObject.SetActive(true);
            }
            else
            {
                // ���� HP���� ū �ε����� ��Ʈ�� ��Ȱ��ȭ
                child.gameObject.SetActive(false);
            }
        }
    }

    // ��Ʈ�� ȭ�鿡 ǥ��
    void ShowHearts()
    {
        heartObject.SetActive(true); // ��Ʈ Ȱ��ȭ
        showingHearts = true;
    }

    // ��Ʈ�� ȭ�鿡�� ����
    void HideHearts()
    {
        heartObject.SetActive(false); // ��Ʈ ��Ȱ��ȭ
        showingHearts = false;
    }

    // �������� �Ծ��� �� HP ����
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            player.Die();
        }
        UpdateHP();
    }

    // HP ȸ��
    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }
}
