using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.CodeDom.Compiler;
using System.CodeDom;
using Newtonsoft.Json;

namespace SignatureDemo.assets
{
    public abstract class DokWriter
    {
        // Local implentation of "Document Utility Library" by Wesley Zloza
        // https://github.com/EWT-Automation/dok/tree/v1.2.0

        public const string PfGroupshares = @"\\swaugfs03.wt001.net\Groupshares\";
        public const string ESPath = PfGroupshares + "Stores\\Engineer Suite - Programs\\publish\\Essential Files\\";

        public static string LocalDirectory = Directory.GetCurrentDirectory();
        public static string PathTemplateSignature = "../../assets/templateSignature.docx";
        public static string PathDok = "../../assets/dok.exe";
        public static string PathSave = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Generated\";

        public static Dictionary<string, string> TemplateRef = new Dictionary<string, string>(){
            {"signature", PathTemplateSignature}
        };

        public static void DebugPath()
        {
            Console.WriteLine("[DEBUG] Verifying paths exist...");
            List<string> paths = new List<string>();
            paths.Add(PathTemplateSignature);
            paths.Add(PathDok);
            paths.Add(PathSave);
            for (int i=0; i<paths.Count; i++)
            {
                string path = paths.ElementAt(i);
                FileAttributes attr;
                try
                {
                    attr = File.GetAttributes(path);
                }
                catch (Exception ex)
                { 
                    Console.WriteLine("[DEBUG] FAILED to find " + path + " : " + ex.Message);
                    continue;
                }
                switch (attr)
                {
                    case FileAttributes.Directory:
                        if (Directory.Exists(path))
                        {
                            Console.WriteLine("[DEBUG] Directory exists: " + path);
                        }
                        else
                        {
                            Console.WriteLine("[DEBUG] Directory DOES NOT exist: " + path);
                        }
                        break;
                    default:
                        if (File.Exists(path))
                        {
                            Console.WriteLine("[DEBUG] File exists: " + path);
                        }
                        else
                        {
                            Console.WriteLine("[DEBUG] File DOES NOT exist: " + path);
                        }
                        break;
                }
            }
            Console.WriteLine("[DEBUG] Path verification finished.");
        }

        /// <summary>
        /// Creates a new Microsoft Word document based on a provided template and JSON data provided to fill the template.
        /// </summary>
        /// <param name="JSON"></param>
        /// <param name="templatePath"></param>
        /// <param name="savePath"></param>
        /// <param name="forceOverwrite"></param>
        /// <returns>
        /// Process exit code. An interger value of 0 indicates succesful exit. Any other integer indicates failure.
        /// </returns>
        public static int Create(string JSON, string template, string save, bool forceOverwrite)
        {
            Console.WriteLine("[DokWriter.Create] Generating document : " + template + " " + save + " " + forceOverwrite);
            DebugPath();

            // Process arguments.
            string jsonPath = Path.Combine(Path.GetTempPath(), "dok.json");
            string args = string.Concat("\"", jsonPath, "\" \"", TemplateRef[template], "\" ", PathSave + save);
            if (forceOverwrite) { args += " -f"; }

            try
            {
                FileAttributes attr = File.GetAttributes(PathSave);
            }
            catch (System.IO.FileNotFoundException)
            {
                Directory.CreateDirectory(PathSave);
            }

            // Write JSON to file. 
            File.WriteAllText(jsonPath, JSON);

            // Process info.
            var processInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = PathDok,
                Arguments = args
            };
            try
            {
                // Starting child process.
                var process = Process.Start(processInfo);
                process.WaitForExit();

                // Delete JSON file.
                File.Delete(jsonPath);

                // Return process exit code.
                return process.ExitCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 999;
            }
        }
    }
}
