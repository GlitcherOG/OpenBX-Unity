﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

namespace SSXMultiTool.JsonFiles.Tricky
{
    [Serializable]
    public class InstanceJsonHandler
    {
        public List<InstanceJson> instances = new List<InstanceJson>();

        public void CreateJson(string path)
        {
            var serializer = JsonUtility.ToJson(this);
            File.WriteAllText(path, serializer);
        }

        public static InstanceJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonUtility.FromJson<InstanceJsonHandler>(stream);
                return container;
            }
            else
            {
                return new InstanceJsonHandler();
            }
        }


        [Serializable]
        public struct InstanceJson
        {
            public string InstanceName;

            public float[] Location;
            public float[] Rotation;
            public float[] Scale;

            public float[] Unknown5;
            public float[] Unknown6;
            public float[] Unknown7;
            public float[] Unknown8;
            public float[] Unknown9;
            public float[] Unknown10;
            public float[] Unknown11;
            public float[] RGBA;
            public int ModelID;
            public int PrevInstance;
            public int NextInstance;

            public float[] LowestXYZ;
            public float[] HighestXYZ;

            public int UnknownInt26;
            public int UnknownInt27;
            public int UnknownInt28;
            public int ModelID2;
            public int UnknownInt30;
            public int UnknownInt31;
            public int UnknownInt32;

            public int LTGState;
        }
    }
}
