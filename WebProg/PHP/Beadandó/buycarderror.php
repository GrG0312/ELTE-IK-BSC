<?php
session_start();
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>IKémon | Error</title>
    <link rel="stylesheet" href="styles/main.css">
    <link rel="stylesheet" href="styles/trade.css">
    <link rel="stylesheet" href="styles/button.css">
</head>
<body>
    <header>
        <h1><a href="index.php">IKémon</a> > Error</h1>
        <?php if(!isset($_SESSION['name'])): ?>
            <span>Please <a href="login.php">log in</a></span>
        <?php else: ?>
            <div id="username">
                <a href="profile.php"><?= $_SESSION['name'] ?></a>
                <?php if($_SESSION['isAdmin'] == false): ?>
                    <span><span class="icon">💰</span> <?= $_SESSION['money'] ?></span>
                <?php endif; ?>
            </div>
        <?php endif; ?>
    </header>
    <div id="content">
        <div id="errorMessage">
            <?php if($_GET['reason'] == 'admin'): ?>
                <p>An admin cannot purchase cards! Pro tip: create your own!</p>
            <?php endif; ?>
            <?php if($_GET['reason'] == 'full'): ?>
                <p>You have reached your maximum card capacity! Sell some cards before continuing to purchase new ones.</p>
            <?php endif; ?>
            <?php if($_GET['reason'] == 'money'): ?>
                <p>Unfortunately you do not have enough gold to buy the card.</p>
            <?php endif; ?>
        </div>
        <div id="return">
            <a href="index.php"><button type="button" class="button-52">Return to Home page</button></a>
        </div>
    </div>
    <footer>
        <p>IKémon | ELTE IK Webprogramozás | Márton Gergő</p>
    </footer>
</body>