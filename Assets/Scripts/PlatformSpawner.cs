using System.Collections.Generic;
using UnityEngine;

// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab; // 생성할 발판의 원본 프리팹
    private Transform spawnPoint; // 다음 발판이 생성될 위치
    private GameObject currentPlatform; // 현재 생성된 발판
    private int platformCount = 0;  // 생성된 플랫폼 수, 단계

    private List<GameObject> platforms = new List<GameObject>(); // 생성된 플랫폼을 저장할 리스트
    private int maxPlatforms = 3; // 최대 플랫폼 개수

    //public float yMin = -3.5f; // 배치할 위치의 최소 y값
    //public float yMax = 1.5f; // 배치할 위치의 최대 y값
    //private float xPos = 20f; // 배치할 위치의 x 값

    //private int currentIndex = 0; // 사용할 현재 순번의 발판

    //private Vector2 poolPosition = new(0, -25); // 초반에 생성된 발판들을 화면 밖에 숨겨둘 위치
    //private float lastSpawnTime; // 마지막 배치 시점


    //특정 씬을 로드하는 함수
    void Start()
    {
        // spawnPoint가 할당되지 않은 경우, 기본 위치를 현재 위치로 설정
        if (spawnPoint == null)
        {
            GameObject defaultSpawnPoint = new GameObject("DefaultSpawnPoint");
            defaultSpawnPoint.transform.position = new Vector3(0, -1, 0); // 기본 위치 설정
            spawnPoint = defaultSpawnPoint.transform;
        }

        // 변수들을 초기화하고 사용할 발판들을 미리 생성
        SpawnPlatform(); // 게임 시작 시 첫 번째 발판 생성
    }

    void Update()
    {

        // 순서를 돌아가며 주기적으로 발판을 배치

    }
    //버튼 클릭시 호출되는 함수


    void SpawnPlatform()
    {
        // 현재 생성된 플랫폼 수가 최대치를 넘지 않으면 새 발판을 생성
        if (platforms.Count < maxPlatforms)
        {
            ++platformCount;    // 플랫폼 카운트 증가
            spawnPoint.position = new Vector3(platformCount * 22, -1, 0);   // 플랫폼이 생성될 위치

            // 플랫폼 생성
            GameObject newPlatform = Instantiate(platformPrefab, spawnPoint.position, Quaternion.identity);
            platforms.Add(newPlatform); // 생성된 발판을 리스트에 추가


            // 충돌 감지용 Zone에 PlatformTrigger의 spawner 변수 설정
            PlatformTrigger platformTrigger = newPlatform.GetComponentInChildren<PlatformTrigger>();
            if (platformTrigger != null)    // 플랫폼 트리거 재대로 생성되었다면
            {
                platformTrigger.spawner = this; // 자식, 플랫폼 트리거의 spawner에 자신(PlatformSpawner) 저장
            }
        }
    }

    // 플랫폼을 스폰하는 함수가 외부에서 호출될 수 있도록 공개
    public void TrySpawnPlatform()
    {
        Debug.Log(platforms.Count + ", " + maxPlatforms);
        // 최대 개수를 초과하면 가장 오래된 플랫폼 삭제
        if (platforms.Count >= maxPlatforms)
        {
            Destroy(platforms[0]);  // 첫번째 플랫폼 파괴
            platforms.RemoveAt(0);  // 
        }

        SpawnPlatform();    // 플랫폼 생성
    }

}