using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CGPlay : MonoBehaviour
{
    // ���� CG.unity ��ʼ����
    // �Ȼ�ȡ ChatBuilder.cs �� CG ��λ��
    public ChatBuilder Builder;

    public RawImage rawImage;   //������UI����ʾ��Ƶ��ͼ��
    public VideoClip clip;
    private VideoPlayer videoPlayer;  //��Ƶ���������

    public bool VideoCompleted = false;

    void Awake()
    {
        // ��ȡ ChatBuilder
        Builder = GameObject.Find("ChatBuilder").GetComponent<ChatBuilder>();
    }

    void Start()
    {
        VideoCompleted = false;
        clip = Resources.Load<VideoClip>(Builder.CG_Path);

        videoPlayer = gameObject.GetComponent<VideoPlayer>();      //��ȡVideoPlayer���
        videoPlayer.prepareCompleted += OnVideoPrepared;         //ע����Ƶ׼�����ʱִ�еĻص�����
        videoPlayer.errorReceived += OnVideoError;  //ע�ᵱ��Ƶδ��ȡ��ʱִ�еĻص�����
        videoPlayer.clip = clip;

        videoPlayer.loopPointReached += OnVideoFinished;  //ע����Ƶ���Ž���ʱִ�еĻص�����
    }

    //��Ƶ׼�����ʱִ�еĻص�����
    private void OnVideoPrepared(VideoPlayer source)
    {
        Debug.Log("Well done");
        rawImage.texture = source.texture;
    }
    //����Ƶδ��ȡ��ʱִ�еĻص�����
    private void OnVideoError(VideoPlayer source, string message)
    {
        Debug.LogError("Video error: " + message);
    }
    //��Ƶ���Ž���ʱִ�еĻص�����
    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("��Ƶ������ϣ�");
        // ������ִ����Ƶ������Ϻ���߼�

        // ��ɲ���
        VideoCompleted = true;
    }
}
