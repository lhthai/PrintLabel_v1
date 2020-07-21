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
    class AB0801_04
    {
        private static Stream XmlData(string customerPart, string qrCode, string serialNumber, string week, string direction)
        {
            string xml =
               "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                + "<file _FORMAT=\"AB0801-04.ZPL\">"
                    + " <label>\n"
                        + "     <variable name=\"customerPart\">" + customerPart + "</variable>"
                        + "     <variable name=\"qrCode\">" + qrCode + "</variable>"
                        + "     <variable name=\"serialNumber\">" + serialNumber + "</variable>"
                        + "     <variable name=\"week\">" + week + "</variable>"
                        + "     <variable name=\"direction\">" + direction + "</variable>"
                        + "  </label>\n"
                        + "</file>";

            return new MemoryStream(Encoding.UTF8.GetBytes(xml));
        }


        public void PrintAB(DataTable dt, string printerConnection)
        {
            string destinationDevice = printerConnection;
            string templateFilename = "D:\\AB0801-04.prn";
            string defaultQuantityString = "1";
            bool verbose = true;
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    using (MemoryStream outputDataStream = new MemoryStream())
                    {
                        string serialNumber = row["serialNumber"].ToString();
                        string week = row["week"].ToString();
                        string customerPart = row["customerPart"].ToString();
                        string direction = row["direction"].ToString();
                        string qrCode = row["qrCode"].ToString();

                        using (Stream sourceStream = XmlData(customerPart, qrCode, serialNumber, week, direction))
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
