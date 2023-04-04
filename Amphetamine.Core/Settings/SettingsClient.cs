using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Timers;
using System.Xml.Linq;
using Amphetamine.Core.IO;
using Timer = System.Timers.Timer;

namespace Amphetamine.Core.Settings
{
    public class SettingsClient
  {
    private static SettingsClient instance;
    private Timer timer;
    private object timerMutex = new object();
    private bool delayWrite;
    private string baseSettingsFile = Path.Combine(ProcessExecutable.ExecutionFolder(), "BaseSettings.xml");
    private XDocument baseSettingsDoc;
    private string applicationFolder;
    private string settingsFile;
    private XDocument settingsDoc;

    private SettingsClient()
    {
      this.timer = new Timer(100.0);
      this.timer.Elapsed += new ElapsedEventHandler(this.Timer_Elapsed);
      this.baseSettingsDoc = XDocument.Load(this.baseSettingsFile);
      if (this.applicationFolder == null)
      {
        bool flag = false;
        this.applicationFolder = !this.TryGetBaseValue<bool>("Configuration", "IsPortable", ref flag) ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ProcessExecutable.Name()) : (!flag ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ProcessExecutable.Name()) : Path.Combine(ProcessExecutable.ExecutionFolder(), ProcessExecutable.Name()));
        this.TryCreateApplicationFolder();
      }
      this.settingsFile = Path.Combine(this.applicationFolder, "Settings.xml");
      if (!File.Exists(this.settingsFile))
        File.Copy(this.baseSettingsFile, this.settingsFile, true);
      this.LoadSettings();
    }

    public static bool IsMigrationNeeded() => SettingsClient.Instance.CheckSettingsVersion() != 0;

    public static void Migrate()
    {
      if (SettingsClient.Instance.CheckSettingsVersion() == 0)
        return;
      if (SettingsClient.Instance.CheckSettingsVersion() == 1)
      {
        SettingsClient.Instance.UpgradeSettings();
      }
      else
      {
        if (SettingsClient.Instance.CheckSettingsVersion() != -1)
          return;
        SettingsClient.Instance.DowngradeSettings();
      }
    }

    private void LoadSettings()
    {
      try
      {
        this.settingsDoc = XDocument.Load(this.settingsFile);
      }
      catch (Exception ex)
      {
        File.Copy(this.baseSettingsFile, this.settingsFile, true);
        this.settingsDoc = XDocument.Load(this.settingsFile);
      }
    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      lock (this.timerMutex)
      {
        if (this.delayWrite)
        {
          this.delayWrite = false;
        }
        else
        {
          this.timer.Stop();
          this.settingsDoc.Save(this.settingsFile);
        }
      }
    }

    public static SettingsClient Instance
    {
      get
      {
        if (SettingsClient.instance == null)
          SettingsClient.instance = new SettingsClient();
        return SettingsClient.instance;
      }
    }

    private bool SettingExists<T>(string settingNamespace, string settingName) => (object) this.GetValue<T>(settingNamespace, settingName) != null;

    private void TryCreateApplicationFolder()
    {
      if (Directory.Exists(this.applicationFolder))
        return;
      Directory.CreateDirectory(this.applicationFolder);
    }

    private void QueueWrite()
    {
      lock (this.timerMutex)
      {
        if (!this.timer.Enabled)
          this.timer.Start();
        else
          this.delayWrite = true;
      }
    }

    private void WriteToFile()
    {
      this.timer.Stop();
      this.delayWrite = false;
      this.settingsDoc.Save(this.settingsFile);
    }

    private int CheckSettingsVersion()
    {
      this.LoadSettings();
      int num = 0;
      int baseValue = this.GetBaseValue<int>("Configuration", "Version");
      try
      {
        num = this.GetValue<int>("Configuration", "Version");
      }
      catch (Exception ex)
      {
      }
      if (num == baseValue)
        return 0;
      return num < baseValue ? 1 : -1;
    }

    private void DowngradeSettings()
    {
      File.Copy(this.baseSettingsFile, this.settingsFile, true);
      this.LoadSettings();
    }

    private void UpgradeSettings()
    {
      List<SettingEntry> list = this.settingsDoc.Element((XName) "Settings").Elements((XName) "Namespace").SelectMany((Func<XElement, IEnumerable<XElement>>) (n => n.Elements((XName) "Setting")), (n, s) => new
      {
        n = n,
        s = s
      }).SelectMany(_param1 => _param1.s.Elements((XName) "Value"), (_param1, v) => new SettingEntry()
      {
        Namespace = _param1.n.Attribute((XName) "Name").Value,
        Name = _param1.s.Attribute((XName) "Name").Value,
        Value = (object) v.Value
      }).ToList<SettingEntry>();
      File.Copy(this.baseSettingsFile, this.settingsFile, true);
      this.settingsDoc = XDocument.Load(this.settingsFile);
      foreach (SettingEntry settingEntry in list)
      {
        try
        {
          if (settingEntry.Namespace != "Configuration")
          {
            if (this.SettingExists<string>(settingEntry.Namespace, settingEntry.Name))
              SettingsClient.Set<string>(settingEntry.Namespace, settingEntry.Name, settingEntry.Value.ToString());
          }
        }
        catch (Exception ex)
        {
        }
      }
    }

    private T GetBaseValue<T>(string settingNamespace, string settingName)
    {
      lock (this.baseSettingsDoc)
      {
        XElement xelement = this.baseSettingsDoc.Element((XName) "Settings").Elements((XName) "Namespace").SelectMany((Func<XElement, IEnumerable<XElement>>) (n => n.Elements((XName) "Setting")), (n, s) => new
        {
          n = n,
          s = s
        }).SelectMany(_param1 => _param1.s.Elements((XName) "Value"), (_param1, v) => new
        {
          __TransparentIdentifier0 = _param1,
          v = v
        }).Where(_param1 => _param1.__TransparentIdentifier0.n.Attribute((XName) "Name").Value.Equals(settingNamespace) && _param1.__TransparentIdentifier0.s.Attribute((XName) "Name").Value.Equals(settingName)).Select(_param1 => _param1.v).FirstOrDefault<XElement>();
        if (typeof (T) == typeof (float))
        {
          float result;
          float.TryParse(xelement.Value, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
          return (T) Convert.ChangeType((object) result, typeof (T));
        }
        if (!(typeof (T) == typeof (double)))
          return (T) Convert.ChangeType((object) xelement.Value, typeof (T));
        float result1;
        float.TryParse(xelement.Value, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result1);
        return (T) Convert.ChangeType((object) result1, typeof (T));
      }
    }

    private bool TryGetBaseValue<T>(string settingNamespace, string settingName, ref T value)
    {
      value = this.GetBaseValue<T>(settingNamespace, settingName);
      return (object) value != null;
    }

    private void SetValue<T>(string settingNamespace, string settingName, T value)
    {
      lock (this.settingsDoc)
      {
        this.settingsDoc.Element((XName) "Settings").Elements((XName) "Namespace").SelectMany((Func<XElement, IEnumerable<XElement>>) (n => n.Elements((XName) "Setting")), (n, s) => new
        {
          n = n,
          s = s
        }).SelectMany(_param1 => _param1.s.Elements((XName) "Value"), (_param1, v) => new
        {
          __TransparentIdentifier0 = _param1,
          v = v
        }).Where(_param1 => _param1.__TransparentIdentifier0.n.Attribute((XName) "Name").Value.Equals(settingNamespace) && _param1.__TransparentIdentifier0.s.Attribute((XName) "Name").Value.Equals(settingName)).Select(_param1 => _param1.v).FirstOrDefault<XElement>()?.SetValue((object) value);
        this.QueueWrite();
      }
    }

    private T GetValue<T>(string settingNamespace, string settingName)
    {
      lock (this.settingsDoc)
      {
        XElement xelement = this.settingsDoc.Element((XName) "Settings").Elements((XName) "Namespace").SelectMany((Func<XElement, IEnumerable<XElement>>) (n => n.Elements((XName) "Setting")), (n, s) => new
        {
          n = n,
          s = s
        }).SelectMany(_param1 => _param1.s.Elements((XName) "Value"), (_param1, v) => new
        {
          __TransparentIdentifier0 = _param1,
          v = v
        }).Where(_param1 => _param1.__TransparentIdentifier0.n.Attribute((XName) "Name").Value.Equals(settingNamespace) && _param1.__TransparentIdentifier0.s.Attribute((XName) "Name").Value.Equals(settingName)).Select(_param1 => _param1.v).FirstOrDefault<XElement>();
        if (typeof (T) == typeof (float))
        {
          float result;
          float.TryParse(xelement.Value, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
          return (T) Convert.ChangeType((object) result, typeof (T));
        }
        if (!(typeof (T) == typeof (double)))
          return (T) Convert.ChangeType((object) xelement.Value, typeof (T));
        float result1;
        float.TryParse(xelement.Value, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result1);
        return (T) Convert.ChangeType((object) result1, typeof (T));
      }
    }

    private void OnTimerElapsedEvent(object sender, ElapsedEventArgs e)
    {
      lock (this.timerMutex)
      {
        if (this.delayWrite)
        {
          this.delayWrite = false;
        }
        else
        {
          this.timer.Stop();
          this.settingsDoc.Save(this.settingsFile);
        }
      }
    }

    public static string ApplicationFolder() => SettingsClient.Instance.applicationFolder;

    public static void Write() => SettingsClient.Instance.WriteToFile();

    public static void Set<T>(string @namespace, string name, T value, bool raiseEvent = false)
    {
      SettingsClient.Instance.SetValue<T>(@namespace, name, value);
      if (!raiseEvent)
        return;
      SettingsClient.SettingChanged((object) SettingsClient.Instance, new SettingChangedEventArgs()
      {
        Entry = new SettingEntry()
        {
          Namespace = @namespace,
          Name = name,
          Value = (object) value
        }
      });
    }

    public static T Get<T>(string settingNamespace, string settingName) => SettingsClient.Instance.GetValue<T>(settingNamespace, settingName);

    public static T BaseGet<T>(string settingNamespace, string settingName) => SettingsClient.Instance.GetBaseValue<T>(settingNamespace, settingName);

    public static bool IsSettingChanged(SettingChangedEventArgs e, string @namespace, string name) => e.Entry != null && e.Entry.Namespace.Equals(@namespace) && e.Entry.Name.Equals(name);

    public static event SettingChangedEventHandler SettingChanged = (_param1, _param2) => { };
  }
}