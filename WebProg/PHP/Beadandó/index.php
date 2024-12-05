<?php
session_start();

@include_once("storage.php");
@include_once("types.php");

$storage = new Storage(new JsonIO("pokemon.json"));
$pokemons = $storage->findAll();

?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>IKémon | Home</title>
    <link rel="stylesheet" href="styles/main.css">
    <link rel="stylesheet" href="styles/index.css">
    <link rel="stylesheet" href="styles/button.css">
    <link rel="stylesheet" href="styles/cards.css">
</head>
    <header>
        <h1><a href="index.php">IKémon</a> > Home</h1>
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
    <?php var_dump($_POST); ?>
    <div id="content">
        <div class="separateContent">
            <form method="post">
                <select name="type" size="1">
                    <option value="any"
                        <?php if(isset($_POST['type']) && "any" == $_POST['type'] ): ?> 
                            selected="selected"
                        <?php endif; ?>>
                        Any
                    </option>
                    <?php foreach($types as $var => $type ): ?>
                        <option value="<?= $var ?>"
                            <?php if(isset($_POST['type']) && $var == $_POST['type'] ): ?> 
                                selected="selected"
                            <?php endif; ?>>
                            <?= $type ?>
                        </option>
                    <?php endforeach; ?>
                </select>
                <button class="button-52" type="submit">Apply Filter</button>
            </form>
        </div>
        <div class="mainContent">
        <?php foreach($pokemons as $poke): ?>
            <?php if(!isset($_POST['type']) || $_POST['type'] == "any" || $_POST['type'] == $poke['type']): ?>
                <div class="pokemon-card">
                    <div class="image clr-<?= $poke['type'] ?>">
                        <img src=<?= $poke['image'] ?> alt="">
                    </div>
                    <div class="details">
                        <h2><a href="details.php?id=<?= $poke['id'] ?>"><?= $poke['name'] ?></a></h2>
                        <span class="card-type"><span class="icon">🏷</span> <?= $poke['type'] ?></span>
                        <span class="attributes">
                            <span class="card-hp"><span class="icon">❤</span> <?= $poke['hp'] ?></span>
                            <span class="card-attack"><span class="icon">⚔</span> <?= $poke['attack'] ?></span>
                            <span class="card-defense"><span class="icon">🛡</span> <?= $poke['defense'] ?></span>
                        </span>
                    </div>
                    <div class="buy">
                        <span class="card-price"><span class="icon">💰</span>
                            <?php if(isset($_SESSION['name']) && $poke['owner'] == $_SESSION['name']): ?>
                                Owned
                            <?php else: ?>
                                <a href="buycard.php?id=<?= $poke['id'] ?>" style="text-decoration:none;"><?= $poke['price'] ?></a>
                            <?php endif; ?>
                        </span>
                    </div>
                </div>
            <?php endif; ?>
        <?php endforeach; ?>
        </div>
        <?php if(isset($_SESSION['name']) && $_SESSION['name'] != "admin"): ?>
            <div class="separateContent">
                <button id="buyRandomCard" class="button-52" type="button">
                    Buy Random (<span class="icon">💰</span>275)
                </button>
                <script src="ajax_sender.js"></script>
            </div>
        <?php endif; ?>
    </div>

    <footer>
        <p>IKémon | ELTE IK Webprogramozás | Márton Gergő</p>
    </footer>
</body>