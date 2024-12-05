const mainDiv = document.getElementById("main");
const startBTN = document.getElementById("start");

const divOfTable = document.createElement("div");
const gameTable = document.createElement("table");

const divOfSeasons = document.createElement("div");
const divOfQuests = document.createElement("div");
const seasonsTable = document.createElement("table");
const randomElementTable = document.createElement("table");
const currentSeasonNamespace = document.createElement("span");

let currentSeason = 0;
let currentTime = 0;
let selected = 0;
let nextEl;
let mountainScore = 0;

//PRESETS
const missions = 
[
  {
    title: "Tree line",
    description: "A leghosszabb, függőlegesen megszakítás nélkül egybefüggő erdőmezők mindegyikéért kettő-kettő pontot kapsz. Két azonos hosszúságú esetén csak az egyikért.",
    /*EZ JÓL MŰKÖDIK*/
    img: "missions_eng/Group_68.png",
    calculate: function(){
      let longest = 0;
      let found = 0;
      for(let i = 0; i < 11; i++){
        for(let j = 0; j < 11; j++){
          if(gameTable.children[j].children[i].classList.contains("forest")){
            found = found + 1;
          } else{
            if(found > longest){ longest = found; }
            found = 0;
          }
        }
      }
      return (longest * 2);
    }
  },
  {
    title: "Edge of the forest",
    description: "A térképed szélével szomszédos erdőmezőidért egy-egy pontot kapsz.",
    /*EZ JÓL MŰKÖDIK*/
    img: "missions_eng/Group_69.png",
    calculate: function(){
      let pointsForThis = 0;
      for(let i = 1; i<10; i++){
        if(gameTable.children[i].children[0].classList.contains("forest")){/*Első oszlop*/
          pointsForThis += 1;
        }
        if(gameTable.children[i].children[10].classList.contains("forest")){/*Utolsó oszlop*/
          pointsForThis += 1;
        }
        if(gameTable.children[0].children[i].classList.contains("forest")){/*Első sor*/
          pointsForThis += 1;
        }
        if(gameTable.children[10].children[i].classList.contains("forest")){/*Utolsó sor*/
          pointsForThis += 1;
        }
      }
      if(gameTable.children[0].children[0].classList.contains("forest")){/*Bal felső*/
        pointsForThis += 1;
      }
      if(gameTable.children[0].children[10].classList.contains("forest")){/*Jobb oszlop*/
        pointsForThis += 1;
      }
      if(gameTable.children[10].children[0].classList.contains("forest")){/*Bal alsó*/
        pointsForThis += 1;
      }
      if(gameTable.children[10].children[10].classList.contains("forest")){/*Jobb alsó*/
        pointsForThis += 1;
      }
      return pointsForThis;
    }
  },
  {
    title: "Sleepy valley",
    description: "Minden olyan sorért, amelyben három erdőmező van, négy-négy pontot kapsz.",
    /*EZ JÓL MŰKÖDIK*/
    img: "missions_eng/Group_74.png",
    calculate: function(){
      let pointsForThis = 0;
      for(let i = 0; i<11; i++){
        let found = 0;
        for(let j = 0; j<11; j++){
          if(gameTable.children[i].children[j].classList.contains("forest")){
            found += 1;
          }
        }
        if(found == 3){
          pointsForThis += 4;
        }
      }
      return pointsForThis;
    }
  },
  {
    title: "Watering potatoes",
    description: "A farmmezőiddel szomszédos vízmezőidért két-két pontot kapsz.",
    /*EZ JÓL MŰKÖDIK*/
    img: "missions_eng/Group_70.png",
    calculate: function(){
      let pointsForThis = 0;
      for(let i = 0; i < 11; i++){
        for(let j = 0; j < 11; j++){
          if(gameTable.children[i].children[j].classList.contains("farm")){
            if((j > 0) && (gameTable.children[i].children[j-1].classList.contains("water"))){
              pointsForThis+=2;
            }
            if((j < 10) && (gameTable.children[i].children[j+1].classList.contains("water"))){
              pointsForThis+=2;
            }
            if((i > 0) && (gameTable.children[i-1].children[j].classList.contains("water"))){
              pointsForThis+=2;
            }
            if((i < 10) && (gameTable.children[i+1].children[j].classList.contains("water"))){
              pointsForThis+=2;
            }
          }
        }
      }
      return pointsForThis;
    }
  },
  {
    title: "Borderlands",
    description: "Minden teli sorért vagy oszlopért 6-6 pontot kapsz.",
    /*EZ JÓL MŰKÖDIK*/
    img: "missions_eng/Group_78.png",
    calculate: function(){
      let cols = 0;
      let rows = 0;
      for(let i = 0; i < 11; i++){
        let fullR = true;
        let fullC = true;
        for(let j = 0; j < 11; j++){
          if((gameTable.children[i].children[j].classList.contains("empty"))){
            fullR = false;
          }
          if((gameTable.children[j].children[i].classList.contains("empty"))){
            fullC = false;
          }
        }
        if(fullR){
          rows++;
        }
        if(fullC){
          cols++;
        }
      }
      return (rows + cols)*6;
    }
  },
  {
    title: "Wealthy town",
    description: "A legalább három különböző tereptípussal szomszédos falurégióidért három-három pontot kapsz.",
    /*EZ JÓL MŰKÖDIK*/
    img: "missions_eng/Group_71.png",
    calculate: function(){
      let pointsForThis = 0;
      for(let i = 0; i < 11; i++){
        for(let j = 0; j < 11; j++){
          if(gameTable.children[i].children[j].classList.contains("town")){
            let nextTo = ["emtpy", "empty", "empty"];
            let num = 0;
            if(i < 10 && (!nextTo.includes(gameTable.children[i+1].children[j].classList[0]))){
              nextTo[num] = gameTable.children[i+1].children[j].classList[0];
              num++;
              /*Ha a felette lévő nincs benne a nextTo-ban akkor nem empty, akkor már van 1*/
            }
            if(i > 0 && !nextTo.includes(gameTable.children[i-1].children[j].classList[0])){
              nextTo[num] = gameTable.children[i-1].children[j].classList[0];
              num++;
              /*Ha az alatta lévő nincs benne a nextTo-ban akkkor nem empty és nem az 1. így van 2*/
            }
            if(j < 10 && !nextTo.includes(gameTable.children[i].children[j+1].classList[0])){
              nextTo[num] = gameTable.children[i].children[j+1].classList[0];
              num++;
              /*Ha a jobb oldali nincs benne, akkor nem az első 2 és nem empty, tehát van 3*/
            }
            if(num == 3){/*Ha megvan 3 akkor a pointsForThis megnő 3al*/
              pointsForThis += 3;
            } else {/*Ha nincs meg 3 a num, akkor */
              if(j > 0 && !nextTo.includes(gameTable.children[i].children[j-1].classList[0])){
                nextTo[num] = gameTable.children[i].children[j-1].classList[0];
                num++;
                /*Ha a bal oldali nincs benne nextToban akkor nem empty és nem az első 2 */
                if(num == 3){
                  pointsForThis += 3;
                }
              }
            }
          }
        }
      }
      return pointsForThis;
    }
  },
  {
    title: "Watering canal",
    description: "Minden olyan oszlopodért, amelyben a farm illetve a vízmezők száma megegyezik, négy-négy pontot kapsz. Mindkét tereptípusból legalább egy-egy mezőnek lennie kell az oszlopban ahhoz, hogy pontot kaphass érte.",
    /*EZ JÓL MŰKÖDIK*/
    img: "missions_eng/Group_75.png",
    calculate: function(){
      let pointsForThis = 0;
      for(let i = 0; i < 11; i++){
        let farm = 0;
        let water = 0;
        for(let j = 0; j < 11; j++){
          if(gameTable.children[j].children[i].classList.contains("water")){
            water++;
          }
          if(gameTable.children[j].children[i].classList.contains("farm")){
            farm++;
          }
        }
        if(water != 0 && water == farm){
          pointsForThis += 4;
        }
      }
      return pointsForThis;
    }
  },
  {
    title: "Magicians' valley",
    description: "A hegymezőiddel szomszédos vízmezőidért három-három pontot kapsz.",
    /*EZ JÓL MŰKÖDIK*/
    img: "missions_eng/Group_76.png",
    calculate: function(){
      let pointsForThis = 0;
      mountains.forEach(element => {
        if(element.x < 11 && gameTable.children[element.x+1].children[element.y].classList.contains("water")){
          pointsForThis += 3;
        }
        if(element.x > 0 && gameTable.children[element.x-1].children[element.y].classList.contains("water")){
          pointsForThis += 3;
        }
        if(element.y < 11 && gameTable.children[element.x].children[element.y+1].classList.contains("water")){
          pointsForThis += 3;
        }
        if(element.x > 0 && gameTable.children[element.x].children[element.y-1].classList.contains("water")){
          pointsForThis += 3;
        }
      });
      return pointsForThis;
    }
  },
  {
    title: "Empty site",
    description: "A városmezőiddel szomszédos üres mezőkért 2-2 pontot kapsz.",
    /*EZ JÓL MŰKÖDIK*/
    img: "missions_eng/Group_77.png",
    calculate: function(){
      let pointsForThis = 0;
      for(let i = 0; i < 11; i++){
        for(let j = 0; j < 11; j++){
          if(gameTable.children[i].children[j].classList.contains("town")){
            if((j > 0) && (gameTable.children[i].children[j-1].classList.contains("empty"))){
              pointsForThis+=2;
            }
            if((j < 10) && (gameTable.children[i].children[j+1].classList.contains("empty"))){
              pointsForThis+=2;
            }
            if((i > 0) && (gameTable.children[i-1].children[j].classList.contains("empty"))){
              pointsForThis+=2;
            }
            if((i < 10) && (gameTable.children[i+1].children[j].classList.contains("empty"))){
              pointsForThis+=2;
            }
          }
        }
      }
      return pointsForThis;
    }
  },
  {
    title: "Row of houses",
    description: "A leghosszabb, vízszintesen megszakítás nélkül egybefüggő falumezők mindegyikéért kettő-kettő pontot kapsz.",
    /*EZ JÓL MŰKÖDIK*/
    img: "missions_eng/Group_72.png",
    calculate: function(){
      let pointsForThis = 0;
      let longest = 0;
      let found = 0;
      for(let i = 0; i < 11; i++){
        for(let j = 0; j < 11; j++){
          if(gameTable.children[i].children[j].classList.contains("town")){
            found = found + 1;
          } else{
            if(found == longest){ pointsForThis += longest*2; }
            if(found > longest){ longest = found; pointsForThis = longest*2; }
            found = 0;
          }
        }
      }
      return pointsForThis;
    }
  },
  {
    title: "Odd numbered silos",
    description: "Minden páratlan sorszámú teli oszlopodért 10-10 pontot kapsz.",
    /*EZ JÓL MŰKÖDIK*/
    img: "missions_eng/Group_73.png",
    calculate: function(){
      let pointsForThis = 0;
      for(let i = 0; i < 11; i += 2){
        let qualify = true;
        let j = 0;
        while(qualify && j < 11){
          if(gameTable.children[j].children[i].classList.contains("empty")){
            qualify = false;
          }
          j++;
        }
        if(qualify){
          pointsForThis += 10;
        }
      }
      return pointsForThis;
    }
  },
  {
    title: "Rich countryside",
    description: "Minden legalább öt különböző tereptípust tartalmazó sorért négy-négy pontot kapsz.",
    /*EZ JÓL MŰKÖDIK*/
    img: "missions_eng/Group_79.png",
    calculate: function(){
      let pointsForThis = 0;
      for(let i = 0; i< 11; i++){
        let already = ["empty"];
        let found = 0;
        for(let j = 0; j < 11; j++){
          if(!already.includes(gameTable.children[i].children[j].classList[0])){
            already.push(gameTable.children[i].children[j].classList[0]);
            found++;
          }
        }
        if(found >= 5){
          pointsForThis += 4;
          continue;
        }
      }
      return pointsForThis;
    }
  }
]

const mountains = [
  {
    x:1,
    y:1
  },
  {
    x:3,
    y:8
  },
  {
    x:5,
    y:3
  },
  {
    x:8,
    y:9
  },
  {
    x:9,
    y:5
  }
]

const elements = [
    {
        time: 2,
        type: 'water',
        shape: [[1,1,1],
                [0,0,0],
                [0,0,0]],
        rotation: 0,
        mirrored: false
    },
    {
        time: 2,
        type: 'town',
        shape: [[1,1,1],
                [0,0,0],
                [0,0,0]],
        rotation: 0,
        mirrored: false        
    },
    {
        time: 1,
        type: 'forest',
        shape: [[1,1,0],
                [0,1,1],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        time: 2,
        type: 'farm',
        shape: [[1,1,1],
                [0,0,1],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        time: 2,
        type: 'forest',
        shape: [[1,1,1],
                [0,0,1],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        time: 2,
        type: 'town',
        shape: [[1,1,1],
                [0,1,0],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        time: 2,
        type: 'farm',
        shape: [[1,1,1],
                [0,1,0],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        time: 1,
        type: 'town',
        shape: [[1,1,0],
                [1,0,0],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        time: 1,
        type: 'town',
        shape: [[1,1,1],
                [1,1,0],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        time: 1,
        type: 'farm',
        shape: [[1,1,0],
                [0,1,1],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        time: 1,
        type: 'farm',
        shape: [[0,1,0],
                [1,1,1],
                [0,1,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        time: 2,
        type: 'water',
        shape: [[1,1,1],
                [1,0,0],
                [1,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        time: 2,
        type: 'water',
        shape: [[1,0,0],
                [1,1,1],
                [1,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        time: 2,
        type: 'forest',
        shape: [[1,1,0],
                [0,1,1],
                [0,0,1]],
        rotation: 0,
        mirrored: false  
    },
    {
        time: 2,
        type: 'forest',
        shape: [[1,1,0],
                [0,1,1],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
    {
        time: 2,
        type: 'water',
        shape: [[1,1,0],
                [1,1,0],
                [0,0,0]],
        rotation: 0,
        mirrored: false  
    },
]

const startingMountains = [
  {
    x: 1,
    y: 1
  },
  {
    x: 3,
    y: 8
  },
  {
    x: 5,
    y: 3
  },
  {
    x: 8,
    y: 9
  },
  {
    x: 9,
    y: 5
  }
]

const seasons = [
  {
    name: "Spring",
    activeQuests: [
      0, 1
    ],
    currentScore: 0
  },
  {
    name: "Summer",
    activeQuests: [
      1, 2
    ],
    currentScore: 0
  },
  {
    name: "Autumn",
    activeQuests: [
      2, 3
    ],
    currentScore: 0
  },
  {
    name: "Winter",
    activeQuests: [
      3, 0
    ],
    currentScore: 0
  },
]

const questScores = [0,0,0,0]
const selectedQuests = [0,0,0,0]
//PRESETS END

startBTN.addEventListener("click", function() {
    startBTN.style.display = "none";

    divOfTable.id = "tableHolder";
    divOfTable.classList.add("holder");

    divOfSeasons.classList.add("rightSide");
    divOfSeasons.classList.add("holder");


    for(let i = 0; i < 11; i++){
      const row = document.createElement("tr");
      for(let j = 0; j < 11; j++){
        const col = document.createElement("td");
        col.key = i + " " + j;
        col.classList.add("empty");
        //col.innerHTML = j;
        row.appendChild(col);
      }
      gameTable.appendChild(row);
    }
    divOfTable.appendChild(gameTable);

    gameTable.addEventListener("click", function(){
      event.stopPropagation();
      if(event.target.nodeName != "TD"){
        return;
      }
      if(selected == 1){
        const asd = event.target.key.split(" ");
        const x = ~~asd[0];
        const y = ~~asd[1];
        let okay = checkIfItFits(x, y);
        if(okay){
          selected = 0;
          for(let i = 0; i < 3; i++){
            for(let j = 0; j< 3; j++){
              if(nextEl.shape[i][j] == 1){
                placeField(nextEl.type, x-1+i, y-1+j);
              }
            }
          }
          currentTime = currentTime+ nextEl.time;
          currentSeasonNamespace.innerHTML = `Current season: ${seasons[currentSeason].name} ${currentTime%7}/7`
          if(currentTime >= (currentSeason+1)*7 ){
            nextSeason();
          }
          newRandomField(currentSeasonNamespace);
          if(currentTime > 27){
            console.log("game over -- would be");
            divOfRandom.style.display = "none";

            const divOfEnd = document.createElement("div");
            divOfEnd.classList.add("holder");
            divOfEnd.classList.add("rightSide");
            divOfEnd.id = "end";
            let total = 0;
            for(let i = 0; i < 4; i++){
              total += seasons[i].currentScore;
            }
            divOfEnd.innerHTML = `Total Points: ${total + mountainScore}`;
            mainDiv.appendChild(divOfEnd);
          }
        }
      }
    });

    mainDiv.appendChild(divOfTable);
    mainDiv.appendChild(divOfSeasons);


    for(const element of startingMountains){
      placeField("mountain", element.x, element.y);
    }


    const rowSeasons = document.createElement("tr");
    for(let i = 0; i < 4; i++){

      const cell = document.createElement("td");
      cell.classList.add("seasons");
      cell.id = seasons[i].name.toLowerCase();

      const seasonNamespace = document.createElement("p");
      seasonNamespace.classList.add("seasonName");
      seasonNamespace.innerHTML = seasons[i].name;
      
      const seasonScore = document.createElement("p");
      seasonScore.classList.add("seasonScore");
      seasonScore.innerHTML = `Score: ${seasons[i].currentScore}`;

      cell.appendChild(seasonNamespace);
      cell.appendChild(seasonScore);
      rowSeasons.appendChild(cell);
    }
    seasonsTable.appendChild(rowSeasons);
    divOfSeasons.appendChild(seasonsTable);
    
    
    let randomQuests = [];
    divOfQuests.classList.add("holder");
    divOfQuests.classList.add("rightSide");
    divOfQuests.classList.add("flexer");
    for(let i = 0; i < 4; i++){
      let rand = getRandomNumber(missions.length);
      while(randomQuests.includes(rand)){
        rand = getRandomNumber(missions.length);
      }
      randomQuests.push(rand);
      selectedQuests[i] = rand;
      console.log("random quest " + i + ": " + rand);
      const quest = document.createElement("img");
      quest.src = missions[rand].img;
      quest.classList.add("quests");
      divOfQuests.appendChild(quest);
    }
    
    const qa = document.createElement("div");
    const qb = document.createElement("div");
    const qc = document.createElement("div");
    const qd = document.createElement("div");

    qc.id = "questC";
    qa.id = "questA";
    qa.innerHTML = `Score: ${questScores[0]}`
    divOfQuests.appendChild(qa);

    qb.id = "questB";
    qb.innerHTML = `Score: ${questScores[0]}`
    divOfQuests.appendChild(qb);

    qc.innerHTML = `Score: ${questScores[0]}`
    divOfQuests.appendChild(qc);

    qd.id = "questD";
    qd.innerHTML = `Score: ${questScores[0]}`
    divOfQuests.appendChild(qd);

    mainDiv.appendChild(divOfQuests);
    activateQuests();
    


    const divOfRandom = document.createElement("div");
    divOfRandom.classList.add("holder");
    divOfRandom.classList.add("rightSide");

    currentSeasonNamespace.innerHTML = `Current season: ${seasons[currentSeason].name} ${currentTime%7}/7`
    currentSeasonNamespace.id = "currentSeason";
    divOfRandom.appendChild(currentSeasonNamespace);

    const rotate = document.createElement("button");
    rotate.innerHTML = "Rotate";
    rotate.classList.add("controlsButton");
    rotate.addEventListener("click", function(){
      nextEl.rotation += 1;
      updateNextEl();
    });
    const mirror = document.createElement("button");
    mirror.innerHTML = "Mirror";
    mirror.classList.add("controlsButton");
    mirror.addEventListener("click", function(){
      nextEl.mirrored = true;
      updateNextEl();
    });
    const randomElement = document.createElement("div");

    randomElement.id = "randomElement";
    for(let i = 0; i < 3; i++){
      const row = document.createElement("tr");
      for(let j = 0; j < 3; j++){
        const col = document.createElement("td");
        col.classList.add("empty");
        row.appendChild(col);
      }
      randomElementTable.appendChild(row);
    }
    const time = document.createElement("p");
    time.id="time";
    newRandomField(currentSeasonNamespace);
    
    randomElement.appendChild(rotate);
    randomElement.appendChild(randomElementTable);
    randomElement.appendChild(mirror);

    randomElementTable.addEventListener("click", function(){
      if(selected == 0){ selected = 1 }else{ selected = 0 }
    });
    divOfRandom.appendChild(randomElement);
    mainDiv.appendChild(divOfRandom);
});

function placeField(type, x, y){
  gameTable.children[x].children[y].classList.remove(...gameTable.children[x].children[y].classList);
  gameTable.children[x].children[y].classList.add(type);
}

function getRandomNumber(max){
  return Math.floor(Math.random() * max);
}

function newRandomField(time){
  const k = getRandomNumber(elements.length);
  nextEl = elements[k];
  for(let i = 0; i<3; i++){
    for(let j = 0; j < 3; j++){
      randomElementTable.children[i].children[j].classList.remove(...randomElementTable.children[i].children[j].classList);
      if(nextEl.shape[i][j] == 1){
        randomElementTable.children[i].children[j].classList.add(nextEl.type);
      }else{
        randomElementTable.children[i].children[j].classList.add("empty");
      }
    }
  }
  time.innerHTML = time.innerHTML + `(+${nextEl.time})`;
}

function checkIfItFits(x, y){
  for(let i = 0; i < 3; i++){
    for(let j = 0; j < 3; j++){
      if(x-1+i < 0 || x-1+i > 10 || y-1+j < 0 || y-1+j > 10){
        if(nextEl.shape[i][j] == 1){
          return false;
        }
      } else{
        if(!(gameTable.children[x-1+i].children[y-1+j].classList.contains("empty")) && nextEl.shape[i][j] == 1){
          return false;
        }
      }
    }
  }
  return true;
}

function updateNextEl(){
  let newNextEl = [[0,0,0],[0,0,0],[0,0,0]];
  while(nextEl.rotation > 0){
    for(let i = 0; i<3; i++){
      for(let j = 0; j<3;j++){
        newNextEl[j][2-i] = nextEl.shape[i][j];
      }
    }
    nextEl.shape = newNextEl;
    nextEl.rotation = nextEl.rotation-1;
  }

  if(nextEl.mirrored){
    nextEl.mirrored = false;
    for(let i = 0; i<3; i++){
      newNextEl[i] = nextEl.shape[i].reverse();
    }
    nextEl.shape = newNextEl;
  }

  for(let i = 0; i<3; i++){
    for(let j = 0; j < 3; j++){
      randomElementTable.children[i].children[j].classList.remove(...randomElementTable.children[i].children[j].classList);
      if(nextEl.shape[i][j] == 1){
        randomElementTable.children[i].children[j].classList.add(nextEl.type);
      }else{
        randomElementTable.children[i].children[j].classList.add("empty");
      }
    }
  }
}

function nextSeason(){
  deactivateQuests();
  currentSeason = currentSeason + 1;
  if(currentSeason < 4){
    currentSeasonNamespace.innerHTML = `Current season: ${seasons[currentSeason].name} ${currentTime%7}/7`
    activateQuests();
  }
  mountainScore = calculateMountains();
}

function activateQuests(){
  for(let i = 0; i < seasons[currentSeason].activeQuests.length; i++){
    divOfQuests.children[seasons[currentSeason].activeQuests[i]].classList.add("activeQuest");
  }
}

function deactivateQuests(){
  for(let i = 0; i < seasons[currentSeason].activeQuests.length; i++){
    let questScore = missions[selectedQuests[seasons[currentSeason].activeQuests[i]]].calculate();
    questScores[seasons[currentSeason].activeQuests[i]] = questScore;
    divOfQuests.children[seasons[currentSeason].activeQuests[i] + 4].innerHTML = `Score: ${questScores[seasons[currentSeason].activeQuests[i]]}`;
    divOfQuests.children[seasons[currentSeason].activeQuests[i]].classList.remove("activeQuest");
    seasons[currentSeason].currentScore += questScore;
    updateSeason();
  }
}

function updateSeason(){
  console.log(`Mountains: ${mountainScore}`);
  divOfSeasons.children[0].children[0].children[currentSeason].children[1].innerHTML = `Score: ${seasons[currentSeason].currentScore + mountainScore}`;
}

function calculateMountains(){
  let pointsForThis = 0;
  mountains.forEach(element => {
    let up = (!gameTable.children[element.x+1].children[element.y].classList.contains("empty"));
    let down = (!gameTable.children[element.x-1].children[element.y].classList.contains("empty"));
    let left = (!gameTable.children[element.x].children[element.y-1].classList.contains("empty"));
    let right = (!gameTable.children[element.x].children[element.y+1].classList.contains("empty"))
    if(up && down && left && right){
      pointsForThis++;
    }
  });
  return pointsForThis;
}