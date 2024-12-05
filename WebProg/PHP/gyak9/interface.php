<?php

interface PersonsAPI {
    function getPersons(): array;
    function getPersonsOverAge(int $age): array;
}

abstract class PersonsFromWhatever implements PersonsAPI {
    abstract function getPersons(): array;

    function getPersonsOverAge(int $age): array {
        $result = [];
        foreach($this->getPersons() as $person) {
            if ($person['age'] >= $age) {
                $result[] = $person;
            }
        }
        return $result;
    }
}

class PersonsFromCSV extends PersonsFromWhatever {
    function getPersons(): array {
        $results = [];
        $csvPointer = fopen("persons.csv", "r");
        $headers = fgetcsv($csvPointer, NULL, ",");
        while($row = fgetcsv($csvPointer, NULL, ",")) {
            $results[] = array_combine($headers, $row);
        }
        fclose($csvPointer);
        return $results;
    }
}

class PersonsFromJSON extends PersonsFromWhatever {
    function getPersons(): array {
        return json_decode(file_get_contents('persons.json'), true);
    }
}

$personsCSVAPI = new PersonsFromCSV();
$personsJSONAPI = new PersonsFromJSON();

foreach ($personsCSVAPI->getPersonsOverAge(30) as $person) {
    echo nl2br($person['name'] . PHP_EOL);
}

echo nl2br('----------------' . PHP_EOL);

foreach($personsJSONAPI->getPersonsOverAge(18) as $person) {
    echo nl2br($person['name'] . PHP_EOL);
}
