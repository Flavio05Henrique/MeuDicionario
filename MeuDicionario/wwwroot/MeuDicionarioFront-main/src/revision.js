import { showMessage } from "./messageManager.js"
import { revisionDelete, revisionGetSome } from "./revisionControler.js"

let revisionList = []
const revisionListDefault = [{name: "Friend", meaning: "Amigo"},{name: "House", meaning: "Casa"}]
const revisionContainer = document.querySelector('[data="revisionContainer"]')
let currentWordNum = 0
const revisionStates = {
    'mistakes': 0,
    'successes': 0,
    'successesList': []
}

export const setRevisionList = (list) => {
    console.log(list)
    revisionList = list
} 

revisionContainer.addEventListener('click', e => {
    const elementClicked = e.target
    const hasAttribute = elementClicked.hasAttribute('data-i')

    if(!hasAttribute) return

    const getDataValue = elementClicked.getAttribute('data-i')

    switch(getDataValue) {
        case 'revisionInit': initRevision();
        break;
        case 'revisionClose': closeRevision();
        break;
        case 'revisionOption': clickOptionEvent(elementClicked);
        break;
    }
})

const initRevision = () => {
    revisionGetSome()
}

export const loadRevision = () => {
    if(revisionList.length == 0) return showMessage("Não há palavras para revisar")
    changeContentRevisionContainer(cardRevisionWord())
}

const changeContentRevisionContainer = (string) => {
    if(string) {
        revisionContainer.innerHTML = string
    }
}

const closeRevision = () => {
    changeContentRevisionContainer(cardRevisionInitialize())
    resetStatus()
}

const cardRevisionInitialize = () => {
    return `
        <p>* Você participará de um quiz com no máximo 50 palavras. Se terminar e quiser verificar se há mais palavras para revisar, basta reiniciar.</p>
        <button data="revisionQuizBnt" data-i="revisionInit">Iniciar</button>`
}

const cardRevisionReportResult = () => {
    return `
            <div class="resultsReport">
                <h3>Acertos</h3>
                <div class="result successes">${revisionStates.successes}</div>
                <h3>Erros</h3>
                <div class="result mistakes">${revisionStates.mistakes}</div>
                <button data-i="revisionClose">Encerrar</button>
            </div>
    ` 
}

const cardRevisionWord = () => {
    const word = revisionList[currentWordNum].wordRef
    let wrongWordList = []
    
    while(wrongWordList.length < 2) {
        let randomNum
        
        revisionList.length > 2 
        ? randomNum = generateRandomNum(0, revisionList.length -1) 
        : randomNum = generateRandomNum(0, revisionListDefault.length -1)
        
        if(randomNum != currentWordNum){
            if(revisionList.length > 2) {
                wrongWordList.push(revisionList[randomNum].wordRef)
            } else {
                wrongWordList = revisionListDefault
            }
        }
    }
    
    const wordsSorted = shuffleArray([word, ...wrongWordList])
    const html = `
    <div class="tools">
        <button class="close" data-i="revisionClose">X</button>
        <div class="result successes">${revisionStates.successes}</div>
        <div class="result mistakes">${revisionStates.mistakes}</div>
    </div>
    <h3>${word.name}</h3>
    ${wordsSorted.map(e => `<div class="option" data-i="revisionOption" id="${e.id}">${e.meaning}</div>`)}
    `
    
    return html
}

const generateRandomNum = (min, max) => {
    const randomValue = Math.random()
    const randomNum = randomValue * (max + 1 - min)

    return parseInt(randomNum)
} 

const shuffleArray = (array)  => {
    for (let i = array.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1)); 
        [array[i], array[j]] = [array[j], array[i]]; 
    }
    return array;
}

const clickOptionEvent = (element) => {
    const word = revisionList[currentWordNum].wordRef
    
    if(word.id === parseInt(element.id)) {
        revisionSuccesses()
    } else {
        revisionMistakes()
    }
}

const revisionSuccesses = () => {
    revisionStates.successes += 1
    revisionStates.successesList.push(revisionList[currentWordNum].id)
    nextRevision()
}

const revisionMistakes = () => {
    revisionStates.mistakes += 1 
    const word = revisionList[currentWordNum].wordRef
    console.log(word)
    changeContentRevisionContainer(`
        <h3>${word.name}</h3>
        <p>${word.meaning}</p>
    `)
    setTimeout(() => {
        nextRevision()
    },5000)
}

const nextRevision = () => {
    if(revisionList.length -1 > currentWordNum) {
        currentWordNum += 1
        changeContentRevisionContainer(cardRevisionWord())
    } else {
        changeContentRevisionContainer(cardRevisionReportResult())
        revisionDelete(revisionStates.successesList)
    }
}

const resetStatus = () => {
    revisionStates.mistakes = 0
    revisionStates.successes = 0
    revisionStates.successesList = []
    currentWordNum = 0
}
