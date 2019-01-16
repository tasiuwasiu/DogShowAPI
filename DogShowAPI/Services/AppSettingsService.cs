using DogShowAPI.Helpers;
using DogShowAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogShowAPI.Services
{
    public interface IAppSettingsService
    {
        AppSetting setTitle(string title);
        AppSetting getTitle();
        AppSetting getAppState();
        AppSetting setAppState(string appState);
        bool canGrade();
        bool canEnter();
    }

    public class AppSettingsService : IAppSettingsService
    {

        private DogShowContext context;
        public static string TITLE_SETTING = "app_title";
        public static string APP_STATE_SETTING = "app_state";

        public AppSettingsService(DogShowContext context)
        {
            this.context = context;
        }

        public AppSetting setTitle(string title)
        {
            AppSetting currentTitleObject = context.AppSetting.Where(a => a.SettingName == TITLE_SETTING).FirstOrDefault();
            if (currentTitleObject == null)
            {
                AppSetting titleSetting = new AppSetting
                {
                    SettingName = TITLE_SETTING,
                    SettingValue = title
                };
                context.AppSetting.Add(titleSetting);
                context.SaveChanges();
            }
            else
            {
                currentTitleObject.SettingValue = title;
                context.SaveChanges();
            }
            return context.AppSetting.Where(a => a.SettingName == TITLE_SETTING).FirstOrDefault();
        }

        public AppSetting getTitle()
        {
            return context.AppSetting.Where(a => a.SettingName == TITLE_SETTING).FirstOrDefault();
        }

        public AppSetting getAppState()
        {
            return context.AppSetting.Where(a => a.SettingName == APP_STATE_SETTING).FirstOrDefault();
        }


        public AppSetting setAppState(string appState)
        {
            AppSetting currentStateObject = context.AppSetting.Where(a => a.SettingName == APP_STATE_SETTING).FirstOrDefault();
            if (currentStateObject == null)
            {
                AppSetting appStateSetting = new AppSetting
                {
                    SettingName = APP_STATE_SETTING,
                    SettingValue = appState
                };
                context.AppSetting.Add(appStateSetting);
                context.SaveChanges();
            }
            else
            {
                currentStateObject.SettingValue = appState;
                context.SaveChanges();
            }
            return context.AppSetting.Where(a => a.SettingName == APP_STATE_SETTING).FirstOrDefault();
        }

        public bool canGrade()
        {
            AppSetting stateSetting = getAppState();
            if (stateSetting == null)
                return false;
            AppStateSetting a = JsonConvert.DeserializeObject<AppStateSetting>(stateSetting.SettingValue);
            return a.appState == 20;
        }

        public bool canEnter()
        {
            AppSetting stateSetting = getAppState();
            if (stateSetting == null)
                return true;
            AppStateSetting a = JsonConvert.DeserializeObject<AppStateSetting>(stateSetting.SettingValue);
            return a.appState == 10;
        }
    }
}
