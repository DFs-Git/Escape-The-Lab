using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    public SettingsLoader loader;

    public TMP_Dropdown screen;
    public Toggle fullsc;

    // ������Dropdown��������Ļ�ֱ��ʵ�ӳ��
    private Vector2[] indexToScreenSize;

    private void Start()
    {
        // ���
        indexToScreenSize = new Vector2[5];

        indexToScreenSize[0] = new Vector2(1920, 1080);
        indexToScreenSize[1] = new Vector2(1600, 900);
        indexToScreenSize[2] = new Vector2(1360, 768);
        indexToScreenSize[3] = new Vector2(1024, 768);
        indexToScreenSize[4] = new Vector2(800, 600);

        // �޸ĸ�ѡ��Ϊ�趨��ѡ��
        fullsc.isOn = loader.settings.fullscreen;
        if (loader.settings.width == 1920) screen.value = 0;
        if (loader.settings.width == 1600) screen.value = 1;
        if (loader.settings.width == 1360) screen.value = 2;
        if (loader.settings.width == 1024) screen.value = 3;
        if (loader.settings.width == 800) screen.value = 4;
    }

    // ���ذ�ť
    public void Back()
    {
        SceneManager.LoadScene(0);
    }

    // Ӧ�����ð�ť
    public void Apply()
    {
        // ��ȡDropdown/Toggle����
        int screenSizeIndex = screen.value;
        bool fullScreen = fullsc.isOn;

        // ������д��SettingsLoader���е�settings
        Vector2 screenSize = indexToScreenSize[screenSizeIndex];
        loader.settings.fullscreen = fullScreen;
        loader.settings.width = (int)screenSize.x;
        loader.settings.height = (int)screenSize.y;

        // д���ļ�
        loader.WriteSettings();

        // Ӧ�ô��ڸ���
        Screen.SetResolution(loader.settings.width, loader.settings.height, loader.settings.fullscreen);
    }
}
