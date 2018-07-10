using System;
/*
 * Seems not working on android, I saw it too late
 * TODO:Make a excel importer multiplatform
using System.Data;
using System.Data.Odbc;//Can't find a way to import it for now
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Define all the localisation
public class Localize : MonoBehaviour {

    Dictionary<string, string> words;//Dictionary for the current language
    UnityEngine.UI.Text[] texts;//List of texts in the entire game

    //Quite dirty stocks of strings to replace the Excel importer
    //TODO: replace this with a proper Excel Importer multiplatform
    public string[] ids;
    public string[] english;
    public string[] french;
   

    static Localize Instance = null;
    // Use this for initialization
    void Awake()
    {
        //readXLS(Application.dataPath + "/Localize.xls");
        Instance = this;
        words = new Dictionary<string, string>();
        texts = gameObject.GetComponentsInChildren<UnityEngine.UI.Text>(true);
        Reload(PlayerPrefs.GetInt("language"));
    }

/*
    //No way to make it work on Android... let's keep it for later
    public static void ReloadExcel(int idLanguage)
    {
         // Must be saved as excel 2003 workbook, not 2007, mono issue really
        string con = "Driver={Microsoft Excel Driver (*.xls)}; DriverId=790; Dbq=" + Application.dataPath + "/Localize.xls" + ";";
        Debug.Log(con);
        string yourQuery = "SELECT * FROM [Sheet1$]";
        // our odbc connector 
        OdbcConnection oCon = new OdbcConnection(con);
        // our command object 
        OdbcCommand oCmd = new OdbcCommand(yourQuery, oCon);
        // table to hold the data 
        DataTable dtYourData = new DataTable("YourData");
        // open the connection 
        oCon.Open();
        // lets use a datareader to fill that table! 
        OdbcDataReader rData = oCmd.ExecuteReader();
        // now lets blast that into the table by sheer man power! 
        dtYourData.Load(rData);
        // close that reader! 
        rData.Close();
        // close your connection to the spreadsheet! 
        oCon.Close();
        // wow look at us go now! we are on a roll!!!!! 
        // lets now see if our table has the spreadsheet data in it, shall we? 

        if (dtYourData.Rows.Count > 0)
        {
            // do something with the data here 
            // but how do I do this you ask??? good question! 
            for (int i = 0; i < dtYourData.Rows.Count; i++)
            {
                Instance.words[dtYourData.Rows[i][dtYourData.Columns[0].ColumnName].ToString()] = dtYourData.Rows[i][dtYourData.Columns[idLanguage + 1].ColumnName].ToString();
                              
            }
        }
        for (int i = 0; i < Instance.texts.Length; ++i)
        {
            if (Instance.words.ContainsKey(Instance.texts[i].name))
                Instance.texts[i].text = Instance.words[Instance.texts[i].name];
            else
                Debug.LogError("Key : " + Instance.texts[i].name + "not found");
        }
    }
    */
     public static void Reload(int idLanguage)//Rebuild the dictionary with the specified language and update all the texts
    {
        string[] values = Instance.english;
        if (idLanguage == 1)
            values = Instance.french;
      

        for (int i = 0; i < Instance.ids.Length; ++i)
            Instance.words[Instance.ids[i]] = values[i];
        for (int i = 0; i < Instance.texts.Length; ++i)
        {
            if (Instance.words.ContainsKey(Instance.texts[i].name))
                Instance.texts[i].text = Instance.words[Instance.texts[i].name];
            else
                Debug.LogError("Localization missing Key : " + Instance.texts[i].name + "not found");
        }
    }

    public static string getString(string key)//Get the text matching the selected key for the current language
    {
       
        if (Instance.words.ContainsKey(key))
            return Instance.words[key];
        else return ("Text missing");

    }
}
