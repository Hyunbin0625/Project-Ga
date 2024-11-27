using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startMenuUI; // 메뉴 UI 캔버스

    [Header("Game Controllers")]
    public PlayerController playerController; // PlayerController 스크립트를 참조
    public Monster01Controller monster01Controller; // Monster01Controller 스크립트를 참조
    public Monster02Controller monster02Controller; // Monster02Controller 스크립트를 참조

    private bool isMenuActive = false; // 메뉴 활성화 상태를 추적

    private void Update()
    {
        // ESC 키 입력 감지하여 메뉴 활성화/비활성화 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void OnStartButtonClicked()
    {
        // 메뉴 비활성화 및 게임 재개
        ToggleMenu(false);
        Debug.Log("Start 버튼 클릭됨!");

        // 버튼 상태를 강제로 초기화
        ResetButtonSelection();
    }

    public void OnSettingsButtonClicked()
    {
        Debug.Log("Settings 버튼 클릭됨!");
        // 설정 메뉴를 띄우는 코드 작성 가능

        // 버튼 상태를 강제로 초기화
        ResetButtonSelection();
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("Exit 버튼 클릭됨!");

        Debug.Log("Exit 버튼 클릭됨!");
        #if UNITY_EDITOR
        // 에디터에서 플레이 모드를 중지
            UnityEditor.EditorApplication.isPlaying = false;
        #else
        // 빌드된 환경에서 게임 종료
            Application.Quit();
        #endif

        // 버튼 상태를 강제로 초기화
        ResetButtonSelection();
    }

    private void ToggleMenu(bool? forceState = null)
    {
        // forceState가 null이면 현재 상태를 반전, 아니면 강제 설정
        isMenuActive = forceState ?? !isMenuActive;
        startMenuUI.SetActive(isMenuActive);

        // 게임 멈춤 또는 재개
        SetGamePaused(isMenuActive);
    }

    private void SetGamePaused(bool isPaused)
    {
        // 플레이어 및 몬스터 상태 조정
        playerController?.EnableMovement(!isPaused);

        // 마우스 커서 상태 설정
        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;

        // 게임 시간 정지 (필요 시)
        Time.timeScale = isPaused ? 0 : 1;
    }

    private void ResetButtonSelection()
    {
        // EventSystem의 선택 상태 초기화
        EventSystem.current?.SetSelectedGameObject(null);
    }
}
