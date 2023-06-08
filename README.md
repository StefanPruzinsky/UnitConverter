# UnitConverter
Aplikácia vytvorená pre súťaž "Machr na C# .NET - Vícejazyčná aplikace":http://www.itnetwork.cz/csharp/diskuzni-forum-c-sharp-visual-studio-net-xna/machr-na-c-net-vicejazycna-aplikace-5592e70f8dd10. Aplikácia je lokalizovaná do **51 svetových jazykov** pomocou "Yandex Translate API":https://tech.yandex.com/translate/. Je jednoducho rozšíriteľná o ďalšie veličiny, či jednotky. Taktiež slúži ako praktická ukážka objektového prístupu k Xml súborom - **LINQ to XML**.

Algoritmus výpočtu
##################
Algoritmus výpočtu je triviálny. Funguje na takomto princípe:
/--code
výsledok = hodnota * odchýlkaVstupnejJednotky / odchýlkaVýstupnejJednotky
\--

Príklad:
********
/--code html
<Unit offset="0,001">mg</Unit> <!-- Vstupná jednotka -->
<Unit offset="1000">Kg</Unit> <!-- Výstupná jednotka -->
\--
/--code
232 mg * 0,001 / 1000 = 0,000232 kg
\--


-------------------

*Odchýlka (offset) je odchýlka od základnej jednotky danej veličiny napr. 1kg = 1000g - g(gram) je v tomto prípade základná jednotka tzn. od nej sa odvodzujú vzťahy k ostatným jednotkám danej veličiny.*
Pridávanie veličín/jednotiek
##################
Pridávanie veličín sa uskutočňuje v súbore *quantities.xml*, ktorý sa nachádza v zložke zo samotnou aplikáciu. Princíp rozšírenia je jednoduchý: Medzi tagy `<Quantities></Quantities>` pridáme nový element `<Quantity>`, do ktorého vnoríme ľubovoľný počet elementov `Unit` (jednotka), pričom jeden z týchto elementov musí byť základná jednotka (`offset="1"`). Atribút `offset` definuje vzťah medzi atribútom `offset` základnej jednotky a ním samotným:
/--code
1 meter = 0,001 milimeter
\--
Príklad:
*******
/--code html
  <Quantity name="Length">
    <Unit offset="0,000000001">nm</Unit>
    <Unit offset="0,000001">μm</Unit>
    <Unit offset="0,001">mm</Unit>
    <Unit offset="0,01">cm</Unit>
    <Unit offset="0,1">dm</Unit>
    <Unit offset="1">m</Unit>
    <Unit offset="1000">Km</Unit>
    <Unit offset="1609,344">m(mile)</Unit>
  </Quantity>
\--

Použitie Yandex Translate API
#############################
Pre preklad aplikácie je použitá prekladacia API od spoločnosti Yandex. Túto cestu som si zvolil kvôli jednoduchosti použitia a taktiež kvôli tomu, že Microsoft Translator od Bing-u momentálne vstupuje do druhej verzie a žiadny web (ani StackOverflow :D ) nevie presne vysvetliť ako funguje.
Pre používanie tejto API je nutné získať tzv. *ApiKey*, ktorý je možné bezplatne získať na "tejto adrese":https://tech.yandex.com/keys/get/?service=trnsl.

Samotné použitie je realizované cez triedu *WebClient*, ktorá sa nachádza v .NET framework-u od verzie 4.5.
Tvar požiadavky je následovný:
/--code
https://translate.yandex.net/api/v1.5/tr/translate?key=ApiKey&lang=inputLanguage-outputLanguage&text=text
\--
Príklad:
********
/--code java
	    WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
	    string apiKey = "trnsl.1.1.xxxxxxxxxxxxxxx.xxxxxxxxxxx.xxxxxxxxxxxxxxxxxxxxxxxxxxxx";

            string request = "https://translate.yandex.net/api/v1.5/tr/translate?key=" + apiKey + "&lang=" + inputLanguage + "-" + outputLanguage + "&text=" + text;
            XDocument = XDocument.Parse(webClient.DownloadString(request));

            var query = from x in XDocument.Element("Translation").Elements("text")
                        select x.Value;

            string translatedText = query.ElementAt(0);
\--

V kóde, ktorý je na stiahnutie sa nachádza API kľúč, ktorý som použil pri vývoji tejto aplikácie. Je vám k dispozícii.
