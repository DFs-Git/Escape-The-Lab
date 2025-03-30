using ChemicalDatabaseLoader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int ID;

    public string Name;
    public string Formula;
    public int Count;
    public string State;
    public string Form;

    void Start()
    {
        // Initialize the chemical loader if needed
        List<ChemicalDatabaseLoader.ChemicalDatabaseLoader.Chemical> Results;
        Results = ChemicalDatabaseLoader.ChemicalDatabaseLoader.FindChemicals(ID);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
