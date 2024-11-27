using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startMenuUI; // �޴� UI ĵ����

    [Header("Game Controllers")]
    public PlayerController playerController; // PlayerController ��ũ��Ʈ�� ����
    public Monster01Controller monster01Controller; // Monster01Controller ��ũ��Ʈ�� ����
    public Monster02Controller monster02Controller; // Monster02Controller ��ũ��Ʈ�� ����

    private bool isMenuActive = false; // �޴� Ȱ��ȭ ���¸� ����

    private void Update()
    {
        // ESC Ű �Է� �����Ͽ� �޴� Ȱ��ȭ/��Ȱ��ȭ ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    public void OnStartButtonClicked()
    {
        // �޴� ��Ȱ��ȭ �� ���� �簳
        ToggleMenu(false);
        Debug.Log("Start ��ư Ŭ����!");

        // ��ư ���¸� ������ �ʱ�ȭ
        ResetButtonSelection();
    }

    public void OnSettingsButtonClicked()
    {
        Debug.Log("Settings ��ư Ŭ����!");
        // ���� �޴��� ���� �ڵ� �ۼ� ����

        // ��ư ���¸� ������ �ʱ�ȭ
        ResetButtonSelection();
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("Exit ��ư Ŭ����!");

        Debug.Log("Exit ��ư Ŭ����!");
        #if UNITY_EDITOR
        // �����Ϳ��� �÷��� ��带 ����
            UnityEditor.EditorApplication.isPlaying = false;
        #else
        // ����� ȯ�濡�� ���� ����
            Application.Quit();
        #endif

        // ��ư ���¸� ������ �ʱ�ȭ
        ResetButtonSelection();
    }

    private void ToggleMenu(bool? forceState = null)
    {
        // forceState�� null�̸� ���� ���¸� ����, �ƴϸ� ���� ����
        isMenuActive = forceState ?? !isMenuActive;
        startMenuUI.SetActive(isMenuActive);

        // ���� ���� �Ǵ� �簳
        SetGamePaused(isMenuActive);
    }

    private void SetGamePaused(bool isPaused)
    {
        // �÷��̾� �� ���� ���� ����
        playerController?.EnableMovement(!isPaused);

        // ���콺 Ŀ�� ���� ����
        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;

        // ���� �ð� ���� (�ʿ� ��)
        Time.timeScale = isPaused ? 0 : 1;
    }

    private void ResetButtonSelection()
    {
        // EventSystem�� ���� ���� �ʱ�ȭ
        EventSystem.current?.SetSelectedGameObject(null);
    }
}
