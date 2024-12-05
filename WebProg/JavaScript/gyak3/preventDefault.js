const linkEl = document.querySelector('#googleLink');

linkEl.addEventListener('click', (event) => {
    event.preventDefault();
})


const checkEl = document.querySelector('#checker');
const allowEl = document.querySelector('#allowChecking');

checkEl.addEventListener('click', (e) => {
    if (allowEl.value === 'no') {
        e.preventDefault();
    }
})

