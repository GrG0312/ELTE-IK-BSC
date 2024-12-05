<?php

include_once 'storage.php';
$storage = new Storage(new JsonIO('vulnerabilities.json'));

$form_data = [
    'shortdesc' => $_GET['shortdesc'] ?? '',
    'danger' => $_GET['danger'] ?? '',
    'longdesc' => $_GET['longdesc'] ?? '',
    'image' => $_GET['image'] ?? ''
];

$id = $storage->add($form_data);

print_r($id);

header('Location: index.php');
die();