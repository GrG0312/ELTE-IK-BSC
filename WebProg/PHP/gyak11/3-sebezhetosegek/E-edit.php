<?php

include_once 'storage.php';
$storage = new Storage(new JsonIO('vulnerabilities.json'));

$storage->update($_GET['id'], $_GET);

// header('Location: D-details.php?id=' . $_GET['id']);
// exit();

?>