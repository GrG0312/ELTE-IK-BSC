-- G�PES R�SZ
-- 1. feladat:
--a) K�sz�ts egy saj�t t�bl�t dolgozo_zh n�ven a VDANI.DOLGOZO_ZH t�bla alapj�n. (1 pont)
create table dolgozo_zh as select * from vdani.dolgozo_zh;
--b) Hozz l�tre tetsz�leges t�pus� indexet a l�trehozott t�bl�d valamelyik oszlopra. (4 pont)
create index zh_index_1 on dolgozo_zh(fizetes desc);
--c) �rj egy lek�rdez�st, amely haszn�lja a l�trehozott indexet (ak�r hintek seg�ts�g�vel), �s mutasd meg a v�grehajt�si terv�t. (5 pont)
select * from dolgozo_zh where fizetes > 2000;

explain plan set statement_id = 'zh2_plan_1' for
select /*+ index(zh2_plan_1) */ * from dolgozo_zh where fizetes > 2000;

select plan_table_output from table(dbms_xplan.display('plan_table', 'zh2_plan_1', 'typical'));


--2 feladat:
--Az al�bbi feladatokhoz haszn�ld a VDANI.DOLGOZO_ZH �s VDANI.OSZTALY_ZH t�bl�kat.
--Minden feladathoz m�sold be a lek�rdez�st �s a v�grehajt�si tervet is.

--a) Add meg egy lek�rdez�sben azoknak a dolgoz�knak a nev�t, akik fizet�se nagyobb, mint 2000 �s a NEW YORK-i telephelyen dolgoznak. A lek�rdez�sben haszn�lj �sszekapcsol�st.
select * from vdani.osztaly;
select * from vdani.dolgozo_zh dolg join VDANI.OSZTALY oszt on dolg.oazon = oszt.oazon where fizetes > 2000 and telephely = 'NEW YORK';
--b) Add meg �gy a lek�rdez�st, hogy az �sszekapcsol�s NESTED LOOP algoritmussal t�rt�njen.
select /*+ use_nl(dolg, oszt) */ * from vdani.dolgozo_zh dolg join VDANI.OSZTALY oszt on dolg.oazon = oszt.oazon where fizetes > 2000 and telephely = 'NEW YORK';

explain plan set statement_id = 'zh2_plan_2' for
select /*+ use_nl(dolg, oszt) */ * from vdani.dolgozo_zh dolg join VDANI.OSZTALY oszt on dolg.oazon = oszt.oazon where fizetes > 2000 and telephely = 'NEW YORK';

select plan_table_output from table(dbms_xplan.display('plan_table', 'zh2_plan_2', 'typical'));
--c) Add meg �gy a lek�rdez�st, hogy az �sszekapcsol�s MERGE JOIN algoritmussal t�rt�njen.
