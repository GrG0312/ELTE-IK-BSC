<?php
session_start();
@include("storage.php");
@include("types.php");

$storageOfCards = new Storage(new JsonIO("pokemon.json"));
$errors = [];
function ValidateCard($input){
    global $errors;
    if($input['name'] == ""){
        $errors['name'] = "The new pokemon has to have a name!";
    }

    if($input['hp'] == ""){
        $errors['hp'] = "The new pokemon has to have some HP!";
    } else if($input['hp'] > 999 || $input['hp'] < 1){
        $errors['hp'] = "The HP value must be between 1 and 999!";
    }

    if($input['atk'] == ""){
        $errors['atk'] = "The new pokemon has to have some ATK!";
    } else if($input['atk'] > 999 || $input['atk'] < 1){
        $errors['atk'] = "The ATK value must be between 1 and 999!";
    }

    if($input['def'] == ""){
        $errors['def'] = "The new pokemon has to have some DEF!";
    } else if($input['def'] > 999 || $input['def'] < 1){
        $errors['def'] = "The DEF value must be between 1 and 999!";
    }

    if($input['price'] == ""){
        $errors['price'] = "The new pokemon has to have a price!";
    } else if($input['price'] < 1 || $input['price'] > 9999){
        $errors['price'] = 'The price of a card must be between 1 and 9999 gold!';
    }

    if($input['image'] == ""){
        $errors["image"] = "The new pokemon has to have an image";
    } else {
        if(!filter_var($input["image"], FILTER_VALIDATE_URL)){
            $errors['image'] = "Please provide a valid URL address!";
        } else if(!filter_var($input['image'], FILTER_VALIDATE_REGEXP, [
            "options" => [
                "regexp" => "#^https://assets.pokemon.com/assets/cms2/img/pokedex/full/(\d+)#"
                /* ez állítólag akkor nekem azt fogja megvizsgálni, hogy a pokedex oldaláról van belinkelve egy kép egy pokemonról, úgy, hogy:
                https://assets.pokemon.com/assets/cms2/img/pokedex/full/ ezzel kezdődik
                és van legalább 1 darab számjegy utána
                */
            ]
        ])){
            $errors['image'] = "Please provide a valid picture of a pokemon from: https://www.pokemon.com/us/pokedex";
        }
    }

    if($input["desc"] == ""){
        $errors['desc'] = "The new pokemon has to have a description!";
    } else if(strlen($input['desc']) > 200){
        $errors['desc'] = "The description must be maximum 200 characters long!";
    }
}

$oldCard;
if(isset($_GET['id'])){
    $oldCard = $storageOfCards->findById($_GET['id']);
}

if (!empty($_POST)) {
    ValidateCard($_POST);
    if($errors == []){
        $newCard = array(
            "name" => $_POST['name'],
            "type" => $_POST['type'],
            "hp" => $_POST['hp'],
            "attack" => $_POST['atk'],
            "defense" => $_POST['def'],
            "price" => $_POST['price'],
            "description" => $_POST['desc'],
            "image" => $_POST['image'],
            "owner" => "admin"
        );
        if(isset($_GET['id'])){
            $newCard['id'] = $_GET['id'];
            $storageOfCards->update($_GET['id'], $newCard);
        } else {
            $storageOfCards->add($newCard);
        }

        header("Location: index.php");
        die();
    }
}
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>IKémon | Create Card</title>
    <link rel="stylesheet" href="styles/main.css">
    <link rel="stylesheet" href="styles/button.css">
    <link rel="stylesheet" href="styles/form.css">
</head>
<body>
    <header>
        <h1><a href="index.php">IKémon</a> > Create Card</h1>
    </header>
    <?= var_dump($_POST); ?>
    <div id="content" class="logincontent">
        <div>
            <h2>Card Attributes</h2>
            <form method="post">
                <div class="login_field"><span>Name:</span><input type="text" name="name" value="<?= isset($_POST['name']) ? $_POST['name'] : (isset($_GET['id']) ? $oldCard['name'] : "") ?>"/></div>
                <?php if(isset($errors['name'])): ?>
                    <div class="errorText"><?= $errors['name'] ?></div>
                <?php endif; ?>
                <div>
                    <span><span class="icon">🏷</span>Type:</span>
                    <select name="type" size="1">
                        <?php foreach($types as $var => $type ): ?>
                            <option value="<?= $var ?>"
                                <?php if(isset($_POST['type']) && $var == $_POST['type'] ): ?> 
                                    selected="selected"
                                <?php elseif(isset($_GET['id']) && $oldCard['type'] == $var): ?>
                                    selected="selected"
                                <?php endif; ?>>
                                <?= $type ?>
                            </option>
                        <?php endforeach; ?>
                    </select>
                </div>
                <div class="fields_col">
                    <div>
                        <span><span class="icon">❤</span>HP:</span><input type="number" name="hp" value="<?= isset($_POST['hp']) ? $_POST['hp'] : (isset($_GET['id']) ? $oldCard['hp'] : "") ?>"/>
                    </div>
                    <?php if(isset($errors['hp'])): ?>
                        <div class="errorText"><?= $errors['hp'] ?></div>
                    <?php endif; ?>
                    <div>
                        <span><span class="icon">⚔</span>ATK:</span><input type="number" name="atk" value="<?= isset($_POST['atk']) ? $_POST['atk'] : (isset($_GET['id']) ? $oldCard['attack'] : "") ?>"/>
                    </div>
                    <?php if(isset($errors['atk'])): ?>
                        <div class="errorText"><?= $errors['atk'] ?></div>
                    <?php endif; ?>
                    <div>
                        <span><span class="icon">🛡</span>DEF:</span><input type="number" name="def" value="<?= isset($_POST['def']) ? $_POST['def'] : (isset($_GET['id']) ? $oldCard['defense'] : "") ?>"/>
                    </div>
                    <?php if(isset($errors['def'])): ?>
                        <div class="errorText"><?= $errors['def'] ?></div>
                    <?php endif; ?>
                </div>
                <div class="login_field">
                    <div>
                        <span><span class="icon">💰</span>Price:</span><input type="number" name="price" value="<?= isset($_POST['price']) ? $_POST['price'] : (isset($_GET['id']) ? $oldCard['price'] : "") ?>"/>
                    </div>
                    <?php if(isset($errors['price'])): ?>
                        <div class="errorText"><?= $errors['price'] ?></div>
                    <?php endif; ?>
                </div>
                <div class="login_field"><span>Image:</span><input type="url" name="image" value="<?= isset($_POST['image']) ? $_POST['image'] : (isset($_GET['id']) ? $oldCard['image'] : "") ?>"/></div>
                <?php if(isset($errors['image'])): ?>
                    <div class="errorText"><?= $errors['image'] ?></div>
                <?php endif; ?>
                <div class="login_field"><span>Description:</span><textarea id="description" name="desc"><?= isset($_POST['desc']) ? $_POST['desc'] : (isset($_GET['id']) ? $oldCard['description'] : "") ?></textarea></div>
                <?php if(isset($errors['desc'])): ?>
                    <div class="errorText"><?= $errors['desc'] ?></div>
                <?php endif; ?>
                <button class="button-52" type="submit"><?= isset($_GET['id']) ? "Modify" : "Create" ?></button>
            </form>
            <?= var_dump($oldCard); ?>
        </div>
    </div>
    <footer>
        <p>IKémon | ELTE IK Webprogramozás | Márton Gergő</p>
    </footer>
</body>