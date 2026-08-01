using GraphMolWrap;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using static Unity.VectorGraphics.SVGParser;

public class RDKitTest : MonoBehaviour
{
    public SVGImage img;

    // Start is called before the first frame update
    void Start()
    {
        var che = RWMol.MolFromSmiles("CC(=O)OCC");
        if (che == null) Debug.Log("No!");

        GraphMolWrap.MolDraw2DSVG svg = new GraphMolWrap.MolDraw2DSVG(100, 100);
        // svg.addMoleculeMetadata(che);
        svg.drawMolecule(che);
        svg.finishDrawing();
        string svgtxt = svg.getDrawingText();

        Debug.Log(svgtxt);

        var doc = SVGParser.ImportSVG(new System.IO.StringReader(svgtxt));
        var tessOptions = new VectorUtils.TessellationOptions
        {
            StepDistance = 0.1f,          // 步进距离，控制整体精细度
            SamplingStepSize = 100,      // 采样步数
            MaxCordDeviation = 0.5f,    // 最大弦偏差
            MaxTanAngleDeviation = 0.1f // 最大切角偏差
        };
        var geos = VectorUtils.TessellateScene(doc.Scene, tessOptions);

        var sprite = VectorUtils.BuildSprite(geos, 200f, VectorUtils.Alignment.Center, Vector2.zero, 128, false);

        img.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
