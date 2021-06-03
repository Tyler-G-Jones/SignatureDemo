using Caliburn.Micro;
using SignatureDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DOK;

namespace SignatureDemo.ViewModels
{
    public class MainViewModel : Conductor<object>
    {
        private string _name = "Tyler";
        private string _evoquaSignature = "tyler.jones@evoqua.com";
        private BindableCollection<UserModel> _users = new BindableCollection<UserModel>();
        private UserModel _selectedUser;

        private string _statusGenerated = "";

        public MainViewModel()
        {
            Users.Add(new UserModel { Name = "Tyler", EvoquaSignature = @"
                {
                    ""SName"" : ""Tyler Jones"",
                    ""STitle"" : ""Intern"",
                    ""SPhones"" : [
                    {
                    ""SPNum"" : ""Tel:      +1 (858) 705-8300""
                    }
                    ],
                    ""SEmail"" : ""tyler.jones@evoqua.com""
                }
                "});
            Users.Add(new UserModel { Name = "Matt", EvoquaSignature = @"
                {
                    ""SName"" : ""Matt Roegner"",
                    ""STitle"" : ""Automation and Design Tools Lead"",
                    ""SPhones"" : [
                    {
                    ""SPNum"" : ""Desk:     +1 (262) 521-8215""
                    },
                    {
                    ""SPNum"" : ""Cell:     +1 (262) 278-2059""
                    }
                    ],
                    ""SEmail"" : ""matthew.roegner@evoqua.com""
                }
                " });
            Users.Add(new UserModel { Name = "Anthony", EvoquaSignature = @"
                {
                    ""SName"" : ""Anthony Bailey"",
                    ""STitle"" : ""Master Field Service Technician"",
                    ""SPhones"" : [
                    {
                    ""SPNum"" : ""Office:       (262) 521-8570 ""
                    }
                    ],
                    ""SEmail"" : ""anthony.bailey@evoqua.com""
                }
                " });
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string EvoquaSignature
        {
            get { return _evoquaSignature; }
            set
            {
                _evoquaSignature = value;
                NotifyOfPropertyChange(() => EvoquaSignature);
            }
        }

        public BindableCollection<UserModel> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }

        public string StatusGenerated
        {
            get { return _statusGenerated; }
            set 
            { 
                _statusGenerated = value;
                NotifyOfPropertyChange(() => StatusGenerated);
            
            }
        }

        public void GenerateDoc()
        {
            if (SelectedUser is null)
            {
                StatusGenerated = "Invalid Selection!";
            }
            else
            {
                string jsonSignature = SelectedUser.EvoquaSignature;
                string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Generated");
                try
                {
                    FileAttributes attr = File.GetAttributes(savePath);
                }
                catch (System.IO.FileNotFoundException)
                {
                    Directory.CreateDirectory(savePath);
                }
                savePath = Path.Combine(savePath, SelectedUser.Name + "_Signature.docx");
                string templatePath = Path.Combine(Environment.CurrentDirectory, "assets", "templateSignature.docx");
                int exitCode = Templater.Create(jsonSignature, templatePath, savePath, true);
                //Console.WriteLine(jsonSignature);
                //Console.WriteLine(savePath);
                //Console.WriteLine(templatePath);
                //Console.WriteLine(exitCode);

                if (exitCode != 0)
                {
                    StatusGenerated = "Error generating file.";
                }
                else
                {
                    StatusGenerated = "Created! (Desktop/Generated/" + SelectedUser.Name + "_Signature.docx)";
                }
            }
        }
    }
}
