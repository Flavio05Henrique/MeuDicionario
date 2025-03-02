import {  apiMessage, showMessage } from "./messageManager.js"
import { addOneInView, addSomeInView, changeValueCardWord, closeOpenElementsView, setListWord, viewUpdate, wordCardContainerObj } from "./viewManager.js"

const url = "https://localhost:7167/palavra"
const skipTake = {
    'skip': 3,
    'take': 3, 
    'page': 0,
    'add' : 0
}
 
export const addNewWord = (obj) => {
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type' : 'application/json'
        },
        body: JSON.stringify(obj)
    })
    .then(response => response.json())
    .then(date =>  {
        addOneInView(date)
        skipTake.add += 1
    })
    .catch(error => {
        messageError(error)
    })
}

export const findById = (id) => {
    const urlWithId = url + `/${id}` 
    fetch(urlWithId, {
        method: 'GET'
    })
    .then(resp => resp.json())
    .then(date => date)
    .catch(error => {
        messageError(error)
    })
} 

export const findByWord = (word) => {
    const urlWithId = url + `/search?word=${word}`
    fetch(urlWithId, {
        method: 'GET'
    })
    .then(resp => resp.json())
    .then(date => {
        changeValueCardWord(date)
        closeOpenElementsView(wordCardContainerObj.container, false)
    })
    .catch(error => {
        messageError(error)
    })
} 

export const getSome = () => {
    fetch(url + `?skip=${(skipTake.skip * skipTake.page) + skipTake.add}&take=${skipTake.take}`, {
        method: 'GET'
    })
    .then(resp => resp.json())
        .then(date => {
            setListWord(date)
            addSomeInView(date)
            skipTake.page += 1
    })
    .catch(error => {
        messageError(error)
    })
}

export const excludeWord = (id) => {
    const urlWithId = url + `/${id}` 
    fetch(urlWithId, {
        method: 'DELETE'
    })
    .then(resp => {
        skipTake.add -= 1
    })
    .catch(error => {
        messageError(error)
    })
}

export const changeWord = (obj, id) => {
    const urlWithId = url + `/${id}` 
    fetch(urlWithId, {
        method: 'PUT',
        headers: {
            'Content-Type' : 'application/json'
        },
        body: JSON.stringify(obj)
    })
    .then(response => {
        if(response.status == 204) showMessage('Atualizado com sucesso')
        if(response.status == 400) showMessage('Atualizção falhou')
    })
    .catch(error => {
        messageError(error)
    })
}

const messageError = (error) => {
    apiMessage(error.message, "Erro")
}