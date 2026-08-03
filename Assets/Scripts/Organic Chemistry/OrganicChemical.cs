using GraphMolWrap;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UIElements;

public class OrganicChemical : MonoBehaviour
{
    public float scale = 1.5f;
    public float padding = 10f;

    public SVGImage img;
    public RectTransform parentTransform;

    public GraphMolWrap.RWMol OrgChemical;

    /// <summary>
    /// 按 SMILES 加载有机物
    /// </summary>
    /// <param name="smiles">有机物的 SMILES 序列</param>
    public void LoadChemicalFromSmiles(string smiles)
    {
        OrgChemical = RWMol.MolFromSmiles(smiles);
        if (OrgChemical == null) Debug.LogWarning($"有机物(SMILES) {smiles} 加载失败，请留意。");
        else Debug.Log($"有机物 {smiles} 加载成功！");
    }

    /// <summary>
    /// 把有机物的结构绘制出来
    /// </summary>
    public void Display()
    {
        if (OrgChemical == null)
        {
            Debug.LogWarning("有机物未加载，无法绘制，请留意。");
            return;
        }

        // 生成 SVG，自动设置大小
        GraphMolWrap.MolDraw2DSVG svg = new GraphMolWrap.MolDraw2DSVG(-1, -1);
        svg.setFlexiMode(false);
        svg.drawMolecule(OrgChemical);
        svg.finishDrawing();
        string svgtxt = svg.getDrawingText();

        parentTransform.sizeDelta = new Vector2(svg.width() * scale + padding * 2, svg.height() * scale + padding * 2);
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(svg.width() * scale, svg.height() * scale);
        img.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, padding + svg.height() * scale);

        // 绘制 SVG
        var doc = SVGParser.ImportSVG(new System.IO.StringReader(svgtxt));
        var tessOptions = new VectorUtils.TessellationOptions
        {
            StepDistance = 0.1f,              // 步进距离，控制整体精细度
            SamplingStepSize = 1,         // 采样步数
            MaxCordDeviation = 0.1f,        // 最大弦偏差
            MaxTanAngleDeviation = 0.1f     // 最大切角偏差
        };
        var geos = VectorUtils.TessellateScene(doc.Scene, tessOptions);

        var sprite = VectorUtils.BuildSprite(geos, 200f, VectorUtils.Alignment.Center, Vector2.zero, 128, false);

        img.sprite = sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        LoadChemicalFromSmiles("P(=O)(O)(O)(O)");
        Display();
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
