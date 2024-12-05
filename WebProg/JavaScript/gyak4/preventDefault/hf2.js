let books = [
    {
        title: "11.22.63",
        author: "Stephen King",
        release: 2011,
        inStock: true
    },
    {
        title: "A Man Called Ove",
        author: "Fredrik Backman",
        release: 2012,
        inStock: true
    },
    {
        title: "Hitchiker's Guide to the Galaxy",
        author: "Douglas Adams",
        release: 1979,
        inStock: true
    },
    {
        title: "Masters of Doom",
        author: "David Kushner",
        release: 2003,
        inStock: false
    },
    {
        title: "Mistborn: The Final Empire",
        author: "Brandon Sanderson",
        release: 2006,
        inStock: true
    },
    {
        title: "Project Hail Mary",
        author: "Andy Weir",
        release: 2021,
        inStock: true
    },
    {
        title: "Storm Front",
        author: "Jim Butcher",
        release: 2000,
        inStock: false
    },
    {
        title: "The Eye of the World",
        author: "Robert Jordan",
        release: 1990,
        inStock: true
    },
    {
        title: "The Lies of Locke Lamora",
        author: "Scott Lynch",
        release: 2006,
        inStock: true
    },
    {
        title: "The Sandman",
        author: "Neil Gaiman",
        release: 2020,
        inStock: true
    },
    {
        title: "The Stormlight Archive: The Way of Kings",
        author: "Brandon Sanderson",
        release: 2010,
        inStock: true
    },
    {
        title: "The Stormlight Archive: Words of Radience",
        author: "Brandon Sanderson",
        release: 2014,
        inStock: true
    },
    {
        title: "We Are Legion (We Are Bob)",
        author: "Dennis E. Taylor",
        release: 2016,
        inStock: false
    }
]

const queryByTitleBtn = document.getElementById("queryBookByTitle");
const bookSelectorByTitle = document.getElementById("bookSelectorByTitle");
const queryByAuthorBtn = document.getElementById("queryBookByAuthor");
const bookSelectorByAuthor = document.getElementById("bookSelectorByAuthor");

const resultsContainer = document.getElementById("resultsContainer");
const selectedBook = document.getElementById("selectedBook");
const openWebshop = document.getElementById("openWebshop");

let clickedBook;

queryByTitleBtn.addEventListener("click", e => {
    const query = queryBooks("title", bookSelectorByTitle.value);
    console.log(query);
    createTable(query);
});

queryByAuthorBtn.addEventListener("click", e => {
    const query = queryBooks("author", bookSelectorByAuthor.value);
    console.log(query);
    createTable(query);
});

let selectedRow = null;

function createTable(content) {
    resultsContainer.innerHTML = "";
    selectedBook.innerHTML = "";
    openWebshop.style.display = "block";

    const table = document.createElement("table");
    const headerRow = document.createElement("tr");
    ["Cím", "Szerző", "Megjelenés"].forEach((th) => createCell("th", th, headerRow));
    table.appendChild(headerRow);

    content.forEach(book => {
        const bookRow = document.createElement("tr");
        ["title", "author", "release"].forEach((key) => createCell("td", book[key], bookRow));
        table.appendChild(bookRow);

        bookRow.addEventListener("click", e => {
            if (selectedRow !== null) {
                selectedRow.style.backgroundColor = "transparent";
                selectedBook.innerHTML = "";
            }

            if (selectedRow !== bookRow) {
                bookRow.style.backgroundColor = "lightgray";
                selectedRow = bookRow;
                selectedBook.innerHTML = `${book.author}: ${book.title} (${book.release})`;
                clickedBook = book;
                // if (!book.inStock) openWebshop
                //     .addEventListener("click", e => e.preventDefault());
            } else if (selectedRow === bookRow) {
                selectedRow = null;
            }
        });
    });

    resultsContainer.appendChild(table);
}

// Helper functions
function queryBooks(by, value) {
    if (!(by === "author" || by === "title")) return;
    return books.filter(book => book[by] === value);
};

function createCell(type, content, row) {
    if (!(type === "td" || type === "th")) return;
    const cell = document.createElement(type);
    cell.innerHTML = content;
    row.appendChild(cell);
}

openWebshop.addEventListener('click', (event) => {
    if (!clickedBook.inStock) {
        event.preventDefault();
    }
})
