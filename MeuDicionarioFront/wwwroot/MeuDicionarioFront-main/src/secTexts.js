import { showMessage } from "./messageManager.js"
import { closePopup, openPopup } from "./popupManager.js"
import { textAdd, textChange, textExclude, textGetRelationTextWord, textGetSome, textSearchRelationTextWord } from "./textController.js"

const textTools = document.querySelector('[data="textTools"]')
const viewTexts = document.querySelector('[data="viewTexts"]')
const textContainer = document.querySelector('[data="textContainer"]')
const textWordContainer = document.querySelector('[data="TextWordContainer"]')
const cardWordExpanded = document.querySelector('[data="wordExpanded"]')

let textList = []
let extraToolsIsOpen = false
let currentText = {
    'text' : null,
    'index': 0,
    'open' : false,
    'textWordList': []
}

export const setTextList = (list) => {
    if(textList.length > 0) {
        textList = [...textList, ...list]
    } else {
        textList = list
    }
    loadTextCards()
}

viewTexts.addEventListener('click', e => {
    const elementClicked = e.target
    const isInteractive = elementClicked.hasAttribute('data-i')

    if(!isInteractive) return

    const elementClickedValue = elementClicked.getAttribute('data-i')

    switch(elementClickedValue) {
        case 'textAdd': openFormText(addNewText);
        break;
        case 'textDoubt': openDoubtCard();
        break;
        case 'textCard': openTextCard(elementClicked.id);
        break;
        case 'textReturn': returnTo();
        break;
        case 'textDelete': excludeText();
        break;
        case 'textEdit': openFormText(updateText, currentText.text);
        break;
        case 'textSearch': updateRelationTextWord();
        break;
        case 'textWord': loadWordInCard(elementClicked.id);
        break;
        case 'textLoad': textGetSome();
        break;
    }
})

const openFormText = (func, text = null) => {
    const card = `
        <div class="newText">
            <label for="title">Titulo</label>
            <input type="text" id="title" required data="AddNewTextTitle">
            <label for="text">Texto</label>
            <textarea name="text" required data="AddNewTextText">
            </textarea>

            <button type="button" data="addNewText">OK</button>
        </div>
    `
    openPopup(card)
    text ? textFormStarted(text) : 0
    document.querySelector('[data="addNewText"]').addEventListener('click', e => func())
}

const openDoubtCard = () => {
    const card = `
        <div class="item">
            <img src="assets/img/magnifying-glass-solid.svg" alt="Imagem de uma lupa">
            <p>
                &nbsp;Utilize para buscar palavras relacionadas ao texto. 
                As palavras que já foram buscadas anteriormente serão carregadas automaticamente ao abrir o texto.
            </p>
        </div>
        <div class="item">
            <img src="assets/img/plus-solid.svg" alt="Imagem de uma de um simbolo de mais">
            <p>
                &nbsp;Ultilize para adicionar novos textos.
            </p>
        </div>
        <div class="item">
            <img src="assets/img/arrows-rotate-solid.svg" alt="Imagem de uma seta em rotação">
            <p>
                &nbsp;Carregue mais textos, se houver.
            </p>
        </div>
        <div class="item">
            <img src="assets/img/arrow-left-solid.svg" alt="Imagem de uma seta em rotação">
            <p>
                &nbsp;Retorne à lista de textos.
            </p>
        </div>
    `

    openPopup(card)
}

const textFormStarted = (text) => {
    const inputTitle = document.querySelector('[data="AddNewTextTitle"]')
    const inputText = document.querySelector('[data="AddNewTextText"]')

    inputTitle.value = text.title
    inputText.value = text.textItSelf
}

const textFormGetValues = () => {
    const inputTitle = document.querySelector('[data="AddNewTextTitle"]')
    const inputText = document.querySelector('[data="AddNewTextText"]')

    const newText = {
        'Title': inputTitle.value,
        'TextItSelf': inputText.value
    }
    
    return newText
} 

const addNewText = () => {
    const newText = textFormGetValues()
    textAdd(newText)
}

const excludeText = () => {
    const result = window.confirm("Você tem certeza que deseja continuar?")

    if(result) {
        textExclude(currentText.text.id)
        textList.splice(currentText.index, 1)
        returnTo()
    }
}

const updateText = () => {
    const text = textFormGetValues()
    textChange(text, currentText.text.id)
    textList[currentText.index].title = text.Title
    textList[currentText.index].textItSelf = text.TextItSelf
    openTextCard(currentText.index)
    closePopup()
}

export const loadTextCards = () => {
    textContainer.innerHTML = ""
    textContainer.innerHTML += textList.map((e, i) => textCard(e, i)).join('')
}

const textCard = (text, index) => {
   const textCard = `
        <div class="text__card" data-i="textCard" id="${index}">
            <h3 class="title" data-i="textCard" id="${index}">${text.title}</h3>
            <p data-i="textCard" id="${index}">${text.textItSelf.replace(/\n/g, '<br>')}</p>
        </div>
   ` 

   return textCard
} 

const openTextCard = (cardId) => {
    const item = textList[cardId]
    const card = `
        <div class="text__cardExpanded">
            <h3 class="title">${item.title}</h3>
            <p>
                ${item.textItSelf.replace(/\n/g, '<br>')}
            </p>
        </div>
    `

    textContainer.innerHTML = card
    openCloseExtraTools()
    currentText.text = item
    currentText.index = cardId
    currentText.open = true
    textGetRelationTextWord(currentText.text.id)
}

const openCloseExtraTools = () => {
    const extraTools = textTools.querySelector('[data="extraTools"]')
    extraToolsIsOpen = !extraToolsIsOpen
    extraTools.classList.toggle("widthZero")
}

const returnTo = () => {
    loadTextCards()
    currentText.open = false
    extraToolsIsOpen ? openCloseExtraTools() : 0
    closeRelationsTextWord()
}

export const addTextToList = (text) => {
    textList = [text, ...textList]
    console.log(textList)
} 

const updateRelationTextWord = () => {
    if(currentText.open) {
        textSearchRelationTextWord(currentText.text.id)
    }
}

export const loadRelationsTextWord = () => {
    textWordContainer.innerHTML = `
        <h3>Palavras relacionadas</h3>
        ${currentText.textWordList.map((e, i) => `<div class="word" data-i="textWord" id="${i}">${e.name}</div>`)}
    `
}

const closeRelationsTextWord = () => {
    textWordContainer.innerHTML = `
        <h3>Palavras relacionadas</h3>
        <div class="word" >...</div>
    `
}

export const setTextWordList = (list) => {
    currentText.textWordList = list
}

const loadWordInCard = (wordId) => {
    const word = currentText.textWordList[wordId]
    cardWordExpanded.innerHTML = `
        <div class="words__card" id="${word.id}" data-card>
            <h3 data-name>${word.name}</h3>
            <p data-meaning>${word.meaning}</p>
        </div>
    `
}
