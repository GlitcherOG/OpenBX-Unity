using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AddonInfoJson
{
    public int Version = 1;
    public string AddonName = "";
    public string Author = "";
    public string PackVersion = "";
    public string Description = "";


    public void CreateJson(string path)
    {
        var serializer = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(path, serializer);
    }

    public static AddonInfoJson Load(string path)
    {
        string paths = path;
        if (File.Exists(paths))
        {
            var stream = File.ReadAllText(paths);
            var container = JsonConvert.DeserializeObject<AddonInfoJson>(stream);
            return container;
        }
        else
        {
            return new AddonInfoJson();
        }
    }
}
