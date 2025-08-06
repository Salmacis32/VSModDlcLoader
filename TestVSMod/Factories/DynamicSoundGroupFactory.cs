using Il2CppDarkTonic.MasterAudio;
using Il2CppZenject;
using static Il2CppDarkTonic.MasterAudio.MasterAudio;
using Il2Col = Il2CppSystem.Collections.Generic;

namespace TestVSMod.Factories
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
            foreach (var song in Core.Music)
            {
                Playlist playlist = new Playlist() { playlistName = song.Key.ToString() };
                var setting = new MusicSetting() { clip = song.Value.Clip, songName = song.Key.ToString(), alias = song.Value.Name, isLoop = true, audLocation = AudioLocation.Clip };
                if (song.Value.LoopStart > 0f)
                {
                    setting.songStartTimeMode = CustomSongStartTimeMode.SpecificTime;
                    setting.customStartTime = song.Value.LoopStart;
                }
                
                if (song.Key == 1410) playlist.MusicSettings.Add(new MusicSetting() { clip = Core.TestSong.Clip, songName = Core.TestSong.Name, isLoop = false, audLocation = AudioLocation.Clip });
                playlist.MusicSettings.Add(setting);
                sounds.musicPlaylists.Add(playlist);
            }

            return sounds;
        }
    }
}