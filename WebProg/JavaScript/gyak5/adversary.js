// Delegation
function delegate(parent, type, selector, handler) {
    parent.addEventListener(type, function (event) {
        const targetElement = event.target.closest(selector);

        if (this.contains(targetElement)) {
            handler.call(targetElement, event);
        }
    });
}

function randomNumber(from, to) {
    return Math.floor(Math.random() * to) + from;
}

// Init
const containerEl = document.getElementById('container');
const rowColNumber = 10;
const solution = randomNumber(1, Math.pow(rowColNumber, 2))
console.log('Solution: ', solution);
let gameOver = false;
let intervalId;

function adversaryGuess() {
    const allCells = document.getElementsByClassName('cell');
    const availableCells = [...allCells]
        .filter((cell) => cellAvailable(cell));
    // console.log('Available cells', availableCells);
    const guessedCell =
        availableCells[randomNumber(0, availableCells.length - 1)];
    const guess = +guessedCell.innerText;
    if (guess === solution) {
        guessedCell.classList.add('lost-selection');
        gameOver = true;
        window.clearInterval(intervalId);
    } else {
        guessedCell.classList.add('adversary-selection');
    }
}

function cellAvailable(cell) {
    return !cell.classList.contains('user-selection') &&
        !cell.classList.contains('adversary-selection') 
}

function createGrid(size) {
    const gridHTML = [...new Array(size)].map((_, i) => `
    <div>
        ${[...new Array(size)].map((_, j) => `
            <div class="cell">${i * size + j + 1}</div>
        `).join('')}
    </div>
    `).join('');
    containerEl.innerHTML = gridHTML;
}

createGrid(rowColNumber);

delegate(containerEl, 'click', '.cell', function (event) {
    if (!cellAvailable(event.target) || gameOver) {
        return;
    }
    // console.log(this, event);
    const guess = +event.target.innerHTML;
    if (guess === solution) {
        event.target.classList.add('won-selection');
        gameOver = true;
        window.clearInterval(intervalId);
    } else {
        event.target.classList.add('user-selection');
    }
});


intervalId = window.setInterval(() => {
    adversaryGuess();
}, 2000);








