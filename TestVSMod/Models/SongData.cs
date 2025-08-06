using UnityEngine;

namespace TestVSMod.Models
{
    public class SongData
    {
        public SongData(AudioClip clip, string name)
        {
            Clip = clip;
            Name = name;
        }

        public AudioClip Clip { get; set; }
        public string Name { get; set; }
        public float LoopStart { get; set; }
        public float LoopEnd { get; set; }
    }
}
