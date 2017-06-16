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

            Debug.WriteLine("ScanCallback::OnScanFailed: " + errorCode);
        }

        public override void OnBatchScanResults(IList<ScanResult> results)
        {
            base.OnBatchScanResults(results);

            Debug.WriteLine("ScanCallback::OnBatchScanResults:");

            foreach (var scanResult in results)
            {
                HandleScanResult(scanResult);
            }
        }

        public override void OnScanResult(ScanCallbackType callbackType, ScanResult scanResult)
        {
            base.OnScanResult(callbackType, scanResult);

            Debug.WriteLine($"OnScanResult ({callbackType}):");

            HandleScanResult(scanResult);
        }

        private bool _triedConnect = false;
        private string _connectAddressTarget = "CA:99:61:99:B2:DA";
        private string _connectDeviceNameTarget = "Wahoo KICKR F284";

        private void HandleScanResult(ScanResult scanResult)
        {
            Debug.WriteLine($"    {scanResult.Device.Address}, DeviceName:{scanResult.Device.Name}, ScanRecord:{scanResult.ScanRecord.DeviceName}");

            if (!_triedConnect && scanResult.Device.Address == _connectAddressTarget &&
                scanResult.ScanRecord.DeviceName == _connectDeviceNameTarget)
            {
                _triedConnect = true;

                Debug.WriteLine($"    Attempting connect....");
                scanResult.Device.ConnectGatt(null, false, new SimpleGattCallback());
            }
        }
    }

    public class SimpleGattCallback : BluetoothGattCallback
    {
        public override void OnConnectionStateChange(BluetoothGatt gatt, GattStatus status, ProfileState newState)
        {
            base.OnConnectionStateChange(gatt, status, newState);

            Debug.WriteLine($"BluetoothGattCallback::OnConnectionStateChange: {gatt.Device.Address}, {status}, {newState}");
        }
    }
}

