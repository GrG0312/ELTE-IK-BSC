<?php
session_start();
@include_once("storage.php");
$storageOfUsers = new Storage(new JsonIO("users.json"));
$admin = $storageOfUsers->findById("admin");

$randomCard = array_rand($admin['cards'], 1);
echo $admin['cards'][$randomCard];