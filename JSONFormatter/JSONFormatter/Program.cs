using System;
using System.IO;
using System.Linq;
using System.Data;
using Newtonsoft.Json;

namespace JSONFormatter
{
    /// <Summary>
    /// This class converts json to any required file type
    /// First argument is input json file
    /// Second argument is any output file type
    /// </Summary>
    class Program
    {
        static void Main(string[] args)
        {
            string inputJsonFile = args[0];
            string outputFile = args[1];

            Program objProgram = new JSONFormatter.Program();

            objProgram.GetItems(inputJsonFile, outputFile);
        }

        /// <Summary>
        /// This method reads json file and Deserialize it
        /// Data table is created with the required output format
        /// </Summary>
        private void GetItems(string inputJsonFile, string outputFile)
        {
            DataTable dt = new DataTable();

            var json = File.ReadAllText(inputJsonFile);

            try
            {
                Root myDeserializedJson = JsonConvert.DeserializeObject<Root>(json);

                dt.Columns.Add("Id");
                dt.Columns.Add("Type");
                dt.Columns.Add("Name");
                dt.Columns.Add("Batter");
                dt.Columns.Add("Topping");

                for (int i = 0; i < myDeserializedJson.items.item.Count(); i++)
                {
                    for (int j = 0; j < myDeserializedJson.items.item[i].batters.batter.Count(); j++)
                    {
                        for (int k = 0; k < myDeserializedJson.items.item[i].topping.Count(); k++)
                        {
                            DataRow dr = dt.NewRow();

                            dr["Id"] = myDeserializedJson.items.item[i].id;
                            dr["Type"] = myDeserializedJson.items.item[i].type;
                            dr["Name"] = myDeserializedJson.items.item[i].name;
                            dr["Batter"] = myDeserializedJson.items.item[i].batters.batter[j].type;
                            dr["Topping"] = myDeserializedJson.items.item[i].topping[k].type;

                            dt.Rows.Add(dr);
                        }
                    }
                }

                WriteDataToFile(dt, outputFile);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <Summary>
        /// The Data table is written to an output file
        /// </Summary>
        public static void WriteDataToFile(DataTable dt, string outputFile)
        {
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            StreamWriter sw = new StreamWriter(outputFile, false);
            //headers    
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sw.Write(dt.Columns[i]);
                if (i < dt.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            //rows   
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dt.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();

            Console.WriteLine("Output file created successfully");
        }
    }
}
