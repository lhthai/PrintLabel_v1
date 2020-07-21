using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintLabel.Helpers
{
    class LoadBatchcodeFromWO
    {
        LoadData loadData = new LoadData();

        public DataTable LoadBatchcode(string createDate, string product)
        {
            if (product == "103AB0801011066" || product == "103AB0802011066" || product == "103AB0803011066" || product == "103AB0804011066")
            {
                return LoadAB(createDate, product);
            }
            else if (product == "144AB0805011066" || product == "144AB0806021066" || product == "144AB0807011066" || product == "144AB0808021066" ||
                product == "126AB0809001066" || product == "138AB0810011066" || product == "138AB0811021066")
            {
                return LoadJ1AndCUV(createDate, product);
            }
            else if(product == "243AH1501001066" || product == "243AH1502001066" || product == "103AH1503011066" || product == "103AH1504011066" || product == "126AH1505001066")
            {
                return LoadAH1501_05(createDate, product);
            }
            else
            {
                return LoadAH1506_09(createDate, product);
            }
        }



        public DataTable LoadAB(string createDate, string product)
        {
            string query = $@"
                           SELECT [BATCHCODE] AS SERIALNUMBER,REPLACE(REPLACE(REMARK,'FFF',''),'_','') AS CUSTOMERPART,
                            CONCAT(DATEPART(WW,CREATEDATE),'W ',YEAR(CREATEDATE)) AS WEEK,
                            '003' AS REVISION, 
                            CASE
                                WHEN PRODUCT = '103AB0801011066' THEN 'FL'
                                WHEN PRODUCT = '103AB0802011066' THEN 'FR'
                                WHEN PRODUCT = '103AB0803011066' THEN 'RL'
                                ELSE 'RR'
                            END AS DIRECTION,
                            CONCAT('[A]',',',[BATCHCODE],',',PRODUCT,',',PRODUCTNAME,',',MAINWONO,',',CONO,',',REMARK,',',REPLACE(CONVERT(NVARCHAR,CREATEDATE,106),' ','-')) AS QRCODE
                            FROM BATCHCODE WITH (NOLOCK)
                            WHERE PRODUCT = '{product}'
                            AND CONVERT(DATE,[CREATEDATE],103) = '{createDate}' ORDER BY BATCHCODE, MAINWONO";
            return loadData.FillDataTable(query);
        }

        public DataTable LoadJ1AndCUV(string createDate, string product)
        {
            string query = $@"
                            SELECT BC.BATCHCODE +'    '+ SUBSTRING(B.REMARK, 0, CHARINDEX('-', B.REMARK)) AS serialNumber,REPLACE(BC.REMARK, SUBSTRING(BC.REMARK, 0, CHARINDEX('|', BC.REMARK) + 1), '') AS customerPart,
                            CONCAT(DATEPART(WW, BC.CREATEDATE),'W ',YEAR(BC.CREATEDATE)) AS week,
                            CONCAT('[A]',',',BC.BATCHCODE,',',BC.PRODUCT,',',BC.PRODUCTNAME,',',BC.MAINWONO,',',BC.CONO,',',BC.REMARK,',',REPLACE(CONVERT(NVARCHAR, BC.CREATEDATE,106),' ','-')) AS qrCode
                            FROM BATCHCODE AS BC WITH (NOLOCK)
                            INNER JOIN BOM AS B WITH (NOLOCK) ON B.BOMNO = BC.BOMNO
                            WHERE BC.PRODUCT = '{product}'
                            AND CONVERT(DATE,BC.CREATEDATE ,103) = '{createDate}' ORDER BY BC.BATCHCODE,BC.MAINWONO";
            return loadData.FillDataTable(query);
        }

        public DataTable LoadAH1501_05(string createDate, string product)
        {
            string query = $@"
                            SELECT [BATCHCODE] AS SERIALNUMBER,REPLACE(REMARK, SUBSTRING(REMARK, 0, CHARINDEX('|', REMARK) + 1), '') AS CUSTOMERPART,
                            CONCAT('W.',DATEPART(WW,CREATEDATE),'/', RIGHT(YEAR(CREATEDATE), 2) ) AS WEEK,
                            REPLACE(REMARK, '.','') AS BARCODE,
                            CONCAT('[A]',',',[BATCHCODE],',',PRODUCT,',',PRODUCTNAME,',',MAINWONO,',',CONO,',',REMARK,',',REPLACE(CONVERT(NVARCHAR,CREATEDATE,106),' ','-')) AS QRCODE
                            FROM BATCHCODE WITH (NOLOCK)
                            WHERE PRODUCT = '{product}'
                            AND CONVERT(DATE,CREATEDATE,103) = '{createDate}' ORDER BY BATCHCODE, MAINWONO";
            return loadData.FillDataTable(query);
        }

        public DataTable LoadAH1506_09(string createDate, string product)
        {
            string query = $@"
                            SELECT [BATCHCODE] AS SERIALNUMBER,REPLACE(REMARK, SUBSTRING(REMARK, 0, CHARINDEX('|', REMARK) + 1), '') AS CUSTOMERPART,
                            CONCAT('W.',DATEPART(WW,CREATEDATE),'/', RIGHT(YEAR(CREATEDATE), 2) ) AS WEEK,
                            CONCAT('[A]',',',[BATCHCODE],',',PRODUCT,',',PRODUCTNAME,',',MAINWONO,',',CONO,',',REMARK,',',REPLACE(CONVERT(NVARCHAR,CREATEDATE,106),' ','-')) AS QRCODE
                            FROM BATCHCODE WITH (NOLOCK)
                            WHERE PRODUCT = '{product}'
                            AND CONVERT(DATE,CREATEDATE,103) = '{createDate}' ORDER BY BATCHCODE, MAINWONO";
            return loadData.FillDataTable(query);
        }
    }
}
