using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPSkip : MonoBehaviour
{
    public CGPlay player;

    // 跳过 CG
    public void Skip()
    {
        player.VideoCompleted = true;  // 设置视频播放完成标志为 true
    }
}
