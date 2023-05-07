using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class  CharacterInfoJson : MonoBehaviour
{
    //Names
    public string FullName;
    public string ShortName;
    public string NickName;

    //Bio Data
    public string Bio; //Bio in tricky, DNA/Mixed in with fav in SSX3
    public string Backstory;
    public string Interview; //Also Q&A In 3
    public int Stance; //0 is regular, 1 is goofy
    public int Age;
    public int Gender; //0 is male, 1 is female
    public string Height;
    public string Nationality;

    //Important Overall Details
    public int Scale; //Based in Percentage
    public List<int> DefaultOutfitIDS = new List<int>();

    public void CreateJson(string path)
    {
        var serializer = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(path, serializer);
    }

    public static CharacterInfoJson Load(string path)
    {
        if (File.Exists(path))
        {
            var stream = File.ReadAllText(path);
            var container = JsonConvert.DeserializeObject<CharacterInfoJson>(stream);
            return container;
        }
        else
        {
            return new CharacterInfoJson();
        }
    }
}
