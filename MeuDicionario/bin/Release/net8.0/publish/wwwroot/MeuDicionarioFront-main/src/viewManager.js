import { binarySearch } from "./binarySearch.js"
import { showMessage } from "./messageManager.js"
import { addNewWord, changeWord, excludeWord, findByWord, getSome } from "./wordControler.js"

const viewWordsContainer = document.querySelector('[data="secWords"]')
const formAddNewWord = document.querySelector('[data="newWord"]')
const search = document.querySelector('[data="search"]') 
let wordsContainer = document.querySelector('[data="wordsContainer"]')
export let wordCardContainerObj = null
let openCloseObj = null
let currentListWord = []

viewWordsContainer.addEventListener('click', e => {
    const elementClicked = e.target

    if(!elementClicked.hasAttribute('data-i')) return

    switch (elementClicked.getAttribute('data-i')) {
        case 'wordClose': closeOpenElementsView(wordCardContainerObj.container);
        break;
        case 'wordExclude': excludeFunc()
        break;
        case 'wordEdit': openEditWordCard()
        break;
        case 'wordCancel': swapWordCards()
        break;
        case 'wordConfirm': confirmEditWordCard()
        break;
        case 'wordLoad': getSome()
        break;
        case 'wordAdd': closeOpenElementsView(formAddNewWord)
        break;
        case 'word': openCardWord(elementClicked)
        break;
    }
})

formAddNewWord.addEventListener("submit", e => {
    e.preventDefault()

    const nameInput = formAddNewWord.querySelector("#word")
    const meaningInput = formAddNewWord.querySelector("#wordDescription")

    const value = {
        'Name': nameInput.value,
        'Meaning' : meaningInput.value
    }

    nameInput.value = ""
    meaningInput.value = ""

    addNewWord(value)
})

search.addEventListener("submit", e => {
    e.preventDefault()
    findByWord(search.querySelector("#search").value)
})

const buildWordCardContainerObj = () => {
    const container = document.querySelector('[data="word_cardContainer"]')
    const card = container.querySelector('[data-card]')
    const cardName = card.querySelector('[data-name]')
    const cardMeaning = card.querySelector('[data-meaning]')

    wordCardContainerObj = {
        'container': container,
        'card': card,
        'cardName': cardName,
        'cardMeaning': cardMeaning
    }
}

if(wordCardContainerObj == null) {
    buildWordCardContainerObj()
}

export const setListWord = (list) => {
    if (currentListWord.length > 0) {
        currentListWord = [...currentListWord, ...list]
    } else {
        currentListWord = list
    }
}

export const viewClear = () => {
    wordsContainer.innerHTML = ""
}

export const viewUpdate = (list) => {
    if(list.length > 0) {
        wordsContainer.innerHTML = generateValidList(list)
    }
}

export const addOneInView = (obj) => {
    currentListWord = [obj, ...currentListWord]
    viewUpdate(currentListWord)
}

export const addSomeInView = (list) => {
    if(list.length > 0 ) {
        wordsContainer.innerHTML += generateValidList(list)
    }
} 

export const viewRefresh = () => {
    if(currentListWord.length > 0) {
        wordsContainer.innerHTML = generateValidList(currentListWord)
    } 
}

const generateValidList = (list) => {
    return list.map((obj) => generateHtmlWord(obj)).join("")
} 

const generateHtmlWord = (obj) => {
    return `<div class="word" id="${obj.id}" data-i="word">${obj.name}</div>`
}

const openCardWord = (elementClicked) => {
    if(!wordsContainer) return
    const wordObj = binarySearch(currentListWord, elementClicked.id).element
        
    changeValueCardWord(wordObj)

    closeOpenElementsView(wordCardContainerObj.container, false)
}

const openEditWordCard = () => {
    const cardEditContainer = document.querySelector('[data="ChangeWordContainer"]')
    const inputs = cardEditContainer.querySelectorAll('input')
    
    cardEditContainer.id = wordCardContainerObj.card.id
    inputs[0].value = wordCardContainerObj.cardName.innerText
    inputs[1].value = wordCardContainerObj.cardMeaning.innerText

    swapWordCards()
}

const confirmEditWordCard = () => {
    const cardEditContainer = document.querySelector('[data="ChangeWordContainer"]')
    const inputs = cardEditContainer.querySelectorAll('input')
    const word = binarySearch(currentListWord, cardEditContainer.id)
    
    if(!inputs[0].value  || !inputs[1].value ) return showMessage('Valores não podem ser vazios')

    if(word) {
        const currentWord = currentListWord[word.index]
        
        currentWord.name = inputs[0].value
        currentWord.meaning = inputs[1].value
    }

    const wordChanged = {
        'Name': inputs[0].value,
        'Meaning': inputs[1].value
    }     
    
    changeWord(wordChanged, cardEditContainer.id)
    changeValueCardWord({'id': cardEditContainer.id, 'name': inputs[0].value, 'meaning': inputs[1].value})
    viewUpdate(currentListWord)
    swapWordCards()
}

const swapWordCards = () => {
    document.querySelector('[data="ChangeWordContainer"]').classList.toggle('displayNone')
    wordCardContainerObj.card.classList.toggle('displayNone')
}

const excludeFunc = () => {
    const result = window.confirm("Você tem certeza que deseja continuar?")

    if(result) {
        const cardId = wordCardContainerObj.card.id
        console.log(cardId)
        if(cardId == 'none') return 

        excludeWord(cardId)
        currentListWord.splice(binarySearch(currentListWord, cardId).index, 1)
        viewUpdate(currentListWord)
        clearValueCardWord()
        closeOpenElementsView(wordCardContainerObj.container)
    }
}

const clearValueCardWord = () => {
    wordCardContainerObj.card.id = 'none'
    wordCardContainerObj.cardName.textContent = 'none'
    wordCardContainerObj.cardMeaning.textContent = 'none'
}

export const changeValueCardWord = (wordObj) => {
    wordCardContainerObj.card.id = wordObj.id
    wordCardContainerObj.cardName.textContent = wordObj.name
    wordCardContainerObj.cardMeaning.textContent = wordObj.meaning
}

export const closeOpenElementsView = (element = null, openClose = true) => {
    if(openCloseObj == null) buildOpenCloseObj()
    
    if(element) {
        if(openClose) {
            element.classList.toggle('heightZero')
            openCloseObj.wordsContainer.classList.toggle('wordsReduce')
        } else {
            element.classList.remove('heightZero')
            openCloseObj.wordsContainer.classList.add('wordsReduce')
        }
        
        if(openCloseObj.lastElement) {
            if(!openCloseObj.lastElement.id.includes(element.id)) {
                element.classList.remove('heightZero')
                openCloseObj.wordsContainer.classList.add('wordsReduce')
                openCloseObj.lastElement.classList.add('heightZero')
            } 
        }
        openCloseObj.lastElement = element
    }
}

const buildOpenCloseObj = () => {
    openCloseObj = {
        'wordsContainer': document.querySelector('[data-words]'),
        'lastElement': null
    }
}





