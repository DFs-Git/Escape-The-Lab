using Fungus;
using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatBuilder : MonoBehaviour
{
    public ChatController Controller;
    public GameObject Canvas;

    public GameObject NormalDialog;
    public GameObject SpecialDialog;
    public GameObject ChoiceButton;
    public GameObject SkipButton;

    public Transform Choice;

    public Mask mask;
    public ChoiceCollector Collector;

    void Awake()
    {
        mask = GameObject.Find("Mask").GetComponent<Mask>();
    }

    void Start()
    {
        Instantiate(SkipButton, mask.transform);
        StartCoroutine(StartDialog());
    }


    public IEnumerator StartDialog()
    {
        // ����ָ�룬��¼��ǰ�����еĶԻ�
        // ��� index Ϊ -1��˵�����ζԻ�����
        int index = 0;
        List<string> single = new List<string>();

        // ȷ���Ի�ʱ���ܽ�����Ϸ����
        mask.image.raycastTarget = true;

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
                Debug.Log("Next " + StringToInt(single[3]).ToString());

                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

                // ���ٶԻ���
                Destroy(dia);
            }

            // �Լ��Ի�(�����䰵)
            if (single[0] == "1")
            {
                StartCoroutine(mask.MaskFadeIn(0.7F));

                // ��ȡ�Ի�����
                string content = single[1];

                // ���ɶԻ���
                GameObject dia = Instantiate(SpecialDialog, mask.transform);
                TMP_Text cont = dia.GetComponent<TMP_Text>();

                // ����Ի�����
                cont.text = "";
                for (int i = 0; i < content.Length; i++)
                {
                    cont.text += content[i];
                    yield return new WaitForSeconds(0.02F);
                }

                // ׼����һ�ζԻ�
                if (single[2] == "!") index = -1;
                else index = StringToInt(single[2]);
                Debug.Log("Next " + StringToInt(single[2]).ToString());

                // û�а��»س�����һֱ�ȴ�
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

                // ���ٶԻ���
                Destroy(dia);
                StartCoroutine(mask.MaskFadeOut());
            }

            // ѡ��
            if (single[0] == "2")
            {
                // ��������ʹ�����䰵
                StartCoroutine(mask.MaskFadeIn(0.7F));

                // ��ȡѡ�����
                int count = StringToInt(single[1]);

                // ѡ�ť���б������������
                List<GameObject> allBtn = new List<GameObject>();

                // ���� json �ж�ÿһ��ѡ�������
                for (int i = 2; i < single.Count; i += 2)
                {
                    string content = single[i];                             // ѡ������
                    int jump = StringToInt(single[i + 1]);                  // ���ѡ�����ת���ĶԻ�

                    GameObject btn = Instantiate(ChoiceButton, Choice);     // ����ѡ�ť
                    btn.GetComponent<ChatButton>().jump = jump;             // ����ѡ����ת
                    btn.GetComponentInChildren<TMP_Text>().text = content;  // ����ѡ���ı�
                    allBtn.Add(btn);                                        // ���밴ť���б�
                }

                // û�е��ѡ��͵ȴ�
                yield return new WaitUntil(() => Collector.ChoiceJump != -1);

                // ��ָ��(index)��ת��Ӧ����ת������
                index = Collector.ChoiceJump;
                Collector.ChoiceJump = -1;

                // �������а�ť
                foreach (GameObject btn in allBtn)
                {
                    Destroy(btn);
                }
                StartCoroutine(mask.MaskFadeOut());
            }
        } while (index != -1);

        // �������Ȩ��
        mask.image.raycastTarget = false;
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
