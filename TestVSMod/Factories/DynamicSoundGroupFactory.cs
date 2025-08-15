using Il2CppDarkTonic.MasterAudio;
using Il2CppZenject;
using static Il2CppDarkTonic.MasterAudio.MasterAudio;
using Il2Col = Il2CppSystem.Collections.Generic;

namespace vsML.Factories
{
    public class DynamicSoundGroupFactory
    {
        public static DynamicSoundGroupCreator DefaultModdedGroup()
        {
            if (ProjectContext._instance == null) return null;
            var sounds = ProjectContext._instance._container.InstantiateComponentOnNewGameObject<DynamicSoundGroupCreator>();
            sounds.musicPlaylists = new Il2Col.List<Playlist>();
            sounds.customEventsToCreate = new Il2Col.List<CustomEvent>();
            sounds.customEventCategories = new Il2Col.List<CustomEventCategory>();
            sounds.customEventCategories.Add(new CustomEventCategory() { CatName = "Loops" });
            foreach (var song in vsMLCore.Music)
            {
                Playlist playlist = new Playlist() { playlistName = song.Key.ToString() };
                foreach (var clip in song.Value)
                {
                    var setting = new MusicSetting() { clip = clip.Clip, songName = clip.Name, isLoop = clip.Loop, audLocation = AudioLocation.Clip };
                    playlist.MusicSettings.Add(setting);
                }

                sounds.musicPlaylists.Add(playlist);
            }

            return sounds;
        }
    }
}