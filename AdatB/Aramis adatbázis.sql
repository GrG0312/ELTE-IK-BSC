select * from vdani.dolgozo;
/*
* - minden oszlop
vdani.dolgozo - vdani felhaszn�l�(?) dolgoz� t�bl�ja
*/

CREATE TABLE dolgozo AS SELECT * FROM vdani.dolgozo;
/*
AS - ez ut�ni lek�rdez�s visszat�r�si �rt�k�t adja tov�bb a create-nek
drop table-el t�r�lsz
*/

SELECT * FROM dolgozo;