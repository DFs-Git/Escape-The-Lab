using System.Collections;
using System.Collections.Generic;
using System.Linq; // 添加LINQ用于集合操作
using TMPro;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    public SettingsLoader loader;
    public Mask mask;

    public TMP_Dropdown screen;
    public Toggle fullsc;
    public Slider volume;

    // 存储设备支持的可用分辨率列表（已去重且排序）
    private List<Resolution> availableResolutions = new List<Resolution>();

    void Awake()
    {
        mask = GameObject.Find("Mask").GetComponent<Mask>();
    }

    private void Start()
    {
        try
        {
            // 获取设备支持的所有分辨率
            Resolution[] rawResolutions = Screen.resolutions;

            // 异常处理：如果没有可用分辨率
            if (rawResolutions.Length == 0)
            {
                Debug.LogError("未检测到可用分辨率！");
                return;
            }

            // 去重处理：只保留不同宽高组合的分辨率（忽略刷新率差异）
            // 按宽高从大到小排序
            availableResolutions = rawResolutions
                .GroupBy(r => $"{r.width}x{r.height}") // 按宽高分组
                .Select(g => g.OrderByDescending(r => r.width).First()) // 取每组最大宽度
                .OrderByDescending(r => r.width) // 整体按宽度降序
                .ThenByDescending(r => r.height) // 其次按高度降序
                .ToList();

            // 清空并重新填充下拉菜单
            screen.ClearOptions();
            List<string> options = new List<string>();
            foreach (Resolution res in availableResolutions)
            {
                options.Add($"{res.width}x{res.height}");
            }
            screen.AddOptions(options);

            // 设置默认选中项（匹配当前分辨率或第一个）
            int savedWidth = PlayerPrefs.GetInt("width", Screen.currentResolution.width);
            int savedHeight = PlayerPrefs.GetInt("height", Screen.currentResolution.height);
            int defaultIndex = availableResolutions.FindIndex(r =>
                r.width == savedWidth && r.height == savedHeight);
            screen.value = defaultIndex;
        }
        catch (System.Exception ex)
        {
            // 异常处理：记录错误并设置默认值
            Debug.LogError($"分辨率初始化失败： {ex.Message}");
            screen.value = 0;
        }

        // 初始化全屏状态（从PlayerPrefs读取，默认使用当前状态）
        bool defaultFullscreen = Screen.fullScreen;
        fullsc.isOn = PlayerPrefs.GetInt("fullscreen", defaultFullscreen ? 1 : 0) == 1;
        volume.value = PlayerPrefs.GetFloat("volume");
    }

    public void Back()
    {
        StartCoroutine(mask.MaskFadeIn(0));
    }

    public void Apply()
    {
        try
        {
            // 获取选中的分辨率索引
            int selectedIndex = screen.value;

            // 异常处理：索引越界时使用第一个分辨率
            if (selectedIndex < 0 || selectedIndex >= availableResolutions.Count)
            {
                Debug.LogWarning("分辨率索引无效，使用第一个可用索引");
                selectedIndex = 0;
            }

            // 获取分辨率数据
            Resolution selectedRes = availableResolutions[selectedIndex];
            bool fullScreen = fullsc.isOn;

            // 保存设置
            PlayerPrefs.SetInt("fullscreen", fullScreen ? 1 : 0);
            PlayerPrefs.SetInt("width", selectedRes.width);
            PlayerPrefs.SetInt("height", selectedRes.height);
            PlayerPrefs.Save();

            // 应用分辨率设置
            Screen.SetResolution(selectedRes.width, selectedRes.height, fullScreen);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"应用设置失败： {ex.Message}");
            // 可以在这里添加UI错误提示 （懒得写了
        }
    }
}