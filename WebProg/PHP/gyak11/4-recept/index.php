<?php
include_once 'storage.php';
$storage = new Storage(new JsonIO('recipes.json'));
$recipes = $storage->findAll();

session_start();
if (!isset($_SESSION['fridge'])) {
    $_SESSION['fridge'] = [];
}
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="index.css">
    <title>Task 4</title>
</head>
<body>
    <h1>Task 4: Recipe tracker</h1>
    
    <h2>List of recipes</h2>
    <ul>
        <?php foreach ($recipes as $food => $ingredients): ?>
        <li>
            <a href="details.php?food=<?= $food ?>"><?= $food ?></a>
            <?php
                if (
                    !count(
                    array_filter(
                        $ingredients,
                        fn($i) => !in_array($i, $_SESSION['fridge'])
                    )
                    )
                ):
            ?>
                <a style="color: green"
                   href="make.php?food=<?= $food ?>">Elkészíthető</a>
            <?php endif; ?>
        </li>
        <?php endforeach; ?>
    </ul>

    <h2>Fridge contents</h2>
    <ul>
        <?php foreach ($_SESSION['fridge'] as $ingredient): ?>
            <li>
                <?= $ingredient ?>
            </li>
        <?php endforeach; ?>
    </ul>

    
</body>
</html>