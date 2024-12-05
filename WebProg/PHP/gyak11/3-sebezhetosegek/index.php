<?php
include_once 'storage.php';
$storage = new Storage(new JsonIO('vulnerabilities.json'));
$vulnerabilities = $storage->findAll();
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="src/index.css">
    <title>3. feladat - Sebezhetőségek</title>
</head>
<body>
    <h1>3. feladat - Sebezhetőségek</h1>
    <div id="introduction">
        <img src="src/character.png">
        <span>Keressünk sebezhetőségeket<br>a Halálcsillag terveiben!</span>
    </div>

    <h2>Sebezhetőség hozzáadása</h2>
    <form action="A-add.php">
        <label for="shortdesc">Rövid név</label>
        <input name="shortdesc" placeholder="Gyenge pajzs">
        
        <br>
        
        <label for="danger">Komolyság</label>
        <input name="danger" type="number" min="1" max="5" placeholder="3">

        <br>

        <label for="longdesc">Leírás</label>
        <textarea name="longdesc">Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</textarea>

        <br>

        <label for="image">Kép</label>
        <input name="image" placeholder="src/error0.png">
        <!-- src/error0.png src/error1.png src/error2.png src/error3.png -->

        <input type="submit" value="Sebezhetőség adminisztrálása">
    </form>

    <h2>Sebezhetőségek</h2>
    <ul>
        <?php foreach ($vulnerabilities as $v): ?>
        <li>
            <a href="D-details.php?id=<?= $v['id'] ?>"><?= $v['shortdesc'] ?></a>
        </li>
        <?php endforeach; ?>
    </ul>
    
</body>
</html>