using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mask : MonoBehaviour
{
    // 遮罩的Image组件
    public Image image;
    // 遮罩渐变的时间，默认为0.7秒
    public float TuringDuration = 0.7F;
    // 标记遮罩是否正在渐显
    public bool fadingIn = false;
    // 标记遮罩是否正在渐隐
    public bool fadingOut = false;

    // 在脚本初始化时调用，启动遮罩渐隐的协程
    void Start()
    {
        StartCoroutine(MaskFadeOut());
    }

    // 遮罩渐隐的协程
    public IEnumerator MaskFadeOut()
    {
        // 如果遮罩不在渐显状态
        if (!fadingIn)
        {
            // 当遮罩的透明度大于0且不在渐显状态时，继续渐隐
            while (image.color.a > 0 && !fadingIn)
            {
                fadingOut = true; // 设置渐隐标记为true
                // 减少遮罩的透明度
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.01F);
                // 等待一段时间，总渐隐时间由TuringDuration控制
                yield return new WaitForSeconds(TuringDuration / 100.0F);
            }

            fadingOut = false; // 渐隐完成，设置渐隐标记为false
        }
    }

    // 遮罩渐显的协程
    public IEnumerator MaskFadeIn(string sceneName)
    {
        // 如果遮罩正在渐隐，停止渐隐协程
        if (fadingOut)
        {
            StopCoroutine(MaskFadeOut());
            fadingOut = false;
        }
        // 如果遮罩不在渐隐状态
        if (!fadingOut)
        {
            // 当遮罩的透明度小于1且不在渐隐状态时，继续渐显
            while (image.color.a < 1.0F && !fadingOut)
            {
                fadingIn = true; // 设置渐显标记为true
                // 增加遮罩的透明度
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.01F);
                // 等待一段时间，总渐显时间由TuringDuration控制
                yield return new WaitForSeconds(TuringDuration / 100.0F);
            }

            fadingIn = false; // 渐显完成，设置渐显标记为false
            // 切换到指定场景
            SceneManager.LoadScene(sceneName);
        }
    }
}
