using AlexejheroYTB.Common;
using Harmony;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using Story;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MAC.PocketRadio
{
    public static class Mod
    {
        public static void Patch()
        {
            HarmonyInstance.Create("MAC.PocketRadio").PatchAll();
            new PocketRadioItem().Patch();
        }
    }

    public class PocketRadioItem : Craftable
    {
        public new void Patch()
        {
            base.Patch();

            KnownTechHandler.SetAnalysisTechEntry(TechType.Exosuit, new TechType[] { this.TechType });
            SpriteHandler.RegisterSprite(this.TechType, SpriteManager.Get(TechType.Radio));

            ItemActionHelper.RegisterAction(MouseButton.Left, this.TechType, PocketRadio.OnClick, "play message", PocketRadio.Condition);
        }

        public PocketRadioItem() : base("PocketRadio", "Pocket Radio", "Receive short-range communications and play them wirelessly.") { }

        protected override TechData GetBlueprintRecipe() => new TechData() { Ingredients = new List<Ingredient>() { new Ingredient(TechType.Titanium, 2), new Ingredient(TechType.Copper, 1), new Ingredient(TechType.CopperWire, 2), new Ingredient(TechType.Battery, 1) } };
        public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
        public override string[] StepsToFabricatorTab => "Personal/Tools".Split('/');

        public override TechGroup GroupForPDA => TechGroup.Personal;
        public override TechCategory CategoryForPDA => TechCategory.Tools;

        public override string AssetsFolder => Path.Combine(new DirectoryInfo(Path.Combine(Assembly.GetExecutingAssembly().Location, "..")).Name, "Assets");

        public override GameObject GetGameObject()
        {
            GameObject prefab = Resources.Load<GameObject>("worldentities/tools/battery");
            GameObject obj = GameObject.Instantiate(prefab);

            Pickupable pickupable = obj.GetComponent<Pickupable>();
            pickupable.destroyOnDeath = false;

            obj.AddComponent<PocketRadio>();

            GameObject.DestroyImmediate(obj.GetComponent<Battery>());

            return obj;
        }
    }

    public class PocketRadio : MonoBehaviour
    {
        public bool hasMessage;
        public FMOD_CustomEmitter playSound;
        public FMODASRPlayer radioSound;

        public static Radio.CancelIcon CancelIconEvent;

        public void Awake()
        {
            try
            {
                var sound = GameObject.FindObjectsOfType<Radio>().Where(r => r.playSound != null).ToArray()[0].playSound;
                playSound = gameObject.AddComponent<FMOD_CustomEmitter>();
                playSound.asset = sound.asset;
                playSound.debug = sound.debug;
                playSound.followParent = sound.followParent;
                playSound.playOnAwake = sound.playOnAwake;
                playSound.restartOnPlay = sound.restartOnPlay;
            }
            catch (Exception)
            {
                Console.WriteLine("[Pocket Radio] [ERROR] Could not get playSound!");
            }
            try
            {
                var sound = GameObject.FindObjectsOfType<Radio>().Where(r => r.radioSound != null).ToArray()[0].radioSound;
                radioSound = gameObject.AddComponent<FMODASRPlayer>();
                radioSound.debug = sound.debug;
                radioSound.startLoopSound = sound.startLoopSound;
                radioSound.stopSound = sound.stopSound;
            }
            catch (Exception)
            {
                Console.WriteLine("[Pocket Radio] [ERROR] Could not get radioSound!");
            }
        }

        public void Update()
        {
            ErrorMessage.AddDebug(hasMessage.ToString());
        }

        public void OnClick()
        {
            if (!hasMessage || IsInvoking("PlayRadioMessage")) return;
            playSound.Play();
            Invoke("PlayRadioMessage", 1.25f);

            CancelIconEvent?.Invoke();
        }

        public void OnEnable()
        {
            StoryGoalManager.PendingMessageEvent -= NewRadioMessage;
            StoryGoalManager.PendingMessageEvent += NewRadioMessage;
            StoryGoalManager.main.PulsePendingMessages();
        }

        public void OnDisable()
        {
            StoryGoalManager.PendingMessageEvent -= NewRadioMessage;
        }

        public void NewRadioMessage(bool newMessages)
        {
            ToggleBlink(newMessages);
            hasMessage = newMessages;
        }

        public void PlayRadioMessage()
        {
            StoryGoalManager.main.ExecutePendingRadioMessage();
        }

        public void ToggleBlink(bool on)
        {
            if (on)
            {
                radioSound.Play();
            }
            else
            {
                radioSound.Stop();
            }
        }

        public static bool Condition(InventoryItem item)
        {
            return item.item.GetComponent<PocketRadio>().hasMessage;
        }

        public static void OnClick(InventoryItem item)
        {
            item.item.GetComponent<PocketRadio>().OnClick();
        }
    }

    public static class Patches
    {
        [HarmonyPatch(typeof(uGUI_RadioMessageIndicator))]
        [HarmonyPatch("OnEnable")]
        public static class uGUI_RadioMessageIndicator_OnEnable
        {
            public static void Postfix(uGUI_RadioMessageIndicator __instance)
            {
                PocketRadio.CancelIconEvent += __instance.DisableSprite;
            }
        }

        [HarmonyPatch(typeof(uGUI_RadioMessageIndicator))]
        [HarmonyPatch("OnDisable")]
        public static class uGUI_RadioMessageIndicator_OnDisable
        {
            public static void Postfix(uGUI_RadioMessageIndicator __instance)
            {
                PocketRadio.CancelIconEvent -= __instance.DisableSprite;
            }
        }
    }
}
