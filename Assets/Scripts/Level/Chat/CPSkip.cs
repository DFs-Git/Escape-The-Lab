using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPSkip : MonoBehaviour
{
    public CGPlay player;

    // ���� CG
    public void Skip()
    {
        player.VideoCompleted = true;  // ������Ƶ������ɱ�־Ϊ true
    }
}
