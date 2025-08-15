using UnityEngine;

namespace vsML.Models
{
    public class SongData
    {
        public SongData(AudioClip clip, string name, bool loop = true)
        {
            Clip = clip;
            Name = name;
            Loop = loop;
        }

        public AudioClip Clip { get; set; }
        public string Name { get; set; }

        public bool Loop { get; set; }
    }
}
