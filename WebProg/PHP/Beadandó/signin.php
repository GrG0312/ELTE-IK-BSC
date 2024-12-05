<?php
session_start();
@include("storage.php");

$storageOfUsers = new Storage(new JsonIO("users.json"));
$errors = [];

function ValidateRegister($input){
    global $errors;
    global $storageOfUsers;
    if($input['name'] == ""){
        $errors['name'] = "You must provide a name for your account!";
    } else {
        $users = $storageOfUsers->findAll();
        foreach($users as $user){
            if($user["name"] == $input["name"]){
                $errors["name"] = "This username if already taken, please choose an another one!";
            }
        }
    }

    if($input['email'] == ""){
        $errors['email'] = "You must provide an email for your account!";
    } else if(!filter_var($input['email'], FILTER_VALIDATE_EMAIL)){
        $errors['email'] = "You must provide a valid email address!";
    }

    if($input['password'] == ""){
        $errors['password'] = "You must provide a password!";
    }

    if($input['password2'] == ""){
        $errors['password2'] = "You must repeat your password for safety reasons!";
    } else if($input['password'] != $input['password2']){
        $errors['password2'] = "Your passwords do not match, please try again!";
    }
}

if (!empty($_POST)) {
    ValidateRegister($_POST);
    if($errors == []){
        $newUser = array(
            "name" => $_POST['name'],
            "password" => $_POST['password'],
            "email" => $_POST['email'],
            "isAdmin" => false,
            "money" => 500,
            "cards" => [],
        );
        $storageOfUsers->addWithID($newUser, $newUser['name']);
        $_SESSION = $newUser;
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
    <title>IKémon | Register</title>
    <link rel="stylesheet" href="styles/main.css">
    <link rel="stylesheet" href="styles/form.css">
    <link rel="stylesheet" href="styles/button.css">
</head>
<body>
    <header>
        <h1><a href="index.php">IKémon</a> > Register</h1>
    </header>
</body>
<body>
<div id="content" class="logincontent">
        <div>
            <h2>Register</h2>
            <form method="post">
                <div class="login_field">
                    <span>Name:</span><input type="text" name="name" value="<?= isset($_POST['name']) ? $_POST['name'] : '' ?>"/>
                </div>
                <?php if(isset($errors['name'])): ?>
                    <div class="errorText"><?= $errors['name'] ?></div>
                <?php endif; ?>
                <div class="login_field">
                    <span>Email:</span><input type="text" name="email" value="<?= isset($_POST['email']) ? $_POST['email'] : '' ?>"/>
                </div>
                <?php if(isset($errors['email'])): ?>
                    <div class="errorText"><?= $errors['email'] ?></div>
                <?php endif; ?>
                <div class="login_field">
                    <span>Password:</span><input type="password" name="password" value="<?= isset($_POST['password']) ? $_POST['password'] : '' ?>"/>
                </div>
                <?php if(isset($errors['password'])): ?>
                    <div class="errorText"><?= $errors['password'] ?></div>
                <?php endif; ?>
                <div class="login_field">
                    <span>Password again:</span><input type="password" name="password2" value="<?= isset($_POST['password2']) ? $_POST['password2'] : '' ?>"/>
                </div>
                <?php if(isset($errors['password2'])): ?>
                    <div class="errorText"><?= $errors['password2'] ?></div>
                <?php endif; ?>
                <button class="button-52" type="submit">Sign me in!</button>
            </form>
        </div>
        <div class="centertext">
            If already have an account, please <a href="login.php">log in</a>
        </div>
    </div>
    <footer>
        <p>IKémon | ELTE IK Webprogramozás | Márton Gergő</p>
    </footer>
</body>