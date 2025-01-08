using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using TMPro;
using Configgy;

namespace UK_TextShopped
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    [BepInProcess("ULTRAKILL.exe")]
    public class TextShoppedMain : BaseUnityPlugin
    {
        public const string pluginGuid = "jcg.ultrakill.textshopped";
        public const string pluginName = "Text ShoppEd";
        public const string pluginVersion = "0.1";
        [Configgable]
        private static ConfigToggle patchConfirmPrint = new ConfigToggle(true);
        private readonly Harmony harmony = new Harmony(pluginGuid);
        private ConfigBuilder config;

        private void Awake()
        {
            harmony.PatchAll();
            config = new ConfigBuilder(pluginGuid, pluginName);
            config.BuildAll();
            Debug.Log("Plugin loaded: Text ShoppEd");
        }

        [HarmonyPatch(typeof(ShopZone))]
        public static class ChangeTipMessage
        {
            [Configgable]
            private static ConfigInputField<String> normalTerminalText = new ConfigInputField<string>("Normal Terminal Text");
            [Configgable]
            private static ConfigInputField<String> testamentTerminalText = new ConfigInputField<string>("Testament Terminal Text");
            [Configgable]
            private static ConfigInputField<String> allTerminalTitle = new ConfigInputField<string>("Terminal Title");
            [HarmonyPostfix]
            [HarmonyPatch(typeof(ShopZone), "Start")]
            public static void ChangeTip(ShopZone __instance, ref Canvas ___shopCanvas)
            {
                Transform tipTextTransform = ___shopCanvas.transform.FindDeep("TipText");
                Transform testamentTextTransform = ___shopCanvas.transform.FindDeep("Text");
                if (tipTextTransform != null) 
                {
                    TMP_Text tipText = tipTextTransform.GetComponent<TMP_Text>();
                    if (tipText != null) 
                    {
                        tipText.text = normalTerminalText.Value;
                    }
                }
                if (testamentTextTransform != null && __instance.name == "Testament Shop")
                {
                    TMP_Text testamentText = testamentTextTransform.GetComponent<TMP_Text>();
                    if (testamentText != null)
                    {
                        testamentText.text = testamentTerminalText.Value;
                    }
                }
                ___shopCanvas.GetComponentInChildren<TMPro.TMP_Text>(true).text = allTerminalTitle.Value;
                if (patchConfirmPrint.Value == true)
                    Debug.Log("TextShoppEd: Canvas Text Patched");
                MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("TIPS PATCHED.", "", "", 1);
            }
        }
    }
}
