<?php
session_start();

print_r(array_keys($_POST));

// array_push($_SESSION['fridge'], ...array_keys($_POST));
// $_SESSION['fridge'] = array_unique($_SESSION['fridge']);

$_SESSION['fridge'] = array_unique(
    array_merge(
        $_SESSION['fridge'],
        array_keys($_POST ?? [])
    )
);

header('Location: index.php');
die();