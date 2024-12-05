# Webprogramozás 2023-2024-1 HF 1

A megoldásokat küldjétek a babuafi@inf.elte.hu e-mail címre.

## 1. feladat (hf1_1) (1 pont)

Adott egy tömb könyvek adataival.

Az egyes könyvekről a következő adatokat tároljuk:
- title: A könyv címe.
- author: A könyv szerzője.
- release: A könyv megjelenésének éve.
- inStock: A könyv van-e raktáron.

Az alábbi alfeladatok eredményét írasd ki a console-ra.

1.1 **Keresd meg a 2010 és 2020 között megjelent könyveket. (A 2010-ben és 2020-ban megjelent könyveket is vedd bele!)**

1.2 **Keresd meg azt a könyvet, amelyet "Scott Lynch" írt.
      Csak a könyv címét írasd ki console-ra.
      Építs arra, hogy csak egy ilyen van a tömbben.**

1.3 **Készíts egy egyedi listát a könyvek szerzőiről.
      Tehát az eredményként kiadott tömbben a szerzők nevei szerepeljenek, azonban minden szerző csak egyszer,
      akkor is ha több könyvnek is Ő a szerzője az eredeti tömbben.**

    Segítség:

    Az egyediség eléréséhez használhatod a Set típust.

1.4 **Döntsd el, hogy van-e olyan könyv könyv 1985 után jelent-e meg.**

1.5 **Keresd meg a három legrégebben megjelent könyvet.**


## 2. feladat (hf1_2) (1 pont)

2.1 **Készíts egy űrlapot 2 mezővel: négyzetek száma (szám típusú), kezdő szín (választó típusú két opcióval: fekete, fehér).**

2.2 **Az űrlap alá kerüljön egy gomb, amellyel az űrlap ellenőrizhető és submitolható.**
Az űrlapot JavaScriptből validáld (NE a HTML5-ös validációk használatával).
Szükséges validációk:
- A négyzetek száma nem üres.
- Számot tartalmaz.
- Értéke kisebb mint 50, de nagyobb mint 0.

2.3 **Validációs hiba esetén az egész oldal háttere változzon pirosra. Ha később már egy valid űrlap kerül beküldésre, akkor ez a háttér váltson vissza fehérre.**

2.4 **Amennyiben az űrlap valid beküldéskor, szúrd be a megadott számú négyzetet.**

A beszúrt négyzetek színe váltakozzon, az első négyzet színe legyen a kiválasztott színű.
Tehát ha például a felhasználó "4"-t ad meg a négyzetek számánál és a "fekete" színt választja akkor 4 négyzet jelenne meg a felületen, amelyek sorban fekete, fehér, fekete, fehér színűek.

      Segítség:

      A négyzetek valamint a színek beállítására definiálj stílusokat egy CSS fájlban.

2.5 **Újbóli valid beküldés esetén a korábbi négyzetek tűnjenek el.**
Tehát ürítsd a konténert.
