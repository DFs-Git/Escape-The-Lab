using ChemicalDatabaseLoader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CDL = ChemicalDatabaseLoader.ChemicalDatabaseLoader;
public class Card : MonoBehaviour
{
    public int ID;

    public string Name;
    public string Formula;
    public int Count;
    public string State;
    public string Form;

    private void Awake()
    {
        CDL.LoadChemicals();
    }
    void Start()
    {
        // Initialize the chemical loader if needed
        List<CDL.Chemical> Results;
        Results = CDL.FindChemicals(ID);
        CDL.PrintChemicals(Results);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
