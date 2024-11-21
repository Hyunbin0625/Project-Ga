using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs;   // 몬스터 프리팹 배열 (Monster01, Monster02 등)
    public float spawnInterval = 5f;     // 몬스터 생성 간격
    public int maxMonstersPerPlatform = 3; // 플랫폼당 최대 몬스터 수

    // 플랫폼-몬스터 연관성을 관리할 Dictionary
    private Dictionary<GameObject, List<GameObject>> platformToMonsters = new Dictionary<GameObject, List<GameObject>>();

    public void SpawnMonstersForPlatform(GameObject platform, int platformIndex)
    {
        // 플랫폼에 생성될 몬스터 수 결정
        int monstersToSpawn = Random.Range(1, maxMonstersPerPlatform + 1);

        // 플랫폼 위치를 기준으로 몬스터 생성
        List<GameObject> spawnedMonsters = new List<GameObject>();
        for (int i = 0; i < monstersToSpawn; i++)
        {
            // 랜덤 몬스터와 위치 선택
            int randomMonsterIndex = Random.Range(0, monsterPrefabs.Length);
            Vector3 spawnPosition = platform.transform.position + new Vector3(0, 2, 0);

            // 몬스터 생성
            GameObject newMonster = Instantiate(monsterPrefabs[randomMonsterIndex], spawnPosition, platform.transform.rotation);
            
            //newMonster.transform.SetParent(platform.transform); // 플랫폼의 자식으로 설정
            spawnedMonsters.Add(newMonster);
        }

        // 플랫폼과 해당 몬스터를 Dictionary에 저장
        platformToMonsters[platform] = spawnedMonsters;
    }

    public void RemoveMonstersForPlatform(GameObject platform)
    {
        if (platformToMonsters.ContainsKey(platform))
        {
            // 플랫폼에 연결된 모든 몬스터 삭제
            foreach (GameObject monster in platformToMonsters[platform])
            {
                if (monster != null)
                    Destroy(monster);
            }

            // Dictionary에서 해당 플랫폼 제거
            //platformToMonsters.Remove(platform);
        }
    }
}
