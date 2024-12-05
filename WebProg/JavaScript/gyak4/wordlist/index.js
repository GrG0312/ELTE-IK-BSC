// ========= Utility functions =========
function random(a, b) {
  return Math.floor(Math.random() * (b - a + 1)) + a;
}

// ========= Selected elements =========
const inputGuess = document.querySelector("#inputGuess");
const form = document.querySelector("form");
const tableGuesses = document.querySelector("#guesses");
const divTheWord = document.querySelector("details > div");
const spanError = document.querySelector("#error");
const btnGuess = document.querySelector("form > button");
const divEndOfGame = document.querySelector("#end-of-game");
const btnRestart = document.querySelector("#restart");

// ========= Solution =========

// a
const theWord = wordList[random(0, wordList.length - 1)];
divTheWord.innerText = theWord;

// b
form.addEventListener('submit', (event) => {
  event.preventDefault();

  spanError.innerHTML = '';
  // c
  if (inputGuess.value.length !== 5) {
    spanError.innerHTML = 'The length of the word is not 5!';
    return;
  }

  // d
  if (!wordList.some((w) => w === inputGuess.value)) {
    spanError.innerHTML = 'The word is not considered acceptable!';
    return;
  }

  // e
  const matchingLetters = theWord.split('').reduce((subResult, char, index) => {
    return char === inputGuess.value[index] ? subResult + 1 : subResult
  }, 0)
  console.log('matchingLetters', matchingLetters);

  const isCorrect = matchingLetters === 5;

  // f
  const rowHTML =
    `<tr ${isCorrect ? 'class="correct"' : ''}>
      <td>${inputGuess.value}</td>
      <td>${matchingLetters}</td>
    </tr>`;
  tableGuesses.innerHTML = `${rowHTML}${tableGuesses.innerHTML}`;

  // g
  if (isCorrect) {
    divEndOfGame.removeAttribute('hidden');
  }

});

// h
btnRestart.addEventListener('click', (event) => {
  location.reload();
})



