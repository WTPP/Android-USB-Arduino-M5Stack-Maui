using Android.Content;
using Android.Hardware.Usb;
using Hoho.Android.UsbSerial.Driver;
using System.Text;
using Android.App;

namespace MauiATOM;

public partial class MainPage : ContentPage
{
    private IUsbSerialPort _port;
    public MainPage()
	{
		InitializeComponent();
        Connect();
    }

    public void Connect()
    {
        string ACTION_USB_PERMISSION = "com.android.example.USB_PERMISSION";
        Context context = Android.App.Application.Context;
        UsbManager manager = (UsbManager)context.GetSystemService(Context.UsbService);

        try
        {
            if (manager.DeviceList.Count == 0)
                return;

            UsbDevice device = manager.DeviceList.Values.FirstOrDefault();

            PendingIntent mPermissionIntent = PendingIntent.GetBroadcast(context, 0, new Intent(ACTION_USB_PERMISSION), 0);
            IntentFilter filter = new IntentFilter(ACTION_USB_PERMISSION);

            manager.RequestPermission(device, mPermissionIntent);
            bool hasPermision = manager.HasPermission(device);

            while (!hasPermision)
            {
                manager.RequestPermission(device, mPermissionIntent);
                hasPermision = manager.HasPermission(device);
            }

            UsbDeviceConnection connection = manager.OpenDevice(device);

            IList<IUsbSerialDriver> availableDrivers = UsbSerialProber.DefaultProber.FindAllDrivers(manager);
            IUsbSerialDriver driver = availableDrivers[0];

            _port = driver.Ports.FirstOrDefault();
            _port.Open(connection);
            _port.SetParameters(115200, 8, StopBits.One, Parity.None);
            _port.Write(Encoding.ASCII.GetBytes("COL3n"), 0);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void Blue(object sender, EventArgs e)
    {
        _port.Write(Encoding.ASCII.GetBytes("COL1n"), 0);
    }

    private void Red(object sender, EventArgs e)
    {
        _port.Write(Encoding.ASCII.GetBytes("COL2n"), 0);
    }

    private void Green(object sender, EventArgs e)
    {
        _port.Write(Encoding.ASCII.GetBytes("COL3n"), 0);
    }
}

