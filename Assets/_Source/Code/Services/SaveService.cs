using _Client_.Scripts.Interfaces;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Interfaces;
using CodeStage.AntiCheat.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.UnityConverters.Math;
using UnityEngine;

namespace _Source.Code.Services
{
    public class SaveService : ISaveService
    {
        private JsonSerializerSettings _settings;
        
        [AKInject]
        private void Init()
        {
            
#if UNITY_EDITOR

            if (AKDebug.IsDebug)
            {
                if (!PlayerPrefs.HasKey("HasSaves"))
                {
                    PlayerPrefs.SetInt("HasSaves",1);
                }
            }
#endif
            
            _settings = new JsonSerializerSettings {
                Converters = new JsonConverter[] 
                {
                    new Vector3Converter(),
                    new StringEnumConverter(),
                    new AKTagConverter()
                },
                ContractResolver = new DefaultContractResolver()
            };
        }
    
        public bool Has(string key)
        {
            return ObscuredPrefs.HasKey(key);
        }

        public T Load<T>(string key, T value)
        {
            return typeof(T).IsClass && typeof(T) != typeof(string)
                ? JsonConvert.DeserializeObject<T>(ObscuredPrefs.Get(key, JsonConvert.SerializeObject(value, _settings)))
                : ObscuredPrefs.Get(key, value);
        }

        public void Save<T>(string key, T value)
        {
            if (typeof(T).IsClass && typeof(T) != typeof(string))
            {
                ObscuredPrefs.Set(key, JsonConvert.SerializeObject(value, _settings));
                ObscuredPrefs.Save();
                return;
            }

            ObscuredPrefs.Set(key, value);
            ObscuredPrefs.Save();
        }

        public void Remove(string key)
        {
            ObscuredPrefs.DeleteKey(key);
        }
    }
}