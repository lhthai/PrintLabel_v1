using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zebra.Sdk.Printer;

namespace PrintLabel.Labels
{
    class AH1506_09
    {
        private static Stream XmlData(DataTable dt)
        {
            string xml =
               "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                + "<file _FORMAT=\"AH1506-AH1509.ZPL\">"
                    + XmlLabels(dt)
                        + "</file>";

            return new MemoryStream(Encoding.UTF8.GetBytes(xml));
        }

        public static string XmlLabels(DataTable dt)
        {
            string result = "";
            foreach (DataRow row in dt.Rows)
            {
                string serialNumber = row["serialNumber"].ToString();
                string week = row["week"].ToString();
                string customerPart = row["customerPart"].ToString();
                string qrCode = row["qrCode"].ToString();

                result += " <label>\n"
                        + "     <variable name=\"customerPart\">" + customerPart + "</variable>"
                        + "     <variable name=\"qrCode\">" + qrCode + "</variable>"
                        + "     <variable name=\"serialNumber\">" + serialNumber + "</variable>"
                        + "     <variable name=\"week\">" + week + "</variable>"
                        + "  </label>\n";
            }
            return result;
        }

        public void PrintAH1506_09(DataTable dt, string printerConnection)
        {
            string destinationDevice = printerConnection;
            string templateFilename = "D:\\AH1506-AH1509.prn";
            string defaultQuantityString = "1";
            bool verbose = true;
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    using (MemoryStream outputDataStream = new MemoryStream())
                    {
                        using (Stream sourceStream = XmlData(dt))
                        {
                            XmlPrinter.Print(destinationDevice, sourceStream, templateFilename, defaultQuantityString, outputDataStream, verbose);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
