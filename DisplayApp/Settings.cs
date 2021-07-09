using System;
using System.IO;
using System.Reflection;
using IniParser;
using IniParser.Model;
using static DisplayApp.frmMain;
namespace DisplayApp
{
    public class Settings
    {
        private int _lineNo;
        private float _tactTime;
        private string _address, _database, _user, _password, _port;
        private string _connectionString;

        //raspberrypi/linux-nál (úgytűnik windowsnál is működik ez a formátum)( / ):
        private string inipath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)+@"/settings.ini";
        //windowsnál ( \ ):
        //private string inipath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\settings.ini";

        public int LineNo { get => _lineNo; set => _lineNo = value; }
        public float TactTime { get => _tactTime; set => _tactTime = value; }
        public string Address { get => _address; set => _address = value; }
        public string Database { get => _database; set => _database = value; }
        public string User { get => _user; set => _user = value; }
        public string Password { get => _password; set => _password = value; }
        public string Port { get => _port; set => _port = value; }
        public string ConnectionString { get => _connectionString; set => _connectionString = value; }

        public void LoadSettings()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(inipath);

            LineNo = int.Parse(data["settings"]["line"]);
            TactTime = float.Parse(data["settings"]["tactTime"]);
            Address = data["database"]["address"];
            Database = data["database"]["database"];
            User = data["database"]["user"];
            Password = data["database"]["password"];
            Port = data["database"]["port"];

            ConnectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}", Address, Port, User, Password, Database);
        }

        public void SaveSettings()
        {
            var parser = new FileIniDataParser();
            IniData data = new IniData();
            try
            {
                data["settings"]["line"] = LineNo.ToString();
                parser.WriteFile(inipath, data);

                data["settings"]["tactTime"] = TactTime.ToString("#0.0");
                parser.WriteFile(inipath, data);

                data["database"]["address"] = Address;
                parser.WriteFile(inipath, data);

                data["database"]["database"] = Database;
                parser.WriteFile(inipath, data);

                data["database"]["port"] = Port;
                parser.WriteFile(inipath, data);

                data["database"]["user"] = User;
                parser.WriteFile(inipath, data);

                data["database"]["password"] = Password;
                parser.WriteFile(inipath, data);
            }
            catch(Exception ex)
            {
                LogException(ex);
            }
            
        }




    }
}
