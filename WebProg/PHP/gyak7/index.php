<?php
declare(strict_types=1);
?>
<!--
PHP install:
https://github.com/totadavid95/PhpComposerInstaller
OR
https://www.apachefriends.org/download.html

PHP documentation: https://www.php.net/manual/en/index.php
-->

<?php
/*
phpinfo();
*/
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
</head>
<body>
    
<div style="color: green">Hello class!</div>
<?php echo "Second hello!"; ?>
<?= '<div>Third hello!</div>' ?>

<?php
$bool = true;
$int = 4;
$float = 4.12;
$string = 'sztring';
$string2 = "string2: $float";
echo $string2;
$array = ["element1", "element2"];
$callable = function() {};

echo "<br> Type boolean: " . gettype($bool);
echo "<br> Type int: " . gettype($int);
echo "<br> Type float: " . gettype($float);
echo "<br> Type array: " . gettype($array);
echo "<br> Type function: " . gettype($callable);

echo 5 + 10;
echo 9 ?? NULL;
echo NULL ?? NULL ?? 3;
echo 1 ? 2 : 0;
echo 1 ?: 0; // 1 ? 1 : 0;

function add(float $number1, float $number2): float {
    return $number1 + $number2;
}
echo '<br>' . add(1.2, 3.9);

function even(array $array): array {
    $result = [];
    foreach($array as $element) {
        if ($element % 2 == 0) {
            // $result[] = $element;
            array_push($result, $element);
        }
    }
    return $result;
}

$numberArray = [-2, 4, 13, 11, -1, 10, 16, 1];
// echo $numberArray;
echo '<br>Even numbers:';
print_r(even($numberArray));

function filter(array $array, callable $filterFunc): array {
    $result = [];
    foreach($array as $element) {
        if ($filterFunc($element)) {
            $result[] = $element;
        }
    }
    return $result;
}

echo '<br>Odd numbers:';
var_dump(filter($numberArray, function($number) {
    return $number % 2 == 1;
}));

echo '<br>Negative numbers:';
print_r(array_filter($numberArray, function($n) {
    return $n < 0;
}));
?>

<ul>
<?php foreach(even($numberArray) as $evenNumber): ?>
    <li><?= $evenNumber ?></li>
<?php endforeach ?>
</ul>

<?php

$settings = [
    1 => [
        'name' => 'Theme',
        'type' => 'radio',
        'options' => [
            'Dark' => false,
            'Light' => true
        ]
    ],
    2 => [
        'name' => 'Mode',
        'type' => 'checkbox',
        'options' => [
            'mode1' => true,
            'mode2' => false,
            'mode3' => true
        ]
    ]
];


?>
<h3>Categories</h3>
<input type="radio" name="test" value="a" />A
<input type="radio" name="test" value="b" />B

<?php
foreach($settings as $id => $setting): ?>
<h3><?= $setting['name']?></h3>
    <?php foreach($setting['options'] as $option => $checked):?>
        <input type="<?= $setting['type']?>"
               name="<?= $setting['name'] ?>"
               value="<?= $option ?>"
               <?= $checked ? 'checked' : '' ?>
               />
        <?= $option ?>
    <?php endforeach ?>
<?php endforeach ?>

<a href="http://localhost:8000?color=green">Green</a>
<a href="http://localhost:8000?color=blue">Blue</a>

<?php
$color = $_GET["color"] ?? 'white';
?>
<style>
    html {
        background-color: <?= $color ?>;
    }
</style>







</body>
</html>