using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GraphMolWrap;

public class ChemicalManager : MonoBehaviour
{
    /// <summary>
    /// 管理场景中所有的有机物
    /// </summary>
    public List<GameObject> Matters = new List<GameObject>();
    public List<string> MatterSmiles;

    public GameObject OrganicMatterPrefab;

    public Transform ProductContent;
    public Transform ReactantContent;

    public void InstantiateMatter(string smiles)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        // 酯化反应
        GraphMolWrap.ChemicalReaction rxn =
            GraphMolWrap.ChemicalReaction.ReactionFromSmarts("[CX3:1](=[OX1:2])([OX2H:3]) . [OX2H:4][*:5] >> [CX3:1](=[OX1:2])([OX2:4][*:5])");
        GraphMolWrap.ChemicalReaction rxn_inv =
            GraphMolWrap.ChemicalReaction.ReactionFromSmarts("[OX2H:4][*:5] . [CX3:1](=[OX1:2])([OX2H:3]) >> [CX3:1](=[OX1:2])([OX2:4][*:5])");

        List <RWMol> reactants = new List<RWMol>();
        foreach (var smiles in MatterSmiles)
        {
            RWMol rct = RWMol.MolFromSmiles(smiles);
            GameObject reactant = Instantiate(OrganicMatterPrefab, ReactantContent);
            reactant.GetComponent<OrganicChemical>().LoadChemicalFromSmiles(smiles);
            reactant.GetComponent<OrganicChemical>().Display();
            reactants.Add(rct);
        }
        
        var r1 = RWMol.MolFromSmiles("CC(=O)O");
        var r2 = RWMol.MolFromSmiles("CCO");

        var products = rxn.runReactants(new ROMol_Vect(reactants));
        if (products.Count == 0) products = rxn_inv.runReactants(new ROMol_Vect(reactants));

        Dictionary<string, bool> vis = new Dictionary<string, bool>();

        // products 是一个列表，每个元素是一个产物的元组
        foreach (var productTuple in products)
        {
            // 每个productTuple可能包含多个产物分子
            foreach (var productMol in productTuple)
            {
                // 在这里处理你的产物分子，例如转换为SMILES显示
                string productSmiles = productMol.MolToSmiles();

                if (!vis.ContainsKey(productSmiles))
                {
                    vis[productSmiles] = true;
                    GameObject product = Instantiate(OrganicMatterPrefab, ProductContent);
                    product.GetComponent<OrganicChemical>().LoadChemicalFromSmiles(productSmiles);
                    product.GetComponent<OrganicChemical>().Display();
                    Debug.Log("产物: " + productSmiles);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
