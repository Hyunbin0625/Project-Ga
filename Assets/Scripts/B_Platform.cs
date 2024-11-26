using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// 발판으로서 필요한 동작을 담은 스크립트
public class B_Platform : MonoBehaviour
{
    //public GameObject[] obstacles;  // 장애물 오브젝트들
    //private bool stepped = false;   // 플레이어 캐릭터가 밟았었는가

    private int stage;
    private bool isTemporaryUIVisible = false;

    public void SetStage(int stage) {  this.stage = stage; }

    // 컴포넌트가 활성화될때 마다 매번 실행되는 메서드
    void Start()
    {
        isTemporaryUIVisible = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTemporaryUIVisible)
        {
            isTemporaryUIVisible = true;

            // GameManager의 ShowTemporaryUI를 StartCoroutine으로 호출
            GameManager gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            string stageText = "!> Boss Stage <!";
            StartCoroutine(gameManager.ShowTemporaryUI(stageText, Color.red));
        }
    }
}