﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace ProMama.Droid
{
    [Activity(Label = "Pró-Mamá", Icon = "@drawable/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            // CarouselView
            var cv = typeof(Xamarin.Forms.CarouselView);
            var assembly = System.Reflection.Assembly.Load(cv.FullName);

            // ImageCircle
            ImageCircle.Forms.Plugin.Droid.ImageCircleRenderer.Init();

            // FFImageLoading
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            // Iconize
            Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeModule());

            // Acr.UserDialogs
            Acr.UserDialogs.UserDialogs.Init(this);

            // Notification Icon
            Plugin.LocalNotifications.LocalNotificationsImplementation.NotificationIconId = Resource.Drawable.icon;

            LoadApplication(new ProMama.App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

