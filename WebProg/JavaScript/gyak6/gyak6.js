function random(a, b) {
    return Math.floor(Math.random() * (b - a + 1)) + a;
}

document.addEventListener('keydown', (event) => {
    if (event.code === 'Space') {
        bird.velocity -= 100;
    }
})

class Bird {
    image;
    constructor(context, x, y, width, height, velocity, acceleration) {
        this.context = context;
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.velocity = velocity;
        this.acceleration = acceleration;
        this.image = new Image();
        this.image.src = './assets/bird.png';
    }

    draw() {
        this.context.drawImage(
            this.image,
            this.x,
            this.y,
            this.width,
            this.height
        )
    }

    update(dt) {
        this.velocity += this.acceleration * dt / 1000;
        this.y += this.velocity * dt / 1000;

        if (this.y <= 0 || this.y >= canvas.height - bird.height ) {
            gameOver = true;
        }
    }
}

class Column {
    constructor(context, x, y, width, height, velocity) {
        this.context = context;
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.velocity = velocity;
    }

    draw() {
        context.fillStyle = 'green';
        context.fillRect(this.x, this.y, this.width, this.height);
    }

    update(dt) {
        this.x -= this.velocity * dt / 1000;
    }
}

const canvas = document.getElementById('flappyCanvas');
const context = canvas.getContext('2d');
let gameOver = false;
let points = 0;

const bird = new Bird(context, 50, 175, 50, 50, 30, 100);
let columns = [];
let columnGap = 200;
let columnDistance = 150;

function addColumns() {
    const topColumnHeight = random(75, 175);

    columns.push(
        new Column(
            context,
            canvas.width,
            0,
            40,
            topColumnHeight,
            125
        ),
        new Column(
            context,
            canvas.width,
            topColumnHeight + columnGap,
            40,
            canvas.height - topColumnHeight - columnGap,
            125
        )
    );
}

addColumns();

let previousUpdate = performance.now();
function cycle(now = performance.now()) {
    // console.log('cycle');
    const dt = now - previousUpdate;
    previousUpdate = now;

    update(dt);
    draw();

    if (!gameOver) {
        requestAnimationFrame(cycle);
    } else {
        context.font = '20px Arial';
        context.fillStyle = 'red';
        context.fillText('The bird is dead.', 200, 50);
    }
}

function draw() {
    context.fillStyle = 'lightblue';
    context.fillRect(0, 0, canvas.width, canvas.height);

    // context.clearRect(0, 0, canvas.width, canvas.height);

    context.beginPath();
    context.moveTo(0, 50);
    context.lineTo(25, 0);
    context.lineTo(50, 50);
    context.lineTo(0, 50);
    context.closePath();
    context.stroke();

    context.font = '14px Arial';
    context.fillStyle = 'yellow';
    context.fillText(`${points}`, 20, 30);

    bird.draw();

    columns.forEach((column) => {
        column.draw();
    })
}

function rectanglesCollide(a, b) {
    return a.x <= b.x + b.width &&
        a.x + a.width >= b.x &&
        a.y <= b.y + b.height &&
        a.y + a.height >= b.y;
}

function update(dt) {
    bird.update(dt);

    columns.forEach((column) => {
        column.update(dt);
    })

    if (columns.some((column) => rectanglesCollide(bird, column))) {
        gameOver = true;
    }

    const previousColumnLength = columns.length;
    columns = columns.filter((column) => {
        return column.x + column.width >= 0;
    })
    if (columns.length !== previousColumnLength) {
        points++;
    }

    if (columns.length > 0 &&
        canvas.width - columns.at(-1).x >= columnDistance) {
        addColumns();
    }
}

cycle();