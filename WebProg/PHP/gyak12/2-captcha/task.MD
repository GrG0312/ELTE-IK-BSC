## 2. feladat: Captcha (2-captcha, 8 pont)

Napjainkban számos megoldás létezik (talán a mesterséges intelligencia feltörekvésének idejében nem is alaptalanul) annak ellenőrzésére, hogy a számítógép előtt ember vagy robot ül. Egyik ilyen módszer, hogy a felhasználónak egy kirakós egy darabját kell a megfelelő helyre elhelyeznie egy képen. Ebben a feladatban éppen egy ilyen captchát fogunk elkészíteni! A kiinduló csomagban található háttérkép szélessége 600 pixel, magassága 338 pixel. Ebből kiemelünk egy `100 * 60` méretű részletet, amelyet a felhasználónak két csúszka segítségével fedésbe kell hoznia az eredeti képpel.

- a. (1 pont) Generálj egy véletlen X és Y koordinátát a képen belül, amely a kiemelt részlet bal felső sarka lesz! Írd ki a konzolra a generált koordinátákat! (pl. `Target: X = 143, Y = 30`)
  - Tipp: Mivel bal felső sarkot választunk a rögzített méretű részlethez, ezért célszerű a koordinátákat úgy megválasztani, mintha a kép `500 * 278` pixel lenne, hogy véletlenül se lógjon ki a kivágás a kép határain túlra.
- b. (1 pont) Told el a `piece` azonosítójú `div` elem háttérképét negatív irányba a generált koordinátákkal a `background-position` stílustulajdonság beállításával!
  - Példa: ha az előzőleg generált pont `(143, 30)`, akkor legyen `background-position: -143px -30px`!
- c. (1 pont) A `pieceX` és `pieceY` azonosítójú csúszkák mozgatásakor írd ki a konzolra mindkét csúszka jelenlegi értékét! (pl. `Current: X = 210, Y = 120`)
- d. (1 pont) A `pieceX` és `pieceY` azonosítójú csúszkák mozgatásakor állítsd át a `piece` azonosító `div` bal felső pontjának pozícióját a csúszkák jelenlegi értékére!
  - Technikai segítség: a `left` és `top` stílustulajdonságokat kell dinamikusan módosítani, és a mértékegységet sem illik elfelejteni.
- e. (1 pont) Az "OK" feliratú gombra kattintáskor írd ki a konzolra, hogy mennyi a távolság a korábban kiválasztott pont és a `piece` bal felső sarka között! (pl. `Distance: 3.605551275463989`) Ehhez használhatod a kiindulási állományban adott `distanceTo()` függvényt is.
- f. (1 pont) Amennyiben a kiszámított távolság több mint 10 egység, akkor jelenítsd meg az `error` azonosítójú elemet, illetve csökkentsd az `attempts` változó értékét eggyel, amelyet írass is ki az azonos nevű `span` elembe!
  - Technikai segítség: a megjelenést a `display` stílustulajdonság értékével tudod szabályozni, amelyet `block` vagy `none` értékre kell állítani ebben a feladatban.
- g. (1 pont) Amennyiben a kiszámított távolság 10 egység vagy kevesebb, akkor jelenítsd meg a `success` azonosítójú elemet!
  - Tipp: előfordulhat, hogy korábban az `error` azonosítójú elem már megjelent, ilyenkor illendő elrejteni.
- h. (1 pont) Ha a captchát sikeresen megoldottuk (tehát a távolság 10 egység vagy kevesebb) vagy pedig elfogyott az összes próbálkozási lehetőség, akkor tiltsuk le az oldalon lévő csúszkákat és gombot!
