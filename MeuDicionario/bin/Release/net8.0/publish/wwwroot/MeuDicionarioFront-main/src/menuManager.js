import { textGetSome } from "./textController.js"
import { getSome } from "./wordControler.js"


const menu = document.querySelector('[data="menu"]')
const secRevision = document.querySelector('[data="viewRevision"]')
const secTexts = document.querySelector('[data="viewTexts"]')
let lastElementOpen = null

let isFirstOpenSecText = true

getSome()

menu.addEventListener('click', e => {
    const elementClicked = e.target

    if(!elementClicked.hasAttribute('data-i')) return

    switch (elementClicked.getAttribute('data-i')) {
        case 'words': openCloseSecs();
        break;
        case 'revision': openCloseSecs(secRevision);
        break;
        case 'texts': openSecText();
        break;
    }
})

const openCloseSecs = (sec = null) => {
    if(sec) {
       sec.classList.toggle('transformClear')
    }
    if(lastElementOpen) {
        lastElementOpen.classList.toggle('transformClear')
    } 
    lastElementOpen = sec
}

const openSecText = () => {
    openCloseSecs(secTexts)
    const getSome = () => {
        textGetSome()
        isFirstOpenSecText = false
    }
    
    isFirstOpenSecText ? getSome() : 0
}