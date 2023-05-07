using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class AddonLoading : MonoBehaviour
{
    public static AddonLoading instance;
    public bool loaded;
    public List<AddonData> addonDatas = new List<AddonData>();
    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        string[] PossibleAddons = Directory.GetDirectories(Application.streamingAssetsPath + "/Addons");

        for (int i = 0; i < PossibleAddons.Length; i++)
        {
            if (File.Exists(PossibleAddons[i] + "/AddonInfo.json"))
            {
                try
                {
                    AddonInfoJson addonInfoJson = AddonInfoJson.Load(PossibleAddons[i] + "/AddonInfo.json");
                    AddonData addonData = new AddonData();
                    addonData.Name = addonInfoJson.AddonName;
                    addonData.Description = addonInfoJson.Description;
                    addonData.Author = addonInfoJson.Author;
                    addonData.PackVersion = addonInfoJson.PackVersion;
                    addonData.Path = PossibleAddons[i];
                    addonData.Load = true;


                    try
                    {
                        string path = PossibleAddons[i] + "/Icon.png";
                        byte[] bytes = File.ReadAllBytes(path);
                        addonData.icon = new Texture2D(1, 1);
                        addonData.icon.LoadImage(bytes);
                        addonData.icon.alphaIsTransparency = true;
                    }
                    catch
                    {
                        Debug.Log("Failed To Load Icon " + PossibleAddons[i]);
                    }

                    addonDatas.Add(addonData);
                }
                catch
                {
                    Debug.LogWarning("Error Loading Addon " + PossibleAddons[i]);
                }
            }
        }
        loaded = true;
    }

    [Serializable]
    public struct AddonData
    {
        public string Name;
        public string Description;
        public string Author;
        public string PackVersion;
        public string Path;

        public Texture2D icon;
        public bool Load;
    }
}
