using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CDL = ChemicalDatabaseLoader.ChemicalDatabaseLoader;

public class ReactionPool : MonoBehaviour
{
    public List<GameObject> Chemicals;
    public List<CDL.MolChemicals> MolChemicalsInReactionPool=new();

    [SerializeField, SetProperty("ÄÚÈÝ")]
    public string content;

    private void Update()
    {
        if (content != ToString()) content = ToString();
    }
    public void AddData(List<CDL.MolChemicals> data)
    {
        foreach (var c in data)
        {
            for (int i = 0; i < MolChemicalsInReactionPool.Count; i++)
            {
                if (MolChemicalsInReactionPool[i].Chemicals.ID == c.Chemicals.ID)
                {
                    MolChemicalsInReactionPool[i] = new CDL.MolChemicals(c.Chemicals, c.MolNum + MolChemicalsInReactionPool[i].MolNum);
                    return;
                }
            }

            MolChemicalsInReactionPool.Add(c);
        }
    }
    public void ReduceData(List<CDL.MolChemicals> data)
    {
        foreach (var c in data)
        {
            MolChemicalsInReactionPool.RemoveAll(n => (n.Chemicals.ID == c.Chemicals.ID));
        }
    }
    override public string ToString()
    {
        string s = "";
        foreach (CDL.MolChemicals t in MolChemicalsInReactionPool)
        {
            s += $"{t.ToString()},";
        }
        return s;
    }
}

