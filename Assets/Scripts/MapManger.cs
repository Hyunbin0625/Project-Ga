using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject platformPrefab; //생성할 플랫폼 프리팹
    public int platformCount = 1; // 생성할 플랫폼의 개수
    public Vector3 startPosition = new Vector3(0, 0, 0); //맵 시작위치
    public float distanceBetweenPlatforms = 3.0f; // 플랫폼간의 거리
    public float maxPlatformHeightDifference = 1.5f; //플랫폼 높이 변화범위
    // Start is called before the first frame update
    void Start()
    {
        GeneratMap();
    }

    void GeneratMap()
    {
        Vector3 currentPosition = startPosition;
        
        for(int i=0;i<platformCount;i++)
        {
            //플랫폼 생성
            GameObject platform = Instantiate(platformPrefab, currentPosition, Quaternion.identity);
            //현재 위치 업데이트(가로로 거리 이동)
            currentPosition.x += distanceBetweenPlatforms;

            //랜덤 높이 변화 추가
            currentPosition.y += Random.Range(-maxPlatformHeightDifference, maxPlatformHeightDifference);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
