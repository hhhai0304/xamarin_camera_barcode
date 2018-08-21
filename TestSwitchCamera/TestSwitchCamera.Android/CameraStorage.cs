using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TestSwitchCamera.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(CameraStorage))]
namespace TestSwitchCamera.Droid
{
    public class CameraStorage : IStorage
    {
        public string GetPath()
        {
            return Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "Logs";
        }
    }
}