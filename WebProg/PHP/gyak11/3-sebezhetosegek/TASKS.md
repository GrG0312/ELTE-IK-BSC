## 3. feladat (Adattárolás) - Sebezhetőségek (3-sebezhetosegek, 8 pont)
Segíts Leia hercegnőnek elemezni a Halálcsillag terveit sebezhetőségekért. Ezeket szeretnénk szépen megjeleníteni, hogy könnyen elemezhessük őket.

- a. (2 pont) Legyen lehetőség új sebezhetőségeket felvenni egy űrlapon kereszül. Minden hozzáadott sebezhetőséget ments el egy JSON fájlba.
    - Rövid név (`shortdesc`), komolyság (`danger`, 1-5 mennyire komoly hiba), leírás (`longdesc`, hosszú szöveg, megadtunk egy példát), kép (`image`, ez egy link, megadtunk pár képet, amivel tesztelheted).
    - Az űrlap értékeit **nem** kell ellenőrizni helyesség szempontjából.
- b. (1 pont) Hozzáadás után kerüljünk vissza a főoldalra (`index.php`).
- c. (1 pont) Listázd ki a sebezhetőségek rövid neveit a főoldalra (célszerű `<ul>` elembe tenni őket).
- d. (1 pont) Minden sebezhetőség legyen egy link, ami egy részletező oldalra visz minket, ahol az adott sebezhetőségről minden információt láthatunk.
    - Megint csak nem kell ellenőrizni a GET paraméter helyességét.
    - A részletező oldalon a `h2` elem tartalmazza a rövid leírást és zárójelben a sebezhetőség komolyságát, a `vulnerability` azonosítójú `div` pedig a hosszú leírást és a képet.
- e. (1 pont) A részletező oldalon is található egy űrlap. Ezzel legyen lehetőségünk megváltoztatni a sebezhetőség értékét.
- f. (1 pont) Szerkesztés után a sebezhetőség saját (részletező) oldalára kerüljünk vissza.
- g. (1 pont) A szerkesztő űrlap alapértelmezetten legyen kitöltve a sebezhetőség aktuális adataival. 

![3. feladat](f3-sebezhetosegek.gif)