
using System;
using AplicativoMeditacao.Droid.SettingsManager;
using AplicativoMeditacao.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppVersion))]
namespace AplicativoMeditacao.Droid.SettingsManager
{
    public class AppVersion : Java.Lang.Object, IInstalledAppVersion
    {
        public string GetAppVersion()
        {
            var context = Forms.Context;
            return context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
        }
    }
}