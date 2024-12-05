let persons = [
    {
        name: 'Dabasi Zoltán Bence',
        age: 36,
        interests: [
            'Békaboncolás',
            'Futás'
        ]
    },
    {
        name: 'Dallos Loránd',
        age: 41,
        interests: [
            'Fényképezés',
            'Napozás',
            'Rovatégetés'
        ]
    },
    {
        name: 'Fábián Ágota',
        age: 22,
        interests: [
            'Alvás',
            'Futás',
            'Úszás',
            'Zártörés'
        ]
    },
    {
        name: 'Áfullér Levente',
        age: 25,
        interests: [
            'Futás',
            'Karate'
        ]
    },
    {
        name: 'Horváth Benjámin',
        age: 21,
        interests: [
            'Bélyeggyűjtés'
        ]
    },
    {
        name: 'Horváth Tamás',
        age: 18,
        interests: [
            'LEGO',
            'Paintball'
        ]
    },
    {
        name: 'Teszt Tamás',
        age: 55,
        interests: [
            'Nem létezés',
            'Strandolás'
        ]
    }
]


// Legalább 25 évesek
const atLeast25 = persons.filter((p) => p.age >= 25);

// Nevek listája fordított ABC rendben
const sortedPersons = persons.map(a => a.name).sort((a, b) => {
    // if (a - b > 0) {
    //     return 1;
    // } else {
    //     return -1;
    // }
    a.localeCompare(b)
}).reverse();
// Alternative: b.localeCompare(a) without reverse

// Egy személy aki szeret futni
const likesRunning = persons.find(p => p.interests.includes('Futás'));

// Az első "Tamás" indexe a tömbben
const indexOfFirstTamas = persons.findIndex(p => p.name.includes('Tamás'));

// Hobbik egyedi listája
const uniqueInterests = [...new Set(persons.flatMap((p) => p.interests))];

// Keresztnevek egyedi listája
const uniqueNames = new Set(
    persons.flatMap((p) => {
        const splitName = p.name.split(' ');
        splitName.shift();
        return splitName;
    })
)

// Legfiatalabb személy
const youngestPerson = persons.sort((p1, p2) => p1.age - p2.age)[0];

// Átlagos hobbiszám
const averageInterestNumber = persons
    .map(p => p.interests.length)
    .reduce((partialResult, currentInterestNumber) => partialResult + currentInterestNumber)
    / persons.length;

console.log(
    `Legalább 25 évesek:`,  atLeast25,
    'Rendezett személyes:', sortedPersons,
    'Szeret futni: ', likesRunning,
    'Index of first Tamás', persons, indexOfFirstTamas,
    'Unique interests', uniqueInterests,
    'Unique names', uniqueNames,
    'Youngest', youngestPerson,
    'Average interest number', averageInterestNumber
);

/*
join("separator")= array1"separator"array2"separator"array3

*/