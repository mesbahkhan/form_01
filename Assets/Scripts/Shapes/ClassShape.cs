using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassShape : Shape
{
    [SerializeField]
    TextMeshPro classFrontNameText;
    [SerializeField]
    TextMeshPro classLayersNameText;

    protected override void UpdateNameView()
    {
        classFrontNameText.text = ShapeName;
        classLayersNameText.text = ShapeName;
    }

}
