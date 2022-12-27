using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Boreal
{
    public class SOMenuInjector
    {
        [MenuItem("Assets/ULTRAKIT/SceneExporter")]
        public static void NewSceneBundleData()
        {
            CreateObject<SceneBundleData>("SceneExporter");
        }

        private static void CreateObject<T>(string name) where T : ScriptableObject
        {
            var obj = ScriptableObject.CreateInstance<T>();
            string windowPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string finalPath = AssetDatabase.GenerateUniqueAssetPath($@"{windowPath}\{name}.asset");

            AssetDatabase.CreateAsset(obj, finalPath);
            AssetDatabase.Refresh();
            Selection.activeObject = obj;
        }
    }
}
