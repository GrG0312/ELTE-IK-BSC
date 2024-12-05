export const tasks = [
  {
    A: {
      imageUrl: 'assets/images/tasks/1/A/hypnotoad.gif',
      story: '',
      subTasks: [
        {
          text: `A Canvas-on történő kattintásra helyezz le részleges körívet (Arc).
          A körív ne legyen teljes (360°), a szögét 270°-ra konfiguráld alapesetben.

Segítség: https://konvajs.org/docs/shapes/Arc.html`,
          xp: 1
        },
        {
          text: `Helyezz el a füleleten egy darab ng-zorro slidert
          (https://ng.ant.design/components/slider/en).

A slider értékét arra használd, hogy a megjelenő körvonalak mekkora szöggel jelenjenek meg.
A slider tartományát ennek megfelelően állítsd be.

A slider értékének változtatása után az újonnan lehelyezett körvonalak már a slideren beállított
szöggel jelenjenek meg.`,
          xp: 1
        },
        {
          text: `A slider változtatásával automatikusan módosuljon az összes korábban lehelyezett Arc szöge is.`,
          xp: 2
        },
        {
          text: `Helyezz el egy radio gombot a felületen:
          https://ng.ant.design/components/radio/en#components-radio-demo-solid

A kiválasztott érték azt határozza meg, hogy mi történjen a Konva stage-re kattintáskor
(tehát hasonló célt szolgál, mint a toolbar az órai projektben).

Az egyik (az alap) opció az körívek lehelyezésének lehetősége, amit az előző feladatokban implementáltál is.
A másik egy pont lehelyezése. Erre használhatsz például fix színnel kitöltött kört.

Ebből a pontból csak egy darab lehessen lehelyezve, tehát ha a felhasználó máshova kattint akkor a már létező
pont legyen áthelyezve (vagy törölve és újra létrehozva az új pozícióban).`,
          xp: 1
        },
        {
          text: `Minden Konva stage-n szereplő körív középpontjából vezessen egy él az előző feladatban felhelyezett
          pontba.

Természetesen amennyiben a pont elmozdul, az élek is mozogjanak vele.`,
          xp: 2
        },
        {
          text: `A pont közvetlen közelében jelenjen meg egy szám, amely az aktuálisan felvett körívek számát
          jelzi.

Ez a szám frissüljön amikor új körív kerül lehelyezésre.
A szám egy Group-ban legyen a ponttal, tehát ha a pont mozog, akkor a szám is menjen vele.

Segítség: https://konvajs.org/docs/groups_and_layers/Groups.html`,
          xp: 2
        },
        {
          text: `A körívek legyenek mozgathatóak (azaz dragelhetőek) és a mozgatásukkal automatikusan mozogjanak
          az élek is, a kapcsolat ne szűnjön meg!`,
          xp: 2
        },
        {
          text: `A pont lehelyezése után 5 másodpercenként vándoroljon el egy véletlenszerű
          - de a Konva Stage-n látható - pozícióba.

Az átúszás animálva történjen, tehát ne azonnali "ugrás" legyen. A mozgás animáció 1 másodpercig tartson.

A kapcsolódó élek végpontja természetesen ilyenkor is mozogjon tovább a ponttal!`,
          xp: 2
        },
        {
          text: `A ponthoz legközelebb elhelyezkedő körív színe legyen piros.
          Az összes többi színe maradjon az eredeti beállításon.

Ezt folyamatosan számold újra időszakosan (például másodpercenként).`,
          xp: 2
        },
        {
          text: `Az előző feladat színezéséhez kapcsolódó számítás időszakos frissítés helyett történjen
          inkább esemény-vezérelten.

Azaz, amikor olyan esemény történik ami a számítás eredményére kihathat (például lehelyezésre kerül egy új körív,
elmozog a pont, stb.), akkor automatikusan kerüljön újból kiszámításra a ponthoz legközelebbi körív és ennek
megfelelően változzon is a színe ha szükséges.`,
          xp: 2
        },
        {
          text: `A fentiekben felvázolt jelenthez lehessen képet betölteni háttérnek.

A kép feltöltése tetszőleges módon megoldható, működhet Drag&Drop-al a fájlrendszerből behúzva,
de akár ng-zorro-s fájl választóval is
(https://ng.ant.design/components/upload/en) is.
A felhúzott képet feszítsd ki a Stage háttereként.
Ehhez nyugodtan használj külön layer-t ha segít.

Új háttérkép importálásakor az előző eltávolítható.

A következő fájltípusok legyenek támogatottak:
- JPG
- BMP
- SVG`,
          xp: 3
        },
      ],
      title: `Animation`
    },
    B: {
      imageUrl: 'assets/images/tasks/1/B/south_park_cast.webp',
      story: '',
      subTasks: [
        {
          text: `A felületen találsz egy toolbart négy gombbal.
          Az elsővel az órai feladathoz hasonlóan kiválasztó módba tudsz kerülni,
          a többivel az a cél, hogy sorban ellipszist,
          poligont (első körben háromszöget),
          valamint téglalapot lehessen elhelyezni a Konva felületen.

Egy megoldáskezdeményt találhatsz a téglalaphoz segítségként.`,
          xp: 2
        },
        {
          text: `A poligon opció egy háromszöget helyezzen el a felületen, azonban legyen lehetőség ezt
          négyzetté, ötszöggé vagy hatszöggé átalakítani utólag.

Ennek módja úgy nézne ki, hogy a poligonra történő jobbklikk esetén felugrana egy menü a kurzor pozíciójában
a következő lehetőségekkel (amennyiben még háromszögre kattintunk):
- Átalakítás háromszöggé (aktív) -> ez az opció kikapcsolt állapotban jelenjen meg, mivel már most is háromszög
- Átalakítás négyzetté
- Átalakítás ötszöggé
- Átalakítás hatszöggé

Egy opciót kiválasztva mérettartó módon történjen meg a konverzió.

Természetesen az aktív (nem kattintható, kikapcsolt) opció attól függően változzon, hogy milyen poligonra
kattintottunk.
`,
          xp: 4
        },
        {
          text: `Készíts egy negyedik felhelyezhető alakzatot: egy tetszőlegesen kiválasztott South Park karaktert.

A karakteren nyugodtan egyszerűsíts, nem muszáj ívelt vonalakat használni, csak egyenes vonalakból, körökből és téglalapokból
álló megvalósítás is elfogadható, a lényeg, hogy az alakzat és a színezés alapján azért megismerhető legyen a karakter.

![Cartman](/assets/images/tasks/1/B/cartman.png)

Segítség: https://www.southparkstudios.com/w/index.php?title=List_of_Characters&oldid=14766#Featured_4th_Graders`,
          xp: 3
        },
        {
          text: `A Konva-hoz tartozó Canvas-tól balra jelenjen meg egy fa struktúra, amin látszik a Konva interfészre
          (az egyetlen layer-re) felhelyezett grafikus elemek szerkezete.

Az elemek sorrendjével nem szükséges foglalkozni, lehet felhelyezési sorrendben.

Természetesen új elem felkerülése esetén az azonnal jelenjen meg a listán (a fa struktúrában) is.

Alapesetben az alakzatok típusa (pl.: Polygon) jelenjen meg a fában.

Komplex alakzat esetén (South Park figura) a gyerek alakzatok is látszanak.
Tehát lenne egy Group típusú elem a fában, aminek lenne például 2 kör, 3 vonal és egy téglalap gyereke.

Segítség: https://ng.ant.design/components/tree/en`,
          xp: 3
        },
        {
          text: `Ha a fa struktúrában rákattintunk egy elemre, akkor a rajzfelületen kerüljön kiválasztásra
          a kapcsolódó alakzat, mintha kiválasztó módban rákattintottunk volna.`,
          xp: 1
        },
        {
          text: `A fa struktúrában minden alakzat (típus, vagy a későbbi feladatok megoldása után név)
          mellett jelenjen meg egy ikont (pl.: lakat, ng-zorro: lock/unlock) tartalmazó gomb.

Ezzel a gombbal legyen oda-vissza állítható, hogy kiválasztó módban az alakzat kattintható vagy pedig nem.`,
          xp: 1
        },
        {
          text: `A fa struktúrában minden alakzat (típus, vagy a későbbi feladatok megoldása után név)
          mellett jelenjen meg egy ikont (pl.: szem, ng-zorro: eye/eye-invisible) tartalmazó gomb.

Ezzel a gombbal legyen oda-vissza állítható, hogy az alakzat megjelenik-e a Konva stage-n vagy pedig nem.`,
          xp: 1
        },
        {
          text: `Lehessen elnevezni a felhelyezett grafikus elemeket.

A név megadása egy felugró ablakban (modal) történjen, amikor a felhasználó bal klikkel kattint az alakzatra.
A modalban legyen egy input mező a névnek és két gomb: "Mégsem" és "Név mentése".

Név mentése esetén kerüljön be a név az alakzat adatai közé.

Segítség: https://ng.ant.design/components/modal/en`,
          xp: 3
        },
        {
          text: `Ha a felhasználó egy olyan alakzatra kattint amelynek már van neve, akkor a felugró ablakban
          megjelenő input mező értékében a korábbi név szerepeljen és ezáltal legyen lehetőség a nevet módosítani.`,
          xp: 1
        },
        {
          text: `A név változása után a baloldali fa struktúrában az alakzat aktuális neve jelenjen meg a típusa
          (vagy az előző neve) helyett.`,
          xp: 1
        },
      ],
      title: `Character Tree`
    },
  },
  {
    A: {
      imageUrl: 'assets/images/tasks/2/A/aqua.avif',
      story: ``,
      subTasks: [
        {
          text: `A feladat célja egy layer-ek (rétegek) kezelésére szolgáló felület kialakítása.

A Canvas-tól balra jelenjen meg egy blokk, ahol alapesetben látszik a Stage-hez adott mindkét layer.
A megjelenített név első körben lehet csak az index (Layer 1, Layer 2, ...).
          `,
          xp: 1
        },
        {
          text: `A bal oldali felületen legyen lehetőség rétegek kiválasztására.
          A lehelyezett objektumok a kiválasztott layer-re kerüljenek.
          Kiválasztó módban csak a választott layer-ről lehessen alakazatot kijelölni.`,
          xp: 1
        },
        {
          text: `Legyen lehetőség layer-ek hozzáadására és törlésére.

Törléskor természetesen a grafikus felületről is kerüljön le a layer, az összes általa tartalmazott
grafikus elemmel együtt.`,
          xp: 2
        },
        {
          text: `Legyen lehetőség layer-ek el- és átnevezésére.

Alapesetben a réteg neve (elnevezés előtt például "Layer 1") sima szövegként jelenjen meg,
viszont amikor a felhasználó rákattint, a helyén egy input mező jelenjen meg.
Az input mezőben megadható a réteg neve, ENTER lenyomásával menthető.
Mentés után visszakerülünk az alapállapotba (az input mező eltűnik és szövegesen megjelenik a név).

Amennyiben a felhasználó üres nevet ment el, úgy a réteglistában NE üres blokk jelenjen meg, térjünk vissza
az index használatára, mintha még nem is lett volna megadva név (például: "Layer 3").`,
          xp: 2
        },
        {
          text: `A layer neve mellett zárójelben tüntesd fel, hogy hány darab grafikus objektum van az adott layer-en.

Grafikus objektumok hozzáadása esetén frissüljön ez a szám dinamikusan.`,
          xp: 1
        },
        {
          text: `Lehessen módosítani a layer-ek sorrendjét. Ez történhet drag&drop-al, de akár
          fel le nyilakkal is az egyes listaelemek (layer-ek) neve mellett.

A layer-ek sorrendje hasson ki a Konva Stage-re is. A magasabb rétegeken található alakzatok kerüljenek "magasabbra".
Tehát ha két különböző rétegen vannak olyan alakzatok melyeknek van közös metszete, akkor a magasabb szintű layeren
elhelyezkedő objektum legyen feljebb.
Természetesen a layer-ek sorrendjének megváltozásakor ez is forduljon.`,
          xp: 3
        }
      ],
      title: `
        Layer Management
      `
    },
    B: {
      imageUrl: 'assets/images/tasks/2/B/happy.jpg',
      story: '',
      subTasks: [
        {
          text: `A toolbar-on két grafikus objektum lehelyezésére van lehetőséged: autó és parkoló.

A komponens betöltésekor véletlenszerűen helyezz le 10 autót és 2 parkolót.
Az autók legyenek mozgathatóak (dragelhetőek), a parkolók viszont ne!`,
          xp: 1
        },
        {
          text: `Integrálj egy web workert az órai projekt alapján.

A worker és a fő szál tudjon üzeneteken keresztül kommunikálni, az ehhez szükséges
feliratkozásokat készítsd el.`,
          xp: 1
        },
        {
          text: `A fő szál 10 másodpercenként utasítsa a web workert az autók pozíciójának ellenőrzésére.

A web worker minden autóhoz keresse meg a hozzá legközelebbi parkolót.
Először is rendelj egy véletlenszerű és eltérő színt minden parkolóhoz (10 szín támogatása elegendő).
A parkoló és minden autó ami hozzá van legközelebb kapjon egy szegélyt (bordert) a parkolóhoz rendelt színnel.`,
          xp: 3
        },
        {
          text: `Grafikus objektum létrehozásakor illetve mozgatásakor utasítsd a web workert, hogy az előző
          részfeladatban definiált folyamatot futassa le újból.

Ennek célja, hogy ha releváns változás történik akkor ne kelljen megvárni <10 másodperc elteltét a helyes színezés
életbe lépéséhez, történjen meg azonnal.`,
          xp: 1
        },
        {
          text: `A színezésekről vezess statisztikát a Konva stage felett.

A statisztikában két dolgot jeleníts meg:
- A parkolóhoz rendelt színt
- Mennyi autó van legközelebb ahhoz a parkolóhoz

A statisztika legyen csökkenő sorrendbe rendezve az autók száma szerint.

Például:

#89434F: 6

#804F04: 5

#1B2124: 2
`,
          xp: 2
        },
        {
          text: `A web worker az ellenőrzés során még nézze meg azt is, hogy mely autók vannak 150 pixelnél távolabb
          minden parkolótól.

Az ilyen autók villogjanak (hátterük változzon fél másodpercenként) egészen addig amíg nem "épül" a közelében egy
parkoló vagy az autót nem mozgatjuk egy már létező parkoló 150 pixeles körzetébe.`,
          xp: 2
        },
      ],
      title: `Worker`
    }
  },
]
