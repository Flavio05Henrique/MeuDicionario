import { closeOpen, viewContainerActivateEvents, viewRefresh } from "./viewManager.js"
import { addNewWord, findByWord, getSome } from "./wordControler.js"


const formAddNewWord = document.querySelector('[data="newWord"]')
const search = document.querySelector('[data="search"]') 
const addNewWordOpenBnt = document.querySelector('[data="openAdd"]')
const refreshBnt = document.querySelector('[data="refresh"]')
const plusBnt = document.querySelector('[data="plus"]')

let addNewWordIsOpen = false

viewContainerActivateEvents()
getSome()

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

addNewWordOpenBnt.addEventListener('click', e => {
    closeOpen(formAddNewWord)
})

refreshBnt.addEventListener('click', e => {
    viewRefresh()
})

plusBnt.addEventListener('click', e => {
    getSome()
})
