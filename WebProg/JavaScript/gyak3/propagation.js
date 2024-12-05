const outerRectangleEl = document.getElementById('outerRectangle');
const innerRectangleEl = document.getElementById('innerRectangle');

document.addEventListener('click', (event) => {
    console.log('document');
})

outerRectangleEl.addEventListener('click', (event) => {
    console.log('red');
    event.stopPropagation();
})

innerRectangleEl.addEventListener('click', (event) => {
    console.log('green');
    event.stopPropagation();
})
