SELECT * FROM dolgozo;
/*
* - minden oszlopot szeretnénk látni (* = minden)
*/
SELECT dnev, dkod, belepes FROM dolgozo;--sorrend nem számít, ahogy tetszik

SELECT fizetes*12 as ÉVIFIZETES FROM dolgozo;
--attól még hogy 12-szeresét kérdezem, az eredeti érték nem módosul
--as: custom nevet/aliast csinálni a lekérdezésre, as amúgy nem muszáj, elég utána is 

SELECT dnev || ' a neve' FROM dolgozo;-- || konkatenáció

SELECT dnev || ' fogalkozasa ' || foglalkozas FROM dolgozo;

SELECT * FROM dolgozo WHERE fizetes>2500;
--select: oszlopokat szûrsz
--where: sorokat szûrsz
/*
< kisebb/nagyobb/egyenlõ szokásosan
<> kisebb és nagyobb DE NEM egyenlõ
*/

SELECT * FROM dolgozo WHERE fizetes > 3000 and not fizetes > 5000;

SELECT * FROM dolgozo WHERE fizetes < 2000 and not (foglalkozas ='SALESMAN');


--FELADATOK:
SELECT dnev FROM dolgozo WHERE fizetes > 2800;

SELECT dnev FROM dolgozo WHERE oazon = 10 or oazon = 20;

SELECT dnev FROM dolgozo WHERE jutalek > 600;

SELECT dnev, jutalek FROM dolgozo WHERE not(jutalek > 600) or jutalek is null;
--is null: ezzel tudunk null értékre szûrni
--null-al nem lehet számolni, nem vizsgálható, max az is null-nál


/*
HALMAZMÛVELETEK:
unió - UNION
metszet - INTERSECT
különbség - MINUS !!! figyelj a sorrendre
*/
SELECT dkod, dnev FROM dolgozo WHERE oazon = 10
UNION
SELECT dkod, dnev FROM dolgozo WHERE oazon = 20;
--csak akkor lehet ezt használni, amikor a két táblában a sorok száma =, és a típusuk is ugyanaz ugyanabban a sorrendben

SELECT oazon FROM dolgozo WHERE foglalkozas = 'SALESMAN'
INTERSECT
SELECT oazon FROM dolgozo WHERE foglalkozas = 'MANAGER';

SELECT * FROM dolgozo WHERE belepes > to_date('1982.01.01', 'YYYY.MM.DD');--Y - year, M - month, D - day
--sysdate - visszaadja a mai dátumot

SELECT * FROM dolgozo WHERE dnev like '%A%';
-- _ pontosan egy akármilyen karakter
-- % akárhány akármilyen karakter

SELECT dnev FROM dolgozo WHERE dnev like '%L%L%';

SELECT * FROM dolgozo WHERE fizetes between 2000 and 3000;--intervallumhatárokat beleértve

SELECT * FROM dolgozo WHERE oazon in (10,20);

SELECT * FROM dolgozo ORDER BY oazon desc, -fizetes;
--desc: descending rendezés

SELECT * FROM vdani.szeret;

SELECT gyumolcs FROM vdani.szeret
MINUS
SELECT gyumolcs FROM vdani.szeret WHERE nev = 'Micimackó';


/*
-------------------------3. GYAK------------------------------------
*/
SELECT nev FROM vdani.szeret WHERE gyumolcs='alma';

SELECT nev FROM vdani.szeret
MINUS
SELECT nev FROM vdani.szeret WHERE gyumolcs='körte';

SELECT nev FROM vdani.szeret WHERE gyumolcs='dinnye'
UNION
SELECT nev FROM vdani.szeret WHERE gyumolcs='körte';

SELECT * FROM vdani.osztaly;

CREATE TABLE osztaly AS
SELECT * FROM vdani.osztaly;

--táblák összekapcsolása:::::::::::::::
SELECT * FROM dolgozo NATURAL JOIN osztaly;

SELECT telephely FROM dolgozo NATURAL JOIN osztaly WHERE dnev='BLAKE';

SELECT * FROM dolgozo JOIN osztaly USING (oazon);--u.a. mint a natural

--feltétel alapú:::::::::::
SELECT * FROM dolgozo JOIN osztaly ON dolgozo.oazon = osztaly.oazon;

SELECT * FROM dolgozo d JOIN osztaly o ON d.oazon = o.oazon;

--DESCARTES-SZORZAT::::::::::::
SELECT * FROM dolgozo CROSS JOIN osztaly WHERE dolgozo.oazon = osztaly.oazon;

SELECT d1.dnev, d2.dnev FROM dolgozo d1 CROSS JOIN dolgozo d2;

SELECT * FROM dolgozo RIGHT OUTER JOIN osztaly ON dolgozo.oazon = osztaly.oazon;--dolgozo left, osztaly right

SELECT * FROM dolgozo LEFT OUTER JOIN osztaly ON dolgozo.oazon = osztaly.oazon;

SELECT * FROM dolgozo FULL OUTER JOIN osztaly ON dolgozo.oazon = osztaly.oazon;

--tetszõleges számú táblát össze lehet kapcsolni
SELECT * FROM (dolgozo d1 CROSS JOIN dolgozo d2) JOIN osztaly ON d2.oazon = osztaly.oazon;

--FELADATOK:::::::::::::::
SELECT dnev FROM dolgozo NATURAL JOIN osztaly WHERE telephely='DALLAS' OR telephely='CHICAGO';

SELECT dnev FROM dolgozo WHERE fonoke = (SELECT dkod FROM dolgozo WHERE dnev='KING');

SELECT beosztott.dnev FROM dolgozo beosztott JOIN dolgozo fonok ON beosztott.fonoke = fonok.dkod WHERE fonok.dnev = 'KING';

SELECT beosztott.dnev FROM dolgozo beosztott JOIN dolgozo fonok ON beosztott.fonoke = fonok.dkod WHERE beosztott.fizetes > fonok.fizetes;

---------------------------------------------- 4. GYAK ----------------------------------------------------
--legalább 2
SELECT DISTINCT * FROM vdani.szeret sz1 CROSS JOIN vdani.szeret sz2 WHERE sz1.nev = sz2.nev AND NOT sz1.gyumolcs = sz2.gyumolcs;

--legalább 3
SELECT DISTINCT sz1.nev FROM vdani.szeret sz1 CROSS JOIN vdani.szeret sz2 CROSS JOIN vdani.szeret sz3 WHERE sz1.nev = sz2.nev AND sz2.nev = sz3.nev AND sz1.gyumolcs < sz2.gyumolcs AND sz2.gyumolcs < sz3.gyumolcs;

/*
union - halmaz lesz multihalmazból
union all - megmarad az imsétlõdés
*/

--azok a telephelyek, ahol van ANALYST
SELECT * FROM dolgozo WHERE foglalkozas = 'ANALYST';-- FORD, SCOTT
SELECT DISTINCT telephely FROM dolgozo NATURAL JOIN osztaly WHERE foglalkozas = 'ANALYST';--<tábla1> JOIN <tábla2> ON <feltétel>

SELECT * FROM vdani.fiz_kategoria;
SELECT * FROM dolgozo;
SELECT * FROM dolgozo JOIN vdani.fiz_kategoria ON dolgozo.fizetes >= vdani.fiz_kategoria.also AND dolgozo.fizetes <= vdani.fiz_kategoria.felso;
SELECT * FROM dolgozo JOIN vdani.fiz_kategoria ON dolgozo.fizetes >= vdani.fiz_kategoria.also AND dolgozo.fizetes <= vdani.fiz_kategoria.felso WHERE vdani.fiz_kategoria.kategoria = 3;
SELECT dolgozo.dnev FROM dolgozo JOIN vdani.fiz_kategoria ON dolgozo.fizetes >= vdani.fiz_kategoria.also AND dolgozo.fizetes <= vdani.fiz_kategoria.felso WHERE vdani.fiz_kategoria.kategoria = 3;

--legmagasabb fizetés
SELECT dnev FROM dolgozo
MINUS
SELECT dolgozo.dnev FROM dolgozo CROSS JOIN dolgozo help WHERE dolgozo.fizetes < help.fizetes;

DESCRIBE dolgozo;
/*
varchar2 - maximum n hosszú
varchar - pontosan n hosszú

*/

SELECT dnev, fizetes+NVL(jutalek, 0) FROM dolgozo;--NVL helyettesíti a-t b-vel

---------------------------------------------- 5. GYAK ---------------------------------------------------
SELECT * FROM dual;

SELECT 3+4 FROM dual;

SELECT SUBSTR('ABCDEFG',3,4) FROM dual;--szöveg, honnan (1-tõl kezd), hány db betû onnantól

SELECT LENGTH('alma') FROM dual;

SELECT LENGTH(dnev) FROM dolgozo;

SELECT dnev, LENGTH(dnev) FROM dolgozo WHERE LENGTH(dnev) = 4;

SELECT dnev FROM dolgozo WHERE SUBSTR(dnev, 2,1) = 'A';

SELECT dnev FROM dolgozo WHERE INSTR(dnev, 'L', 1, 2) > 0;-- szöveg, karakter, honnan kezdve nézi, hanyadikat keresi

SELECT TO_CHAR(sysdate, 'YY-MM-DD') FROM dual;
--day: hét melyik napja
--months: hónap neve
--year: szövegesen kiírja az évet

SELECT * FROM dolgozo WHERE dnev = 'ADAMS';

SELECT TO_CHAR(belepes, 'month'), ROUND(sysdate - belepes, 0) eltelt_nap FROM dolgozo WHERE dnev = 'ADAMS';

--OSZLOPFÜGGVÉNYEK
SELECT MAX(fizetes) FROM dolgozo;
/*
MAX
MIN
SUM
AVG
COUNT
*/

SELECT AVG(fizetes) átlag FROM dolgozo WHERE oazon = 20;

SELECT COUNT(DISTINCT foglalkozas) foglalkozasok FROM dolgozo;
--UNIQUE? pontosan egyszer szerepel? de akkor miért jó?

SELECT jutalek FROM dolgozo;

SELECT MAX(jutalek), MIN(jutalek), AVG(jutalek), COUNT(jutalek) FROM dolgozo;

SELECT foglalkozas, oazon, COUNT(*), AVG(fizetes) FROM dolgozo GROUP BY foglalkozas, oazon;

SELECT telephely, oazon, ROUND(AVG(dolgozo.fizetes)) FROM dolgozo NATURAL JOIN osztaly GROUP BY telephely, oazon;

SELECT oazon, ROUND(AVG(fizetes)) FROM dolgozo GROUP BY oazon HAVING ROUND(AVG(fizetes)) > 2000;

--Kulcsszavak sorrendje: select, from, where, group by, having, order by

SELECT ROUND(AVG(fizetes)) atlag FROM dolgozo GROUP BY oazon HAVING COUNT(dnev) > 4;

SELECT f.kategoria FROM dolgozo d CROSS JOIN vdani.fiz_kategoria f WHERE d.fizetes BETWEEN f.also and f.felso GROUP BY f.kategoria HAVING COUNT(DISTINCT oazon) = 1;

------------------------------------------- 6. GYAK -----------------------------------------------
SELECT dnev FROM (SELECT * FROM dolgozo);

SELECT dnev FROM dolgozo WHERE dolgozo.fizetes > (SELECT dolgozo.fizetes AS milu FROM dolgozo WHERE dnev='MILLER');

SELECT dnev FROM dolgozo WHERE dolgozo.fizetes IN (SELECT dolgozo.fizetes AS milu FROM dolgozo WHERE dnev='MILLER' OR dnev='BLAKE');
--in, all, any

SELECT dnev FROM dolgozo WHERE dolgozo.fizetes = (SELECT MIN(fizetes) FROM dolgozo);

SELECT dnev FROM dolgozo WHERE foglalkozas = ANY (SELECT DISTINCT foglalkozas FROM dolgozo JOIN osztaly ON dolgozo.oazon = osztaly.oazon WHERE osztaly.oazon = 20);

SELECT dnev FROM dolgozo kulso WHERE kulso.fizetes > (SELECT AVG(belso.fizetes) FROM dolgozo belso WHERE belso.oazon = kulso.oazon);

--EXISTS - visszatér igazzal, ha létezik
SELECT dnev FROM dolgozo kulso WHERE EXISTS (SELECT * FROM dolgozo belso WHERE fonoke = kulso.dkod);

--FONTOSAK: in, all, any, exists



---------------------------- ZH (7. GYAK) -----------------------
SELECT * FROM vdani.got_hazak;
SELECT * FROM vdani.got_karakterek;
SELECT * FROM vdani.got_csatak;

SELECT nev FROM vdani.got_karakterek WHERE vagyon >= sereg+3000;

((SELECT haz_nev FROM vdani.got_hazak MINUS
SELECT haz_nev FROM vdani.got_csatak)) INTERSECT
((SELECT haz_nev FROM vdani.got_hazak MINUS
SELECT haz_nev FROM vdani.got_karakterek));

SELECT * FROM dolgozo WHERE dnev like '%A%';
-- _ pontosan egy akármilyen karakter
-- % akárhány akármilyen karakter

SELECT UNIQUE vdani.got_karakterek.nev FROM vdani.got_karakterek NATURAL JOIN vdani.got_hazak NATURAL JOIN vdani.got_csatak
WHERE motto like '%harag%' OR motto like '%vér%' AND csata_nev = 'Harangok csatája';

SELECT * FROM vdani.got_csatak cs1 CROSS JOIN vdani.got_csatak cs2;

SELECT DISTINCT cs1.haz_nev HAZ1, cs2.haz_nev HAZ2 FROM vdani.got_csatak cs1 JOIN vdani.got_csatak cs2 
ON NOT cs1.haz_nev = cs2.haz_nev
WHERE cs1.csata_nev = cs2.csata_nev AND cs1.gyozott = cs2.gyozott;



----------------------------- 8. GYAk ------------------------------
CREATE TABLE dolgozo AS
SELECT * FROM vdani.dolgozo WHERE 1=2; --sehol sem igaz

--számmal nem kezdõdhet a táblanév
CREATE TABLE sorozatok (
    cim VARCHAR2(50) PRIMARY KEY,-- UNIQUE és PRIMARY KEY ugyanazt csinálja, annyi különbséggel, hogy UNIQUE-nál lehet null érték
    keszito VARCHAR2(20) NOT NULL,
    pilot DATE,
    evadok INTEGER CHECK (evadok > 0)
);

DESCRIBE sorozatok;

SELECT * FROM sorozatok;

INSERT INTO sorozatok
VALUES(1, 'The Leftovers', 'Damon Lindelof', sysdate, 3);

INSERT INTO sorozatok
VALUES(2, 'Lost', 'Damon Lindelof', to_date('2004.01.01', 'YYYY.MM.DD'), null);

INSERT INTO sorozatok(azon, cim, keszito)
VALUES (3, 'Breaking Bad','Vince Gilligan');

TRUNCATE TABLE sorozatok; -- összes sor törlése

DROP TABLE sorozatok; -- egész tábla törlése

CREATE TABLE sorozatok (
    azon INTEGER,
    cim VARCHAR2(50) UNIQUE,-- UNIQUE és PRIMARY KEY ugyanazt csinálja, annyi különbséggel, hogy UNIQUE-nál lehet null érték
    keszito VARCHAR2(20) NOT NULL REFERENCES keszitok(nev) ON DELETE SET NULL, --ha nem mondunk semmit, nem engedné a keszitokbol törölni Davidet pl
    pilot DATE,
    evadok INTEGER CHECK (evadok > 0),
    CONSTRAINT sorozatok_pk PRIMARY KEY (azon, cim)
);

CREATE TABLE keszitok (
    nev VARCHAR2(50) PRIMARY KEY,
    szuletes DATE
);

INSERT INTO keszitok VALUES ( 'Damon Lindelof', sysdate);

ALTER TABLE sorozatok
ADD CONSTRAINT sorozatok_notnull unique (cim, keszito);

ALTER TABLE sorozatok
DROP CONSTRAINT sorozatok_notnull;

ALTER TABLE sorozatok
ADD epizodok INTEGER;

ALTER TABLE sorozatok
MODIFY epizodok NUMBER(2,3);

ALTER TABLE sorozatok
DROP COLUMN epizodok;


CREATE TABLE sw_osztalyok (
    nev VARCHAR(50) PRIMARY KEY,
    hossz INTEGER NOT NULL CHECK (hossz > 0),
    legenyseg INTEGER NOT NULL CHECK (legenyseg > 0),
    kapacitas INTEGER,
    CONSTRAINT osztalyok_pos CHECK (kapacitas > 0)
);

INSERT INTO sw_osztalyok
VALUES ('Venator', 1200, 2000, 160);

INSERT INTO sw_osztalyok
VALUES ('Acclamator', 600, 800, 560);

INSERT INTO sw_osztalyok
VALUES ('Arquitens', 120, 40, 2);

CREATE TABLE sw_hajok (
    nev VARCHAR2(50),
    osztaly VARCHAR(50) REFERENCES sw_osztalyok(nev),
    egyseg INTEGER REFERENCES sw_egysegek(azon),
    CONSTRAINT hajok_uniquenev UNIQUE (nev)
);

INSERT INTO sw_hajok
VALUES ('Redeemer', 'Venator', 501);

INSERT INTO sw_hajok
VALUES ('Negotiator', 'Acclamator', 332);

INSERT INTO sw_hajok
VALUES ('Breacher', 'Arquitens', 7);

CREATE TABLE sw_egysegek (
    azon INTEGER,
    tipus VARCHAR(15) NOT NULL,
    CONSTRAINT  egyseg_primary PRIMARY KEY (azon)
);

INSERT INTO sw_egysegek
VALUES (501, 'Legion');

INSERT INTO sw_egysegek
VALUES (332, 'Core');

--hozzáadás plusz plus insert add
INSERT INTO sw_egysegek
VALUES (7, 'Core');

SELECT * FROM sw_egysegek;
SELECT * FROM sw_hajok;
SELECT * FROM sw_osztalyok;

--törlés delete remove
DELETE FROM sorozatok
WHERE evadok IS NULL;

UPDATE sorozatok
SET evadok = 4
WHERE cim = 'Breaking Bad';

UPDATE sorozatok
SET evadok = evadok + 1
WHERE cim = 'Breaking Bad';

UPDATE sorozatok
SET evadok = evadok + 1
WHERE keszito = (SELECT keszito FROM sorozatok WHERE cim = 'Lost');

UPDATE dolgozo
SET fizetes = fizetes * 2
WHERE fizetes < (SELECT avg(fizetes) FROM dolgozo belso WHERE belso.oazon = oazon);

---------------------------- 9. GYAK -------------------------------
CREATE TABLE dolgozo AS
SELECT * FROM vdani.dolgozo;

DROP TABLE dolgozo;

SELECT * FROM dolgozo;

DELETE FROM dolgozo WHERE fizetes = (SELECT MAX(fizetes) FROM dolgozo);

--MÓDOSÍTÁS
--növel null átlag
UPDATE dolgozo SET fizetes = fizetes + 500 WHERE jutalek is null OR fizetes < (SELECT AVG(fizetes) FROM vdani.dolgozo);

--Növeljük meg azon dolgozók fizetését az osztályuk fizetésének átlagával, akik nem fõnökök
--növel átlag osztályátlag közvetlen leszármazott elnevez név külsõ
UPDATE dolgozo kulso 
SET fizetes = fizetes + (SELECT AVG(fizetes) FROM dolgozo WHERE oazon = kulso.oazon) 
WHERE kulso.dnev != ANY (SELECT dnev FROM dolgozo WHERE dkod = ANY (SELECT fonoke FROM dolgozo));

--visszavonás visszavon undo
rollback;

SELECT dnev FROM dolgozo WHERE dkod = ANY (SELECT fonoke FROM dolgozo);


UPDATE dolgozo SET fizetes = fizetes + 10;

rollback;

--Adjuk meg a legjobban keresõ dolgozót
SELECT dnev, fizetes FROM dolgozo WHERE fizetes = (SELECT MAX(fizetes) FROM dolgozo);

SELECT dnev, fizetes, ROWNUM FROM dolgozo ORDER BY fizetes DESC;

WITH rendezett AS (SELECT dnev, fizetes, ROWNUM FROM dolgozo ORDER BY fizetes DESC)
SELECT * FROM rendezett WHERE ROWNUM = 1;

--nézet create new új nézet
CREATE OR REPLACE VIEW d20 AS
SELECT * FROM dolgozo
WHERE oazon = 20;

SELECT * FROM d20;

SELECT * FROM vdani.d_clerk;

--GRANT, REMOVE
GRANT SELECT ON d_clerk TO abc123;
GRANT SELECT ON d_clerk TO PUBLIC;
--grant 'lekérdezési jog' on 'tábla/view' to 'személy'

GRANT SELECT ON dolgozo TO MJR7PO;

SELECT * FROM ap3558.dolgozo;
SELECT * FROM mjr7po.d10;

CREATE OR REPLACE VIEW doszt AS
SELECT * FROM vdani.dolgozo NATURAL JOIN vdani.osztaly;

UPDATE doszk
SET jutalek=99
WHERE telephely='NEW YORK';

--WITH : view
--FROM : inline view
--WHERE : subquery

SELECT beosztott.dnev FROM vdani.dolgozo beosztott, vdani.dolgozo fonok
WHERE beosztott.fonoke = fonok.dkod AND fonok.dnev='KING';


SELECT dnev, LEVEL, sys_connect_by_path(dnev, '->')
FROM dolgozo
--WHERE PRIOR dnev='KING' ezzel azokra szûrök, ahol a szülõ king
--WHERE level = 2
START WITH dnev='KING'
CONNECT BY PRIOR dkod = fonoke;

SELECT * FROM vdani.jaratok;

--NOCYCLE, START, CONNECT, CONNECT BY
--adjuk meg az összes olyan várost, ahova el lehet jutni SF-bõl
SELECT UNIQUE hova
FROM vdani.jaratok
START WITH honnan='San Francisco'
CONNECT BY NOCYCLE PRIOR hova = honnan;

-------------------------------------------- 10. GYAK -----------------------------------
/
declare
    a constant integer := 10;
    b real;
    c double precision;
    
    d char(10);--konstans hosszúságú string
    e varchar2(10);--változó hosszúságú string
    
    f date;
    g timestamp;
    
    h boolean;--ilyen sima SQL-ben nincs, erre figyelni kell ha vegyíteni akarjuk a kettõt
begin
    null;--speciális utasítás, nem csinál semmit, legalább egy utasítást kell írni
    --utasításokat lehet megadni
    dbms_output.put_line('HELLO');
    dbms_output.put_line(a);
end;
/
--ez jelzi, hogy a PLSQL kódrészlet itt ér véget, a Ctrl-Enter-hez kell

set serveroutput on;--ez a parancs fog kelleni, hogy üzenetet kapj

declare
    a integer := 10;
    b integer := 5;
    c integer;
    d integer := 10;
    i integer := 0;
begin
    c := a+b;
    dbms_output.put_line(a);
    if a < b then
        dbms_output.put_line('A kisebb mint B');
        end if;
    if a > b then
        null;
        end if;
    if a = d then
        dbms_output.put_line('A egyenlõ B');
    else
        dbms_output.put_line('A nagyobb mint B');
        end if;
    for i in 0..10 loop
        b := b + 1;
        end loop;
    dbms_output.put_line(b);
    
    while i < 6 loop
        dbms_output.put_line(i);
        end loop;
end;
/
/*
CREATE OR REPLACE PROCEDURE összeg(a in integer, b in integer) AS --az in paraméter csak input, belül megváltoztatni nem lehet
    c integer;
begin
    c := a + b;
    dbms_output.put_line(c);
end;
/
*/
EXECUTE összeg(5,6);
CALL összeg(5,6);
/

declare
begin
    összeg(1,2);
    end;
    /
    
create or replace procedure összeg2(a in integer, b in integer, c out integer) as
begin
    c := a + b;
end;
/

declare
    ossz integer;
begin
    összeg2(10, 11, ossz);
    dbms_output.put_line(ossz);
    end;
    /
    
create or replace procedure negyzet(x in out integer) as
begin
    x := x*x;
    end;
    /
    
declare
    n integer := 7;
begin
    dbms_output.put_line(n);
    negyzet(n);
    dbms_output.put_line(n);
    end;
    /
    
create or replace function negyzetF(x integer) return integer as--execute és call nem mûködik függvényeknél
    a integer;
begin
    a := x*x;
    return a;
    end;
    /
    
declare 
    n integer := 5;
begin
    n := negyzetF(n);
    end;
    /

select negyzetF(7) from dual;

create or replace function prim(x integer) return integer as
    i integer := 1;
    osztok integer := 0;
begin
    if x = 1 then
        return 1;--1 nem prim
    end if;
    for i in 1..x loop
        if mod(x, i) = 0 then
            osztok := osztok + 1;
        end if;
    end loop;
    if osztok = 2 then
        return 0;-- igen, prim
    else
        return 1;-- nem, nem prim
    end if;
end;
/

select prim(8) from dual;

create or replace function fibonacci(x integer) return integer as
    fib integer := 0;
    elo integer := 1;
    kov integer;
begin
    for i in 1..x loop
        kov := fib + elo;
        elo := fib;
        fib := kov;
    end loop;
    return fib;
end;
/

select fibonacci(0) from dual;

declare
    nev dolgozo.dnev%type;
    fizu dolgozo.fizetes%type;
    sor dolgozo%rowtype;
begin
    select *
    into sor
    from vdani.dolgozo
    where dkod=7839;
    dbms_output.put_line('Név: ' || sor.dnev || ', fizu: ' || sor.fizetes);
exception
    when no_data_found then
    dbms_output.put_line('Nincs ilyen sor');
    end;
    /

/*
create or replace function hanyszor(elso in varchar2, masodik in  varchar2) return integer as
    a integer := 0;
    b integer := 0;
    result integer := 0;
begin
    
    if a > b then
        for i in 0..a loop
            if strcmp(substr(elso, 0+i, b), masodik) = 0 then
                result := result + 1;
            end if;
        end loop;
    else
        for i in 0..b loop
            if strcmp(substr(masodik, 0+i, a), elso) = 0 then
                result := result + 1;
            end if;
        end loop;
    end if;
    return result;
end;
/

select hanyszor('ab ab adc acb ab', 'ab') from dual;*/


---------------------------- MINTA ZH---------------------------

---------------------------- 12. GYAK---------------------------
set serveroutput on;

create or replace function factor(x integer) return integer as
    faktor integer := 1;
begin
    for i in 1..x loop
        faktor := faktor * i;
    end loop;
    return faktor;
end;
/

select factor(10) from dual;

/
declare
    fizu integer;
    cursor k is 
        select fizetes
        from vdani.dolgozo
        where oazon=20;
begin
    open k;
    loop
        fetch k into fizu; -- az aktuális sor értékét betöltöttem a változóba, és a mutató túllép a következõre
        exit when -- feltétel, hogy mikor lépünk ki a ciklusból
            k%notfound; -- akkor lépünk ki, amikor a k nem mutat sehová
    end loop;
    close k;
    
    dbms_output.put_line(fizu);
end;
/


declare
    cursor k is select fizetes from vdani.dolgozo where oazon=20;
begin
    for sor in k loop -- a forban automatikusan megnyitja a kurzort
        dbms_output.put_line(sor.fizetes);
    end loop;
end;
/

create or replace procedure paratlan_dolgozo is
    sorszam integer := 0;
    cursor k is select * from vdani.dolgozo order by dnev;
begin
    for sor in k loop
        sorszam := sorszam + 1;
        if MOD(sorszam, 2) = 1 then
            dbms_output.put_line(sor.dnev || ', ' || sor.fizetes);
        end if;
    end loop;
end;
/

execute paratlan_dolgozo;

--paraméteres kurzor
declare
    cursor k (o_azon integer) is select * from vdani.dolgozo where oazon = o_azon;
begin
    for sor in k(20) loop
        dbms_output.put_line(sor.dnev);
    end loop;
    
    for sor in k(10) loop
        dbms_output.put_line(sor.dnev);
    end loop;
end;
/


--módosító kurzor
drop table dolgozo2;
create table dolgozo2 as select * from vdani.dolgozo;
declare
    cursor k is select * from dolgozo2 for update;
begin
    for sor in k loop
        update dolgozo2
        set fizetes = fizetes*2
        where current of k;--mindig csak a mostani sort módosítom, ha nem lenne minden egyes alkalommal az egész táblában módosítanám
    end loop;
    
    --commit; véglegesíti a módosításokat
    rollback; -- visszavonja a módosításokat az utolsó commitig
end;
/
/*
create or replace procedure novel_fiz(kat integer) is
    cursor k is 
    select * from dolgozo2 d join vdani.fiz_kategoria f
    where d.fizetes between f.also and f.felso and f.kategoria = kat;
    -- mi a tetves kurvaanyád van már te nyomorék szar
begin
    for sor in k loop
        update dolgozo2
        set fizetes = fizetes + 1
        where (sor.kategoria = kat or sor.kategoria is null) and current of k;
    end loop;
end;
/
*/

declare
    type dolgozo_rek is record (nev varchar2(10), fizetes integer, osztaly integer);-- rekord deklaráció 
    dsor dolgozo_rek;
begin
    select dnev, fizetes, oazon
    into dsor
    from vdani.dolgozo
    where dnev='KING';
    
    dbms_output.put_line(dsor.nev || ', ' || dsor.fizetes);
end;
/


-- KOLLEKCIÓK: beágyazott tábla
declare
    type dolgozo_tabla is table of varchar2(20);--varchar2 helyére mehetne record is
    dtabla dolgozo_tabla := dolgozo_tabla();
    i integer := 1;
    
    cursor k is select * from vdani.dolgozo;
begin
    for sor in k loop
        dtabla.extend; --extendnek meg lehet adni számot mint paraméter, hogy hányszor akarjuk megnövelni
        dtabla(i) := sor.dnev;
        i := i + 1;
    end loop;
    
    for j in 1..dtabla.count loop
        dbms_output.put_line(dtabla(j));
    end loop;
end;
/

-- KOLLEKCIÓK: asszociatív tömb
declare
    type dolgozo_tabla is table of integer index by varchar2(20);--varchar2 helyére mehetne record is
    dtabla dolgozo_tabla;
    
    cursor k is select * from vdani.dolgozo;
    
    akt varchar(20);
begin
    for sor in k loop
        dtabla(sor.dnev) := sor.fizetes;
    end loop;
    
    akt := dtabla.first; -- last
    while akt is not null loop
        dbms_output.put_line(akt || ': ' || dtabla(akt));
        akt := dtabla.next;
    end loop;
end;
/

-- KOLLEKCIÓK: dinamikus tömb ///aminek nem dinamikus a mérete xdddd
declare
    type szamtomb is varray(6) of integer;
    lotto szamtomb;
begin
    lotto := szamtomb(4,8,11,16,23,49);
    
    for i in 1..lotto.count loop
        dbms_output.put_line(i);
    end loop;
end;
/

declare
    type dtable is table of dolgozo%rowtype;
    d dtable := dtable();
begin
    select *
    bulk collect into d --minden sort beletöltünk
    from dolgozo
    where oazon=20;
    
    dbms_output.put_line(d(1).dnev);
end;
/


-- ABC sorrend
create or replace procedure abc_sorrend is
    type dtable is table of vdani.dolgozo%rowtype;
    d dtable := dtable();
begin
    select * 
    bulk collect into d
    from vdani.dolgozo
    order by dnev;
    
    for i in 1..d.count loop
        if i = 1 then
            dbms_output.put_line(d(i).dnev || ': ' || d(i).fizetes);
        else 
            if d(i-1).fizetes < d(i).fizetes then
                dbms_output.put_line(d(i).dnev || ': ' || d(i).fizetes);
            else
                dbms_output.put_line('---' || d(i).dnev || ': ' || d(i).fizetes);
            end if;
        end if;
    end loop;
end;
/

execute abc_sorrend;