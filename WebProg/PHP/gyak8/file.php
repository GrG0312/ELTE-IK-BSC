<?php

$personsFile = fopen("persons.json", "r");

$personsAsString = fread($personsFile, filesize("persons.json"));

$parsedPersons = json_decode($personsAsString, true);
print_r($parsedPersons);

fclose($personsFile);


$childPersons = array_filter($parsedPersons, fn($person) => $person['age'] < 18);
print_r($childPersons);


$childPersonsFile = fopen("child_persons.csv", "w");
fwrite($childPersonsFile, 'name,age,sex' . PHP_EOL);
foreach($childPersons as $childPerson) {
    fwrite($childPersonsFile,
        $childPerson['name'] . ',' . $childPerson['age'] . ',' .
        $childPerson['sex'] . PHP_EOL
    );
}
fclose($childPersonsFile);
?>


