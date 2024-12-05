const pieceX = document.querySelector('#pieceX');
const pieceY = document.querySelector('#pieceY');
const pieceDiv = document.querySelector('#piece');
const okButton = document.querySelector('#ok');
const errorDiv = document.querySelector('#error');
const successDiv = document.querySelector('#success');
const attemptsSpan = document.querySelector('#attempts');
let attempts = 3;

const distanceTo = (x1, y1, x2, y2) =>
    Math.sqrt((x1 - x2) ** 2 + (y1 - y2) ** 2);

function random(a, b) {
    return Math.floor(Math.random() * (b - a + 1)) + a;
}

// Work here...

// A
const selectedCoor = {
    x: random(1, 500),
    y: random(1, 278)
}
console.log('selectedCoor', selectedCoor);

// B
pieceDiv.style.backgroundPosition =
    `-${selectedCoor.x}px -${selectedCoor.y}px`;

// D
let piecePos = {
    x: pieceDiv.style.left,
    y: pieceDiv.style.top
};

// C
[pieceX, pieceY].forEach(element => {
    element.addEventListener('input', (event) => {
        piecePos = {
            x: pieceX.value,
            y: pieceY.value
        }
        pieceDiv.style.left = `${piecePos.x}px`;
        pieceDiv.style.top = `${piecePos.y}px`;
    })
})

// E
okButton.addEventListener('click', (event) => {
    const distance =
        distanceTo(
            selectedCoor.x,
            selectedCoor.y,
            piecePos.x,
            piecePos.y
        );
    console.log('distance', distance);

    if (distance > 10) {
        // Error
        errorDiv.style.display = 'block';
        attempts--;
        attemptsSpan.innerText = attempts;
        if (attempts <= 0) {
            okButton.disabled = true;
            pieceX.disabled = true;
            pieceY.disabled = true;
        }
    } else {
        // Success
        errorDiv.style.display = 'none';
        successDiv.style.display = 'block';
    }
});

