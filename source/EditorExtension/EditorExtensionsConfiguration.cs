using System;
using System.Collections.Generic;
using EditorExtension.AutoFormat;
using EditorExtension.Styles;
using PluginSystem.Configuration;
using System.ComponentModel;

namespace EditorExtension
{
    [Configuration("Rozszerzenia edytora")]
    public class EditorExtensionsConfiguration : ModuleConfiguration
    {
        [DisplayName("Style")]
        [Description("Kolekcja styli formatowań, które można szybko zastosować do zaznaczonego tekstu")]
        [Category("Style")]
        public StyleCollection Styles
        {
            get; set;
        }

        [DisplayName("Symbole IPA")]
        [Description("Kolekcja symboli IPA do szybkiego wstawienia")]
        [Category("Tabela symboli IPA")]
        public IPASymbolCollection IPASymbols
        {
            get; set;
        }

        [DisplayName("Reguły formatowania")]
        [Description("Kolekcja reguł formatowania automaycznego")]
        [Category("Formatowanie automatyczne")]
        public AutoFormatRuleCollection AutoFormatRules { get; set; }

        public EditorExtensionsConfiguration()
        {
            IPASymbols = new IPASymbolCollection();
            Styles = new StyleCollection();
            AutoFormatRules = new AutoFormatRuleCollection();
        }

        public override void CreateDefaults()
        {
            IPASymbols.AddRange(new List<IPASymbol>
                       {
                           new IPASymbol("ʌ", "cup /kʌp/, luck /lʌk/", "vowel"),
                           new IPASymbol("ɑ:", "arm /ɑ:(r)m/, father /ˈfɑ:ðə(r)/", "vowel"),
                           new IPASymbol("æ", "cat /kæt/, black /blæk/", "vowel"),
                           new IPASymbol("e", "met /met/, bed /bed/", "vowel"),
                           new IPASymbol("ə", "away /əˈweɪ/, cinema /ˈsɪnəmə/", "vowel"),
                           new IPASymbol("ɜ:(r)", "turn /tɜ:(r)n/, learn /lɜ:(r)n/", "vowel"),
                           new IPASymbol("ɪ", "hit /hɪt/, sitting /ˈsɪtɪŋ/", "vowel"),
                           new IPASymbol("i:", "see /si:/, heat /hi:t/", "vowel"),
                           new IPASymbol("ɒ", "hot /hɒt/, rock /rɒk/", "vowel"),
                           new IPASymbol("ɔ:", "call /kɔ:l/, four /fɔ:(r)/", "vowel"),
                           new IPASymbol("ʊ", "put /pʊt/, could /kʊd/", "vowel"),
                           new IPASymbol("u:", "blue /blu:/, food /fu:d/", "vowel"),
                           new IPASymbol("aɪ", "five /faɪv/, eye /aɪ/", "vowel"),
                           new IPASymbol("aʊ", "now /naʊ/, out /aʊt/", "vowel"),
                           new IPASymbol("əʊ", "go /ɡəʊ/, home /həʊm/", "vowel"),
                           new IPASymbol("eə(r)", "where /weə(r)/, air /eə(r)/", "vowel"),
                           new IPASymbol("eɪ", "say /seɪ/, eight /eɪt/", "vowel"),
                           new IPASymbol("ɪə(r)", "near /nɪə(r)/, here /hɪə(r)/", "vowel"),
                           new IPASymbol("ɔɪ", "boy /bɔɪ/, join /dʒɔɪn/", "vowel"),
                           new IPASymbol("ʊə(r)", "pure /pjʊə(r)/, tourist /ˈtʊərɪst/", "vowel"),
                           new IPASymbol("b", "bad /bæd/, lab /læb/", "consonant"),
                           new IPASymbol("d", "did /dɪd/, lady /ˈleɪdi/", "consonant"),
                           new IPASymbol("f", "find /faɪnd/, if /ɪf/", "consonant"),
                           new IPASymbol("g", "give /ɡɪv/, flag /flæɡ/", "consonant"),
                           new IPASymbol("h", "how /haʊ/, hello /həˈləʊ/", "consonant"),
                           new IPASymbol("j", "yes /jes/, yellow /ˈjeləʊ/", "consonant"),
                           new IPASymbol("k", "cat /kæt/, back /bæk/", "consonant"),
                           new IPASymbol("l", "leg /leɡ/, little /ˈlɪt(ə)l/", "consonant"),
                           new IPASymbol("m", "man /mæn/, lemon /ˈlemən/", "consonant"),
                           new IPASymbol("n", "no /nəʊ/, ten /ten/", "consonant"),
                           new IPASymbol("ŋ", "sing /sɪŋ/, finger /ˈfɪŋɡə(r)/", "consonant"),
                           new IPASymbol("p", "pet /pet/, map /mæp/", "consonant"),
                           new IPASymbol("r", "red /red/, try /traɪ/", "consonant"),
                           new IPASymbol("s", "sun /sʌn/, miss /mɪs/", "consonant"),
                           new IPASymbol("ʃ", "she /ʃi:/, crash /kræʃ/", "consonant"),
                           new IPASymbol("t", "tea /ti:/, getting ???", "consonant"),
                           new IPASymbol("tʃ", "check /tʃek/, church /tʃɜ:(r)tʃ/ ", "consonant"),
                           new IPASymbol("θ", "think /θɪŋk/, both /bəʊθ/", "consonant"),
                           new IPASymbol("ð", "this /ðɪs/, mother /ˈmʌðə(r)/", "consonant"),
                           new IPASymbol("v", "voice /vɔɪs/, five /faɪv/", "consonant"),
                           new IPASymbol("w", "wet /wet/, window /ˈwɪndəʊ/ ", "consonant"),
                           new IPASymbol("z", "zoo /zu:/, lazy /ˈleɪzi/", "consonant"),
                           new IPASymbol("ʒ", "pleasure /ˈpleʒə(r)/, vision /ˈvɪʒ(ə)n/", "consonant"),
                           new IPASymbol("dʒ", "just /dʒʌst/, large /lɑ:(r)dʒ/", "consonant"),
                       });

            Styles.Add(new Style { Name = "Ostrzeżenie", Color = "#FF0000" });
            Styles.Add(new Style { Name = "Liczba odpowiedzi", Color = "#E0A000" });
            Styles.Add(new Style { Name = "Wymowa", Color = "#008080", FontStyle = "italic" });

            AutoFormatRules.AddRange(new List<AutoFormatRule>
                                      {
                                          new AutoFormatRule
                                              {
                                                  Active = true,
                                                  ItemElement = ItemTextElements.Question,
                                                  Name = "Liczba odpowiedzi",
                                                  SearchText = @"\([0-9]*\)",
                                                  StyleToApply = "Liczba odpowiedzi",
                                                  UseRegularExpressions = true
                                              },
                                          new AutoFormatRule
                                              {
                                                  Active = true,
                                                  ItemElement = ItemTextElements.Answer | ItemTextElements.Question,
                                                  Name = "Wymowa",
                                                  SearchText = @"/['ˈʌɑ:æeəɜ:rɪi:ɒɔ:ʊu:aɪıaʊəʊeəreɪɪərɔɪʊərbdfghjklmnŋprsʃttʃθðvwzʒdʒ]+/",
                                                  StyleToApply = "Wymowa",
                                                  UseRegularExpressions = true
                                              },
                                          new AutoFormatRule
                                              {
                                                  Active = true,
                                                  ItemElement = ItemTextElements.Question,
                                                  Name = "Ostrzeżenie",
                                                  SearchText = @"\(not [a-zA-Z ]+\)",
                                                  StyleToApply = "Ostrzeżenie",
                                                  UseRegularExpressions = true
                                              },
                                          new AutoFormatRule
                                              {
                                                  Active = true,
                                                  ItemElement = ItemTextElements.Question | ItemTextElements.Answer,
                                                  Name = "Flaga brytyjska",
                                                  SearchText = @"(BrE)",
                                                  ReplaceText = @"<gfx item-id=""464"" file=""f"" />",
                                                  UseRegularExpressions = false
                                              },
                                          new AutoFormatRule
                                              {
                                                  Active = true,
                                                  ItemElement = ItemTextElements.Question | ItemTextElements.Answer,
                                                  Name = "Flaga amerykańska",
                                                  SearchText = @"(AmE)",
                                                  ReplaceText = @"<gfx item-id=""464"" file=""e"" />",
                                                  UseRegularExpressions = false
                                              },
                                      });
        }
    }
}