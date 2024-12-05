document.querySelector('#simpleGetButton')
    .addEventListener('click', (event) => {
    fetch('http://localhost:8000/ajax_receiver.php?city=Budapest&zip=1156')
        .then((response) => response.text())
        .then((content) =>
            document.getElementById('simpleGetResult').innerHTML = content
        )
});

document.querySelector('#simplePostButton')
    .addEventListener('click', (event) => {
    const payload = new FormData();
    payload.append('city', 'Budapest');
    payload.append('zip', '1156');
    fetch(
        'http://localhost:8000/ajax_receiver.php', {
            method: 'POST',
            body: payload
        }
    ).then((response) => response.json())
    .then((jsObject) =>
        document.querySelector('#simplePostResult').innerHTML =
            JSON.stringify(jsObject)    
    )
})

document.querySelector('#simpleJSONPostButton')
    .addEventListener('click', (event) => {
    fetch(
        'http://localhost:8000/ajax_receiver.php', {
            method: 'POST',
            headers: {
                'Accept': 'text/plain',
                'Content-Type': 'application/json'
            },
            body: {
                city: 'Budapest',
                zip: 1156
            }
        }
    ).then((response) => response.json())
    .then((jsObject) =>
        document.querySelector('#simpleJSONPostResult').innerHTML =
            JSON.stringify(jsObject)    
    )
});

// document.querySelector('#wordForm')
//     .addEventListener('submit', (event) => {
//     event.preventDefault();
//     const formData = new FormData(document.querySelector('#wordForm'));
//     fetch('http://localhost:8000/ajax_form_receiver.php', {
//             method: 'POST',
//             body: formData
//         })
//         .then((result) => result.text())
//         .then((html) => {
//             document.getElementById('wordFormResult').innerHTML = html;
//         })
// });

document.querySelector('#wordForm')
    .addEventListener('submit', (event) => {
    event.preventDefault();
    const formData = new FormData(document.querySelector('#wordForm'));
    fetch('http://localhost:8000/ajax_form_receiver.php', {
            method: 'POST',
            body: formData
        })
        .then((result) => result.json())
        .then((result) => {
            document.querySelector('#wordFormResult tbody').innerHTML = '';
            Object.keys(result).forEach((key) => {
                document.querySelector('#wordFormResult tbody').innerHTML
                    += `
                        <tr>
                            <td>${key}</td>
                            <td>${result[key]}</td>
                        </tr>
                    `
            });
        })
});
