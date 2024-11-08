using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject platformPrefab; //������ �÷��� ������
    public int platformCount = 1; // ������ �÷����� ����
    public Vector3 startPosition = new Vector3(0, 0, 0); //�� ������ġ
    public float distanceBetweenPlatforms = 3.0f; // �÷������� �Ÿ�
    public float maxPlatformHeightDifference = 1.5f; //�÷��� ���� ��ȭ����
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
            //�÷��� ����
            GameObject platform = Instantiate(platformPrefab, currentPosition, Quaternion.identity);
            //���� ��ġ ������Ʈ(���η� �Ÿ� �̵�)
            currentPosition.x += distanceBetweenPlatforms;

            //���� ���� ��ȭ �߰�
            currentPosition.y += Random.Range(-maxPlatformHeightDifference, maxPlatformHeightDifference);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
