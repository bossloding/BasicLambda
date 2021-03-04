using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BasicLambda
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create Datatable  ข้อมูลโรงพยาบาลสังกัดกรุงเทพมหานคร  https://data.go.th/dataset/hospital_bma//
            DataTable tbhospital_bma = new DataTable("hospital_bma");
            tbhospital_bma.Columns.Add("_id", typeof(Int64));
            tbhospital_bma.Columns.Add("id_9", typeof(Int32));
            tbhospital_bma.Columns.Add("id_5", typeof(Int32));
            tbhospital_bma.Columns.Add("decode", typeof(Int32));
            tbhospital_bma.Columns.Add("name", typeof(string));
            tbhospital_bma.Columns.Add("name_eng", typeof(string));
            tbhospital_bma.Columns.Add("address", typeof(string));
            tbhospital_bma.Columns.Add("bed_54", typeof(string));
            tbhospital_bma.Columns.Add("bed_55", typeof(string));

            List<string[]> raw_data = new List<string[]>();
            raw_data.Add(new string[] { "1", "001154100", "11541", "1031", "โรงพยาบาลเจริญกรุงประชารักษ์", "Charoen Krung Pracharak", "8 ถนนเจริญกรุง เขตบางคอแหลม กรุงเทพมหานคร 10120", "427", "402" });
            raw_data.Add(new string[] { "2", "001153700", "11537", "1008", "โรงพยาบาลกลาง", "Bangkok Metropolitan Administration General", "514 ถนนหลวง เขตป้อมปราบศัตรูพ่าย กรุงเทพมหานคร 10100", "412", "412" });
            raw_data.Add(new string[] { "3", "001153900", "11539", "1018", "โรงพยาบาลตากสิน", "Taksin", "543 ถนนสมเด็จเจ้าพระยา เขตคลองสาน กรุงเทพมหานคร 10600", "402", "430" });
            //....

            foreach (string[] data in raw_data)
            {
                DataRow NewRow = tbhospital_bma.NewRow();
                NewRow.SetField<Int64>("_id", Convert.ToInt64(data[0]));
                NewRow.SetField<Int64>("id_9", Convert.ToInt32(data[1]));
                NewRow.SetField<Int64>("id_5", Convert.ToInt32(data[2]));
                NewRow.SetField<Int32>("decode", Convert.ToInt32(data[3]));
                NewRow.SetField<string>("name", data[4]);
                NewRow.SetField<string>("name_eng", data[5]);
                NewRow.SetField<string>("address", data[6]);
                NewRow.SetField<Int32>("bed_54", Convert.ToInt32(data[7]));
                NewRow.SetField<Int32>("bed_55", Convert.ToInt32(data[8]));
                tbhospital_bma.Rows.Add(NewRow);
            }

            tbhospital_bma.AcceptChanges();


            // Lambda Where name TH and ENG//
            var result = tbhospital_bma.AsEnumerable().Where(x => x.Field<string>("name") == "โรงพยาบาลกลาง" || x.Field<string>("name_eng") == "Bangkok Metropolitan Administration General");
            if (result.Any())
            {
                myconsole.show(result);
            }

            myconsole.shownewline();

            // Lambda Order - Sort order max bed year 2554//
            result = tbhospital_bma.AsEnumerable().OrderByDescending(x => x["bed_54"]);
            if (result.Any())
            {
                myconsole.show(result);
            }

            myconsole.shownewline();

            // Lambda Get result have 1234 in data is null//
            var Check = tbhospital_bma.AsEnumerable().Where(x => x.Field<Int32>("id_5") == 1234).DefaultIfEmpty(null);
            if (Check.Any())
            {
                Console.WriteLine("1234 have = " + (Check.ElementAt(0) == null ? "Not result" : "Have result"));
            }

            myconsole.shownewline();

            // LINQ Get manual result //
            var new_result = tbhospital_bma.AsEnumerable().Select(x => new { IsDecode = x.Field<Int32>("decode"), IsName = x.Field<string>("name") });
            if (new_result.Any())
            {
                foreach (var show in new_result)
                {
                    Console.WriteLine(string.Format("IsDecode : {0} , IsName : {1}", show.IsDecode, show.IsName));
                }
            }
        }

        private static class myconsole
        {
            public static void show(EnumerableRowCollection result)
            {
                foreach (DataRow show in result)
                {
                    foreach (DataColumn column in show.Table.Columns)
                    {
                        Console.WriteLine(string.Format("{0} : {1}", column.ColumnName, show[column.ColumnName].ToString()));
                    }
                }
            }

            public static void shownewline()
            {
                for (int i = 0; i <= 100; i++)
                {
                    Console.Write("-");
                }
                Console.WriteLine();
            }
        }



    }
}
