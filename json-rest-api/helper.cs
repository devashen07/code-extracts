using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaxResults {

    public Result result;

    [System.Serializable]
    public class Result
    {
        public string src_lang;
        public List<string> translations;
        public string tgt_lang;
        public string utt;
    }

    public static MaxResults CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<MaxResults>(jsonString);
    }

}
