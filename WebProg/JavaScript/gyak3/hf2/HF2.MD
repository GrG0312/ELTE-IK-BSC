# Webprogramozás 2023-2024-1 HF 2

A megoldásokat küldjétek a babuafi@inf.elte.hu e-mail címre.

## Feladat (1 pont)

A felületen találsz két választó mezőt valamint két "Adatok lekérése" gombot.

A feladat lényege, hogy gombnyomásra jelenjenek meg a kiválasztott könyv adatai vagy pedig a kiválasztott szerzőhöz tartozó könyv(ek)
adatai táblázatos formátumban.

2.1 **Az "Adatok lekérése" gomb megnyomásakor az előző feladat során is használt "books" tömbből keresd ki a kiválasztott
      könyve(ke)t cím vagy szerző alapján és írasd ki konzolra.**

2.2 **A "resultsContainer" azonosítójú elembe generálj egy táblázatot a könyv(ek) adataival.**
A táblázatnak legyen fejléce a következő mezőkkel: "Cím", "Szerző", "Megjelenés".
A táblázat sorai a könyv(ek) adatait tartalmazzák.

2.3 **A táblázat sorai legyenek kiválaszthatóak, egy adott sorra kattintva kapjon a sor egy enyhe szürke hátteret.**
Egyszerre csak egy sor legyen kiválasztható, azaz a korábbi kiválasztást távolítsd el amikor egy másik sorra kattint a felhasználó.

2.4 **A kiválasztás legyen visszavonható.**
Azaz, ha a felhasználó újra a már kiválasztott sorra kattint, akkor a szürke háttér tűnjön el.

2.5 **Amennyiben van kiválasztott sor, a benne foglalt adatokat írasd ki a "selectedBook" azonosítójú elembe.**
Használd a következő formátumot: `${author}: ${title} (${release})`.
Amikor a kiválasztás megszűnik akkor ez a kiíratás is tűnjön el.

2.6 **Találsz egy "openWebshop" azonosítójú rejtett linket a HTML fájlban.
    A link a "display: none" stílusbeállítással került elrejtésre.
    A táblázat megjelenítésekor tedd láthatóvá ezt a linket a stílusbeállítás megváltoztatásával.
    Például "display: block" stílussal megfelelően fog működni.**

2.7 **Az előző feladatban megjelenített link az egyik nagyobb könyváruház oldalára mutat.**

Amennyiben az éppen kiválasztott könyv nem elérhető a webshopban (az "inStock" mező értéke false),
akkor a linkre kattintva ne nyíljon meg az oldal.

      Segítség:

      **Akadályozd meg** a link **alap** működését.