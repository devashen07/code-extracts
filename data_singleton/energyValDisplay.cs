using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class energyValDisplay : MonoBehaviour
{
    DataSingleton ds;
    private TextMeshProUGUI fieldText;
    int val = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        ds = DataSingleton.SharedInstance;

        fieldText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void FixedUpdate() 
    {
        
            int frameVal = ds.frame;
            switch(gameObject.name)
            {
                
                case "coal":
                    val = ds.months[ds.currentMonth][frameVal].coal;
                    break;
                case "nuclear":
                    val = ds.months[ds.currentMonth][frameVal].nuclear;
                    break;
                case "gas_turbine":
                    val = ds.months[ds.currentMonth][frameVal].gas_turbine;
                    break;
                case "ps":
                    val = ds.months[ds.currentMonth][frameVal].ps;
                    break;
                case "large_scale_hydro":
                    val = ds.months[ds.currentMonth][frameVal].large_scale_hydro;
                    break;
                case "international_imports":
                    val = ds.months[ds.currentMonth][frameVal].international_imports;
                    break;
                case "csp":
                    val = ds.months[ds.currentMonth][frameVal].csp;
                    break;
                case "wind":
                    val = ds.months[ds.currentMonth][frameVal].wind;
                    break;
                case "pv":
                    val = ds.months[ds.currentMonth][frameVal].pv;
                    break;
                case "other_renewable":
                    val = ds.months[ds.currentMonth][frameVal].other_renewable;
                    break;
                case "total":
                    val = ds.months[ds.currentMonth][frameVal].total;
                    break;
                case "totalexclcoal":
                    val = ds.months[ds.currentMonth][frameVal].total - ds.months[ds.currentMonth][frameVal].coal;
                    break;
                case "loadshedding":
                    val = ds.getLoadSheddingValue(frameVal);
                    break;
            
                default:
                    Debug.Log("We should never see this case! Default is here just to catch the case");
                    break;
                
            }
            string text = val+" MW";
            fieldText.SetText(text);
        
    }
}
