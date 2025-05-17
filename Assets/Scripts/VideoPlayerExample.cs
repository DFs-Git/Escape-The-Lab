using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoPlayerExample : MonoBehaviour
{
    public RawImage rawImage;   //������UI����ʾ��Ƶ��ͼ��
    public VideoClip clip;
    private VideoPlayer videoPlayer;  //��Ƶ���������
    
    private string videoPath;   //�洢��Ƶ�ļ���·��

    void Start()
    {
        clip = Resources.Load<VideoClip>("CGs/testCG");

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

    }
}