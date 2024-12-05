<?php 
session_start(); 
@include("storage.php");
$storage = new Storage(new JsonIO("pokemon.json"));
$cardsOfUser;
function GetCards(&$cardsOfUser){
    global $storage;
    foreach($_SESSION['cards'] as $id){
        $cardsOfUser[$id] = $storage->findById($id);
    }
}

GetCards($cardsOfUser);
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>IKémon | Profile</title>
    <link rel="stylesheet" href="styles/main.css">
    <link rel="stylesheet" href="styles/cards.css">
    <link rel="stylesheet" href="styles/button.css">
    <link rel="stylesheet" href="styles/profile.css">
</head>
<body>
    <header>
        <h1><a href="index.php">IKémon</a> > <?= $_SESSION['name'] ?></h1>
        <a href="logout.php"><button class="button-52" type="button">Log out</button></a>
    </header>
</body>
<body>
    <div id="content">
        <div id="profileOverview">
            <div id="nameAndCards">
                <div>
                    <h2><?= $_SESSION['name'] ?></h2>
                    <?php if($_SESSION['name'] != 'admin'): ?>
                        <p id="email"><?= $_SESSION['email'] ?></p>
                    <?php endif; ?>
                </div>
                <p>
                    <span type="icon">🎴</span> Cards: 
                    <span style="<?= count($_SESSION['cards']) >= 5 && $_SESSION['name'] != "admin" ? "color:red;font-weight:bold;" : "color:green;" ?>"><?= count($_SESSION['cards']) ?></span>
                    <span style="<?= count($_SESSION['cards']) >= 5 && $_SESSION['name'] != "admin" ? "color:red;font-weigth:bold;" : "color:black;" ?>">/ <?= $_SESSION['name'] == "admin" ? "Unlimited" : "5" ?></span>
                </p>
            </div>
            <div id="money">
                <span class="icon">💰</span> <?= isset($_SESSION['money']) ? $_SESSION['money'] : "Unlimited" ?>
            </div>
        </div>
        <?php if($_SESSION['name'] == 'admin'): ?>
            <div id="new-card">
                <a href="createcard.php" ><button class="button-52" type="button">Create New Card</button></a>
            </div>
        <?php endif; ?>
        <div id="cardsOfUser">
            <?php if($cardsOfUser != null): ?>
                <?php foreach($cardsOfUser as $card): ?>
                    <div class="pokemon-card">
                        <div class="image clr-<?= $card['type'] ?>">
                            <img src=<?= $card['image'] ?> alt="">
                        </div>
                        <div class="details">
                            <h2><a href="<?= $_SESSION['name'] == "admin" ? "createcard" : "details" ?>.php?id=<?= $card['id'] ?>"><?= $card['name'] ?></a></h2>
                            <span class="card-type"><span class="icon">🏷</span> <?= $card['type'] ?></span>
                            <span class="attributes">
                                <span class="card-hp"><span class="icon">❤</span> <?= $card['hp'] ?></span>
                                <span class="card-attack"><span class="icon">⚔</span> <?= $card['attack'] ?></span>
                                <span class="card-defense"><span class="icon">🛡</span> <?= $card['defense'] ?></span>
                            </span>
                        </div>
                        <?php if($_SESSION['name'] != 'admin'): ?>
                            <div class="buy sell">
                                <span class="card-price"><span class="icon">💰</span> <a href="quicksell.php?id=<?= $card['id'] ?>">Owned</a></span>
                            </div>
                        <?php else: ?>
                            <div class="buy">
                                <span class="card-price"><span class="icon">💰</span> Owned</span>
                            </div>
                        <?php endif; ?>
                    </div>
                <?php endforeach; ?>
            <?php endif; ?>
        </div>
        <?php if($_SESSION['name'] != "admin"): ?>
            <div id="offersDiv">
                <?php foreach($_SESSION['offers'] as $offer): ?>
                    <div class="offerBox">
                        
                    </div>
                <?php endforeach; ?>
            </div>
        <?php endif; ?>
    </div>
    <footer>
        <p>IKémon | ELTE IK Webprogramozás | Márton Gergő</p>
    </footer>
</body>