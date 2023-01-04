using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class slider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI value;
    // Start is called before the first frame update
    void Start()
    {
        value.text = PlayerPrefs.GetString("sensi_multi","1.00");
        _slider.value = float.Parse(PlayerPrefs.GetString("sensi_multi","1,00"));

        _slider.onValueChanged.AddListener((v)=>{
            value.text = v.ToString("0.00");
            PlayerPrefs.SetString("sensi_multi",value.text);
        });
        
    }
}
