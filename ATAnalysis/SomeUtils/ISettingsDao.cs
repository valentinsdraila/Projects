using PmsSettings.Util;

namespace PmsSettings
{
    public interface ISettingsDao
    {
        IIniFile IniFile { get; }
        string GetContentForSectionEntry(string section, string entry);
        string GetWebServer();
        string GetFileServer();
        string GetDepotsServer();
        string GetSQLServer();
        string GetLDAPServer();
        string GetBuildServer();
        string GetLicenseServer();
        string GetPortalDB();
        string GetAutomatedTestingDB();
        int GetATFManagerTimeout();
        int GetATFManagerSleepTime();
        int GetATFManagerCleanupTime();
        int GetATFManagerJobTimeout();
        int GetATFExplorerTimeout();
        int GetATFExplorerSleepTime();
        string GetATFManagerReceivers();
        int GetATManagerMaxAttempts();
        string GetATFManagerMail();
        string GetATFExplorerInstallation();
        string GetATFExplorerDataServerParent();
        string GetATFExplorerLocalAutomation();
        string GetATFExplorerSCMAutomTestingRootParent();
        string GetATFManagerLogfile();
        string GetBuildQualityLogPath();
        string GetSmartTestingCentralPath();
        string GetTecwareLocation(string revision);
        string GetTecwareRegKey(string revision);
        string GetSQLUser();
        string GetSQLPwd();
        string GetProcessesToKill();
        string GetINIFilename();
        string GetCentralLogDB();
        string GetPortalDBConnectionString();
        int GetAtEarlyStartInHours();
        string GetInstallationServerName();
        string GetServerLocation();
        string GetServerTemplate();
        string GetServerReleasedFolder();
        string GetSideKickLeuvenServerReleasedFolder();
        string GetSideKickBrasovServerReleasedFolder();
        string GetSideKickBredaServerReleasedFolder();
        string GetSideKickKaiServerReleasedFolder();
        string GetSideKickDegotServerReleasedFolder();
        string GetMailClient();
        string GetMailSender();
        string GetMailReceivers_installationFailed();
        string GetMailReceivers_diskFull();
        string GetPreReleaseFolder();
        string GetSideKickLeuvenServerPreReleaseFolder();
        string GetSideKickBrasovServerPreReleaseFolder();
        string GetSideKickBredaServerPreReleaseFolder();
        string GetSideKickKaiServerPreReleaseFolder();
        string GetSideKickDegotServerPreReleaseFolder();
        string GetProductTemplate(string product);
        string GetProductPostfix(string product);
        string GetProductLocation(string product);
        string GetNecessaryFiles(string product);
        string[] GetAllReplicationServers();
        string[] GetAllScheduledProducts(string serverName);
        string GetServerLocation(string serverName);
        string GetServerTemplate(string serverName);
        string GetServerBuildDisk(string serverName);
        string GetServerReleasedFolder(string serverName);
        string GetServerSlowDisk(string serverName);
        string GetServerReleases(string serverName);
        string GetServerSchedule(string serverName, string day);
        string GetMailClient(string serverName);
        string GetMailSender(string serverName);
        string GetFaspexSender();
        string GetFaspexHost();
        string GetMailReceivers(string serverName, string product);
        string GetBuildDisk(string serverName);
        string GetSlowDisk(string serverName);
        string GetTDMCVersion(string revision);
        string GetSSDDisk(string serverName);
        string GetMKIILoaderGUID(string revision);
        bool HasToOverwrite(string serverName);
    }
}