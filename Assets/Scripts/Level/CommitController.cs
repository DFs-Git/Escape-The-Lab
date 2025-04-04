using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CDL = ChemicalDatabaseLoader.ChemicalDatabaseLoader;

public class CommitController : MonoBehaviour
{
    public LevelLoader Loader;

    public TMP_Text CommitDescription;

    private void Awake()
    {
        CDL.LoadChemicals();
        Loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    private void Start()
    {
        CommitDescription.text = "Ã·Ωª";
        for (int i = 0; i < Loader.level.commit.Count; i += 2)
        {
            CDL.Chemical che = CDL.FindChemicals(Loader.level.commit[i])[0];
            CommitDescription.text += che.Formula;
            CommitDescription.text += "*" + Loader.level.commit[i + 1].ToString();
            CommitDescription.text += ",";
        }
        CommitDescription.text += "°£";
    }
}
