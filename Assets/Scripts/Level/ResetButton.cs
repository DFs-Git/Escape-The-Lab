using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public Mask mask;

    void Awake()
    {
        // 获取Mask
        mask = GameObject.Find("Mask").GetComponent<Mask>();
    }

    public void Cliked()
    {
        //ReactionPool.MolChemicalsInReactionPool.Clear();
        // 获取当前场景的索引并重新加载
        string currentSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(mask.MaskFadeIn(currentSceneName));
        // SceneManager.LoadScene(currentSceneIndex);
    }
}
