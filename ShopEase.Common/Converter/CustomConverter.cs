using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase.Common.Converter
{
    public class CustomConverter
    {         
        public static string ConvertDatatableToXML(System.Data.DataTable dt)
        {
            MemoryStream str = new MemoryStream();
            dt.WriteXml(str, true);
            str.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(str);
            string xmlstr;
            xmlstr = sr.ReadToEnd();
            return (xmlstr);
        }
    }
}
