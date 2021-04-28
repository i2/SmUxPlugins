using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PluginSystem.Configuration
{
    [Configuration("Ogólne")]
    [Serializable]
    public class PluginSystemConfiguration : ModuleConfiguration
    {
        [DisplayName("Wtyczki")]
        [Description("Lista modułów, z których są ładowane wtyczki")]
        [Category("Konfiguracja wtyczek")]
        [Browsable(false)]
        public string Plugins { get; set; }

        [DisplayName("Powiększanie listy kursów")]
        [Description("Określa, czy lista kursów powinna być powiększona tak, aby mieściło się więcej pozycji")]
        [Category("Lista kursów")]
        public bool ExtendCourseList { get; set; }

        [DisplayName("Dodatkowe informacje o kursach")]
        [Description("Określa, czy na liście kursów powinna być wyświetlana dodatkowa informacja")]
        [Category("Lista kursów")]
        public bool ShowAdditionalInformationAboutCourse { get; set; }

        public PluginSystemConfiguration()
        {
            ExtendCourseList = false;
            ShowAdditionalInformationAboutCourse = true;
        }
    }
}