drop table dolgozo;
create table dolgozo as
select * from vdani.dolgozo;

select * from dolgozo;


--N�velj�k meg a fizet�s�t azoknak, akik jutal�ka null, vagy a fizet�s�k kisebb az �tlagn�l
update dolgozo set fizetes = fizetes + 500 
where jutalek is null 
    or fizetes < (SELECT AVG(fizetes) FROM dolgozo);
  
    
--N�velj�k meg azon dolgoz�k jutal�k�t 3k-val, akiknek legal�bb 2 k�zvetlen dolgoz�juk van. 
--A null jutal�kot kezelj�k nullak�nt
update dolgozo kulso 
set jutalek = coalesce(jutalek, 0) + 3000
WHERE 2 <= (select count(dnev) from dolgozo belso where belso.fonoke = kulso.dkod);

--SEG�D: k�rdezz�k le azokat a dolgoz�kat akiknek 2 vagy 2-n�l t�bb k�zvetlen alkalmazottjuk van
select dnev, jutalek from dolgozo kulso where 2 <= (select count(dnev) from dolgozo belso where belso.fonoke = kulso.dkod);


--N�velj�k meg a nem-f�n�k�k fizet�s�t a f�n�k�k fizet�s�nek 10%-�val
update dolgozo kulso set fizetes = fizetes + (select fizetes from dolgozo ff where ff.dkod = kulso.fonoke) / 10
where kulso.dnev in (select dnev from dolgozo kulso where 0 = (select count(dnev) from dolgozo belso where belso.fonoke = kulso.dkod));

--SSEG�D: k�rdezz�k le a nem f�n�k dolgoz�kat
select dnev from dolgozo kulso where 0 = (select count(dnev) from dolgozo belso where belso.fonoke = kulso.dkod);
--SEG�D: fizetes / 10
select fizetes / 10 from dolgozo;


drop table osztaly;
create table osztaly as select * from vdani.osztaly;

select * from osztaly;
select * from VDANI.fiz_kategoria;

select dnev, telephely from dolgozo d join osztaly o on d.oazon = o.oazon where telephely = 'DALLAS';


--T�r�lj�k ki azokat a dolgoz�kat, akiknek az oszt�ly�nak a telephelye DALLAS
delete from dolgozo
where dnev in (select dnev from dolgozo d join osztaly o on d.oazon = o.oazon where telephely = 'DALLAS');

rollback;

select * from vdani.fiz_kategoria;

--T�r�lj�k ki azokat az oszt�lyokat, amelyeknek 2 olyan dolgoz�ja van, akik 2es fizkatba esnek
delete from osztaly
where oazon in (select oazon from (select oazon, count(dnev) twos from dolgozo d join vdani.fiz_kategoria f on d.fizetes between f.also and f.felso where kategoria = 2 group by oazon) where twos = 2);

-- SEG�D: k�rdezz�k le az oszt�lyokat a kettes kateg�ri�s dolgoz�kkal egy�tt
select oazon, count(dnev) twos from dolgozo d join vdani.fiz_kategoria f on d.fizetes between f.also and f.felso where kategoria = 2 group by oazon;


--T�r�lj�nk minden olyan dolgoz�t, akik az oszt�lyukban legrosszabbul keresnek
delete from dolgozo
where (oazon, fizetes) in (select oazon, min(fizetes) from dolgozo group by oazon);
--SEG�D: k�rdezz�k le az oszt�lyokat �s a minimumkeresetet ott
select oazon, min(fizetes) from dolgozo group by oazon;


--F�ggv�ny azonos helyen �ll� karakterek megsz�mol�s�ra
/
create or replace function ZH2 (word1 varchar2, word2 varchar2) return integer
is
    lenw1 integer := length(word1);
    lenw2 integer := length(word2);
    retval integer := 0;
    loopsize integer := least(lenw1, lenw2);
begin
    if lenw1 != lenw2 then
        return -1;
    end if;
    for i in 1..lenw1 loop
        if substr(word1, i, 1) = substr(word2, i, 1) then
            retval := retval + 1;
        end if;
    end loop;
    return retval;
end;
/

select ZH2('alma', 'k�rte') matching from dual;

select * from dolgozo;

--Procedure: kurzorral param�ter�l kapva az oszt�lyn�v utols� 2 bet�j�t,
-- aki abban dolgozik n�veli a fizuj�t 15%al
create or replace procedure ZH4 (oname varchar2) is
    cursor k is 
        select * from dolgozo d join osztaly o
        on d.oazon = o.oazon
        for update;
begin
    for sor in k loop
        if oname = substr(sor.onev, length(sor.onev) - 1) and sor.fizetes >= 4000 then
            update dolgozo
            set fizetes = fizetes * 1.15
            where current of k;
        end if;
    end loop;
end;
/

execute ZH4('CH');


--F�ggv�ny: felh.n�v, azon, jelszo
create or replace function ZH5 (felh varchar2, kod integer, jelszo varchar2) is
    fonokeKod integer;
begin
    if felh in (select dnev from dolgozo) then
        fonokeKod = (select fonoke from dolgozo where dnev = felh);
        if 0 = ZH5_resz(felh, fonokeKod, jelszo) then
            return 1;
        end if;
    end if;
    return 0;
end;
/

create or replace function ZH5_resz (felh varchar2, kod integer, jelszo varchar2) return integer is
    reversed varchar2(20) := '';
begin
    for i in 1..length(jelszo) loop
        reversed := substr(jelszo, i, 1) || reversed;
    end loop;
    if (reversed, kod) = (select dnev, dkod from dolgozo where dkod = (select fonoke from dolgozo where dnev = felh)) then
        return 0;
    end if;
    return 1;
end;
/
select ZH5('BLAKE',7839,'GNIK') from dual;



------------------------------------------ 2. ZH ---------------------------------------------
drop table orszagok;
create table orszagok as select * from vdani.orszagok;

select * from orszagok;

alter table orszagok add GDP_PER_FO integer;

update orszagok set GDP_PER_FO = GDP / nepesseg 
where gdp > 1;


--2. feladat
drop table orszagok;
create table orszagok as select * from vdani.orszagok;
select * from orszagok;

select regio, max(terulet) from orszagok group by regio;

delete from orszagok
where (regio, terulet) not in (select regio, max(terulet) from orszagok group by regio);


--3. feladat
drop table orszagok;
create table orszagok as select * from vdani.orszagok;
select * from orszagok where regio = 'Middle East' order by terulet desc;

select regio, avg(gdp) avarage from orszagok group by regio;
select avarage from (select regio, avg(gdp) avarage from orszagok group by regio);

delete from orszagok kulso
where kulso.gdp < 
    (select avarage 
    from (select regio, avg(gdp) avarage from orszagok group by regio) he 
    where he.regio = kulso.regio );


--4. feladat
/
create or replace function haromszog (x integer) return integer is
    partialSum integer := 0;
begin
    for i in 1..x loop
        partialSum := partialSum + i;
        if partialSum = x then
            return 1;
        end if;
    end loop;
    return 0;
end;
/

select haromszog(10) from dual;


--5. feladat
drop table orszagok;
create table orszagok as select * from vdani.orszagok;
select * from orszagok order by terulet desc;

set SERVEROUTPUT ON;

create or replace procedure legnagyobbRegioban is
    cursor k is select regio from orszagok group by regio;
    cursor helper is select nev, regio from orszagok order by terulet desc;
    retNum integer := 0;
    output varchar2(4000) := '';
begin
    for reg in k loop
        output := '' || reg.regio || ': ';
        retNum := 0;
        for sor in helper loop
            if sor.regio = reg.regio and retNum != 3 then
                retNum := retNum + 1;
                output := output || sor.nev || ', ';
            end if;
        end loop;
        DBMS_OUTPUT.PUT_LINE(output);
    end loop;
end;
/

execute legnagyobbRegioban;

--6. feladat
drop table magassag;
create table magassag as select * from vdani.magassag;
select * from magassag order by x;

create or replace function volgy return integer is
    retval integer := 0;
    cursor k is select * from magassag order by x;
    lastH integer := -1;
    middleH integer := -1;
    firstH integer := -1;
begin
    for sor in k loop
        lastH := middleH;
        middleH := firstH;
        firstH := sor.y;
        if lastH > middleH and firstH > middleH then
            retval := retval + 1;
        end if;
    end loop;
    return retval;
end;
/

select volgy from dual;