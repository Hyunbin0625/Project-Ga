using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPManager : MonoBehaviour
{
    public int maxHP = 3; // 플레이어의 최대 HP
    public int currentHP; // 현재 HP

    public GameObject heartPrefab; // 하트 프리팹
    public Vector3 offset = new Vector3(0, 0.9f, 0);

    private PlayerController player; // 플레이어

    private GameObject heartObject;
    private bool showingHearts = false; // 하트가 표시 중인지 여부

    // Start is called before the first frame update
    void Start()
    {
        // 플레이어의 현재 HP를 최대 HP로 설정
        currentHP = maxHP;

        // 플레이어를 가져옵니다
        player = GetComponent<PlayerController>();

        // 하트 위치 계산
        Vector3 heartPosition = player.transform.position + offset;
        heartObject = Instantiate(heartPrefab, heartPosition, player.transform.rotation);

        // 초기 하트 UI 설정
        UpdateHP();
    }

    // Update is called once per frame
    void Update()
    {
        // 하트 위치 계산
        Vector3 heartPosition = player.transform.position + offset;
        heartObject.transform.position = heartPosition;
        heartObject.transform.rotation = player.transform.rotation;

        // 무적 상태일 때만 하트를 화면에 표시
        if (player.GetIsInvincible() && !showingHearts)
        {
            ShowHearts();
        }
        else if (!player.GetIsInvincible() && showingHearts)
        {
            HideHearts();
        }
    }

    // 하트 UI 업데이트
    private void UpdateHP()
    {
        // 자식 하트 오브젝트의 개수를 확인
        int childCount = heartObject.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = heartObject.transform.GetChild(i); // i번째 자식 가져오기

            if (i < currentHP)
            {
                // 현재 HP보다 작은 인덱스의 하트는 활성화
                child.gameObject.SetActive(true);
            }
            else
            {
                // 현재 HP보다 큰 인덱스의 하트는 비활성화
                child.gameObject.SetActive(false);
            }
        }
    }

    // 하트를 화면에 표시
    void ShowHearts()
    {
        heartObject.SetActive(true); // 하트 활성화
        showingHearts = true;
    }

    // 하트를 화면에서 숨김
    void HideHearts()
    {
        heartObject.SetActive(false); // 하트 비활성화
        showingHearts = false;
    }

    // 데미지를 입었을 때 HP 감소
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            player.Die();
        }
        UpdateHP();
    }

    // HP 회복
    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }
}
