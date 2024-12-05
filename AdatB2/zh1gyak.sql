------------------------- Papíros rész

------------------------- Gépes rész

--1. feladat: Adjuk meg azokat a táblákat, amelyeknek 26 különbözõ típusú oszlopuk van.
--tulajdonos, táblanév
select * from dba_tables;
select * from dba_tab_columns;

select owner, table_name from dba_tab_columns group by owner, table_name having count(distinct data_type) = 26; --helyes

--2. feladat: Hány olyan felhasználó van az adatbázisban, akinek van olyan táblája, amely több adatfájlban is helyet foglal?
select * from dba_data_files;
select * from dba_tables;
select * from dba_extents;
select * from dba_segments;

select count(distinct owner) from 
    (select owner from dba_extents where segment_type = 'TABLE' group by owner, segment_name having count(distinct file_id) > 1); -- helyes

select owner from dba_extents where segment_type = 'TABLE' group by owner, segment_name having count(distinct file_id) > 1; --sub

SELECT COUNT(DISTINCT owner) --megoldás
FROM (
    SELECT owner, segment_name
    FROM dba_extents
    WHERE segment_type = 'TABLE'
    GROUP BY owner, segment_name
    HAVING COUNT(DISTINCT file_id) > 1
);

--3. feadat: Hány extents van lefoglalva a VDANI felhasználónak a users02.dbf adatfájlban?
--extensek száma.
select * from dba_data_files;
select * from dba_extents;

select count(*) from dba_extents where owner = 'VDANI' and file_id = (select file_id from dba_data_files where file_name like '%users02.dbf%'); -- helyes

SELECT COUNT(*)
FROM dba_extents
WHERE owner = 'VDANI'
AND file_id = (
    SELECT file_id 
    FROM dba_data_files 
    WHERE file_name LIKE '%users02.dbf%'
);
--4. feladat: Adjuk meg azokat a klasztereket, amelyekhez több, mint 10 tábla tartozik.
select * from dba_clusters;
select * from dba_tables;

select owner, cluster_name from dba_tables where cluster_name is not null group by owner, cluster_name having count(table_name) > 10;

SELECT owner, cluster_name
FROM dba_tables
GROUP BY owner, cluster_name
HAVING COUNT(table_name) > 10
and cluster_name is not null;

--5. feladat: Írjunk meg egy PL/SQL procedúrát, amelyik kiírja, hogy a NIKOVITS.HALLGATOK táblának
--melyek azok az adattblokkjai, amelyekben nincs egyetlen sor sem.
--fájl id, blokk id felsorolva

select * from dba_extents;
select * from dba_tablespaces;

select file_id, block_id, blocks from dba_extents where segment_name = 'HALLGATOK';

set serveroutput on;

create or replace procedure nikovits_ures is
    cursor exs is select file_id, block_id, blocks
                from dba_extents
                where owner='NIKOVITS'
                and segment_name='HALLGATOK';
                
    cursor foglalt is 
        select 
            dbms_rowid.rowid_relative_fno(ROWID) fid,
            dbms_rowid.rowid_block_number(ROWID) bid
        from nikovits.hallgatok
        group by 
            dbms_rowid.rowid_relative_fno(ROWID),
            dbms_rowid.rowid_block_number(ROWID);
    vanbenne boolean;
begin
    for ext in exs loop --minden extensre
        for blocc in ext.block_id .. ext.block_id + 7 loop --max 8 db (0-7ig) blokk?? miért nem ext.block_id + ext.blocks-ig??
        
            vanbenne := false;
            for fog in foglalt loop
                if fog.bid = blocc and fog.fid = ext.file_id then
                    vanbenne := true;
                end if;
            end loop;
            
            if not vanbenne then
                dbms_output.put_line(ext.file_id || ', ' || blocc);
            end if;
            
        end loop;
    end loop;
end;
/

execute nikovits_ures;