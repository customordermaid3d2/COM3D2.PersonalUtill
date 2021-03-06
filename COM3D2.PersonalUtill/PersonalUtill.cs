using BepInEx;
using BepInEx.Logging;
using MaidStatus;
using scoutmode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LillyUtill.MyPersonal
{
    class MyAttribute
    {
        public const string PLAGIN_NAME = "PersonalUtill";
        public const string PLAGIN_VERSION = "22.2.26";
        public const string PLAGIN_FULL_NAME = "COM3D25.PersonalUtill.Plugin";
    }

    [BepInPlugin(MyAttribute.PLAGIN_FULL_NAME, MyAttribute.PLAGIN_NAME, MyAttribute.PLAGIN_VERSION)]
    public class PersonalUtill : BaseUnityPlugin
    {
        public static List<Personal.Data> personalDataAll;
        public static List<Personal.Data> personalDataEnable;
        public static bool isLoad=false;
       private static ManualLogSource Log;

        internal void Start()
        {
            Log = Logger;
            CreateData();
        }

        public static List<Personal.Data> GetPersonalData(bool onlyEnabled = true)
        {          
            if (onlyEnabled)
            {
                return personalDataEnable;
            }
            return personalDataAll;
        }

        public static Personal.Data GetPersonalData(int index, bool onlyEnabled = true)
        {            
            if (onlyEnabled)
            {
                return personalDataEnable[index];
            }
            return personalDataAll[index];
        }

        private static void CreateData()
        {
            try
            {
                if (personalDataAll == null)
                {
                    //LillyUtill.Log.LogInfo("CreateData.personalDataAll");
                    personalDataAll = Personal.GetAllDatas(false);
                }
            }
            catch (Exception e)
            {
                Log.LogError($"CreateData.personalDataAll {e.ToString()}");
            }
            try
            {
                if (personalDataEnable == null)
                {
                    Log.LogInfo("CreateData.personalDataEnable");
                    personalDataEnable = Personal.GetAllDatas(true);

                    //bool flag = true;
                    //flag = (GameMain.Instance.CharacterMgr.status.GetFlag("オープニング終了") == 1);
                    List<Personal.Data> list = new List<Personal.Data>();
                    foreach (var data in personalDataEnable)
                    {
                        Log.LogInfo(data.uniqueName);
                        if (data.oldPersonal)
                        {
                            string a = data.uniqueName.ToLower();
                            if (a == "pure" || a == "cool" || a == "pride")
                            {
                                if (!string.IsNullOrEmpty(GameMain.Instance.CMSystem.CM3D2Path) && PluginData.IsEnabled("Legacy"))
                                //    if (GameMain.Instance.CharacterMgr.status.isAvailableTransfer)
                                {
                                    //list.Add(data);
                                }
                                else
                                {
                                    list.Add(data);
                                }
                            }
                            //else if (flag)
                            else
                            {
                                if (data.single)
                                {
                                    //list.Add(data);
                                }
                                else if (!string.IsNullOrEmpty(GameMain.Instance.CMSystem.CM3D2Path) && data.compatible)
                                {
                                    //list.Add(data);
                                }
                                else
                                {
                                    list.Add(data);
                                }
                            }
                        }
                        else
                        {
                            //list.Add(data);
                        }
                    }
                    personalDataEnable = personalDataEnable.Except(list).ToList();
                    isLoad = true;
                }
            }
            catch (Exception e)
            {
                Log.LogError($"CreateData.personalDataEnable {e.ToString()}");
            }

        }

        public static int SetPersonalRandom(Maid maid)
        {
            if (maid == null)
            {
                Log.LogFatal("maid null");
            }
            int a = UnityEngine.Random.Range(0, GetPersonalData().Count);
            Personal.Data data = GetPersonalData(a);
            maid.status.SetPersonal(data);
            maid.status.firstName = data.uniqueName;

            return a;
        }

        public static void SetPersonalCare(Maid maid)
        {

            maid.status.SetPersonal(maid.status.personal);

        }

        public static Personal.Data SetPersonal(Maid maid, int index)
        {
            Personal.Data data = GetPersonalData(index);
            maid.status.SetPersonal(data);
            maid.status.firstName = data.uniqueName;

            return data;
        }

    }
}

