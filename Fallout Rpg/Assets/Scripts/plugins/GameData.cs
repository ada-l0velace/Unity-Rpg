using UnityEngine;
using System.IO;
using System.Collections;

// Summary:
//      Serves has the lowest end for loading/saving files
//      Holds useful data and allows access to it

public class GameData : MonoBehaviour {

    

    public static GameData instance;

    public string jsonPathToLoad;
    public object data;

    void Awake() {
        if (instance != null)
            throw new System.Exception("Only one instance of GameData is allowed.");
        instance = this;
    }



    internal static T LoadData<T>(string path) {
        string s = "";
        s = Resources.Load(path).ToString();
        //Debug.Log("Received: " + s);
        return JSONLoader.Convert<T>(s);
    }


    internal static void SaveString(string jsonstring, string path, bool overwrite) {
        
#if UNITY_EDITOR
        path = "Assets\\Resources\\" + path;
        try {
            if (File.Exists(path)) {
                if (!overwrite) {
                    Debug.Log(path + " already exists.");
                    return;
                } else {
                    File.Delete(path);
                }
            }

            StreamWriter sr = File.CreateText(path);
            sr.WriteLine(jsonstring);
            sr.Close();
        } catch (System.Exception ex) {
            Debug.LogError(ex);
        }
        Debug.Log("File Saved. " + path);
#endif
        
    }

} //class
