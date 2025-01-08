import { binarySearch } from "./binarySearch.js"

let viewContainer = document.querySelector('[data="wordsContainer"]')
let wordCardContainerObj = null
let openCloseObj = null
let currentListWord = []

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
    console.log(wordCardContainerObj)
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
    viewContainer.innerHTML = ""
}

export const viewUpdate = (list) => {
    if(list.length > 0) {
        viewContainer.innerHTML = generateValidList(list)
    }
}

export const addOneInView = (obj) => {
    viewContainer.innerHTML += generateHtmlWord(obj)
    currentListWord.push(obj)
}

export const addSomeInView = (list) => {
    if(list.length > 0 ) {
        viewContainer.innerHTML += generateValidList(list)
    }
} 

export const viewRefresh = () => {
    if(currentListWord.length > 0) {
        viewContainer.innerHTML = generateValidList(currentListWord)
    } 
}

const generateValidList = (list) => {
    return list.map((obj) => generateHtmlWord(obj)).join("")
} 

const generateHtmlWord = (obj) => {
    return `<div class="word" id="${obj.id}" data-i>${obj.name}</div>`
}  

export const viewContainerActivateEvents = () => {
    if(!viewContainer) return

    viewContainer.addEventListener('click', e => {
        const elementClicked = e.target

        if(!elementClicked.hasAttribute('data-i')) return
        
        
        const wordObj = binarySearch(currentListWord, elementClicked.id) 
        
        generateHtmlWordCard(wordObj)

        closeOpen(wordCardContainerObj.container)
    })
}

const generateHtmlWordCard = (wordObj) => {
    wordCardContainerObj.card.id = wordObj.id
    wordCardContainerObj.cardName.textContent = wordObj.name
    wordCardContainerObj.cardMeaning.textContent = wordObj.meaning
}

export const closeOpen = (element = null) => {
    if(openCloseObj == null) buildOpenCloseObj()
    
    if(element) {
        element.classList.toggle('heightZero')
        openCloseObj.wordsContainer.classList.toggle('wordsReduce')
        
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





