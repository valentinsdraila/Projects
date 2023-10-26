using PmsSettings.Util;
using System;
using System.Reflection;


namespace PmsSettings
{
    public class SettingsDao : ISettingsDao
    {

        public IIniFile IniFile { get; private set; }


        /// <summary>
        /// Class to get data from the settings.ini
        /// </summary>
        /// <exception cref="T:System.IO.ArgumentException">The getters could throw this when the value cannot be found in the ini. the actual error will be embedded in InnerException.</exception>
        public SettingsDao()
        {
            try
            {
                IniFile = new IniFile(@"C:\ProgramData\pms\settings.ini");
                //IniFile = new IniFile(Properties.Settings.Default.INILocation); // TODO: CHANGE BACK
            }
            catch
            {
                //MessageBox.Show("INI file not found: " + e.Message);
            }
        }

        public SettingsDao(IIniFile iniFile)
        {
            IniFile = iniFile;
        }


        public string GetContentForSectionEntry(string section, string entry)
        {
            return IniFile.Read(section, entry);
        }


        //[SERVER]
        //Fileserver=belnspdev001
        //SQLserver=belnspdevSQL001
        //LDAP=BELVDPLM001.net.plm.eds.com
        //Buildserver=BELNSPDEVBLD002
        //LicenseServer=@leup-bizlic01.lmsintl.com

        public string GetWebServer()
        {
            try
            {
                return IniFile.Read("SERVER", "WebServer");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetFileServer()
        {
            try
            {
                return IniFile.Read("SERVER", "FileServer");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetDepotsServer()
        {
            try
            {
                return IniFile.Read("SERVER", "DepotsServer");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetSQLServer()
        {
            try
            {
                return IniFile.Read("SERVER", "SQLServer"); 
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetLDAPServer()
        {
            try
            {
                return IniFile.Read("SERVER", "LDAP");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetBuildServer()
        {
            try
            {
                return IniFile.Read("SERVER", "BuildServer");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            } 
        }

        public string GetLicenseServer()
        {
            try
            {
                return IniFile.Read("SERVER", "LicenseServer");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }


        public string GetPortalDB()
        {
            try
            {
                return IniFile.Read("DATABASE", "PortalDB");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetAutomatedTestingDB()
        {
            try
            {
                return IniFile.Read("DATABASE", "ATDB");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }     
        }


        public int GetATFManagerTimeout()
        {
            try
            {
                return int.Parse(IniFile.Read("TIME", "ATFManagerTimeout"));
            }
            catch
            {
                return 0;
            }
        }

        public int GetATFManagerSleepTime()
        {
            try
            {
                return int.Parse(IniFile.Read("TIME", "ATFManagerSleepTime"));
            }
            catch
            {
                return 0;
            }
        }

        [Obsolete("Not used anymore?")]
        public int GetATFManagerCleanupTime()
        {
            try
            {
                return int.Parse(IniFile.Read("TIME", "ATFManagerCleanupTime"));
            }
            catch
            {
                return 0;
            }
        }

        public int GetATFManagerJobTimeout()
        {
            try
            {
                return int.Parse(IniFile.Read("TIME", "ATFManagerJobTimeout"));
            }
            catch
            {
                return 0;
            }
        }

        public int GetATFExplorerTimeout()
        {
            try
            {
                return int.Parse(IniFile.Read("TIME", "ATFExplorerTimeout"));
            }
            catch
            {
                return 0;
            }
        }

        public int GetATFExplorerSleepTime()
        {
            try
            {
                return int.Parse(IniFile.Read("TIME", "ATFExplorerSleepTime"));
            }
            catch
            {
                return 0;
            }
        }


        public string GetATFManagerReceivers()
        {
            try
            {
                return IniFile.Read("MAIL", "ATFManagerReceivers");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            } 
        }


        public int GetATManagerMaxAttempts()
        {
            try
            {
                return int.Parse(IniFile.Read("ATTEMPT", "ATFManagerMaxAttempts"));
            }
            catch 
            { 
                return 0; 
            }
        }


        public string GetATFManagerMail()
        {
            try
            {
                return IniFile.Read("FOLDER", "ATFManagerMail");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }    
        }

        public string GetATFExplorerInstallation()
        {
            try
            {
                return IniFile.Read("FOLDER", "ATFExplorerInstallation");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            } 
        }

        public string GetATFExplorerDataServerParent()
        {
            try
            {
                return IniFile.Read("FOLDER", "ATFExplorerDataServerParent");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetATFExplorerLocalAutomation()
        {
            try
            {
                return IniFile.Read("FOLDER", "ATFExplorerLocalAutomation");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetATFExplorerSCMAutomTestingRootParent()
        {
            try
            {
                return IniFile.Read("FOLDER", "ATFExplorerSCMAutomTestingRootParent");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }  
        }

        public string GetATFManagerLogfile()
        {
            try
            {
                return IniFile.Read("FOLDER", "ATFManagerLogfile");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetBuildQualityLogPath()
        {
            try
            {
                return IniFile.Read("FOLDER", "BuildQualityLogpath");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetSmartTestingCentralPath()
        {
            try
            {
                return IniFile.Read("FOLDER", "SmartTestingCentralFolder");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        //[TECWARE]
        //revision

        public string GetTecwareLocation(string revision)
        {
            try
            {
                return IniFile.Read("TECWARE", revision);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        //[TECWARE]
        //revision

        public string GetTecwareRegKey(string revision)
        {
            try
            {
                return IniFile.Read("TECWAREKEY", revision);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        [Obsolete("Not used anymore?")]
        public string GetSQLUser()
        {
            try
            {
                return IniFile.Read("USER", "DBUser");
            }
            catch (Exception e)
            {
                return "GetSQLUser() Method Exception: " + e.Message;
            }
        }

        [Obsolete("Value no longer in settings.ini")]
        public string GetSQLPwd()
        {
            try
            {
                return IniFile.Read("USER", "DBPwd");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }


        //[CUSTOM]

        public string GetProcessesToKill()
        {
            try
            {
                return IniFile.Read("CUSTOM", "ATFExplorerProcessesToKill");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetINIFilename()
        {
            try
            {
                return IniFile.Read("CUSTOM", "INIFilename");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetCentralLogDB()
        {
            try
            {
                return IniFile.Read("CUSTOM", "CentralLogDB");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetPortalDBConnectionString()
        {
            try
            {
                return IniFile.Read("CUSTOM", "PortalDBConnectionString");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public int GetAtEarlyStartInHours()
        {
            try
            {
                return IniFile.ReadInt("CUSTOM", "ATEarlyStartInHours");
            }
            catch
            {
                return 0;
            }
        }


        //[InstallationServer]

        public string GetInstallationServerName()
        {
            try
            {
                return IniFile.Read("InstallationServer", "Name");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetServerLocation()
        {
            try
            {
                return IniFile.Read("InstallationServer", "Location");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetServerTemplate()
        {
            try
            {
                return IniFile.Read("InstallationServer", "Template");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetServerReleasedFolder()
        {
            try
            {
                return IniFile.Read("InstallationServer", "Released");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetSideKickLeuvenServerReleasedFolder()
        {
            try
            {
                return IniFile.Read("SideKickInstallationServer", "LeuvenReleased");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetSideKickBrasovServerReleasedFolder()
        {
            try
            {
                return IniFile.Read("SideKickInstallationServer", "BrasovReleased");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }
        public string GetSideKickBredaServerReleasedFolder()
        {
            try
            {
                return IniFile.Read("SideKickInstallationServer", "BredaReleased");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetSideKickKaiServerReleasedFolder()
        {
            try
            {
                return IniFile.Read("SideKickInstallationServer", "KaiReleased");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }
        public string GetSideKickDegotServerReleasedFolder()
        {
            try
            {
                return IniFile.Read("SideKickInstallationServer", "DegotReleased");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetMailClient()
        {
            try
            {
                return IniFile.Read("InstallationServer", "SmtpClient");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetMailSender()
        {
            try
            {
                return IniFile.Read("InstallationServer", "Sender");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetMailReceivers_installationFailed()
        {
            try
            {
                return IniFile.Read("InstallationServer", "FAILEDReceivers");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetMailReceivers_diskFull()
        {
            try
            {
                return IniFile.Read("InstallationServer", "DISKFULLReceivers");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetPreReleaseFolder()
        {
            try
            {
                return IniFile.Read("InstallationServer", "PreRelease");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetSideKickLeuvenServerPreReleaseFolder()
        {
            try
            {
                return IniFile.Read("SideKickInstallationServer", "LeuvenPreRelease");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }
        public string GetSideKickBrasovServerPreReleaseFolder()
        {
            try
            {
                return IniFile.Read("SideKickInstallationServer", "BrasovPreRelease");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }
        public string GetSideKickBredaServerPreReleaseFolder()
        {
            try
            {
                return IniFile.Read("SideKickInstallationServer", "BredaPreRelease");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }
        public string GetSideKickKaiServerPreReleaseFolder()
        {
            try
            {
                return IniFile.Read("SideKickInstallationServer", "KaiPreRelease");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }
        public string GetSideKickDegotServerPreReleaseFolder()
        {
            try
            {
                return IniFile.Read("SideKickInstallationServer", "DegotPreRelease");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }


        public string GetProductTemplate(string product)
        {
            try
            {
                return IniFile.Read(product.ToUpper(), "Template");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetProductPostfix(string product)
        {
            try
            {
                return IniFile.Read(product.ToUpper(), "Postfix");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetProductLocation(string product)
        {
            try
            {
                return IniFile.Read(product.ToUpper(), "Location");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetNecessaryFiles(string product)
        {
            try
            {
                return IniFile.Read(product.ToUpper(), "NecessaryFiles");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }


        public string[] GetAllReplicationServers() 
        {
            try
            {
                int nr = IniFile.ReadInt("ReplicationServers", "Number");
                int number;
                string[] servers = new string[nr];
                for (int i = 0; i < nr; i++)
                {
                    number = i + 1;
                    servers[i] = IniFile.Read("ReplicationServers", "Server" + number);
                }
                return servers;
            }
            catch 
            {
                return null;
            }
        }

        public string[] GetAllScheduledProducts(string serverName)
        {
            try
            {
                int nr = IniFile.ReadInt(serverName, "scheduledProductNumber");
                int number;
                string[] servers = new string[nr];
                for (int i = 0; i < nr; i++)
                {
                    number = i + 1;
                    servers[i] = IniFile.Read(serverName, "scheduledProduct" + number).ToUpper();
                }
                return servers;
            }
            catch 
            {
                return null;
            }
        }

        public string GetServerLocation(string serverName)
        {
            try
            {
                return IniFile.Read(serverName, "Location");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetServerTemplate(string serverName)
        {
            try
            {
                return IniFile.Read(serverName, "Template");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetServerBuildDisk(string serverName)
        {
            try
            {
                return IniFile.Read(serverName, "BuildDisk");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetServerReleasedFolder(string serverName)
        {
            try
            {
                return IniFile.Read(serverName, "Released");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }
        public string GetServerSlowDisk(string serverName)
        {
            try
            {
                return IniFile.Read(serverName, "SlowDisk");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetServerReleases(string serverName)
        {
            try
            {
                return IniFile.Read(serverName, "Releases");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        //Monday, Tuesday, Wednesday, Thursday, Friday or Weekend
        public string GetServerSchedule(string serverName, string day)
        {
            try
            {
                return IniFile.Read(serverName, day);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetMailClient(string serverName)
        {
            try
            {
                return IniFile.Read(serverName, "SmtpClient");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetMailSender(string serverName)
        {
            try
            {
                return IniFile.Read(serverName, "Sender");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetFaspexSender()
        {
            try
            {
                return IniFile.Read("faspex", "Sender");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetFaspexHost()
        {
            try
            {
                return IniFile.Read("faspex", "Host");
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public string GetMailReceivers(string serverName, string product)
        {
            try
            {
                return IniFile.Read(serverName, string.Concat(product.ToUpper(),"Receivers"));
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        //Disks for monitors
        [Obsolete("Value no longer in settings.ini")]
        public string GetBuildDisk(string serverName)
        {
            try
            {
                return IniFile.Read("BUILDDISK", serverName);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        [Obsolete("Value no longer in settings.ini")]
        public string GetSlowDisk(string serverName)
        {
            try
            {
                return IniFile.Read("SLOWDISK", serverName);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        [Obsolete("Value no longer in settings.ini")]
        public string GetTDMCVersion(string revision)
        {
            try
            {
                return IniFile.Read("TDMC", revision);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Obsolete("Value no longer in settings.ini")]
        public string GetSSDDisk(string serverName)
        {
            try
            {
                return IniFile.Read("SSDDISK", serverName);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }


        public string GetMKIILoaderGUID(string revision)
        {
            try
            {
                return IniFile.Read("GETMKIILOADERGUID", revision);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{MethodBase.GetCurrentMethod()} threw an exception", e);
            }
        }

        public bool HasToOverwrite(string serverName)
        {
            return IniFile.Read(serverName, "Overwrite").Equals("true");          
        }

    }
}
