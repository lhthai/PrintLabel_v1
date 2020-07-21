using PrintLabel.Labels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;

namespace PrintLabel.Helpers
{
    class Printer
    {
        private static string GetUSBConnection()
        {
            string usbString = "";
            try
            {
                foreach (DiscoveredUsbPrinter usbPrinter in UsbDiscoverer.GetZebraUsbPrinters(new ZebraPrinterFilter()))
                {
                    usbString += usbPrinter.Address;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return usbString;
        }

        private static void Printing(Stream xmlData, string templatePath)
        {
            string destinationDevice = GetUSBConnection();
            string templateFilename = templatePath;
            string defaultQuantityString = "1";
            bool verbose = true;
            try
            {
                using (MemoryStream outputDataStream = new MemoryStream())
                {
                    using (Stream sourceStream = xmlData)
                    {
                        XmlPrinter.Print(destinationDevice, sourceStream, templateFilename, defaultQuantityString, outputDataStream, verbose);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void PrintAll(string product, DataTable items)
        {
            if (product == "103AB0801011066" || product == "103AB0802011066" || product == "103AB0803011066" || product == "103AB0804011066")
            {
                AB0801_04 AB = new AB0801_04();
                AB.PrintAB(items, GetUSBConnection());
            }
            else if (product == "144AB0805011066" || product == "144AB0806021066" || product == "144AB0807011066" || product == "144AB0808021066" ||
                product == "126AB0809001066" || product == "138AB0810011066" || product == "138AB0811021066")
            {
                AB0805_11 J1 = new AB0805_11();
                J1.PrintJ1AndCUV(items, GetUSBConnection());
            }
            
            else if (product == "243AH1501001066" || product == "243AH1502001066" || product == "103AH1503011066" || product == "103AH1504011066" || product == "126AH1505001066")
            {
                AH1501_05 AH = new AH1501_05();
                AH.PrintAH1501_05(items, GetUSBConnection());
            }
            else if (product == "243AH1506001066" || product == "243AH1507001066" || product == "243AH1508001066" || product == "243AH1509001066")
            {
                AH1506_09 AH = new AH1506_09();
                AH.PrintAH1506_09(items, GetUSBConnection());
            }
            else
            {
                AH1506_09 AH = new AH1506_09();
                AH.PrintAH1506_09(items, GetUSBConnection());
            }
        }
    }
}
