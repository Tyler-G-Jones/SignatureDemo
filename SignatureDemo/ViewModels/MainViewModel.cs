using Caliburn.Micro;
using SignatureDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignatureDemo.assets;
using Newtonsoft.Json;

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
                string save = "Signature_" + SelectedUser.Name + ".docx";
                bool forceOverwrite = true;
                int exitCode = DokWriter.Create(jsonSignature, "signature", save, forceOverwrite);
                if (exitCode != 0)
                {
                    StatusGenerated = "Error generating file.";
                }
                else
                {
                    StatusGenerated = "Document generated!";
                }
            }
        }
    }
}
