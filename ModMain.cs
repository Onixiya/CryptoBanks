using MelonLoader;
using System.Collections.Generic;
using HarmonyLib;
using Assets.Scripts.Simulation.Towers.Behaviors;
[assembly:MelonInfo(typeof(CryptoBanks.ModMain),"CryptoBanks","1.1.1","Silentstorm")]
[assembly:MelonGame("Ninja Kiwi","BloonsTD6")]
namespace CryptoBanks{
    public class ModMain:MelonMod{
        private static MelonLogger.Instance mllog=new MelonLogger.Instance("CryptoBanks");
        public static void Log(object thingtolog,string type="msg"){
            switch(type){
                case"msg":
                    mllog.Msg(thingtolog);
                    break;
                case"warn":
                    mllog.Warning(thingtolog);
                    break;
                 case"error":
                    mllog.Error(thingtolog);
                    break;
            }
        }
        public static string[]CSV=Data.BTCValues.Split('\n');
        public static List<decimal>BTCValues=new List<decimal>();
        public override void OnInitializeMelon(){
            foreach(string str in CSV){
                string[]str1=str.Split(',');
                if(decimal.TryParse(str1[2],out decimal value)){
                    BTCValues.Add(value);
                }
            }
        }
        [HarmonyPatch(typeof(Bank),"PayInterest")]
        public class BankPayInterest_patch{
            [HarmonyPostfix]
            public static void Postfix(ref Bank __instance){
                float value=(float)BTCValues[new System.Random().Next(2,BTCValues.Count+1)];
                if(new System.Random().Next(0,2)==1){
                    __instance.cash*=value;
                    __instance.Sim.SetCash(__instance.Sim.GetCash(-1)*value,-1);
                }else{
                    __instance.cash/=value;
                    __instance.Sim.SetCash(__instance.Sim.GetCash(-1)/value,-1);
                }
            }
        }
        [HarmonyPatch(typeof(Bank),"Attatched")]
        public class BankAttached_patch{
            [HarmonyPostfix]
            public static void Postfix(ref Bank __instance){
                __instance.bankModel.capacity=float.MaxValue-1;
            }
        }
    }
}
