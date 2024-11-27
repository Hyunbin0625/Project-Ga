using System.Collections;
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
    public GameObject stageTextUI; // 게임 오버시 활성화 할 UI 게임 오브젝트
    public GameObject gameoverUI; // 게임 오버시 활성화 할 UI 게임 오브젝트

    public CanvasGroup stageCanvasGroup; // CanvasGroup을 추가하여 UI의 투명도를 제어
    public CanvasGroup gameoverCanvasGroup; // GameOver UI의 투명도를 제어

    public float stageUIduration = 3f;          // Stage UI 출력 시간
    public float fadeDuration = 0.5f; // 페이드 효과 시간

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
    }

    void Update()
    {
        // 게임 오버 상태에서 게임을 재시작할 수 있게 하는 처리
        if (Input.GetMouseButtonDown(0) && isGameover)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // 게임 오버 상태에서 마우스 왼쪽 버튼을 누르면 현재 씬 재시작
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public IEnumerator ShowTemporaryUI(string stage, Color color)
    {
        // 텍스트와 색상 설정
        stageText.text = stage;
        stageText.color = color;

        StartCoroutine(FadeCanvasGroup(stageCanvasGroup, 0f, 1f, fadeDuration)); // 페이드 인

        // 텍스트를 일정 시간 표시
        yield return new WaitForSeconds(stageUIduration); // 표시 유지

        StartCoroutine(FadeCanvasGroup(stageCanvasGroup, 1f, 0f, fadeDuration)); // 페이드 아웃
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration); // 부드럽게 투명도 조정
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }

    // 플레이어 캐릭터가 사망시 게임 오버를 실행하는 메서드
    public void OnPlayerDead()
    {
        isGameover = true;

        // GameOver UI 활성화와 페이드인 효과
        StartCoroutine(FadeInGameoverUI());
    }

    private IEnumerator FadeInGameoverUI()
    {
        // CanvasGroup을 통해 페이드인
        if (gameoverCanvasGroup != null)
        {
            gameoverUI.SetActive(true); // GameOver UI 활성화
            yield return StartCoroutine(FadeCanvasGroup(gameoverCanvasGroup, 0f, 1f, fadeDuration)); // 페이드 인
        }
    }
}