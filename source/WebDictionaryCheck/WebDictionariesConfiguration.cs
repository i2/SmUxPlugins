using System.ComponentModel;
using PluginSystem.Configuration;
using System;

namespace WebDictionaries
{
    [Configuration("Słowniki")]
    [Serializable]
    public class WebDictionariesConfiguration : ModuleConfiguration
    {
        [DisplayName("Słowniki zewnętrzne")]
        [Description("Definicje słowników zewnętrznych, za pomocą których można szybko sprawdzić zaznaczony tekst")]
        [Category("Słowniki zewnętrzne")]
        public DictionaryDefinitionCollection Dictionaries { get; set; }

        [DisplayName("Odtwarzaj dźwięk po ściągnięciu")]
        [Description("Czy odtwarzać ściągnięty dźwięk w odtwarzaczu")]
        [Category("Inne")]
        public bool PlayDownloadedSoundInPlayer
        {
            get; set;
        }

        [DisplayName("Źródła dźwięku")]
        [Description("Definicje źródeł dźwięku zewnętrznych")]
        [Category("Dźwięk")]
        public PronunciationSourceCollection PronunciationSources { get; set; }

        [DisplayName("Domyślne źródło dźwięku")]
        [Description("Nr źródła dźwięku z kolekcji źródeł, które będzie pobierane domyślnie")]
        [Category("Dźwięk")]
        public int DefaultPronunciationSource { get; set; }

        [DisplayName("Styl dla transkrypcji")]
        [Description("Nazwa stylu, który powinien być zastosowany dla pobranej transkrypcji")]
        [Category("IPA")]
        public string PronunciationStyle { get; set; }

        [DisplayName("Uprość zapis IPA")]
        [Description("Określa, czy pobrany zapis IPA powinien zostać uproszczony do znaków podstawowych")]
        [Category("IPA")]
        public bool SimplifyIPA { get; set; }

        public WebDictionariesConfiguration()
        {
            Dictionaries = new DictionaryDefinitionCollection();
            PronunciationSources = new PronunciationSourceCollection();
        }

        public override void CreateDefaults()
        {
            SimplifyIPA = true;
            PronunciationStyle = "Wymowa";
            PlayDownloadedSoundInPlayer = true;
            DefaultPronunciationSource = 0;

            PronunciationSources.Add(
                new PronunciationSource()
                    {
                        Name = "Angielski (Brytyjska)",
                        UserAgent = "Mozilla",
                        TimeOut = 8000,
                        Site = "http://www.macmillandictionary.com",
                        Command = "http://www.macmillandictionary.com/dictionary/british/{0}",
                        TextBeforeSoundSource = @"<img onclick=""playSoundFromFlash('",
                        TextAfterSoundSource = @"', this)"" style=""cursor: pointer"" title=",
                    });

            PronunciationSources.Add(
                new PronunciationSource()
                    {
                        Name = "Angielski (Amerykańska)",
                        UserAgent = "Mozilla",
                        TimeOut = 8000,
                        Site = "http://www.macmillandictionary.com",
                        Command = "http://www.macmillandictionary.com/dictionary/american/{0}",
                        TextBeforeSoundSource = @"<img onclick=""playSoundFromFlash('",
                        TextAfterSoundSource = @"', this)"" style=""cursor: pointer"" title=",
                    });

            PronunciationSources.Add(
                new PronunciationSource()
                    {
                        Name = "Hiszpański",
                        UserAgent = "Mozilla",
                        TimeOut = 8000,
                        Site = "",
                        Command = "http://www.wordreference.com/es/en/translation.asp?spen={0}",
                        TextBeforeSoundSource = @"<a title='Spain pronunciation' href='' onClick='DHTMLSound(""",
                        TextAfterSoundSource = @"""); return false'>",
                    });

            Dictionaries.Add(new DictionaryDefinition
                                 {
                                     Name = @"MacMillan",
                                     Command = @"http://www.macmillandictionary.com/dictionary/british/{0}"
                                 });
            Dictionaries.Add(new DictionaryDefinition
                                 {
                                     Name = @"Longman", 
                                     Command = @"http://www.ldoceonline.com/search/?q={0}"
                                 });
            Dictionaries.Add(new DictionaryDefinition
                                 {
                                     Name = @"TheFreeDictionary",
                                     Command = @"http://www.thefreedictionary.com/dict.asp?Word={0}"
                                 });
            Dictionaries.Add(new DictionaryDefinition
                                 {
                                     Name = @"HowJSay",
                                     Command = @"http://www.howjsay.com/index.php?word={0}&submit=Submit"
                                 });
            Dictionaries.Add(new DictionaryDefinition
                                 {
                                     Name = @"Cambridge",
                                     Command = @"http://dictionary.cambridge.org/results.asp?searchword={0}&x=0&y=0"
                                 });
            Dictionaries.Add(new DictionaryDefinition
                                 {
                                     Name = @"Dictionary.com", 
                                     Command = @"http://dictionary.reference.com/browse/{0}"
                                 });
        }
    }
}
