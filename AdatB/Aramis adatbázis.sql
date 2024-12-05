select * from vdani.dolgozo;
/*
* - minden oszlop
vdani.dolgozo - vdani felhasználó(?) dolgozó táblája
*/

CREATE TABLE dolgozo AS SELECT * FROM vdani.dolgozo;
/*
AS - ez utáni lekérdezés visszatérési értékét adja tovább a create-nek
drop table-el törölsz
*/

SELECT * FROM dolgozo;