using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the data object that controls the information that flows through
/// the SA map visual. This class is a Singleton, thus it will live from the start of the visual
/// until the end, all scripts can gain access to the data from the sole instance of this class.
/// Furthermore, the class stores the state of certain aspects like the month and the 
/// maximum energy value per energy type per month, etc.
/// 
/// The data is split into months and stored. When the visual requires a certain month, it will be 
/// retrieved from this class, the max value for a month will be used for scaling 
/// the spheres in the visual.
/// 
/// </summary>
/// <param name=""></param>

/// <returns></returns>
public class DataSingleton 
{
    // reference dates for the start of a month
    // for grouping data points
    private DateTime jan = new DateTime(2019, 1, 1); 
    private DateTime feb = new DateTime(2019, 2, 1); 
    private DateTime mar = new DateTime(2019, 3, 1); 
    private DateTime apr = new DateTime(2019, 4, 1); 
    private DateTime may = new DateTime(2019, 5, 1); 
    private DateTime jun = new DateTime(2019, 6, 1); 
    private DateTime jul = new DateTime(2019, 7, 1); 
    private DateTime aug = new DateTime(2019, 8, 1); 
    private DateTime sep = new DateTime(2019, 9, 1); 
    private DateTime oct = new DateTime(2019, 10, 1); 
    private DateTime nov = new DateTime(2019, 11, 1); 
    private DateTime dec = new DateTime(2019, 12, 1);   

    // sole instance of this class ala Singleton
    private static DataSingleton instance = null;

    // keep track of current month so that 
    // we know what data to use in the visual
    public string currentMonth = "jan";


    // Dictionary of data split into months
    // Each month contains EnergyObjects that holds energy data per hour
    public Dictionary<string,List<EnergyObjects>> months = new Dictionary<string, List<EnergyObjects>>();

    public Dictionary<string,double> energyMaxes = new Dictionary<string, double>();

    public List<string> energyTypes = new List<string>();

    public int frame = 0;

    public int newFrameValue = 0;

    public int originalFrameRate = 20;
    public int frameRate;

    public int speed = 1;

    public bool justResetFrame = false;
    public bool pauseFlag = false;

    public bool lineGraph = false;
    public bool sliderResetFrame = false;
    public int lineResetCount = 100;
    public int lineGraphChangeViewIndex = 0;
    public bool lineGraphViewFlag = false;

    public Dictionary<string,bool> validEnergyTypes = new Dictionary<string, bool>();
    
    public EnergyObjectList centralEnergyObjectList = null;

    private DataSingleton()
    {
        this.frameRate = this.originalFrameRate;
        setupEnergyTypes();

        foreach (string type in this.energyTypes)
        {

            this.validEnergyTypes[type] = true;
        }
        //Debug.Log(this.validEnergyTypes);
    }

    public static DataSingleton SharedInstance 
    {
        get {
            if (instance == null) {
                instance = new DataSingleton();
            }
            return instance;
        }
    }

    public void setFrameFlag(bool resetFrame)
    {
        justResetFrame = resetFrame;
    }

    private void setupEnergyTypes()
    {
        this.energyTypes.Add("coal");
        this.energyTypes.Add("nuclear");
        this.energyTypes.Add("gas_turbine");
        this.energyTypes.Add("ps");
        this.energyTypes.Add("large_scale_hydro");
        this.energyTypes.Add("international_imports");
        this.energyTypes.Add("csp");
        this.energyTypes.Add("wind");
        this.energyTypes.Add("pv");
        this.energyTypes.Add("other_renewable");
    }
    
    public void setEnergyObjectList(EnergyObjectList energyObjectList)
    {
        this.centralEnergyObjectList = energyObjectList;
    }

    public void setCurrentMonth(string month)
    {
        this.currentMonth = month;
    }

    public void setFrame(int frame)
    {
        this.frame = frame;
    }

    public void setEnergyTypeActive(string type, bool isActive)
    {
        this.validEnergyTypes[type] = isActive;
        //Debug.Log("just set status of "+type+" to "+isActive);
        //Debug.Log("it actually is "+this.validEnergyTypes[type]);
    }


    public void printMonthSizes()
    {
        foreach(KeyValuePair<string, List<EnergyObjects>> entry in this.months)
        {
            Debug.Log(entry.Key+ " : length is "+entry.Value.Count);
        }
    }

    public int getMonthSize(string month)
    {
        if (months.ContainsKey(month))
        {
            int monthCount = months[month].Count;
            return monthCount;
        }

        return 744;
    }

    public int getNextValueForType(int index)
    {
        return this.months[this.currentMonth][index].total;
    }

    public float getNextValueForTypeAndProvince(int index,string province)
    {
        double value = this.months[this.currentMonth][index].total*proportions[province];
        float f = Convert.ToSingle(value);
        return f;
    }

    public float getNextValueForTypes(int index, int energyType)
    {
        double energyTypeVal = 0.0;
        
        switch(energyType)
        {
            case 0:
                energyTypeVal = this.months[this.currentMonth][index].gas_turbine;
                break;
            case 1:
                energyTypeVal = this.months[this.currentMonth][index].ps;
                break;
            case 2:
                energyTypeVal = this.months[this.currentMonth][index].other_renewable;
                break;
            case 3:
                energyTypeVal = this.months[this.currentMonth][index].large_scale_hydro;
                break;
            case 4:
                energyTypeVal = this.months[this.currentMonth][index].csp;
                break;
            case 5:
                energyTypeVal = this.months[this.currentMonth][index].pv;
                break;
            case 6:
                energyTypeVal = this.months[this.currentMonth][index].wind;
                break;
            case 7:
                energyTypeVal = this.months[this.currentMonth][index].international_imports;
                break;
            case 8:
                energyTypeVal = this.months[this.currentMonth][index].nuclear;
                break;
            case 9:
                energyTypeVal = this.months[this.currentMonth][index].coal;
                break;
            default:
                energyTypeVal = 0;
                Debug.Log("We should never see this case! Default is here just to catch the case");
                break;
        }

        float f = Convert.ToSingle(energyTypeVal);

        return f;
    }

    public float getNextValueForTypeAndProvince(int index,string province,string energyType)
    {
        double energyTypeVal = 0.0;
        
        switch(energyType)
        {
            case "coal":
                energyTypeVal = this.months[this.currentMonth][index].coal;
                break;
            case "nuclear":
                energyTypeVal = this.months[this.currentMonth][index].nuclear;
                break;
            case "gas_turbine":
                energyTypeVal = this.months[this.currentMonth][index].gas_turbine;
                break;
            case "ps":
                energyTypeVal = this.months[this.currentMonth][index].ps;
                break;
            case "large_scale_hydro":
                energyTypeVal = this.months[this.currentMonth][index].large_scale_hydro;
                break;
            case "international_imports":
                energyTypeVal = this.months[this.currentMonth][index].international_imports;
                break;
            case "csp":
                energyTypeVal = this.months[this.currentMonth][index].csp;
                break;
            case "wind":
                energyTypeVal = this.months[this.currentMonth][index].wind;
                break;
            case "pv":
                energyTypeVal = this.months[this.currentMonth][index].pv;
                break;
            case "other_renewable":
                energyTypeVal = this.months[this.currentMonth][index].other_renewable;
                break;
            default:
                energyTypeVal = 0;
                Debug.Log("We should never see this case! Default is here just to catch the case");
                break;
        }


        double value = energyTypeVal*proportions[province];
        float f = Convert.ToSingle(value);

        return f;
    }

    

    public void createMonthLists()
    {

        foreach (EnergyObjects energyObj in this.centralEnergyObjectList.EnergyObjects)
        {
            addRecordToMonth(energyObj);
        }

    }

    
    /*
        Check if the dictionary contains a key, 
        if it does not then create the key
    */
    private void checkMonthKey(string key)
    {
        
        if(!this.months.ContainsKey(key))
        {
            this.months[key] = new List<EnergyObjects>();
        }
    }

    private void updateEnergyMaxes(EnergyObjects energyObj, string month)
    {
        foreach (string type in energyTypes)
        {
            updateEnergyMax(energyObj,month,type);

        }
    }

    private void updateEnergyMax(EnergyObjects energyObj, string month, string energyType)
    {
        string key = month+"-"+energyType;
        int val = 0;
        switch(energyType)
        {
            case "coal":
                val = energyObj.coal;
                break;
            case "nuclear":
                val = energyObj.nuclear;
                break;
            case "gas_turbine":
                val = energyObj.gas_turbine;
                break;
            case "ps":
                val = energyObj.ps;
                break;
            case "large_scale_hydro":
                val = energyObj.large_scale_hydro;
                break;
            case "international_imports":
                val = energyObj.international_imports;
                break;
            case "csp":
                val = energyObj.csp;
                break;
            case "wind":
                val = energyObj.wind;
                break;
            case "pv":
                val = energyObj.pv;
                break;
            case "other_renewable":
                val = energyObj.other_renewable;
                break;
            default:
                Debug.Log("Something went wrong here!!");
                return;
        }
        if(energyMaxes.ContainsKey(key))
        {
            if(val > energyMaxes[key])
            {
                energyMaxes[key] = val;
            }
        }
        else
        {
            energyMaxes[key] = val;
        }

    }

    public int getMaxValForMonth()
    {
        int max = 0;

        // Energy keys are standard for all my dictionaries except energyMaxes
        // since that's on a monthly basis
        foreach (string energy in validEnergyTypes.Keys)
        {
            if(validEnergyTypes[energy] == true)
            {
                if(energyMaxes[currentMonth+"-"+energy] > max)
                {
                    max = (int)energyMaxes[currentMonth+"-"+energy];
                }
            }
        }

        return max;

        
    }
    
    /*
        Process the energy objects into buckets per month
    */
    private void addRecordToMonth(EnergyObjects energyObj)
    {
        switch(energyObj.getDateTime().Month)
        {
            case 1:
                checkMonthKey("jan");
                months["jan"].Add(energyObj);
                updateEnergyMaxes(energyObj,"jan");
                break;
            case 2:
                checkMonthKey("feb");
                months["feb"].Add(energyObj);
                updateEnergyMaxes(energyObj,"feb");
                break;
            case 3:
                checkMonthKey("mar");
                months["mar"].Add(energyObj);
                updateEnergyMaxes(energyObj,"mar");
                break;
            case 4:
                checkMonthKey("apr");
                months["apr"].Add(energyObj);
                updateEnergyMaxes(energyObj,"apr");
                break;
            case 5:
                checkMonthKey("may");
                months["may"].Add(energyObj);
                updateEnergyMaxes(energyObj,"may");
                break;
            case 6:
                checkMonthKey("jun");
                months["jun"].Add(energyObj);
                updateEnergyMaxes(energyObj,"jun");
                break;
            case 7:
                checkMonthKey("jul");
                months["jul"].Add(energyObj);
                updateEnergyMaxes(energyObj,"jul");
                break;
            case 8:
                checkMonthKey("aug");
                months["aug"].Add(energyObj);
                updateEnergyMaxes(energyObj,"aug");
                break;
            case 9:
                checkMonthKey("sep");
                months["sep"].Add(energyObj);
                updateEnergyMaxes(energyObj,"sep");
                break;
            case 10:
                checkMonthKey("oct");
                months["oct"].Add(energyObj);
                updateEnergyMaxes(energyObj,"oct");
                break;
            case 11:
                checkMonthKey("nov");
                months["nov"].Add(energyObj);
                updateEnergyMaxes(energyObj,"nov");
                break;
            case 12:
                checkMonthKey("dec");
                months["dec"].Add(energyObj);
                updateEnergyMaxes(energyObj,"dec");
                break;
            default:
                Debug.Log("Something is wrong with the dates!!!");
                break;
        }

        
    }
}   
