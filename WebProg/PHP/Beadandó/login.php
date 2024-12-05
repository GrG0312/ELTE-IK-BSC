<?php
session_start();
@include_once("storage.php");
$storage = new Storage(new JsonIO("users.json"));
$users = $storage->findAll();
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>IKémon | Login</title>
    <link rel="stylesheet" href="styles/main.css">
    <link rel="stylesheet" href="styles/form.css">
    <link rel="stylesheet" href="styles/button.css">
</head>
<body>
    <header>
        <h1><a href="index.php">IKémon</a> > Login</h1>
    </header>
</body>
<body>

<?php

function CheckUser($username, $password){
    global $users;
    foreach($users as $user){
        if($user["name"] == $username){
            if($user["password"] == $password){
                $_SESSION = $user;
                return 0;
            }
            return 1;
        }
    }
    return 2;
}
function ValidateLogin($input, &$error) {

    if ($input['name'] == "" || $input['pw'] == "") {
        $error = 'Please provide both a username and a password!';
    } else {
        $retVal = CheckUser($input['name'], $input['pw']);
        if ($retVal == 2) {
            $error = 'There is no user registered with this username!';
        } else if($retVal == 1){
            $error = 'You have provided an incorrect password, please try again!';
        }
    }
}

$error = "";

if (!empty($_POST)) {
  ValidateLogin($_POST, $error);
  if($error == ""){
    header("Location: index.php");
    die();
  }
}

?>
    <div id="content" class="logincontent">
        <div>
            <h2>Login</h2>
            <form method="post">
                <div class="login_field"><span>Username:</span><input type="text" name="name" value="<?= isset($_POST['name']) ? $_POST['name'] : '' ?>"/></div>
                <div class="login_field"><span>Password:</span><input type="password" name="pw" value="<?= isset($_POST['pw']) ? $_POST['pw'] : '' ?>"/></div>
                <button class="button-52" type="submit">Login</button>
            </form>
        </div>
        <?php if($error != ""): ?>
            <div class="centertext error">
                <?= $error ?>
            </div>
        <?php endif; ?>
        <div class="centertext">
            If do not have an account yet, please <a href="signin.php">sign in</a>
        </div>
    </div>
    <footer>
        <p>IKémon | ELTE IK Webprogramozás | Márton Gergő</p>
    </footer>
</body>