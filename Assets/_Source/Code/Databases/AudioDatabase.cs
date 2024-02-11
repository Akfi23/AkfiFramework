using _Client_.Scripts.Objects;
using _Source.Code._AKFramework.AKTags.Runtime;
using _Source.Code.Objects;
using UnityEngine;

namespace _Source.Code.Databases
{
    [CreateAssetMenu(fileName = "db_audio", menuName = "Game/Databases/Audio Database", order = 1)]
    public class AudioDatabase : AKDatabase
    {
        public override string Title => "Audio Database";

        [SerializeField]
        private AudioData[] audioData;

        public AudioData GetAudioData(AKTag tag)
        {
            foreach (var data in audioData)
            {
                if (data.Tag.HasTag(tag)) return data;
            }

            return new AudioData();
        }
    }
}