﻿using Common.Abstract_Classes;
using Common.Enums;
using Common.Utilities;
using MAC.OxStation.Buildables;
using MAC.OxStation.Display;
using MAC.OxStation.Mono;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace MAC.OxStation.Managers
{
    internal class DisplayManager : Display_AB
    {
        private Color colorEmpty = new Color(1f, 0f, 0f, 1f);

        private Color colorHalf = new Color(1f, 1f, 0f, 1f);

        private Color colorFull = new Color(0f, 1f, 0f, 1f);

        private Color dark_bg = new Color(0.27734375f, 0.27734375f, 0.27734375f, 1f);

        private OxStationController _mono;
        private int _isScreenOn;
        private Text _oxPreloaderLBL;
        private Image _oxPreloaderBar;
        private Text _healthPreloaderlbl;
        private Image _healthPreloaderBar;
        private GameObject _giveO2BTN;
        private Text _powerUsage;
        private Text _buttonLbl;
        private InterfaceButton _giveOIntBtn;

        internal void Setup(OxStationController mono)
        {
            _mono = mono;

            _isScreenOn = Animator.StringToHash("IsScreenOn");

            if (FindAllComponents())
            {
                StartCoroutine(CompleteSetup());
            }
            else
            {
                QuickLogger.Error("// ============== Error getting all Components ============== //");
                return;
            }

            StartCoroutine(CompleteSetup());

            InvokeRepeating("UpdateScreen", 0, 0.5f);
            InvokeRepeating("CheckDamaged", 0, 0.5f);
        }

        private void UpdateScreen()
        {
            _oxPreloaderBar.fillAmount = _mono.OxygenManager.GetO2LevelPercentage();
            _oxPreloaderLBL.text = $"{Mathf.RoundToInt(_mono.OxygenManager.GetO2LevelPercentageFull())}%";
            _healthPreloaderBar.fillAmount = _mono.HealthManager.GetHealthPercentage();
            _healthPreloaderlbl.text = $"{Mathf.RoundToInt(_mono.HealthManager.GetHealthPercentageFull())}%";
            _powerUsage.text = $"{OxStationBuildable.PowerUsage()}: <color=#ff0000ff>{_mono.PowerManager.GetPowerUsage()}</color> {OxStationBuildable.PerMinute()}.";

        }

        private void CheckDamaged()
        {
            if (_mono.HealthManager.IsDamageApplied())
            {
                _buttonLbl.text = OxStationBuildable.Damaged();
                _giveOIntBtn.OnDisable();
            }
            else
            {
                _buttonLbl.text = OxStationBuildable.TakeOxygen();
                _giveOIntBtn.OnEnable();
            }
        }

        public override void OnButtonClick(string btnName, object tag)
        {
            if (btnName == string.Empty) return;

            switch (btnName)
            {
                case "giveO2":
                    _mono.OxygenManager.GivePlayerO2();
                    break;
            }
        }

        public override bool FindAllComponents()
        {

            #region Canvas  
            var canvasGameObject = gameObject.GetComponentInChildren<Canvas>()?.gameObject;

            if (canvasGameObject == null)
            {
                QuickLogger.Error("Canvas cannot be found");
                return false;
            }
            #endregion

            #region Home
            var home = canvasGameObject.FindChild("Home")?.gameObject;

            if (home == null)
            {
                QuickLogger.Error("Home cannot be found");
                return false;
            }
            #endregion

            #region Oxygen Preloader
            var oxPreloaderlbl = home.FindChild("Ox_Preloader")?.FindChild("percentage")?.GetComponent<Text>();

            if (oxPreloaderlbl == null)
            {
                QuickLogger.Error("Ox_Preloader cannot be found");
                return false;
            }

            _oxPreloaderLBL = oxPreloaderlbl;
            #endregion

            #region Oxygen Preloader Bar
            var oxPreloaderBar = home.FindChild("Ox_Preloader")?.FindChild("PreLoader_Bar_Front")?.GetComponent<Image>();

            if (oxPreloaderBar == null)
            {
                QuickLogger.Error("Ox_Preloader Bar cannot be found");
                return false;
            }

            _oxPreloaderBar = oxPreloaderBar;
            #endregion

            #region Health Preloader
            var healthPreloaderlbl = home.FindChild("Health_Preloader")?.FindChild("percentage")?.GetComponent<Text>();

            if (healthPreloaderlbl == null)
            {
                QuickLogger.Error("Health_Preloader cannot be found");
                return false;
            }

            _healthPreloaderlbl = healthPreloaderlbl;
            #endregion

            #region Health Preloader Bar
            var healthPreloaderBar = home.FindChild("Health_Preloader")?.FindChild("PreLoader_Bar_Front")?.GetComponent<Image>();

            if (healthPreloaderBar == null)
            {
                QuickLogger.Error("Health_Preloader Bar cannot be found");
                return false;
            }

            _healthPreloaderBar = healthPreloaderBar;
            #endregion

            #region Take O2 BTN
            _giveO2BTN = home.transform.Find("Button")?.gameObject;
            if (_giveO2BTN == null)
            {
                QuickLogger.Error("Button not found.");
                return false;
            }

            _giveOIntBtn = _giveO2BTN.AddComponent<InterfaceButton>();
            _giveOIntBtn.ButtonMode = InterfaceButtonMode.Background;
            _giveOIntBtn.STARTING_COLOR = dark_bg;
            _giveOIntBtn.OnButtonClick = OnButtonClick;
            _giveOIntBtn.BtnName = "giveO2";
            #endregion

            #region Take O2 BTN LBL
            _buttonLbl = home.transform.Find("Button").Find("Text")?.gameObject.GetComponent<Text>();
            if (_buttonLbl == null)
            {
                QuickLogger.Error("Button Text not found.");
                return false;
            }

            _buttonLbl.text = OxStationBuildable.TakeOxygen();

            #endregion

            #region Power Usage
            var powerUsage = home.transform.Find("Power_Usage")?.GetComponent<Text>();
            if (powerUsage == null)
            {
                QuickLogger.Error("Power_Usage not found.");
                return false;
            }

            _powerUsage = powerUsage;
            #endregion

            return true;
        }

        public override IEnumerator PowerOn()
        {
            _mono.AnimationManager.SetBoolHash(_isScreenOn, true);
            yield return null;
        }

        public override IEnumerator ShutDown()
        {
            _mono.AnimationManager.SetBoolHash(_isScreenOn, false);
            yield return null;
        }

        public override IEnumerator CompleteSetup()
        {
            StartCoroutine(PowerOn());
            yield return null;
        }

        public override IEnumerator PowerOff()
        {
            throw new NotImplementedException();
        }

        public override void DrawPage(int page)
        {
            throw new NotImplementedException();
        }

        public override void ItemModified(TechType item, int newAmount = 0)
        {
            throw new NotImplementedException();
        }

        public override void ClearPage()
        {
            throw new NotImplementedException();
        }

        private void Destroy()
        {
            CancelInvoke("UpdateScreen");
            CancelInvoke("CheckDamaged");
        }
    }
}
