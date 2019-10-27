using Harmony;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Options;
using UnityEngine;

namespace MAC.ConfigurableOutcropCount
{
    public static class Mod
    {
        public static void Patch()
        {
            HarmonyInstance.Create("ConfigurableOutcropCount").PatchAll();
            OptionsPanelHandler.RegisterModOptions(new Options());
        }
    }

    public class Options : ModOptions
    {
        public static int Minimum
        {
            get => PlayerPrefs.GetInt("COCMIN", 1);
            set => PlayerPrefs.SetInt("COCMIN", value);
        }

        public static int Maximum
        {
            get => PlayerPrefs.GetInt("COCMAX", 3);
            set => PlayerPrefs.SetInt("COCMAX", value);
        }

        public Options() : base("ConfigurableOutcropCount")
        {
            SliderChanged += (_, e) =>
            {
                if (e.Id == "COC.Min") Minimum = e.IntegerValue;
                if (e.Id == "COC.Max") Maximum = e.IntegerValue;

                if (Minimum > Maximum)
                {
                    var temp = Minimum;
                    Minimum = Maximum;
                    Maximum = temp;
                }
            };
        }

        public override void BuildModOptions()
        {
            AddSliderOption("COC.Min", "Minimum", 0, 10, Minimum);
            AddSliderOption("COC.Max", "Maximum", 0, 10, Maximum);
        }
    }

    [HarmonyPatch(typeof(BreakableResource))]
    [HarmonyPatch("SpawnResourceFromPrefab")]
    public static class BreakableResource_SpawnResourceFromPrefab
    {
        [HarmonyPrefix]
        public static bool Prefix(BreakableResource __instance, GameObject breakPrefab)
        {
            var num = Random.Range(Options.Minimum, Options.Maximum);

            for (var i = 0; i < num; i++)
            {
                GameObject gameObject = GameObject.Instantiate<GameObject>(breakPrefab, __instance.transform.position + __instance.transform.up * __instance.verticalSpawnOffset + new Vector3(Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f)), Quaternion.identity);
                Debug.Log("broke, spawned " + breakPrefab.name);
                if (!gameObject.GetComponent<Rigidbody>())
                {
                    gameObject.AddComponent<Rigidbody>();
                }
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.GetComponent<Rigidbody>().AddTorque(Vector3.right * Random.Range(3, 6));
                gameObject.GetComponent<Rigidbody>().AddForce(__instance.transform.up * 0.1f);
            }

            return false;
        }
    }
}
