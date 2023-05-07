using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterStoreJson : MonoBehaviour
{
    //Used as an overall Skel if used
    public string SkelPath;
    public bool CantEquipSameMainCategory;
    public List<StoreItem> StoreItems = new List<StoreItem>();
    public List<EquipLimitation> EquipLimitations = new List<EquipLimitation>();

    public struct StoreItem
    {
        public int ID;
        public string Name;
        public bool Category;
        public string Description;
        public string IconPath;
        //Unknown if we will replicate full games in any way but something to add
        public int Cost;
        public int UnlockCondition;

        //Paths will look like "Models/Pants.OMF|0" or "Models/Pants.OMF|LOD750"
        public string ModelLODHigh;
        public string ModelLODMed;
        public string ModelLODLow;
        public string ModelLODShdw;
        public List<TextureData> TextureDataList;

        public List<StoreItem> ChildItems; //Only really used if its a category
    }

    public struct TextureData
    {
        public string TextureName;
        public string TexturePath;
    }

    //Used to tell the game what cant be equipted with what
    //Basically will work as a blacklist/whitelist system
    public struct EquipLimitation
    {
        public bool CantEquip;
        public int ID;
        public int ID2;
    }
}
