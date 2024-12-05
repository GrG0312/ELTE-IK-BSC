console.log('Gyak1JS');

function test() {
    const name = 'Barna'
}

console.log(test());

console.log('string', '5', 5, true, false, undefined, null, [0, 5, 7], ['asd', 5, true, { asd: 'asd' }],
    { name: 'Tamás', age: 22, placeOfResidence: { zip: 4533, street: '' } }
)

console.log('5' + 5);
console.log('asd' + 5);

console.log(5 + 6, 11 - 2, 12 * 3, 12 / 4, 10 % 5, 3 ** 2, true || false, true && false)

let mode = 'manual';
switch(mode) {
    case 'dynamic':
        console.log('d');
        break;
    case 'manual':
        console.log('m');
        break;
    default:
        console.log('default');
        break;
}

function filterPositives(array) {
    const results = [];
    for (let i = 0; i < array.length; i++) {
        if (array[i] > 0) {
            results.push(array[i]);
        }
    }
    return results;
}


console.log(filterPositives([-4, 2, 5, -11, 5, 0]));

function filterNegatives(array) {
    const results = [];
    for (const number of array) {
        if (number < 0) {
            results.push(number);
        }
    }
    return results;
}

console.log(filterNegatives([-3, 5, 2, -1, -222, 23, 0]));

function generalFilter(array, filterFn) {
    const results = [];
    for (const number of array) {
        if (filterFn(number)) {
            results.push(number);
        }
    }
    return results;
}

const numberArray = [-3, 10, 67, 31, 0, -2, 5];
console.log(generalFilter(numberArray, (num) => {
    return num % 2 === 0;
}))
console.log(generalFilter(numberArray, function (num) {
    return num % 2 === 1;
}))
console.log(generalFilter(numberArray, (num) => num % 2 === 0));

console.log(numberArray.filter(num => num >= 10));

console.log(numberArray
    .map(n => n * n)
    .filter(n => n > 100)
);

console.log(numberArray.some(n => n < -100));//legalább egy elem megfelel neki

console.log(numberArray.every(n => n < 1000))//minden elem megfelel neki

console.log(numberArray.length);

console.log(numberArray.sort((n1, n2) => n1 - n2));

const sum = numberArray.reduce((partialResult, currentNum) => partialResult + currentNum, 0);
console.log(sum / numberArray.length);

















