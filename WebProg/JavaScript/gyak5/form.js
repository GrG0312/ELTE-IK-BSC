// ^[0-9]{8}-[0-9]{8}$

// Form

const formEl = document.querySelector('#form');
const availableInputEl = document.querySelector('#availableInCinemas');
const nextInputEl = document.querySelector('#next');
const protagonistInputEl = document.querySelector('#protagonist');
const titleInputEl = document.querySelector('#title');

formEl.addEventListener('submit', (event) => {
    event.preventDefault();

    validate();
})

function init() {

}

function save() {

}

[availableInputEl, nextInputEl].forEach((element) => {
    element.addEventListener('input', () => validate());
});

function validate() {
    if (availableInputEl.value === 'yes' && !nextInputEl.value) {
        nextInputEl.setCustomValidity('Please provide a date!');
    } else {
        nextInputEl.setCustomValidity('');
    }
}
