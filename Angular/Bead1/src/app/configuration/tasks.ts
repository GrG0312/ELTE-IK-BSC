export const tasks = [
  {
    A: {
      imageUrl: 'assets/images/tasks/1/A/rectangle.gif',
      story: '',
      subTasks: [
        {
          text: `Helyezz el a füleleten 4 darab ng-zorro slidert
          (https://ng.ant.design/components/slider/en).

A sliderek egy szín beállítását fogják elősegíteni, RGBA (Red Green Blue Alpha) formátumban.
A sliderek tehát sorban a Red, Green, Blue valamint Alpha értékek megadását tegyék lehetővé.
Emiatt az első három slider 0 és 255 közötti értéket tudjon felvenni, az utolsó pedig 0 és 1 közöttit.`,
          xp: 1
        },
        {
          text: `<b>Komponens-szintű stílusosztály</b> segítségével rajzolj ki egy négyzetet a sliderek alá.

Az osztály (pl.: ".rectangle") tehát mindenképpen a komponenshez tartozó stílusfájlban foglaljon helyet és kapjon
legalább egy szélességet, magasságot valamint szegélyt (bordert).`,
          xp: 1
        },
        {
          text: `A sliderek mozgatásával a négyzet háttere automatikusan változzon a beállított értékek által
          definiált színre.
          `,
          xp: 1
        },
        {
          text: `<b>Globális stílus</b>: A megfelelő fájlban definiáld, hogy az oldalon található
          összes gombnak legyen egy vastag, színes kerete (border).

Ez a feladat teljesen független az első 3 részfeladattól. Nem a komponenshez tartozó síluslapon kell megoldani.`,
          xp: 1
        },
      ],
      title: `Styling`
    },
    B: {
      imageUrl: 'assets/images/tasks/1/B/fishnet.jpeg',
      story: '',
      subTasks: [
        {
          text: `Készíts egy grid-et (https://ng.ant.design/components/grid/en) 4 listaelemmel.
          Az egyes elemeknek adj különböző háttérszínt, hogy az elrendezés mindig egyértelmű legyen.

A feladat célja, hogy a rácsok eltérő módon legyenek elrendezve a böngészőablak méretétől függően.

Segítség: A grid dokumentációjában található példák tudnak segíteni az adatstruktúra kialakításában.`,
          xp: 1
        },
        {
          text: `Alapesetben a cellák legyen a következőképpen rendezve:

Nagy felbontás (lg, xl, xxl): Az első sort fele-fele arányban töltse ki az első két cella, a második sorban az 1.
(összesen a 3.) cella a terület harmadát foglalja el, az utolsó cella pedig a maradék 2/3-ot.

Közepes felbontás (md): Két soros legyen továbbra is a rács, egyenlő szélességű legyen minden cella.

Kis felbontás (xs, sm): 4 darab sora legyen a rácsnak, minden cella töltse ki az adott sor teljes szélességét.

Segítség: a méretekhez érdemes a zorro layout által definiált töréspontokat használni
(ezekre hivatkoztam feljebb: xs, sm,...).
Ide kapcsolódó dokumentáció:
https://ng.ant.design/components/layout/en`,
          xp: 1
        },
        {
          text: `Egy cella felé mozgatva az egeret jelenjen meg felugró tooltip-ben, hogy hány oszlop szélességet
          vesz fel az adott cella.
          (A fentebb linkelt grid dokumentáció átnézése után ez tiszta lesz.)

Segítség: https://ng.ant.design/components/tooltip/en`,
          xp: 1
        },
        {
          text: `Minden cellában szerepeljen két gomb. Az egyik a cella szélességének csökkentésére (-), a másik
          pedig a cella méretének növelésére (+).

Gombnyomásra a cella szélessége 1-el több vagy 1-el kevesebb oszlopot foglaljon el.

Nem szükséges a szomszédokat hozzáigazítani, ha a növelés miatt külön sorba törik egy cella, nem gond!`,
          xp: 1
        }
      ],
      title: `Responsivity`
    },
  },
  {
    A: {
      imageUrl: 'assets/images/tasks/2/A/work.mp4',
      story: ``,
      subTasks: [
        {
          text: `A komponensben készíts egy tömböt pár dolgozó adatával.
          Nem kell véletlenszerűnek lennie, lehetnek fix, beégetett adatok.

A dolgozónak hozz létre egy külön típust (tetszőlegesen használhatsz class-t vagy interface-t).

A dolgozók a következő adatokkal rendelkezzenek:
- név: string
- jogosultsági szint: enum (lehetséges értékek: READER, WRITER, ADMIN)
- vétett hibák száma: number
          `,
          xp: 1
        },
        {
          text: `Jelenítsd meg a dolgozók adatait egy kártyás nézetben.

Ehhez hozz létre egy komponenst, amiben egy dolgozó adatait jeleníted meg egy kártyán
(https://ng.ant.design/components/card/en) belül.

A külső komponensben lépkedj végig a dolgozókon, mindegyikre jelenítsd meg az újonnan létrehozott komponenst,
amely Input-ként (https://angular.io/api/core/Input) kapja meg az adott dolgozó adatait.`,
          xp: 1
        },
        {
          text: `A lista felett legyen egy kapcsoló (https://ng.ant.design/components/switch/en)
          a következő szöveggel: "10-nél több hibával rendelkező dolgozók kiemelése".

Amikor a kapcsoló aktív állapotban van, a 10 illetve 10-nél több hibával rendelkező dolgozók kártyájának háttere
váltson egy figyelmeztető színre (pl. piros).

A kapcsoló állapotát Input-ként add be a kártyát tartalmazó komponenseknek!`,
          xp: 1
        },
        {
          text: `Amikor az előző feladatban létrehozott kapcsoló aktív, a piros háttér mellett jelenjen meg
          a kártyán belül egy "Hiba feloldása" szövegű gomb is.

A gomb minden kattintásra csökkentse 1-el a dolgozóhoz tartozó hibák értékét.

Ha 10 alatti hibaszámhoz értünk akkor természetesen tűnjön el a gomb illetve a piros háttér is.

A feladat megoldásához használj Output EventEmitter-t!

Segítség: https://angular.io/guide/inputs-outputs`,
          xp: 1
        }
      ],
      title: `
        Component
      `
    },
    B: {
      imageUrl: 'assets/images/tasks/2/B/2B.jpg',
      story: '',
      subTasks: [
        {
          text: `Készíts egy reaktív űrlapot.
          Az űrlap tartalmazzon 3 mezőt:

- licence kulcs (sztring)
- korlátozott érvényesség (checkbox)
- érvényesség lejárta (date).

Az űrlaphoz hozz létre egy FormGroup-ot is TypeScriptben, amit köss
is össze ( [formGroup]="" ) a .html fájlban létrehozott űrlappal.

Segítség: https://angular.io/guide/reactive-forms

https://ng.ant.design/components/checkbox/en

https://ng.ant.design/components/date-picker/en`,
          xp: 1
        },
        {
          text: `Adj két validációt az előző részben létrehozott űrlaphoz:

- A licence kulcs megadása legyen kötelező.
- A kulcsot a következő formátumokban lehessen megadni:
XXXX-XXXX-XXXX vagy XXXXXX-XXXXXX, ahol az X lehet szám vagy nagybetű.

A fenti ellenőrzésekhez használhatod az Angular alap validátorait, nem szükséges
sajátot létrehozni.`,
          xp: 1
        },
        {
          text: `Készíts egy saját group szintű validátort.

Ez akkor jelezzen hibát, ha a "korlátozott érvényesség" checkbox be van pipálva, viszont az
"érvényesség lejárta" mezőhöz nem lett megadva helyes dátum.`,
          xp: 1
        },
        {
          text: `Az űrlapon szerepeljen egy submit gomb is.
          Gombnyomásra egy felugró modalban
          (https://ng.ant.design/components/modal/en)
          írasd ki, hogy az űrlap valid-e vagy sem.

Amennyiben az űrlap valid, a modalban jelenjenek meg a megadott adatok is.

Például:

          Az űrlap valid!

          Licence: 123456-987654

          Korlátozott érvényesség: Igen

          Érvényesség: 2024. 12. 31.`,
          xp: 1
        },
      ],
      title: `Form validation`
    },
  },
  {
    A: {
      imageUrl: 'assets/images/tasks/3/A/secret.gif',
      story: '',
      subTasks: [
        {
          text: `Készíts egy saját Angular pipe-ot (https://angular.io/guide/pipes).
          A többi részfeladat alapján hozz helyes döntést abban,
          hogy pure vagy impure pipe-ot érdemes-e itt használni.

A pipe titkosítsa a paraméterként fogadott sztringet.
Az alfeladatokban részletezett esetekre írass ki 1-1 példát a komponensben.`,
          xp: 1
        },
        {
          text: `Amennyiben az értéke látszólag egy bankszámlaszám ("[0-9]{8}-[0-9]{8}") az eredmény
          egy csillag legyen amelyet a bankszámlaszám utolsó 4 számjegye követ.

Példa: {{ "12123434-56567878" | encrypt }} kimenete: "*7878"`,
          xp: 1
        },
        {
          text: `Amennyiben az értéke látszólag egy telefonszám, az eredményben
          a számok legyenek csillag karakterre cserélve, leszámítva az utolsó kettőt.

Elegendő egyetlen telefonszám formátumot kezelni, pl.: 06-20-123-4567.

Példa: {{ "06-20-123-4567" | encrypt }} kimenete: "&ast;&ast;-&ast;&ast;-&ast;&ast;&ast;-&ast;&ast;67"`,
          xp: 1
        },
        {
          text: `Ha a sztring se nem bankszámlaszám se nem telefonszám, akkor az összes karakterét
          cseréld csillagra.

Példa: {{ "password" | encrypt }} kimenete: "********"`,
          xp: 1
        },
      ],
      title: `Pipe`
    },
    B: {
      imageUrl: 'assets/images/tasks/3/B/duolingo.png',
      story: '',
      subTasks: [
        {
          text: `Integráld az ngx-translate és a hozzá tartozó http-loader NPM csomagokat (
          https://www.npmjs.com/package/@ngx-translate/core,
          https://www.npmjs.com/package/@ngx-translate/http-loader)
          a projektbe.

Az AppModule-ban konfiguráld be a TranslateModule-t, hogy a fordításokat
mindenhol használni lehessen a projektben.

Hozz létre legalább 2 nyelvi fájlt (pl.: hu.json és en.json).`,
          xp: 1
        },
        {
          text: `A felületen jeleníts meg egy ng-zorro select
          (https://ng.ant.design/components/select/en)
          elemet. Az elérhető opciók a támogatott nyelvek legyenek.
          Maguk az opciók az éppen kiválasztott nyelven jelenítsék
          meg a a nyelveket (pl. magyar nyelv esetén a két opció
          lehet "Angol" és "Magyar", angol nyelvnél pedig "English"
          és "Hungarian").

A felület megnyitásakor legyen kiválasztva az alapértelmezett nyelv.

A kiválasztás módosításakor váltson nyelvet a felület.
(Természetesen nem szükséges a teljes felületet lefordítani, elegendő csak azokat a szövegeket felvenni,
amelyeket ebben a feladatban kell megjeleníteni.)`,
          xp: 1
        },
        {
          text: `A kiválasztott nyelvet tárold el valahol (pl.: localStorage).
          A program inicializációjakor az utoljára kiválasztott nyelven töltsd
          be a felületet.
          Figyelj arra, hogy akkor se dobjon hibát a program, amikor még nem
          lett kiválasztva nyelv!`,
          xp: 1
        },
        {
          text: `Minden nyelvváltáskor növelj egy számlálót.
          A felületen egy fordítás felhasználásával írasd ki, hogy mennyi nyelvváltás történt összesen.
          (Ez a szám minden váltással növekszik.)
          A szöveg magyarul:
          "Összesen 10 nyelvváltás történt eddig."
          Angolul:
          "There were 10 language changes so far."

Segítség: https://www.vitamindev.com/angular/how-to-use-parameters-in-ngx-translate/`,
          xp: 1
        },
      ],
      title: `Translate`
    },
  },
  {
    A: {
      imageUrl: 'assets/images/tasks/4/A/camera.jpg',
      story: '11223344-55667788',
      subTasks: [
        {
          text: `A felületen helyezz el egy selectort a következő értékekkel:
- Olvasási jog
- Írási jog
- Adminisztrátori jog

A felhasználónak legyen lehetősége többet is választani a fenti opciókból.

Segítség: https://ng.ant.design/components/select/en#components-select-demo-multiple.`,
          xp: 1
        },
        {
          text: `Készíts egy "PermissionDirective" nevű direktívát.
                A direktíva célja, hogy a felhasználó jogosultságai alapján módosítsa a HTML elemet amire helyezik.

A komponensben vegyél fel 1-1 DOM elemet (pl.: div) mindhárom jogosultsághoz és helyezd el
rájuk a direktívát.

A direktívának legyen legalább egy Input paramétere is, amivel megadható, hogy az adott DOM elem tartalmát mely jogosultsággal
(olvasási, írási, adminisztrátori) rendelkező felhasználók érhetik el.
Ezt a bemeneti paramétert is állítsd be a létrehozott DOM elemeken.

Mindhárom elembe írd be szövegesen is, hogy melyik jogosultsági szinthez tartozik.

Segítség: https://angular.io/guide/attribute-directives`,
          xp: 1
        },
        {
          text: `Azon funkció (2. részfeladat során felvett és felparaméterezett DOM elem) amelyhez írási jogra lenne
          szükség, de a felhasználónak csak olvasási joga van (írási nincs, az 1. részfeladat választója alapján),
          kapjon sárga hátteret.

Természetesen ez változzon is dinamikusan, tehát ha kiválasztjuk az írási jogot akkor a sárga háttér tűnjön el.
          `,
          xp: 1
        },
        {
          text: `
          Azon funkció (2. részfeladat során felvett és felparaméterezett DOM elem) amelyhez adminisztrátori
          jogosultságra lenne szükség ne is jelenjen meg, ha a felhasználó nem rendelkezik vele.`,
          xp: 1
        },
      ],
      title: `Directive`
    },
    B: {
      imageUrl: 'assets/images/tasks/4/B/nice_crop.jpeg',
      story: '',
      subTasks: [
        {
          text: `Az ngx-image-cropper (https://www.npmjs.com/package/ngx-image-cropper) csomag lehetővé
          teszi, hogy képekről kivágj egy részt és csak ezt töltsd fel egy szerverre.

Integráld ezt a csomagot a projektbe ("package.json", module import).`,
          xp: 1
        },
        {
          text: `A felületre kerüljön ki egy gomb, amelyet megnyomva egy modal
          (https://ng.ant.design/components/modal/en) ugrik fel.
          A modalban legyen a felhasználónak lehetősége kiválasztani egy képet
          (ngx-image-cropper használatával).
          Az integrált csomag működjön is megfelelően, tehát a felhasználó tudja kiválasztani
          a kép "feltöltésre" szánt részét.

A modal footerében legyen két gomb: "Bezárás" és "Mentés".

A "Bezárás" gomb zárja be a modalt.`,
          xp: 1
        },
        {
          text: `A "Mentés" gomb az image cropperben kiválasztott részt mentse le localStorage-be
          Base64 sztring formátumban.

A mentésre elegendő mindig ugyanazt a localStorage kulcsot használni, tehát mindig csak egy
kép lehessen "feltöltve". Ellenkező esetben az 5MB-os localStorage limit könnyen elérhető lenne!`,
          xp: 1
        },
        {
          text: `A modal-nyitó gomb mellett jelenjen meg a kép, amennyiben szerepel localStorage-ben.

Ha még nem szerepel localStorage-ben kép, akkor a "Kérjük először töltsön fel egy képet!"
szöveg jelenjen meg a helyén!`,
          xp: 1
        },
      ],
      title: `Modal & Image Cropper`
    },
  },
  {
    A: {
      imageUrl: 'assets/images/tasks/5/A/tree_cannon.mp4',
      story: '',
      subTasks: [
        {
          text: `Készíts egy service-t. Szerepeljen benne egy függvény, amely autók adatait generálja
          egy fa struktúrában.
          A fának csak két szintje legyen: a felső szinten gyártók legyenek, alatta pedig a konkrét típusok.
          Pl.: generator.service.ts

A generálás legyen randomizált. A gyártók legyenek valósszerűek (például használhatsz egy tömböt, amiből mindig
véletlenszerűen választasz egy elemet), a típusoknál ez nem szükséges, random generált sztring is megfelelő).

Segítség: A struktúra kialakításában segítenek a fa komponenshez kapcsolódó példák (lásd: NzTreeNode, NzTreeNodeOptions):
https://ng.ant.design/components/tree/en`,
          xp: 1
        },
        {
          text: `Az előző feladatban definiált service használatával generálj
          20 autót (típust) és tárold el a struktúrát egy komponens-változóban a komponens inicializációjakor (OnInit).`,
          xp: 1
        },
        {
          text: `A felületen jelenjen meg a fa struktúra a generált gyártók és típusok adataival.

Segítség: https://ng.ant.design/components/tree/en`,
          xp: 1
        },
        {
          text: `A fa bármelyik levelére kattintva annak tartalma (tehát a típus sztring)
          kerüljön be vágólapra. (Tehát CTRL+V lenyomásával bárhova másolható legyen a szöveg.)

Segítség: használható például a https://www.npmjs.com/package/ngx-clipboard csomag.`,
          xp: 1
        },
      ],
      title: `Tree & Clipboard`
    },
    B: {
      imageUrl: 'assets/images/tasks/5/B/burger.PNG',
      story: '*scrolls for hours with great performance*',
      subTasks: [
        {
          text: `Készíts egy service-t. Szerepeljen benne egy függvény, ami
          paraméterként megadott számú autó adatait generálja le.
          Pl.: generator.service.ts

Az autók a következő mezőkkel rendelkezzenek:
- "gyártó" (string)
- "gyártás éve" (number - 1950 és 2024 között)
- "rendszám" (string - AAAA-123 formátumban)

A generálás legyen randomizált, de legalább a gyártás éve és a rendszám reálisnak tűnő adatokat tartalmazzon.`,
          xp: 1
        },
        {
          text: `Az előző feladatban definiált service használatával generálj
          10000 autót és tárold el őket egy komponens-változóban inicializációkor (OnInit).`,
          xp: 1
        },
        {
          text: `Készíts egy táblázatot két oszloppal (gyártó, gyártás éve), amelyben megjeleníted
          az adatokat virtual scrolling használatával.
          A virtual scrolling célja az, hogy a DOM-ban ne jelenjen meg mind a 10000 sor
          (ez az ablak befagyásához vezetne), csak azok, amelyek éppen látszanak is (plusz pár sor bufferként).

Segítség: https://material.angular.io/cdk/scrolling/overview

https://ng.ant.design/components/table/en#components-table-demo-virtual`,
          xp: 1
        },
        {
          text: `Amikor a gyártás éve felé mozgatjuk az egeret, akkor jelenjen meg az autó rendszáma.

Erre használjátok az ng-zorro tooltip funkcióját:
https://ng.ant.design/components/tooltip/en`,
          xp: 1
        }
      ],
      title: `Virtual Scrolling`
    },
    preview: {
      imageUrl: 'assets/images/preview/5.jpg',
      text: 'Csak így tovább, már csak egy feladat van hátra...'
    }
  }
]
