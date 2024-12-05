<?php
session_start();

@include("storage.php");
$storageOfCards = new Storage(new JsonIO("pokemon.json"));
$targetCard = $storageOfCards->findById($_GET['id']);

$storageOfUsers = new Storage(new JsonIO('users.json'));
$targetUser;

if(!isset($_SESSION["name"])){
    header("Location: login.php");
    die();
} elseif ($_SESSION['name'] == 'admin'){
    header('Location: buycarderror.php?reason=admin');
    die();
} elseif ($targetCard['owner'] == "admin" && count($_SESSION['cards']) < 5 && $_SESSION['money'] >= $targetCard['price']){
    global $storageOfUsers;
    $admin = $storageOfUsers->findById("admin");

    $_SESSION['money'] = $_SESSION['money'] - $targetCard['price'];
    array_push($_SESSION['cards'], $targetCard['id']);

    $newCardSet = [];
    foreach ($admin['cards'] as $card) {
        if($card != $targetCard['id']){
            array_push($newCardSet, $card);
        }
    }
    unset($admin['cards']);
    $admin['cards'] = $newCardSet;


    $storageOfUsers->update($admin['name'], $admin);
    $storageOfUsers->update($_SESSION['name'], $_SESSION);

    $targetCard['owner'] = $_SESSION['name'];
    $storageOfCards->update($targetCard['id'], $targetCard);

    header('Location: index.php');
    die();

} elseif (count($_SESSION['cards']) >= 5){
    header('Location: buycarderror.php?reason=full');
    die();
} elseif($_SESSION['money'] < $targetCard['price']){
    header('Location: buycarderror.php?reason=money');
    die();
} else {
    $targetUser = $storageOfUsers->findById($targetCard['owner']);
}

function GetCardName($id){
    $storageOfCards = new Storage(new JsonIO("pokemon.json"));
    $card = $storageOfCards->findById($id);
    return $card["name"];
}

$errors = [];

if(isset($_POST['gold_give'])){
    if($_POST['gold_give'] < 0 || $_POST['gold_give'] > $_SESSION['money']){
        $errors['gold_give'] = "You have entered an invalid number! Please try again!";
    }
    if($_POST['gold_get'] < 0 || $_POST['gold_get'] > $targetUser['money']){
        $errors['gold_get'] = "You have entered an invalid number! Please try again!";
    }

    if($errors == []){
        $newOffer = [];
        $newOffer['gold_get'] = $_POST['gold_give'];
        $newOffer['gold_give'] = $_POST['gold_get'];
        $newOffer['card_give'] = $targetCard['id'];
        if(isset($_POST['card_give'])){
            $newOffer['card_get'] = $_POST['card_give'];
        }
        $newOffer['id'] = uniqid();
        $targetUser['offers'][$newOffer['id']] = $newOffer;


        $storageOfUsers->update($targetUser['name'], $targetUser);
        header('Location: index.php');
        die();
    }
}

?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>IKémon | Trading</title>
    <link rel="stylesheet" href="styles/main.css">
    <link rel="stylesheet" href="styles/cards.css">
    <link rel="stylesheet" href="styles/trade.css">
    <link rel="stylesheet" href="styles/button.css">
</head>
<body>
    <header>
        <h1><a href="index.php">IKémon</a> > Trading for <?= $targetCard['name'] ?></h1>
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
    <?= var_dump($errors); ?>
    <div id="content">
        <form id="my_offer" method="post">
            <div class="offer">
                <div class="form_element">
                    <span><span class="icon">💰</span>Gold:</span>
                    <span>
                        <input type="number" name="gold_give" value="<?= isset($_POST['gold']) ? $_POST['gold'] : "" ?>" min="0" max="<?= $_SESSION['money'] ?>"/>
                        / <?= $_SESSION['money'] ?>
                    </span>
                    <?php if(isset($errors['gold_give'])): ?>
                        <span class="errorText"><?= $errors['gold_give'] ?></span>
                    <?php endif; ?>
                </div>
                <div class="form_element">
                    <span><span class="icon">🎴</span>Offered card:</span>
                    <select name="card_give" size="25">
                        <?php foreach($_SESSION['cards'] as $card): ?>
                            <option value="<?= $card ?>">
                                <?= GetCardName($card); ?>
                            </option>
                        <?php endforeach; ?>
                    </select>
                </div>
            </div>
            <div class="offer">
                <div class="form_element">
                    <span><span class="icon">💰</span>Gold:</span>
                    <span>
                        <input type="number" name="gold_get" value="<?= isset($_POST['gold']) ? $_POST['gold'] : "" ?>" min="0" max="<?= $targetUser['money'] ?>"/>
                        / <?= $targetUser['money'] ?>
                    </span>
                    <?php if(isset($errors['gold_get'])): ?>
                        <span class="errorText"><?= $errors['gold_get'] ?></span>
                    <?php endif; ?>
                </div>
                <div class="pokemon-card">
                    <div class="image clr-<?= $targetCard['type'] ?>">
                        <img src=<?= $targetCard['image'] ?> alt="">
                    </div>
                    <div class="details">
                        <h2><?= $targetCard['name'] ?></h2>
                        <span class="card-type"><span class="icon">🏷</span> <?= $targetCard['type'] ?></span>
                        <span class="attributes">
                            <span class="card-hp"><span class="icon">❤</span> <?= $targetCard['hp'] ?></span>
                            <span class="card-attack"><span class="icon">⚔</span> <?= $targetCard['attack'] ?></span>
                            <span class="card-defense"><span class="icon">🛡</span> <?= $targetCard['defense'] ?></span>
                        </span>
                    </div>
                    <div class="buy">
                        <span class="card-price"><span class="icon">💰</span> <?= $targetCard['price'] ?></span>
                    </div>
                </div>
            </div>
        </form>
        <div style="display:flex;justify-content: center;align-content: center;">
            <button form="my_offer" class="button-52" type="submit">Offer trade</button>
        </div>
    </div>
    <footer>
        <p>IKémon | ELTE IK Webprogramozás | Márton Gergő</p>
    </footer>
</body>