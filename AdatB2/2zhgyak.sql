-- GÉPES RÉSZ
-- 1. feladat:
--a) Készíts egy saját táblát dolgozo_zh néven a VDANI.DOLGOZO_ZH tábla alapján. (1 pont)
create table dolgozo_zh as select * from vdani.dolgozo_zh;
--b) Hozz létre tetszõleges típusú indexet a létrehozott táblád valamelyik oszlopra. (4 pont)
create index zh_index_1 on dolgozo_zh(fizetes desc);
--c) Írj egy lekérdezést, amely használja a létrehozott indexet (akár hintek segítségével), és mutasd meg a végrehajtási tervét. (5 pont)
select * from dolgozo_zh where fizetes > 2000;

explain plan set statement_id = 'zh2_plan_1' for
select /*+ index(zh2_plan_1) */ * from dolgozo_zh where fizetes > 2000;

select plan_table_output from table(dbms_xplan.display('plan_table', 'zh2_plan_1', 'typical'));


--2 feladat:
--Az alábbi feladatokhoz használd a VDANI.DOLGOZO_ZH és VDANI.OSZTALY_ZH táblákat.
--Minden feladathoz másold be a lekérdezést és a végrehajtási tervet is.

--a) Add meg egy lekérdezésben azoknak a dolgozóknak a nevét, akik fizetése nagyobb, mint 2000 és a NEW YORK-i telephelyen dolgoznak. A lekérdezésben használj összekapcsolást.
select * from vdani.osztaly;
select * from vdani.dolgozo_zh dolg join VDANI.OSZTALY oszt on dolg.oazon = oszt.oazon where fizetes > 2000 and telephely = 'NEW YORK';
--b) Add meg úgy a lekérdezést, hogy az összekapcsolás NESTED LOOP algoritmussal történjen.
select /*+ use_nl(dolg, oszt) */ * from vdani.dolgozo_zh dolg join VDANI.OSZTALY oszt on dolg.oazon = oszt.oazon where fizetes > 2000 and telephely = 'NEW YORK';

explain plan set statement_id = 'zh2_plan_2' for
select /*+ use_nl(dolg, oszt) */ * from vdani.dolgozo_zh dolg join VDANI.OSZTALY oszt on dolg.oazon = oszt.oazon where fizetes > 2000 and telephely = 'NEW YORK';

select plan_table_output from table(dbms_xplan.display('plan_table', 'zh2_plan_2', 'typical'));
--c) Add meg úgy a lekérdezést, hogy az összekapcsolás MERGE JOIN algoritmussal történjen.
