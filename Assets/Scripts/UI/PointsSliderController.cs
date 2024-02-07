using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointsSliderController : MonoBehaviour
{
    public TextMeshProUGUI label;

    void Update()
    {
        label.text = gameObject.GetComponent<Slider>().value.ToString();    
    }
}
