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

    /// <summary>
    /// 把一个有机物根据 SMILES 实例化。
    /// </summary>
    /// <param name="smiles">有机物的 SMILES 序列</param>
    /// <param name="parent">有机物生成的位置(父亲)的 Transform 组件</param>
    /// <returns></returns>
    public GameObject InstantiateMatter(string smiles, Transform parent)
    {
        GameObject Matter = Instantiate(OrganicMatterPrefab, parent);
        Matter.GetComponent<OrganicChemical>().LoadChemicalFromSmiles(smiles);
        Matter.GetComponent<OrganicChemical>().Display();
        Matter.name = $"Matter_{{{smiles}}}";

        return Matter;
    }

    /// <summary>
    /// 从一个对象的孩子中读取所有的有机物
    /// </summary>
    /// <param name="content">这个对象的 Transform 组件</param>
    /// <returns></returns>
    public List<RWMol> ReadMolFromContent(Transform content)
    {
        List<RWMol> mols = new List<RWMol>();

        // 遍历所有孩子
        for (int i = 0; i < content.childCount; i++)
        {
            OrganicChemical child = content.GetChild(i).GetComponent<OrganicChemical>();
            if (child == null) continue;

            mols.Add(child.OrgChemical);
        }

        if (mols.Count == 0) Debug.LogWarning("没有从 " + content + " 读取到物质，请留意。");
        return mols;
    }

    /// <summary>
    /// 根据给定的反应物和反应 SMARTS 序列，执行*一次*反应并返回产物。
    /// 请注意让反应物个数匹配 SMARTS 序列的反应物个数。
    /// </summary>
    /// <param name="smarts">所有想要匹配的反应 SMARTS 序列</param>
    /// <param name="reactants">反应物</param>
    /// <returns></returns>
    public List<RWMol> RunReaction(List<string> smarts, List<RWMol> reactants)
    {
        // 读取反应式
        List<GraphMolWrap.ChemicalReaction> rxns = new List<GraphMolWrap.ChemicalReaction>();

        // 尝试使用每个 SMARTS 反应式进行反应
        foreach (var smart in smarts)
        {
            GraphMolWrap.ChemicalReaction rxn = GraphMolWrap.ChemicalReaction.ReactionFromSmarts(smart);

            // 反应物个数与 SMARTS 序列不匹配
            if (rxn.getNumReactantTemplates() != reactants.Count) continue;

            var productsVectVect = rxn.runReactants(new ROMol_Vect(reactants));
            // 匹配到一个可反应的反应式
            if (productsVectVect.Count > 0)
            {
                // 将 ROMol_Vect_Vect 转换为 List<RWMol>
                List<RWMol> productsList = new List<RWMol>();

                foreach (ROMol_Vect productTuple in productsVectVect)
                {
                    foreach (ROMol productMol in productTuple)
                    {
                        // ROMol 转换为 RWMol
                        // RDKit 为什么要用两个类型描述同一个东西？
                        RWMol RW_product = new RWMol(productMol);
                        productsList.Add(RW_product);
                    }
                }

                // 返回产物列表
                return productsList;
            }
        }

        Debug.LogWarning("反应没有产物，请留意。");
        // 如果没有产物，返回空列表
        return new List<RWMol>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 酯化反应
        string rxn_smarts = "[CX3:1](=[OX1:2])([OX2H:3]) . [OX2H:4][*:5] >> [CX3:1](=[OX1:2])([OX2:4][*:5])";
        string rxn_inv_smarts = "[OX2H:4][*:5] . [CX3:1](=[OX1:2])([OX2H:3]) >> [CX3:1](=[OX1:2])([OX2:4][*:5])";

        // 加成反应
        string rxn2 = "[C:1]=[C:2].[F,Cl,Br,I:3][F,Cl,Br,I:4] >> [C:1]([F,Cl,Br,I:3])[C:2]([F,Cl,Br,I:4])";

        // 读取反应物分子
        List<RWMol> reactants = new List<RWMol>();
        foreach (var smiles in MatterSmiles)
        {
            GameObject matter = InstantiateMatter(smiles, ReactantContent);
            RWMol rct = matter.GetComponent<OrganicChemical>().OrgChemical;
            reactants.Add(rct);
        }
        
        var products = RunReaction(new List<string> { rxn2 }, reactants);

        // 防止重复产物
        Dictionary<string, bool> vis = new Dictionary<string, bool>();
        // 输出所有产物分子
        foreach (var productMol in products)
        {
            string productSmiles = productMol.MolToSmiles();

            if (!vis.ContainsKey(productSmiles))
            {
                vis[productSmiles] = true;
                InstantiateMatter(productSmiles, ProductContent);
                Debug.Log("产物: " + productSmiles);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
