const countEl = document.getElementById('count');
const tableEl = document.getElementById('resultTable');

// Create the table
countEl.addEventListener('input', (event) => {
    tableEl.innerHTML = '';
    console.log(event, event.target.value);
    const rowColCount = Math.abs(Math.min(+event.target.value, 50));
    countEl.value = rowColCount;

    for(let i = 1; i <= rowColCount; i++) {
        const rowEl = document.createElement('tr');
        for (let j = 1; j <= rowColCount; j++) {
            const colEl = document.createElement('td');
            colEl.innerHTML = i*j;
            colEl.dataset['double'] = i*j*2;
            // colEl.addEventListener('click', (event) => {
            //     if (event.target.style.backgroundColor === 'aqua') {
            //         event.target.style.backgroundColor = 'transparent';
            //     } else {
            //         event.target.style.backgroundColor = 'aqua';
            //     }
            // });
            rowEl.appendChild(colEl);
        }
        tableEl.appendChild(rowEl);
    }
})

// On click select / unselect cells
tableEl.addEventListener('click', (event) => {
    if (event.target.matches('td')) {
        if (event.target.style.backgroundColor === 'aqua') {
            event.target.style.backgroundColor = 'transparent';
        } else {
            event.target.style.backgroundColor = 'aqua';
        }
        console.log('Value:', event.target.innerHTML);
        console.log('Double value:', event.target.dataset['double']);
    }
})

// Read the data from the cells - using the content and using data attributes