using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CGPlay : MonoBehaviour
{
    // 进入 CG.unity 开始调用
    // 先获取 ChatBuilder.cs 中 CG 的位置
    public ChatBuilder Builder;

    public RawImage rawImage;   //用于在UI中显示视频的图像
    public VideoClip clip;
    private VideoPlayer videoPlayer;  //视频播放器组件

    public bool VideoCompleted = false;

    void Awake()
    {
        // 获取 ChatBuilder
        Builder = GameObject.Find("ChatBuilder").GetComponent<ChatBuilder>();
    }

    void Start()
    {
        VideoCompleted = false;
        clip = Resources.Load<VideoClip>(Builder.CG_Path);

        videoPlayer = gameObject.GetComponent<VideoPlayer>();      //获取VideoPlayer组件
        videoPlayer.prepareCompleted += OnVideoPrepared;         //注册视频准备完成时执行的回调方法
        videoPlayer.errorReceived += OnVideoError;  //注册当视频未获取到时执行的回调函数
        videoPlayer.clip = clip;

        videoPlayer.loopPointReached += OnVideoFinished;  //注册视频播放结束时执行的回调函数
    }

    //视频准备完成时执行的回调方法
    private void OnVideoPrepared(VideoPlayer source)
    {
        Debug.Log("Well done");
        rawImage.texture = source.texture;
    }
    //当视频未获取到时执行的回调函数
    private void OnVideoError(VideoPlayer source, string message)
    {
        Debug.LogError("Video error: " + message);
    }
    //视频播放结束时执行的回调函数
    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("视频播放完毕！");
        // 在这里执行视频播放完毕后的逻辑

        // 完成播放
        VideoCompleted = true;
    }
}
