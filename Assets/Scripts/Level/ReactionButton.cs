using System.Collections;
using System.Collections.Generic;
using TMPro;
using Fungus;
using UnityEngine;
using Unity.VisualScripting;


public class ReactionButton : MonoBehaviour
{
    //在检查器中赋值
    public TMP_Dropdown Condition;
    public Flowchart Flowchart;
    public GameObject ChemicalPrefab;
    public GameObject CardPrefab;

    [HideInInspector]
    public ReactionPool reactionPool;     // 反应池控制器

    public void Start()
    {
        reactionPool = GameObject.Find("Reaction").GetComponent<ReactionPool>();
    }
    public void Clicked()
    {
        string condition = Condition.options[Condition.value].text;
        var result = EquationLoader.StrictSearchWithMultipleReactants(reactants:reactionPool.GetMolChemicalsInReactionPool(), condition: condition);
        Equation abledEquation = result.Item1;
        int multiple = result.Item2;

        Debug.Log($"=======================");
        EquationLoader.PrintEquations(abledEquation);
        Debug.Log($"{multiple}");
        Debug.Log($"=======================");
        //return;

        //Debug.Log($"=======================");
        //EquationLoader.PrintEquations(abledEquation);
        //Debug.Log("相似的化学方程式有：");
        ////EquationLoader.PrintEquations(EquationLoader.AdvancedSearch(reactants: ReactionPool.MolChemicalsInReactionPool, condition: condition));
        //Debug.Log("=======================");

        if (abledEquation.Equals(default(Equation)))
        {
            Flowchart.ExecuteBlock("NothingHappened");
        }
        else
        {
            for (int i = reactionPool.Chemicals.Count - 1; i >= 0; i--)
            {
                Destroy(reactionPool.Chemicals[i]);
            }
            reactionPool.Chemicals.Clear();

            foreach (MolChemical che in abledEquation.Products)
            {
                GameObject newChemical = Instantiate(ChemicalPrefab, reactionPool.transform);
                Chemical _chemicals = che.Chemical;
                newChemical.GetComponent<Chemicals>().ChemicalInclude = _chemicals;
                newChemical.GetComponent<Chemicals>().ParentCard = Instantiate(CardPrefab);
                //// 设置关于这张卡牌的属性，便于重新生成
                newChemical.GetComponent<Chemicals>().ParentMolChemical.Chemical = _chemicals;
                newChemical.GetComponent<Chemicals>().ParentMolChemical.MolNum = che.MolNum * multiple;

                newChemical.GetComponent<Chemicals>().Count = che.MolNum * multiple;
                newChemical.GetComponent<Chemicals>().following = false;
                newChemical.GetComponent<Chemicals>().entering = true;      

                reactionPool.Chemicals.Add(newChemical);
            }
        }
    }
}
