<?php
session_start();
@include("storage.php");

$storageOfCards = new Storage(new JsonIO("pokemon.json"));
$storageOfUsers = new Storage(new JsonIO("users.json"));

$targetCard = $storageOfCards->findById($_GET['id']);
$targetCard['owner'] = "admin";

$storageOfCards->update($targetCard['id'], $targetCard);

$_SESSION['money'] = $_SESSION['money'] + round($targetCard['price'] * 0.9);

$newCardSet = [];
foreach ($_SESSION['cards'] as $card) {
    if($card != $targetCard['id']){
        array_push($newCardSet, $card);
    }
}
unset($_SESSION['cards']);
$_SESSION['cards'] = $newCardSet;

$storageOfUsers->update($_SESSION['name'], $_SESSION);

$admin = $storageOfUsers->findById('admin');
array_push($admin['cards'], $targetCard['id']);
$storageOfUsers->update($admin['name'], $admin);

header('Location: profile.php');
die();
?>