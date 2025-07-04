using Fungus;
using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ChatBuilder : MonoBehaviour
{
    public static ChatBuilder Instance;

    public ChatController Controller;
    public GameObject Canvas;

    public GameObject NormalDialog;
    public GameObject SpecialDialog;
    public GameObject ChoiceButton;

    public Transform Choice;

    public Mask mask;
    public LevelLoader Loader;
    public ChoiceCollector Collector;

    public string CG_Path;

    void Awake()
    {
        Loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        mask = GameObject.Find("Mask").GetComponent<Mask>();
    }

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ȷ���ö������л�����ʱ���ᱻ����
        }
        else
        {
            Destroy(gameObject); // �����ظ���ʵ��
        }

        StartCoroutine(StartDialog());
    }

    public IEnumerator StartDialog()
    {
        // ����ָ�룬��¼��ǰ�����еĶԻ�
        // ��� index Ϊ -1��˵�����ζԻ�����
        int index = 0;
        List<string> single = new List<string>();

        if (Controller.dialog.dialogs.Count == 0)
        {
            yield return null;
        }

        // ȷ���Ի�ʱ���ܽ�����Ϸ����
        mask.image.raycastTarget = true;

        do
        {
            if (Collector == null)
                Collector = GameObject.Find("Choice").GetComponent<ChoiceCollector>();
            if (Choice == null)
                Choice = GameObject.Find("Choice").transform;
            if (Canvas == null)
                Canvas = GameObject.Find("Canvas");
            if (mask == null)
                mask = GameObject.Find("Mask").GetComponent<Mask>();

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

            // ڤ��(�����䰵)
            if (single[0] == "1")
            {
                if (index == 0)
                    yield return new WaitUntil(() => { return mask.image.color.a <= 0.0F; });
                StartCoroutine(mask.MaskFadeIn(0.8F));

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

                if (index == -1)
                    StartCoroutine(mask.MaskFadeOut());
                else if (Controller.dialog.dialogs[index][0] == "0")
                    StartCoroutine(mask.MaskFadeOut());
            }

            // ѡ��
            if (single[0] == "2")
            {
                if (index == 0)
                    yield return new WaitUntil(() => { return mask.image.color.a <= 0.0F; });
                StartCoroutine(mask.MaskFadeIn(0.8F));

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

                if (index == -1)
                    StartCoroutine(mask.MaskFadeOut());
                else if (Controller.dialog.dialogs[index][0] == "0")
                    StartCoroutine(mask.MaskFadeOut());
            }

            // ���� CG
            if (single[0] == "3")
            {
                if (index == 0)
                    yield return new WaitUntil(() => { return mask.image.color.a <= 0.0F; });

                CG_Path = single[1];
                if (single[2] == "!") index = -1;
                else index = StringToInt(single[2]);
                // �л������� CG.unity
                StartCoroutine(mask.MaskFadeIn("CG"));

                // �ȴ� CG �������
                // �ȴ��л������� CG.unity���ٵȴ��������
                yield return new WaitUntil(() => { return SceneManager.GetActiveScene().name == "CG"; });
                // ���ͱ�����������
                GameObject audioManager = GameObject.Find("AudioManager");
                audioManager.GetComponent<AudioSource>().volume = 0f;
                Debug.Log("Oh Ye");
                CGPlay player = GameObject.Find("Video Player").GetComponent<CGPlay>();
                yield return new WaitUntil(() => player.VideoCompleted);                    // �ȴ��������

                // �����ָ�����
                audioManager.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume");
                mask = GameObject.Find("Mask").GetComponent<Mask>();    // ��ȡ CG.unity �� Mask
                StartCoroutine(mask.MaskFadeIn("Level"));               // ���� Level.unity
                yield return new WaitUntil(() => { return SceneManager.GetActiveScene().name == "Level"; });
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
