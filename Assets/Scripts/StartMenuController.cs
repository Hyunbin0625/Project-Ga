using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    public GameObject startMenuUI; // 메뉴 UI 캔버스
    //public GameObject player;      // 플레이어 오브젝트

    public void OnStartButtonClicked()
    {
        Debug.Log("tlqkf");
        // 시작 버튼 누르면 메뉴 비활성화
        startMenuUI.SetActive(false);

        // 플레이어 활성화 (필요시)
        //player.SetActive(true);

        // 마우스 커서 숨기기
        Cursor.visible = false;

        // 마우스 커서를 화면 중앙에 고정
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnSettingsButtonClicked()
    {
        Debug.Log("Settings 버튼 클릭됨.");
        // 설정 창 또는 메뉴를 추가적으로 띄우는 코드 작성
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("Exit 버튼 클릭됨.");
        Application.Quit(); // 게임 종료
    }
}