// 1. Tervezés
// 2. DOM
// 3. Számolás / segédfüggvény
function calculateSqrt(n) {
    return Math.sqrt(n);
}

// 4. Eseménykezelés
const buttonEl = document.querySelector('#submit');
const inputEl = document.querySelector('#numberInput');
const resultEl = document.querySelector('#result');

buttonEl.addEventListener('click', (event) => {
    console.log(event);
    const number = +inputEl.value;
    console.log(number);
    // 5. Validáció
    if (!isNaN(number) && number > 0) {
        resultEl.innerHTML = `<p>~*-${calculateSqrt(number)}-*~`;
        inputEl.style.border = '1px solid black';
    } else {
        resultEl.innerHTML = '';
        // 6. Stílus
        inputEl.style.border = '1px solid red';

        // inputEl.classList.add('error');
    }
})



