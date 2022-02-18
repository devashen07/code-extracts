using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Vuforia;


public class json_rest_api : MonoBehaviour {

    List<string> languages = new List<string>() { "Afrikaans", "Tswana", "isiZulu", "Sepedi", "siSwati" };

    public Dropdown dropdown;

    public WordBehaviour phrase;

    bool Check;

    string TgtPhrase;

    string tgtLang = "Afr";

    public void GetTrgLang(int index)
    {
        if (languages[index] == "Afrikaans") { tgtLang = "Afr"; }
        else if (languages[index] == "Tswana") { tgtLang = "Tsn"; }
        else if (languages[index] == "isiZulu") { tgtLang = "Zul"; }
        else if (languages[index] == "Sepedi") { tgtLang = "Nso"; }
        else if (languages[index] == "siSwati") { tgtLang = "Ssw"; }

        Debug.Log(tgtLang);

    }



    // Use this for initialization
    private void Start()
    {
        PopulateList();
    }

    void PopulateList()
    {
        dropdown.AddOptions(languages);
        
    }


    
    void Update () {

        TgtPhrase = GetPhrase(phrase, Check);
        if(TgtPhrase == "nothing")
        {
            
        }
        else { StartCoroutine(GetResults()); }
        
	}
    

    IEnumerator GetResults()
    {

                string url = "https://language-translation-endpoint.com/" + tgtLang + "&utt=" + TgtPhrase;

                //Debug.Log(url);

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {

            yield return www.Send();

            if (www.isError)
            {

                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    Debug.Log(jsonResult);

                    var MaxResponseObj = MaxResults.CreateFromJSON(jsonResult).result.translations;
                    string trans = string.Join(string.Empty, MaxResponseObj.ToArray());

                    gameObject.GetComponent<TranslationObject>().translationTextval = trans;

                }

            }
        }
        
      
    }

    public string GetPhrase(WordBehaviour phrase, bool check)
    {
        check = DefaultTrackableEventHandler.Detection();
        if (check == true)
        {
            try
            {
                return phrase.Word.Name;
            }
            catch (IOException e)
            {
                Debug.Log("error");
                return "nothing";
            }
            
        }
        else { return "nothing"; }
    }







}
