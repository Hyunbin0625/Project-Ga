using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 게임 오버 상태를 표현하고, 게임 점수와 UI를 관리하는 게임 매니저
// 씬에는 단 하나의 게임 매니저만 존재할 수 있다.
public class GameManager : MonoBehaviour
{
    // GameManager 타입 : GameManager 클래스의 인스턴스를 참조하는 참조 변수
    // 클래스 이름이 타입인 변수의 경우 해당 클래스의 인스턴스를 참조하는 참조 변수
    public static GameManager instance; // 싱글톤을 할당할 전역 변수

    public PlayerController playerController; // PlayerController 스크립트를 참조
    public PlatformSpawner platformSpawner; // PlatformSpawner 스크립트를 참조

    public bool isGameover = false; // 게임 오버 상태
    public Text stageText; // 점수를 출력할 UI 텍스트
    public GameObject gameoverUI; // 게임 오버시 활성화 할 UI 게임 오브젝트

    private int stage = 0; // 게임 점수

    // 게임 시작과 동시에 싱글톤을 구성
    void Awake()
    {
        // 싱글톤 변수 instance가 비어있는가?
        if (instance == null)
        {
            // instance가 비어있다면(null) 그곳에 자기 자신을 할당
            // this : 현재 GameManager(클래스) 인스턴스(컴포넌트)의 참조값
            instance = this;
        }
        else
        {
            // instance에 이미 다른 GameManager 오브젝트가 할당되어 있는 경우

            // 씬에 두개 이상의 GameManager 컴포넌트가 존재한다는 의미.
            // 싱글톤 오브젝트는 GameManager 하나만 존재해야 하므로 자신의 게임 오브젝트를 파괴
            Debug.LogWarning("씬에 두개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }

        // 마우스 커서 숨기기
        Cursor.visible = false;

        // 마우스 커서를 화면 중앙에 고정
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 게임 오버 상태에서 게임을 재시작할 수 있게 하는 처리
        if (Input.GetMouseButtonDown(0) && isGameover)
        {
            // 게임 오버 상태에서 마우스 왼쪽 버튼을 누르면 현재 씬 재시작
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // 점수를 증가시키는 메서드
    public void AddScore(int newStage)
    {
        if (!isGameover)
        {
            stage += newStage;
            stageText.text = "Stage: " + stage;
        }
    }

    // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
    public void OnPlayerDead()
    {
        isGameover = true;
        gameoverUI.SetActive(true);
    }
}