using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs;   // ���� ������ �迭 (Monster01, Monster02 ��)
    public float spawnInterval = 5f;     // ���� ���� ����
    public int maxMonstersPerPlatform = 3; // �÷����� �ִ� ���� ��

    // �÷���-���� �������� ������ Dictionary
    private Dictionary<GameObject, List<GameObject>> platformToMonsters = new Dictionary<GameObject, List<GameObject>>();

    public void SpawnMonstersForPlatform(GameObject platform, int platformIndex)
    {
        // �÷����� ������ ���� �� ����
        int monstersToSpawn = Random.Range(1, maxMonstersPerPlatform + 1);

        // �÷��� ��ġ�� �������� ���� ����
        List<GameObject> spawnedMonsters = new List<GameObject>();
        for (int i = 0; i < monstersToSpawn; i++)
        {
            // ���� ���Ϳ� ��ġ ����
            int randomMonsterIndex = Random.Range(0, monsterPrefabs.Length);
            Vector3 spawnPosition = platform.transform.position + new Vector3(0, 2, 0);

            // ���� ����
            GameObject newMonster = Instantiate(monsterPrefabs[randomMonsterIndex], spawnPosition, platform.transform.rotation);
            
            //newMonster.transform.SetParent(platform.transform); // �÷����� �ڽ����� ����
            spawnedMonsters.Add(newMonster);
        }

        // �÷����� �ش� ���͸� Dictionary�� ����
        platformToMonsters[platform] = spawnedMonsters;
    }

    public void RemoveMonstersForPlatform(GameObject platform)
    {
        if (platformToMonsters.ContainsKey(platform))
        {
            // �÷����� ����� ��� ���� ����
            foreach (GameObject monster in platformToMonsters[platform])
            {
                if (monster != null)
                    Destroy(monster);
            }

            // Dictionary���� �ش� �÷��� ����
            //platformToMonsters.Remove(platform);
        }
    }
}
