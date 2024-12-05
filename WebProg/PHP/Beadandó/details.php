<?php
session_start();
    @include_once 'storage.php';
    $storage = new Storage(new JsonIO('pokemon.json'));
    $poke = $storage->findById($_GET['id'] ?? '');
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>IKémon | <?= $poke['name'] ?></title>
    <link rel="stylesheet" href="styles/main.css">
    <link rel="stylesheet" href="styles/details.css">
    <link rel="stylesheet" href="styles/cards.css">
</head>
    <header>
        <h1><a href="index.php">IKémon</a> > <?= $poke['name'] ?></h1>
        <div id="username">
            <a href="profile.php"><?= $_SESSION['name'] ?></a>
            <?php if($_SESSION['isAdmin'] == false): ?>
                <span><span class="icon">💰</span> <?= $_SESSION['money'] ?></span>
            <?php endif; ?>
        </div>
    </header>
    <div id="content" class="detailsContent">
        <img class="clr-<?= $poke['type'] ?>" src=<?= $poke['image'] ?> alt="">
        <div class="detailsDesc">
            <span>
                <h2><?= $poke['name'] ?></h2>
                <p>
                    <span class="icon">🏷</span> <?= $poke['type'] ?>
                    <span class="card-hp"><span class="icon">❤</span> <?= $poke['hp'] ?></span>
                    <span class="card-attack"><span class="icon">⚔</span> <?= $poke['attack'] ?></span>
                    <span class="card-defense"><span class="icon">🛡</span> <?= $poke['defense'] ?></span>
                </p>
            </span>
            <p><?= $poke['description'] ?></p>
            <p>Current owner: <a id="name"><?= $poke['owner'] ?></a></p>
        </div>
    </div>

    <footer>
        <p>IKémon | ELTE IK Webprogramozás | Márton Gergő</p>
    </footer>
</body>