using System;
using System.Collections.Generic;
using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Debug = System.Diagnostics.Debug;

namespace AndroidBleExperiment
{
    [Activity(Label = "AndroidBleExperiment", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };

            BluetoothAdapter.DefaultAdapter.BluetoothLeScanner.StartScan(new SimpleScanCallback());
        }
    }

    public class SimpleScanCallback : ScanCallback
    {
        public override void OnScanFailed(ScanFailure errorCode)
        {
            base.OnScanFailed(errorCode);

            Debug.WriteLine("OnScanFailed: " + errorCode);
        }

        public override void OnBatchScanResults(IList<ScanResult> results)
        {
            base.OnBatchScanResults(results);

            Debug.WriteLine("OnBatchScanResults:");

            foreach (var scanResult in results)
            {
                TraceScanResult(scanResult);
            }
        }

        public override void OnScanResult(ScanCallbackType callbackType, ScanResult scanResult)
        {
            base.OnScanResult(callbackType, scanResult);

            Debug.WriteLine($"OnScanResult ({callbackType}):");

            TraceScanResult(scanResult);
        }

        private void TraceScanResult(ScanResult scanResult)
        {
            Debug.WriteLine($"    {scanResult.Device.Address}, DeviceName:{scanResult.Device.Name}, ScanRecord:{scanResult.ScanRecord.DeviceName}");
        }
    }
}

