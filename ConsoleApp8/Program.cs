using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ConsoleApp8
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = ConfigurationManager.GetSection("AdditionalSetting") as AdditionalSettingSection ?? new AdditionalSettingSection();
            
            for (var i =0; i < a.Settings.Count; i++)
            {
                var s = (IAdditionalSetting) a.Settings[i];
                Console.WriteLine(((IAdditionalSetting) a.Settings[i]).Name);
                
                for (var j = 0; j < s.hiddenTanks.Count; j++)
                {
                    Console.WriteLine(s.hiddenTanks[j].Settings);
                }
            }
        }
    }

    public interface IAdditionalSetting
    {
        string Name { get; }

        hiddenTanksCollection hiddenTanks { get; }


    }

    public interface IhiddenTank
    {
        string Name { get; }


    }

    public class AdditionalSetting : ConfigurationElement, IAdditionalSetting
    {
        const string NameMap = "name";
        
        [ConfigurationProperty(NameMap, IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base[NameMap];
            }
            set
            {
                base[NameMap] = value;
            }
        }

        const string tanksMap = "hiddenTanks";

        [ConfigurationProperty(tanksMap, IsKey = true, IsRequired = true)]
        public hiddenTanksCollection hiddenTanks
        {
            get { return ((hiddenTanksCollection)(base[tanksMap])); }
        }

    }

    public class hiddenTank : ConfigurationElement, IhiddenTank
    {
        const string NameMap = "name";

        [ConfigurationProperty(NameMap, IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base[NameMap];
            }
            set
            {
                base[NameMap] = value;
            }
        }

        

        const string tanksMap = "hiddenTanks";

        public hiddenTanksCollection hiddenTanks
        {
            get { return ((hiddenTanksCollection)(base[tanksMap])); }
        }
    }

    [ConfigurationCollection(typeof(AdditionalSetting), AddItemName = "setting")]
    public class IndusoftDataProviderCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AdditionalSetting();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AdditionalSetting)(element)).Name;
        }
        public AdditionalSetting this[int idx]
        {
            get { return (AdditionalSetting)BaseGet(idx); }
        }
        public static IEnumerable<IAdditionalSetting> GetDataProviders()
        {
            AdditionalSettingSection section = (AdditionalSettingSection)ConfigurationManager.GetSection("AdditionalSetting");
            return section?.Settings.Cast<IAdditionalSetting>().ToArray();
        }
    }

    
    [ConfigurationCollection(typeof(hiddenTank), AddItemName = "tank")]
    public class hiddenTanksCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new hiddenTank();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((hiddenTank)(element)).Name;
        }
        public HiddenTankSection this[int idx]
        {
            get { return (HiddenTankSection)BaseGet(idx); }
        }
        public static IEnumerable<IhiddenTank> GetDataProviders()
        {
            HiddenTankSection section = (HiddenTankSection)ConfigurationManager.GetSection("hiddenTanks");
            return section?.Settings.Cast<IhiddenTank>().ToArray();
        }
    }

    public class AdditionalSettingSection : ConfigurationSection
    {
        const string DataProvidersMap = "settings";
        [ConfigurationProperty(DataProvidersMap)]
        public IndusoftDataProviderCollection Settings
        {
            get { return ((IndusoftDataProviderCollection)(base[DataProvidersMap])); }
        }
    }

    public class HiddenTankSection : ConfigurationSection
    {
        const string hiddenTanksMap = "hiddenTanks";
        [ConfigurationProperty(hiddenTanksMap)]
        public hiddenTanksCollection Settings
        {
            get { return ((hiddenTanksCollection)(base[hiddenTanksMap])); }
        }
    }
}
