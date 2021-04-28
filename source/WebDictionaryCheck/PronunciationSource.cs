using System;
using System.ComponentModel;

namespace WebDictionaries
{
    [Serializable]
    public class PronunciationSource
    {
        [DisplayName("Nazwa źródła")]
        public string Name { get; set; }
        
        [DisplayName("Symulowana przeglądarka")]
        public string UserAgent { get; set; }

        [DisplayName("Czas oczekiwania")]
        public int TimeOut { get; set; }

        [DisplayName("Adres strony do pobrania")]
        public string Command { get; set; }

        [DisplayName("Źródło dźwięku")]
        public string Site { get; set; }

        [DisplayName("Znacznik przed dźwiękiem")]
        public string TextBeforeSoundSource { get; set; }

        [DisplayName("Znacznik po dźwięku")]
        public string TextAfterSoundSource { get; set; }
    }
}