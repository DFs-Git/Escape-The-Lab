using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CL = ChemicalLoader;

public class ReactionPool : MonoBehaviour
{
    public List<GameObject> Chemicals;
    public List<MolChemicals> MolChemicalsInReactionPool=new();

    [SerializeField, SetProperty("ÄÚÈÝ")]
    public string content;

    private void Update()
    {
        if (content != ToString()) content = ToString();
    }
    public void AddData(List<MolChemicals> data)
    {
        foreach (var c in data)
        {
            for (int i = 0; i < MolChemicalsInReactionPool.Count; i++)
            {
                if (MolChemicalsInReactionPool[i].Chemicals.ID == c.Chemicals.ID)
                {
                    MolChemicalsInReactionPool[i] = new MolChemicals(c.Chemicals, c.MolNum + MolChemicalsInReactionPool[i].MolNum);
                    return;
                }
            }

            MolChemicalsInReactionPool.Add(c);
        }
    }
    public void ReduceData(List<MolChemicals> data)
    {
        foreach (var c in data)
        {
            MolChemicalsInReactionPool.RemoveAll(n => (n.Chemicals.ID == c.Chemicals.ID));
        }
    }
    override public string ToString()
    {
        string s = "";
        foreach (MolChemicals t in MolChemicalsInReactionPool)
        {
            s += $"{t.ToString()},";
        }
        return s;
    }
}

