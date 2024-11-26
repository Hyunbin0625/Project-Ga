using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    public GameObject startMenuUI; // �޴� UI ĵ����
    //public GameObject player;      // �÷��̾� ������Ʈ

    public void OnStartButtonClicked()
    {
        Debug.Log("tlqkf");
        // ���� ��ư ������ �޴� ��Ȱ��ȭ
        startMenuUI.SetActive(false);

        // �÷��̾� Ȱ��ȭ (�ʿ��)
        //player.SetActive(true);

        // ���콺 Ŀ�� �����
        Cursor.visible = false;

        // ���콺 Ŀ���� ȭ�� �߾ӿ� ����
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnSettingsButtonClicked()
    {
        Debug.Log("Settings ��ư Ŭ����.");
        // ���� â �Ǵ� �޴��� �߰������� ���� �ڵ� �ۼ�
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("Exit ��ư Ŭ����.");
        Application.Quit(); // ���� ����
    }
}