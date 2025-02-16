﻿using BepInEx;
#if !BEPIN_5
using BepInEx.Unity.IL2CPP;
#endif
using BepInEx.Configuration;
using HarmonyLib;

namespace TaikoModStuff
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
#if !BEPIN_5
    public class Plugin : BasePlugin
#else
    public class Plugin : BaseUnityPlugin
#endif
    {
        public static ConfigEntry<bool> configForceFontChange;

        public static ConfigEntry<int> configCustomWindowedWidth;
        public static ConfigEntry<int> configCustomWindowedHeight;

        public static ConfigEntry<int> configCustomFramerate;
        public static ConfigEntry<bool> configToggleVSync;

        public static ConfigEntry<bool> configEnableRecording;

        public static ConfigEntry<bool> configOfflineSaveLoad;
        public static ConfigEntry<bool> configQuickRestart;
        public static ConfigEntry<bool> configQuickQuitSong;

#if !BEPIN_5
        public override void Load()
#else
        private void Awake()
#endif
        {
            // Add configurations
            configForceFontChange = Config.Bind("General.Toggles",
                                                "ForceFontChange",
                                                false,
                                                "Force the game font to the JP font");

            configCustomWindowedWidth = Config.Bind("General.Resolution",
                                                     "CustomWidth",
                                                     1920,
                                                     "Custom width to use. Set to 0 to disable setting the custom resolution");

            configCustomWindowedHeight = Config.Bind("General.Resolution",
                                                     "CustomHeight",
                                                     1080,
                                                     "Custom height to use");

            configCustomFramerate = Config.Bind("General.Framerate",
                                                     "CustomFramerate",
                                                     60,
                                                     "Custom framerate.");

            configToggleVSync = Config.Bind("General.Graphics",
                                             "EnableVSync",
                                             true,
                                             "Enable VSync.");

            configEnableRecording = Config.Bind("General.Toggles",
                                             "EnableGameBarRecording",
                                             true,
                                             "Enables Game Recording from the Xbox Game Bar where it was previously disabled.");

            configOfflineSaveLoad = Config.Bind("General.Toggles",
                        "OfflineSaveLoad",
                        false,
                        "Loads local save files even when offline. Bypasses the log-in checks. Requires the FreeLocalSaves Plugin.\nEnabling this without FreeLocalSaves can and will wipe your save file.");

            configQuickRestart = Config.Bind("General.Toggles",
                        "QuickRestart",
                        false,
                        "Hit \"Backspace\" on your keyboard to quickly restart a song.");

            configQuickQuitSong = Config.Bind("General.Toggles",
                        "QuickQuitSong",
                        false,
                        "Hit \"Escape\" on your keyboard to quickly quit a song and return to Song Select.");


            var instance = new Harmony(PluginInfo.PLUGIN_NAME);
            instance.PatchAll(typeof(FontChanger));
            instance.PatchAll(typeof(CustomResolution));
            instance.PatchAll(typeof(ForceFramerate));
            instance.PatchAll(typeof(RecordingEnable));

            if (Plugin.configOfflineSaveLoad.Value)
                instance.PatchAll(typeof(OfflineSaveLoad));

            if (Plugin.configQuickRestart.Value)
                instance.PatchAll(typeof(QuickRestart));

            if (Plugin.configQuickQuitSong.Value)
                instance.PatchAll(typeof(QuickQuitSong));

            // Plugin startup logic
#if !BEPIN_5
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
#else
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
#endif
        }
    }
}
