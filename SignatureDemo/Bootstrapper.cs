using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Reflection;
using SignatureDemo.ViewModels;

namespace SignatureDemo
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
            byte[] file = Properties.Resources.templateSignature;
            var obj = Properties.Resources.ResourceManager.GetObject("assets.templateSignature.docx");
            Assembly assembly = Assembly.GetExecutingAssembly();
            List<string> resourceNames = new List<string>(assembly.GetManifestResourceNames());
            for (int i = 0; i < resourceNames.Count; i++)
            {
                Console.WriteLine(resourceNames[i]);
            }
            Stream rs = assembly.GetManifestResourceStream("SignatureDemo.assets.templateSignature.docx");
            Console.WriteLine(Environment.CurrentDirectory);

        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
