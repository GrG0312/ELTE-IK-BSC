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


--1. feladat: kinek a tuulajdon�ban van a DBA_TABLES nev� n�zet?
select owner from dba_objects where object_name = 'DBA_TABLES' and object_type = 'VIEW';

select * from dba_objects where object_name = 'DBA_TABLES';

select * from sys.dba_objects;


--3. feladat: milyen t�pus� objektumai vannak az orauser nev� felhaszn�l�nak?
select distinct object_type from dba_objects where owner = 'ORAUSER';


--4. feladat: h�ny k�l�nb�z� t�pus� objektum van nyilv�ntartva?
select distinct object_type from dba_objects;

select count(distinct object_type) from dba_objects;


--6. feladat: kik azok a felhaszn�l�k, akiknek t�bb mint 10 f�le objektumuk van
select owner from ( select owner, count(distinct object_type) asd from dba_objects group by owner ) where asd > 10;

select owner from dba_objects group by owner having count(distinct object_type) > 10;


--7. feladat: kik azok a felhaszn�l�k akiknek van triggere �s n�zete is?
select distinct owner from dba_objects where object_type = 'TRIGGER'
intersect
select distinct owner from dba_objects where object_type = 'VIEW';


--------------------------------------- 2. GYAKORLAT --------------------------------------------

select * from DBA_TAB_COLUMNS where owner = 'NIKOVITS' and table_name = 'EMP';

select count(column_id) oszlopok from DBA_TAB_COLUMNS where owner = 'NIKOVITS' and table_name = 'EMP';

--1.gyak. TBA_COLUMNS 3. feladat:
select distinct owner from dba_tab_columns where column_name like 'Z%';

--azon t�bl�k neve, ahol az els� �s negyedik oszlop t�pusa varchar2
select owner, table_name from dba_tab_columns where column_id = 1 and data_type = 'VARCHAR2'
intersect
select owner, table_name from dba_tab_columns where column_id = 4 and data_type = 'VARCHAR2';

--create database link ullmandb connect to zvp7ej identified by jelszo using 'ullman.inf.elte.hu:1521/ullman';

--select * from vdani.dolgozo'kukac'ullmandb;

--szinon�ma
create synonym d for vdani.dolgozo;

drop synonym d;

select * from dba_synonyms;


--szekvencia
create sequence azon start with 100 increment by 1 nocycle;

select azon.nextval from dual;

drop sequence azon;


--nyomoz�s
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


--hol vannak Nikovits sz�ll�t t�bl�j�nak adatai t�rolva?
select * from nikovits.szallit;

select * from dba_objects where owner = 'NIKOVITS' and object_name = 'SZALLIT';

select * from dba_segments where segment_name = 'SZALLIT' and owner = 'NIKOVITS';

select * from dba_extents where segment_name = 'SZALLIT' and owner = 'NIKOVITS';

select * from dba_data_files where file_id = 2;

select * from dba_tablespaces;

select * from dba_free_space where file_id = 2;


--1. feladat
--Melyek azok az objektum t�pusok, amelyek t�nyleges t�rol�st ig�nyelnek, vagyis
--tartoznak hozz�juk adatblokkok? (A t�bbinek csak a defin�ci�ja t�rol�dik adatsz�t�rban)
select distinct object_type from dba_objects where data_object_id is not null;

--2. feladat
--Melyek azok az objektum t�pusok, amelyek nem ig�nyelnek t�nyleges t�rol�st, vagyis nem
--tartoznak hozz�juk adatblokkok? Mi a metszete az el�z� lek�rdez�ssel?
select distinct object_type from dba_objects where data_object_id is null;

select distinct object_type from dba_objects where data_object_id is not null
intersect
select distinct object_type from dba_objects where data_object_id is null;

---------------------------------------- GYAK 3 --------------------------------
select * from (select file_name, bytes from dba_data_files
union
select file_name, bytes from dba_temp_files) order by bytes desc;

--7. feladat: Melyik a legnagyobb m�ret� t�bla szegmens az adatb�zisban (a tulajdonost is adjuk meg) 
--�s h�ny extensb�l �ll? (A particion�lt t�bl�kat most ne vegy�k figyelembe.)
select owner, segment_name, extents, bytes from dba_segments where segment_type = 'TABLE' and bytes = 
(select max(bytes) from dba_segments where segment_type = 'TABLE');

--9. feladat: Adjuk meg adatf�jlonkent, hogy az egyes adatf�jlokban mennyi a foglalt 
--hely osszesen (�rassuk ki a f�jlok m�ret�t is).
select file_name, bytes, foglalt from dba_data_files natural join (select file_id, sum(bytes) foglalt from dba_extents group by file_id);

select * from dba_extents;

select * from dba_segments;

--11. feladat: H�ny extens van a 'users01.dbf' adatf�jlban? Mekkora ezek �sszm�rete?
select count(segment_name), sum(bytes) from dba_extents where file_id = (
select file_id from dba_data_files where file_name like '%users01.dbf%');

--14. Van-e a NIKOVITS felhaszn�l�nak olyan t�bl�ja, amelyik t�bb adatf�jlban is foglal helyet? (Aramis)
select * from dba_extents where owner = 'NIKOVITS';
select segment_name, count(distinct file_id) from dba_extents where owner = 'NIKOVITS' and segment_type = 'TABLE' group by segment_name having count( distinct file_id) > 1;

--20. A NIKOVITS felhaszn�l� CIKK t�bl�ja h�ny blokkot foglal le az adatb�zisban? (ARAMIS)
--(Vagyis h�ny olyan blokk van, ami ehhez a t�bl�hoz van rendelve �s �gy azok m�r m�s t�bl�khoz nem adhat�k hozz�?)
select * from dba_tables where owner = 'NIKOVITS' and table_name = 'CIKK';
select * from dba_extents where owner = 'NIKOVITS' and segment_name = 'CIKK'; --<--

create table kiskutya (lab integer);
select * from dba_segments where owner = 'ZVP7EJ' and segment_name = 'KISKUTYA';
drop table kiskutya;
alter session set deferred_segment_creation = true;

create table kiskutya (lab integer)
pctfree 10 pctused 60
storage (initial 32k minextents 1 maxextents 200 pctincrease 0); --pctincrease: h�ny %-al n�jj�n az �jrafoglal�snak a m�rete

------------------------------------------- GYAK 5 ---------------------------------

create index ind1 on dolgozo(dnev);
create unique index ind2 on dolgozo(dkod);
create index ind3 on dolgozo(fizetes, foglalkozas);
create index ind4 on dolgozo(belepes) reverse;
create index ind8 on dolgozo(fizetes desc);
create index ind5 on dolgozo(foglalkozas, dnev) compress 1; -- az 1 hogy az els� h�ny db oszlopot szeretn�nk t�m�r�teni
create index ind6 on dolgozo(fizetes/12);
create bitmap index ind7 on dolgozo(oazon); --<-- akkor haszn�ljuk, ha rikt�n m�dosul� adataink vannak

select * from user_indexes where table_name = 'DOLGOZO';

--�rjjunk lek�rdez�st ami �sszegzi a masodperc oszlop �rt�keit, ahol a d�tum = "2012.01.31"
select sum(masodperc) from nikovits.hivas where datum = to_date('2012.01.31', 'YYYY.MM.DD');
select sum(masodperc) from nikovits.hivas_v2 where datum = to_date('2012.01.31', 'YYYY.MM.DD');
--az els� t�bb ideig fut, mi�rt? :::

select * from dba_indexes where table_name like 'HIVAS%' and owner = 'NIKOVITS';

select * from dba_ind_columns where table_name like 'HIVAS%' and index_owner = 'NIKOVITS';

--Adjuk meg azon indexek nev�t, amelyek legal�bb 9 oszloposak
select index_name from dba_ind_columns group by index_name having count(*) >= 9;
select index_name from dba_ind_columns where column_position = 9;

-- Adjuk meg azon k�toszlopos indexek nev�t �s tulajdonos�t, amelyeknek legal�bb az egyik kifejez�se f�ggv�ny alap�.
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

-- 1. l�p�s: klaszter l�trehoz�sa
create cluster dolgozo_clust (oazon number(2));

-- 2. l�p�s: t�bla l�trehoz�sa a klaszteren
-- Meg kell adni a klaszter nev�t
create table dolgozocl (dkod number(4), dnev varchar2(10), oazon number(2))
cluster dolgozo_clust(oazon);

-- M�sik t�bla l�trehoz�sa
create table osztalycl (oazon number(2), nev varchar2(14))
cluster dolgozo_clust(oazon);

-- Besz�r�sok a t�bl�kba - nem m�k�dik, mert nincs index
insert into osztalycl select oazon, onev from osztaly;
insert into dolgozocl select dkod, dnev, oazon from dolgozo;

-- 3. l�p�s: index l�trehoz�sa a klaszterhez
create index dolgozocl_ind on cluster dolgozo_clust;

-- Most m�r lehet lek�rdezni �s besz�rni.
-- Eddig ez sem ment
select * from dolgozocl;
select * from osztalycl;

-- K�t sor rowid-ja egyezik, mivel ugyanott t�rol�dnak
SELECT rowid, dnev FROM dolgozocl WHERE oazon=10
UNION
SELECT rowid, nev FROM osztalycl WHERE oazon=10;



-- Hash klaszter l�trehoz�s�hoz t�r�lj�k az eddigieket
drop cluster dolgozo_clust;
drop table dolgozocl;
drop table osztalycl;

-- Hashkeys megad�sa sz�ks�ges
-- Nagyj�b�l a k�l�nb�z� klaszter kulcsok sz�m�ra �rdemes be�ll�teni
create cluster dolgozo_clust (oazon number(2))
hashkeys 5; -- kb 5 k�l�nb�z� kos�rba fogja sz�rni az �rt�keket

-- A t�bl�k l�trehoz�sa ugyan�gy zajlik
create table dolgozocl (dkod number(4), dnev varchar2(10), oazon number(2))
cluster dolgozo_clust(oazon);

create table osztalycl (oazon number(2), nev varchar2(14))
cluster dolgozo_clust(oazon);

-- Nem kell indexet l�trehozni a besz�r�shoz �s a lek�rdez�shez
insert into osztalycl select oazon, onev from osztaly;
select * from osztalycl;

-- Egyt�bl�s klaszter l�trehoz�sa
create cluster dolgozo_single_cluster(oazon number(2))
single table hashkeys 5;

-- T�bla l�trehoz�sa az egyt�bl�s klaszterre
create table dolgozohashcl (dkod number(4), dnev varchar2(10), oazon number(2))
cluster doglozo_single_cluster(oazon);

-- Saj�t hash f�ggv�ny megad�sa
drop table dolgozohashcl;
drop cluster doglozo_single_cluster;

create cluster dolgozo_single_cluster(oazon number(2))
single table hashkeys 3 
hash is MOD(oazon, 3);

-- Inform�ci�k lek�rdez�se a klaszterekr�l
SELECT cluster_name, cluster_type, function, hashkeys, single_table
FROM dba_clusters WHERE owner='VDANI';

-- Kalszteren l�v� t�bl�k a dba_tables-b�l
SELECT cluster_name, table_name FROM dba_tables 
WHERE owner='VDANI' AND cluster_name IS NOT NULL;

-- Klaszter oszlopok lek�rdez�se
SELECT cluster_name, clu_column_name, table_name, tab_column_name 
FROM dba_clu_columns WHERE owner='VDANI';

-- Saj�t hash f�ggv�nyek
SELECT cluster_name, hash_expression 
FROM dba_cluster_hash_expressions WHERE owner='VDANI';

-- Inform�ci�k a clusterekr?l a katal�gusban (adatsz�t�r n�zetekben): 
-- DBA_CLUSTERS
-- DBA_TABLES (cluster_name oszlop -> melyik klaszteren van a t�bla) 
-- DBA_CLU_COLUMNS (t�bl�k oszlopainak megfeleltet�se a klaszter kulcs�nak)
-- DBA_CLUSTER_HASH_EXPRESSIONS (hash klaszterek hash f�ggv�nyei)

--2. feladat
--Adjunk meg egy olyan clustert az adatb�zisban (ha van ilyen), amelyen m�g nincs egy t�bla sem.
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


--1. feladat: �rjunk olyan lek�rdez�seket, amelyek v�grehajt�si terv�ben el�fordulnak a k�vetkez� m�veletek:
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
-- mennyi ideig tart a hivas t�bla teljes v�gigolvas�sa?
select sum(masodperc) from nikovits.hivas;

select sum(masodperc) from nikovits.hivas_v2;


-- hogyan v�ltozik a lefut�si id�, ha adunk meg felt�telt a d�tum oszlopra?
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

-- Bence t�bl�j�val
select * from xj66cw.tr;

update xj66cw.tr set x = 51;

commit;


--Melyik session milyen z�rol�st tart fenn jelen pillanatban �s mi�ta?
SELECT se.sid, se.username, lo.type, lo.lmode, lo.ctime
FROM v$lock lo, v$session se
WHERE se.sid = lo.sid AND username = 'ZVP7EJ';

--Melyik session v�r �ppen egy z�rol�sra, melyik session-re v�rnak �ppen �s milyen r�gen birtokolja a z�rat, illetve v�rnak r�?
SELECT se.sid, se.username, lo.type, lo.lmode,
lo.request, lo.ctime, block
FROM v$lock lo, v$session se
WHERE se.sid = lo.sid AND username = 'VDANI';


LOCK TABLE xj66cw.tr IN EXCLUSIVE MODE;

LOCK TABLE tr IN EXCLUSIVE MODE;

rollback;