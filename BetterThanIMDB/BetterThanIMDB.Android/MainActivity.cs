using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using Acr.UserDialogs;
using BetterThanIMDB.Models;
using BetterThanIMDB.Models.Collections;
using BetterThanIMDB.Services;
using Lottie.Forms.Droid;
using System.Net.Http;

namespace BetterThanIMDB.Droid
{
    [Activity(Label = "BetterThanIMDB", Icon = "@mipmap/icon", Theme = "@style/SplashScreenStyle", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            UserDialogs.Init(this);
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            AnimationViewRenderer.Init();

            try
            {
                await DataHelper.LoadAppSettings();
            }
            catch (System.IO.FileNotFoundException)
            {

            }

            LoadApplication(new App());
        }               
        //catch aggregate exception ??
        protected override async void OnStart()
        {
            base.OnStart();
            try
            {
                TMDBService.Instance.Initialize();
            }
            catch (Exception)
            {

            }
            try
            {
                if (DataHelper.FilmToActorConnections.Count == 0)
                {
                    UserDialogs.Instance.Toast("Attempting to get data from file");
                    await DataHelper.LoadAppInfo();
                }

            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("FileNotFoundException");
            }
        }

        protected override async void OnDestroy()
        {
            base.OnDestroy();
            await DataHelper.SaveAppInfo();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

