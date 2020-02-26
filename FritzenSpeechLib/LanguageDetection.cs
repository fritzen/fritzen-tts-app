using NTextCat;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace FritzenSpeech
{
    public class LanguageDetection
    {
        private readonly RankedLanguageIdentifierFactory factory = new RankedLanguageIdentifierFactory();
        private readonly RankedLanguageIdentifier identifier;

        public LanguageDetection()
        {
            string basepath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            identifier = factory.Load(basepath + "\\LanguageModels\\Core14.profile.xml");
        }

        public string Detect(string text)
        {
            IEnumerable<Tuple<LanguageInfo, double>> languages = identifier.Identify(text);
            Tuple<LanguageInfo, double> mostCertainLanguage = languages.FirstOrDefault();
            CultureInfo culture = CultureInfo.GetCultures(CultureTypes.NeutralCultures).Where(
                                                        ci => string.Equals(ci.ThreeLetterISOLanguageName, mostCertainLanguage.Item1.Iso639_3,
                                                        StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (culture != null)
            {
                Console.WriteLine("The language of the text is '{0}' (ISO639-3  code)", mostCertainLanguage.Item1.Iso639_3);
                return culture.Name;
            }
            else
            {
                Console.WriteLine("The language couldn’t be identified with an acceptable degree of certainty");
                return "";
            }

        }

    }
}