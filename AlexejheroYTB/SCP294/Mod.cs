using Harmony;
using SMLHelper.V2.Assets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SCP294
{
    public static class Mod
    {
        public static void Patch()
        {
            HarmonyInstance.Create("SCP294").PatchAll();
        }
    }

    public static class Data
    {
        public static List<Liquid> DefaultLiquids = new List<Liquid>()
        {
            new Liquid(new string[] { "air", "nothing", "cup", "emptiness", "vacuum", "hl3", "half life 3", "no" }, new Color(0, 0, 0, 0), "There is nothing to drink in the cup."),
            new Liquid(new string[] { "alcohol", "ethanol", "ethanol liquid", "spirit", "vodka" }, new Color(1, 1, 1, .5f), "Damn, that's strong.", new LiquidAction(LiquidAction.Type.Blur, 5)),
            new Liquid(new string[] { "aloe vera drink", "cactus drink", "aloe vera", "cactus" }, new Color(.96f, .96f, .86f, .86f), null, new LiquidAction(LiquidAction.Type.Heal, 10)),
            new Liquid(new string[] { "amnesia" }, new Color(0, 0, 0, .5f), "Daniel, is that you? What are you doing?", new LiquidAction(LiquidAction.Type.Blur, 10)),
            new Liquid(new string[] { "anti-energy drink", "anti energy drink", "anti-energy", "anti energy" }, new Color(1, .5f, 0, .5f), "The drink tastes terrible. You feel tired and drained.", new LiquidAction(LiquidAction.Type.StaminaModifier, 2, 300)),
            new Liquid(new string[] { "antimatter", "anti-matter", "void" }, new Color(0, 0, 0), null, new LiquidAction(LiquidAction.Type.Explode)),
            new Liquid(new string[] { "aqua regia", "aqua" }, new Color(.71f, .40f, .11f), "Hmm... There should be more cuprite.", new LiquidAction(LiquidAction.Type.Refuse)),
            new Liquid(new string[] { "atomic", "nuclear", "nuclear fusion", "nuclear fission", "nuclear reaction" }, )
        };
    }

    public class Cup : Spawnable
    {
        public Cup() : base("scpcup", "Empty Cup", "An empty standard 12-ounce paper cup")
        {
        }

        public override string AssetsFolder => Path.Combine(new DirectoryInfo(Path.Combine(Assembly.GetExecutingAssembly().Location, "..")).Name, "Assets");

        public override GameObject GetGameObject()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Liquid
    {
        public string[] Names;
        public Color Color;
        public string Message;
        public LiquidAction[] Actions;

        public Liquid(IEnumerable<string> names, Color color, string message = null, params LiquidAction[] actions)
        {
            Names = names.ToArray();
            Color = color;
            Message = message;
            Actions = actions;
        }
    }

    public class LiquidAction
    {
        public Type Effect;
        public float[] Modifiers;

        public LiquidAction(Type effect, params float[] modifiers)
        {
            Effect = effect;
            Modifiers = modifiers;
        }

        public enum Type
        {
            Blur,
            Heal,
            StaminaModifier,
            Explode,
            Refuse
        }
    }
}
