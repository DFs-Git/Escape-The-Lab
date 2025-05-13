using Fungus;
using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBuilder : MonoBehaviour
{
    public ChatController Controller;
    public GameObject Canvas;

    public GameObject NormalDialog;

    public Mask mask;

    void Awake()
    {
        mask = GameObject.Find("Mask").GetComponent<Mask>();
    }

    void Start()
    {
        StartCoroutine(StartDialog());
    }


    IEnumerator StartDialog()
    {
        int index = 0;
        List<string> single = Controller.dialog.dialogs[index];


        do
        {
            single = Controller.dialog.dialogs[index];

            // ��ͨ�Ի�
            if (single[0] == "0")
            {
                // ��ȡ�Ի�����
                string speaker = single[1];
                string content = single[2];

                // ���ɶԻ���
                GameObject dia = Instantiate(NormalDialog, Canvas.transform);
                GameObject spk = dia.transform.Find("Speaker").gameObject;
                TMP_Text cont = dia.transform.Find("Content").gameObject.GetComponent<TMP_Text>();

                spk.GetComponentInChildren<TMP_Text>().text = speaker;

                // ����Ի�����
                cont.text = "";
                for (int i = 0; i < content.Length; i++)
                {
                    cont.text += content[i];
                    yield return new WaitForSeconds(0.02F);
                }

                // ׼����һ�ζԻ�
                if (single[3] == "!") index = -1;
                else index = StringToInt(single[3]);
                // Debug.Log("Next " + StringToInt(single[3]).ToString());

                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

                // ���ٶԻ���
                Destroy(dia);
            }

            // �Լ��Ի�
            if (single[0] == "1")
            {
                StartCoroutine(mask.MaskFadeIn(0.5F));

                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
            }

            // ѡ��
            if (single[0] == "2")
            {

            }
        } while (index != -1);
    }

    // ʵ���ַ���ת����(δ���������)
    public int StringToInt(string s)
    {
        int res = 0;
        for (int i = 0; i < s.Length; i++)
        {
            res *= 10;
            res += s[i] - '0';
        }

        return res;
    }
}
