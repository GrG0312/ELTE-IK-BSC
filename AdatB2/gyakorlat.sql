----------------------- 1. GYAKORLAT -------------------------

select * from vdani.dolgozo;

-- Objects
select * from user_objects;

create table dolgozo as
select * from vdani.dolgozo;

select * from user_objects where object_name = 'DOLGOZO';

--Create Table
create table gyak_2024_1 (x integer);

select * from dba_objects where object_name = 'GYAK_2024_1';


--1. feladat: kinek a tuulajdonában van a DBA_TABLES nevû nézet?
select owner from dba_objects where object_name = 'DBA_TABLES' and object_type = 'VIEW';

select * from dba_objects where object_name = 'DBA_TABLES';

select * from sys.dba_objects;


--3. feladat: milyen típusú objektumai vannak az orauser nevû felhasználónak?
select distinct object_type from dba_objects where owner = 'ORAUSER';


--4. feladat: hány különbözõ típusú objektum van nyilvántartva?
select distinct object_type from dba_objects;

select count(distinct object_type) from dba_objects;


--6. feladat: kik azok a felhasználók, akiknek több mint 10 féle objektumuk van
select owner from ( select owner, count(distinct object_type) asd from dba_objects group by owner ) where asd > 10;

select owner from dba_objects group by owner having count(distinct object_type) > 10;


--7. feladat: kik azok a felhasználók akiknek van triggere és nézete is?
select distinct owner from dba_objects where object_type = 'TRIGGER'
intersect
select distinct owner from dba_objects where object_type = 'VIEW';


--------------------------------------- 2. GYAKORLAT --------------------------------------------

select * from DBA_TAB_COLUMNS where owner = 'NIKOVITS' and table_name = 'EMP';

select count(column_id) oszlopok from DBA_TAB_COLUMNS where owner = 'NIKOVITS' and table_name = 'EMP';

--1.gyak. TBA_COLUMNS 3. feladat:
select distinct owner from dba_tab_columns where column_name like 'Z%';

--azon táblák neve, ahol az elsõ és negyedik oszlop típusa varchar2
select owner, table_name from dba_tab_columns where column_id = 1 and data_type = 'VARCHAR2'
intersect
select owner, table_name from dba_tab_columns where column_id = 4 and data_type = 'VARCHAR2';

--create database link ullmandb connect to zvp7ej identified by jelszo using 'ullman.inf.elte.hu:1521/ullman';

--select * from vdani.dolgozo'kukac'ullmandb;

--szinoníma
create synonym d for vdani.dolgozo;

drop synonym d;

select * from dba_synonyms;


--szekvencia
create sequence azon start with 100 increment by 1 nocycle;

select azon.nextval from dual;

drop sequence azon;


--nyomozás
select * from sz1;

select * from dba_synonyms;

select * from dba_objects;

select owner, table_name from dba_tab_columns where column_name = 'FIRST_NAME'
intersect
select owner, table_name from dba_tab_columns where column_name = 'LAST_NAME'
intersect
select owner, table_name from dba_tab_columns where column_name = 'SALARY'
intersect
select owner, table_name from dba_tab_columns where column_name = 'DEPARTMENT_NAME';

select * from nikovits.v1;

select * from dba_objects where object_name ='V1';

select text from dba_views where view_name = 'V1' and owner = 'NIKOVITS';


--hol vannak Nikovits szállít táblájának adatai tárolva?
select * from nikovits.szallit;

select * from dba_objects where owner = 'NIKOVITS' and object_name = 'SZALLIT';

select * from dba_segments where segment_name = 'SZALLIT' and owner = 'NIKOVITS';

select * from dba_extents where segment_name = 'SZALLIT' and owner = 'NIKOVITS';

select * from dba_data_files where file_id = 2;

select * from dba_tablespaces;

select * from dba_free_space where file_id = 2;


--1. feladat
--Melyek azok az objektum típusok, amelyek tényleges tárolást igényelnek, vagyis
--tartoznak hozzájuk adatblokkok? (A többinek csak a definíciója tárolódik adatszótárban)
select distinct object_type from dba_objects where data_object_id is not null;

--2. feladat
--Melyek azok az objektum típusok, amelyek nem igényelnek tényleges tárolást, vagyis nem
--tartoznak hozzájuk adatblokkok? Mi a metszete az elõzõ lekérdezéssel?
select distinct object_type from dba_objects where data_object_id is null;

select distinct object_type from dba_objects where data_object_id is not null
intersect
select distinct object_type from dba_objects where data_object_id is null;

---------------------------------------- GYAK 3 --------------------------------
select * from (select file_name, bytes from dba_data_files
union
select file_name, bytes from dba_temp_files) order by bytes desc;

--7. feladat: Melyik a legnagyobb méretû tábla szegmens az adatbázisban (a tulajdonost is adjuk meg) 
--és hány extensbõl áll? (A particionált táblákat most ne vegyük figyelembe.)
select owner, segment_name, extents, bytes from dba_segments where segment_type = 'TABLE' and bytes = 
(select max(bytes) from dba_segments where segment_type = 'TABLE');

--9. feladat: Adjuk meg adatfájlonkent, hogy az egyes adatfájlokban mennyi a foglalt 
--hely osszesen (írassuk ki a fájlok méretét is).
select file_name, bytes, foglalt from dba_data_files natural join (select file_id, sum(bytes) foglalt from dba_extents group by file_id);

select * from dba_extents;

select * from dba_segments;

--11. feladat: Hány extens van a 'users01.dbf' adatfájlban? Mekkora ezek összmérete?
select count(segment_name), sum(bytes) from dba_extents where file_id = (
select file_id from dba_data_files where file_name like '%users01.dbf%');

--14. Van-e a NIKOVITS felhasználónak olyan táblája, amelyik több adatfájlban is foglal helyet? (Aramis)
select * from dba_extents where owner = 'NIKOVITS';
select segment_name, count(distinct file_id) from dba_extents where owner = 'NIKOVITS' and segment_type = 'TABLE' group by segment_name having count( distinct file_id) > 1;

--20. A NIKOVITS felhasználó CIKK táblája hány blokkot foglal le az adatbázisban? (ARAMIS)
--(Vagyis hány olyan blokk van, ami ehhez a táblához van rendelve és így azok már más táblákhoz nem adhatók hozzá?)
select * from dba_tables where owner = 'NIKOVITS' and table_name = 'CIKK';
select * from dba_extents where owner = 'NIKOVITS' and segment_name = 'CIKK'; --<--

create table kiskutya (lab integer);
select * from dba_segments where owner = 'ZVP7EJ' and segment_name = 'KISKUTYA';
drop table kiskutya;
alter session set deferred_segment_creation = true;

create table kiskutya (lab integer)
pctfree 10 pctused 60
storage (initial 32k minextents 1 maxextents 200 pctincrease 0); --pctincrease: hány %-al nõjjön az újrafoglalásnak a mérete

------------------------------------------- GYAK 5 ---------------------------------

create index ind1 on dolgozo(dnev);
create unique index ind2 on dolgozo(dkod);
create index ind3 on dolgozo(fizetes, foglalkozas);
create index ind4 on dolgozo(belepes) reverse;
create index ind8 on dolgozo(fizetes desc);
create index ind5 on dolgozo(foglalkozas, dnev) compress 1; -- az 1 hogy az elsõ hány db oszlopot szeretnénk tömöríteni
create index ind6 on dolgozo(fizetes/12);
create bitmap index ind7 on dolgozo(oazon); --<-- akkor használjuk, ha riktán módosuló adataink vannak

select * from user_indexes where table_name = 'DOLGOZO';

--Írjjunk lekérdezést ami összegzi a masodperc oszlop értékeit, ahol a dátum = "2012.01.31"
select sum(masodperc) from nikovits.hivas where datum = to_date('2012.01.31', 'YYYY.MM.DD');
select sum(masodperc) from nikovits.hivas_v2 where datum = to_date('2012.01.31', 'YYYY.MM.DD');
--az elsõ több ideig fut, miért? :::

select * from dba_indexes where table_name like 'HIVAS%' and owner = 'NIKOVITS';

select * from dba_ind_columns where table_name like 'HIVAS%' and index_owner = 'NIKOVITS';

--Adjuk meg azon indexek nevét, amelyek legalább 9 oszloposak
select index_name from dba_ind_columns group by index_name having count(*) >= 9;
select index_name from dba_ind_columns where column_position = 9;

-- Adjuk meg azon kétoszlopos indexek nevét és tulajdonosát, amelyeknek legalább az egyik kifejezése függvény alapú.
select * from dba_ind_columns;
select index_owner, index_name from dba_ind_columns group by index_owner, index_name having count(distinct column_name) = 2
intersect
select owner, index_name from dba_indexes where index_type like '%FUNCTION%';
select * from dba_indexes;

select index_owner, index_name from dba_ind_columns group by index_owner, index_name having count(distinct column_name) = 2
intersect
select index_owner, index_name from dba_ind_expressions;

---------------------------------- GYAK 6 --------------------------------------------------

select * from dolgozo;

-- 1. lépés: klaszter létrehozása
create cluster dolgozo_clust (oazon number(2));

-- 2. lépés: tábla létrehozása a klaszteren
-- Meg kell adni a klaszter nevét
create table dolgozocl (dkod number(4), dnev varchar2(10), oazon number(2))
cluster dolgozo_clust(oazon);

-- Másik tábla létrehozása
create table osztalycl (oazon number(2), nev varchar2(14))
cluster dolgozo_clust(oazon);

-- Beszúrások a táblákba - nem mûködik, mert nincs index
insert into osztalycl select oazon, onev from osztaly;
insert into dolgozocl select dkod, dnev, oazon from dolgozo;

-- 3. lépés: index létrehozása a klaszterhez
create index dolgozocl_ind on cluster dolgozo_clust;

-- Most már lehet lekérdezni és beszúrni.
-- Eddig ez sem ment
select * from dolgozocl;
select * from osztalycl;

-- Két sor rowid-ja egyezik, mivel ugyanott tárolódnak
SELECT rowid, dnev FROM dolgozocl WHERE oazon=10
UNION
SELECT rowid, nev FROM osztalycl WHERE oazon=10;



-- Hash klaszter létrehozásához töröljük az eddigieket
drop cluster dolgozo_clust;
drop table dolgozocl;
drop table osztalycl;

-- Hashkeys megadása szükséges
-- Nagyjából a különbözõ klaszter kulcsok számára érdemes beállíteni
create cluster dolgozo_clust (oazon number(2))
hashkeys 5; -- kb 5 különbözõ kosárba fogja szórni az értékeket

-- A táblák létrehozása ugyanúgy zajlik
create table dolgozocl (dkod number(4), dnev varchar2(10), oazon number(2))
cluster dolgozo_clust(oazon);

create table osztalycl (oazon number(2), nev varchar2(14))
cluster dolgozo_clust(oazon);

-- Nem kell indexet létrehozni a beszúráshoz és a lekérdezéshez
insert into osztalycl select oazon, onev from osztaly;
select * from osztalycl;

-- Egytáblás klaszter létrehozása
create cluster dolgozo_single_cluster(oazon number(2))
single table hashkeys 5;

-- Tábla létrehozása az egytáblás klaszterre
create table dolgozohashcl (dkod number(4), dnev varchar2(10), oazon number(2))
cluster doglozo_single_cluster(oazon);

-- Saját hash függvény megadása
drop table dolgozohashcl;
drop cluster doglozo_single_cluster;

create cluster dolgozo_single_cluster(oazon number(2))
single table hashkeys 3 
hash is MOD(oazon, 3);

-- Információk lekérdezése a klaszterekrõl
SELECT cluster_name, cluster_type, function, hashkeys, single_table
FROM dba_clusters WHERE owner='VDANI';

-- Kalszteren lévõ táblák a dba_tables-bõl
SELECT cluster_name, table_name FROM dba_tables 
WHERE owner='VDANI' AND cluster_name IS NOT NULL;

-- Klaszter oszlopok lekérdezése
SELECT cluster_name, clu_column_name, table_name, tab_column_name 
FROM dba_clu_columns WHERE owner='VDANI';

-- Saját hash függvények
SELECT cluster_name, hash_expression 
FROM dba_cluster_hash_expressions WHERE owner='VDANI';

-- Információk a clusterekr?l a katalógusban (adatszótár nézetekben): 
-- DBA_CLUSTERS
-- DBA_TABLES (cluster_name oszlop -> melyik klaszteren van a tábla) 
-- DBA_CLU_COLUMNS (táblák oszlopainak megfeleltetése a klaszter kulcsának)
-- DBA_CLUSTER_HASH_EXPRESSIONS (hash klaszterek hash függvényei)

--2. feladat
--Adjunk meg egy olyan clustert az adatbázisban (ha van ilyen), amelyen még nincs egy tábla sem.
select owner, cluster_name from dba_clusters
minus
select owner, cluster_name from dba_tables where cluster_name is not null;


----------------------------- 8. GYAK --------------------------------------------
create table PLAN_TABLE (
        statement_id       varchar2(30),
        plan_id            number,
        timestamp          date,
        remarks            varchar2(4000),
        operation          varchar2(30),
        options            varchar2(255),
        object_node        varchar2(128),
        object_owner       varchar2(30),
        object_name        varchar2(30),
        object_alias       varchar2(65),
        object_instance    numeric,
        object_type        varchar2(30),
        optimizer          varchar2(255),
        search_columns     number,
        id                 numeric,
        parent_id          numeric,
        depth              numeric,
        position           numeric,
        cost               numeric,
        cardinality        numeric,
        bytes              numeric,
        other_tag          varchar2(255),
        partition_start    varchar2(255),
        partition_stop     varchar2(255),
        partition_id       numeric,
        other              long,
        distribution       varchar2(30),
        cpu_cost           numeric,
        io_cost            numeric,
        temp_space         numeric,
        access_predicates  varchar2(4000),
        filter_predicates  varchar2(4000),
        projection         varchar2(4000),
        time               numeric,
        qblock_name        varchar2(30),
        other_xml          clob
);

drop table plan_table;

explain plan set statement_id = 'terv1' for
    select * from vdani.dolgozo;
    
select * from plan_table;

select plan_table_output from table (dbms_xplan.display('plan_table', 'terv1', 'typical'));

explain plan set statement_id = 'terv2' for
    select * from vdani.dolgozo natural join vdani.osztaly;
    
select plan_table_output from table (dbms_xplan.display('plan_table', 'terv2', 'typical'));


--1. feladat: Írjunk olyan lekérdezéseket, amelyek végrehajtási tervében elõfordulnak a következõ mûveletek:
--SORT GROUP BY, SORT ORDER BY, SORT UNIQUE, SORT AGGREGATE,
--HASH JOIN, MERGE JOIN, NESTED LOOPS, UNION-ALL, MINUS,
--INDEX RANGE SCAN, INDEX UNIQUE SCAN, TABLE ACCESS FULL, BITMAP INDEX
--HASH UNIQUE, HASH GROUP BY

explain plan set statement_id = 'terv_1' for
    select unique fizetes from vdani.osztaly natural join vdani.dolgozo where telephely = 'NEW YORK' group by oazon;
    
select plan_table_output from table (dbms_xplan.display('plan_table', 'terv_1', 'typical'));

--2. feladat:
drop table dolgozo;
create table dolgozo as select * from vdani.dolgozo;
create table osztaly as select * from vdani.osztaly;
create table fizkat as select * from vdani.fiz_kategoria;

select * from osztaly;
select * from fizkat;

explain plan set statement_id = 'fizkat_terv' for
select unique onev from dolgozo natural join osztaly natural join fizkat where fizkat.kategoria = 1;

select plan_table_output from table (dbms_xplan.display('plan_table', 'fizkat_terv', 'typical'));

create index fizkat_index on fizkat(kategoria desc);
drop index fizkat_index;

select unique onev from dolgozo natural join osztaly natural join fizkat where fizkat.kategoria = 1;

explain plan set statement_id = 'terv_tipp' for
    select /*+ use_nl(dolgozo osztaly) */ * from dolgozo natural join osztaly;
    
    
------------------------------------------------- 9. GYAK ----------------------------------------------------

explain plan set statement_id = 'fizetes_1' for
    select sum(fizetes) from vdani.dolgozo;
    
select plan_table_output from table(dbms_xplan.display('plan_table', 'szallit_1', 'typical'));


select * from nikovits.szallit;
select * from nikovits.cikk;

select sum(mennyiseg) from nikovits.szallit join nikovits.cikk on nikovits.szallit.ckod = nikovits.cikk.ckod where szin = 'piros';

explain plan set statement_id = 'szallit_1' for
    select sum(mennyiseg) from nikovits.szallit join nikovits.cikk on nikovits.szallit.ckod = nikovits.cikk.ckod where szin = 'piros';
    
select * from dba_indexes where owner = 'NIKOVITS' and (table_name = 'CIKK' or table_name = 'SZALLIT');

explain plan set statement_id = 'szallit_1' for
    select /*+ index(cikk)*/ sum(mennyiseg) from nikovits.szallit join nikovits.cikk on nikovits.szallit.ckod = nikovits.cikk.ckod where szin = 'piros';
    
-- C feladat:
explain plan set statement_id = 'szallit_1' for
    select /*+ index(cikk) index(szallit)*/ sum(mennyiseg) from nikovits.szallit join nikovits.cikk on nikovits.szallit.ckod = nikovits.cikk.ckod where szin = 'piros';
    
-- D feladat:
explain plan set statement_id = 'szallit_1' for
    select /*+ use_merge(szallit, cikk)*/ sum(mennyiseg) from nikovits.szallit join nikovits.cikk on nikovits.szallit.ckod = nikovits.cikk.ckod where szin = 'piros';
    
-- G feladat:
explain plan set statement_id = 'szallit_1' for
    select /*+ use_nl(szallit cikk) no_index(szallit) no_index(cikk)*/ sum(mennyiseg) from nikovits.szallit join nikovits.cikk on nikovits.szallit.ckod = nikovits.cikk.ckod where szin = 'piros';
    
-- 6. feladat:
--1:
explain plan set statement_id = 'szallit_2' for
select sum(suly) from nikovits.cikk;

select plan_table_output from table(dbms_xplan.display('plan_table', 'szallit_2', 'typical'));
--2:
explain plan set statement_id = 'szallit_3' for
select sum(suly) from nikovits.cikk where ckod = 50;

select plan_table_output from table(dbms_xplan.display('plan_table', 'szallit_3', 'typical'));


explain plan set statement_id = 'szallit_4' for
select /*+ use_hash(szallit, projekt) no_index(szallit) no_index(projekt)*/count(pkod) from nikovits.szallit natural join nikovits.projekt;

select plan_table_output from table(dbms_xplan.display('plan_table', 'szallit_4', 'typical'));


explain plan set statement_id = 'szallit_5' for
select /*+ use_hash(szallit, projekt) no_index(szallit) no_index(projekt) full(szallit) full(projekt)*/pkod from nikovits.szallit natural join nikovits.projekt group by pkod;

select plan_table_output from table(dbms_xplan.display('plan_table', 'szallit_5', 'typical'));


explain plan set statement_id = 'szallit_6' for
select /*+ use_merge(szallit cikk) index(cikk c_szin) no_index(szallit)*/ sum(suly) from nikovits.cikk natural join nikovits.szallit where szin = 'piros';

select plan_table_output from table(dbms_xplan.display('plan_table', 'szallit_6', 'typical'));


--ezeket ulmannban::::
-- mennyi ideig tart a hivas tábla teljes végigolvasása?
select sum(masodperc) from nikovits.hivas;

select sum(masodperc) from nikovits.hivas_v2;


-- hogyan változik a lefutási idõ, ha adunk meg feltételt a dátum oszlopra?
select * from nikovits.hivas where datum > to_date('2011.08.05', 'YYYY.MM.DD');

select * from nikovits.hivas_v2 where datum > to_date('2012.06.05', 'YYYY.MM.DD');


---------------------------------------------- 12. GYAK ----------------------------------------------
select username, saddr from v$session where username is not null;

select username, start_time from v$transaction t join v$session s on t.ses_addr = s.saddr; -- nekem nincs transaction

drop table tr;
create table tr (x integer);
insert into tr values(1);

update tr set x = x + 1;

grant select on tr to public;
grant update on tr to public;

select * from tr;

commit;

set transaction isolation level serializable;

rollback;

-- Bence táblájával
select * from xj66cw.tr;

update xj66cw.tr set x = 51;

commit;


--Melyik session milyen zárolást tart fenn jelen pillanatban és mióta?
SELECT se.sid, se.username, lo.type, lo.lmode, lo.ctime
FROM v$lock lo, v$session se
WHERE se.sid = lo.sid AND username = 'ZVP7EJ';

--Melyik session vár éppen egy zárolásra, melyik session-re várnak éppen és milyen régen birtokolja a zárat, illetve várnak rá?
SELECT se.sid, se.username, lo.type, lo.lmode,
lo.request, lo.ctime, block
FROM v$lock lo, v$session se
WHERE se.sid = lo.sid AND username = 'VDANI';


LOCK TABLE xj66cw.tr IN EXCLUSIVE MODE;

LOCK TABLE tr IN EXCLUSIVE MODE;

rollback;