<?php

session_start();

include_once 'storage.php';
$storage = new Storage(new JsonIO('recipes.json'));
$recipe = $storage->findById($_GET['food'] ?? '');

if ($recipe != null) {
    $_SESSION['fridge'] = array_diff($_SESSION['fridge'], $recipe);
}

header('Location: index.php');
die();