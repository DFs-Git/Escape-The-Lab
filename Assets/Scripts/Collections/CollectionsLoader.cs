using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChemicalDatabaseLoader;
using static ChemicalDatabaseLoader.ChemicalDatabaseLoader;
using CDL =  ChemicalDatabaseLoader.ChemicalDatabaseLoader;
using Unity.VisualScripting;
using Button = UnityEngine.UI.Button;
using TMPro;

public class CollectionsLoader : MonoBehaviour
{
    public Button Item;
    void Start()
    {
        List<Chemical> allChemicals = CDL.allChemicals;
        void OnButtonClick(int ID) {
            Chemical Che = CDL.FindChemicals(ID)[0];
            TextMeshProUGUI Name = GameObject.Find("Shower/Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI Formula = GameObject.Find("Shower/Formula").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI Category = GameObject.Find("Shower/Category").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI Content = GameObject.Find("Shower/Content").GetComponent<TextMeshProUGUI>();
            Name.text=Che.Name;
            Formula.text = Che.Formula;
            Category.text = Che.Category;
            Content.text = $"物理性质：{Che.PhysicaProperty}\n化学性质：{Che.ChemicalProperty}";
        }

        foreach (Chemical Che in allChemicals)
        {
            Button btn = (Button)Instantiate(Item, this.transform);
            btn.transform.SetParent(this.transform, false);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = Che.Name;

            int ID = Che.ID;//勿删，避免闭包
            btn.onClick.AddListener(()=>OnButtonClick(ID));
        }

        OnButtonClick(1);
    }

    void Update()
    {
        
    }
}
