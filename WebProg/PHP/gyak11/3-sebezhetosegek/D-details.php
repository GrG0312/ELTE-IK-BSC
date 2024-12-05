<?php
include_once 'storage.php';
$storage = new Storage(new JsonIO('vulnerabilities.json'));
$v = $storage->findById($_GET['id'] ?? '');

// print_r($v);
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="src/index.css">
    <title>3. Feladat - Sebezhetőségek</title>
</head>
<body>
    <h1>3. Feladat - Sebezhetőségek</h1>
    <div id="introduction">
        <img src="src/character.png">
        <span>Oh, remek, ez egy sebezhetőség!</span>
    </div>

    <h2><?= $v['shortdesc'] ?> (<?= $v['danger'] ?>)</h2>
    <div id="vulnerability">
        <img src="<?= $v['image'] ?>">
        <span>
            <?= $v['longdesc'] ?>
        </span>
    </div>
    
    <a href="index.php">Vissza a listához</a>
    
    <h2>Szerkesztés</h2>
    <form action="E-edit.php">
        <input type="hidden" name="id" value="<?= $v['id'] ?>">

        <label for="shortdesc">Rövid név</label>
        <input name="shortdesc"
               value="<?= $v['shortdesc'] ?>"
               >
        
        <br>
        
        <label for="danger">Komolyság</label>
        <input name="danger" type="number" min="1" max="5"
            value="<?= $v['danger'] ?>">

        <br>

        <label for="longdesc">Leírás</label>
        <textarea name="longdesc"><?=
        $v['longdesc']
        ?></textarea>

        <br>

        <label for="image">Kép</label>
        <input name="image"
            value="<?= $v['image'] ?>">

        <input type="submit" value="Sebezhetőség szerkesztése">
    </form>
</body>
</html>