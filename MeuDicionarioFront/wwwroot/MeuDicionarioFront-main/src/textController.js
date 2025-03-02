import { apiMessage, showMessage } from "./messageManager.js"
import { addTextToList, loadRelationsTextWord, loadTextCards, setTextList, setTextWordList } from "./secTexts.js"

const url = "https://localhost:7167/texto"
const skipTake = {
    'skip': 3,
    'take': 3,
    'page': 0,
    'add' : 0
}

export const textAdd = (obj) => {
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type' : 'application/json'
        },
        body: JSON.stringify(obj)
    })
    .then(response => response.json())
    .then(json => {
        addTextToList(json)
        loadTextCards()
        showMessage("Adicionado")
        skipTake.add += 1
    })
    .catch(error => {
        messageError(error)
    })
}

export const textGetSome = () => {
    fetch(url + `?skip=${(skipTake.skip + skipTake.add) * skipTake.page}&take=${skipTake.take}`, {
        method: 'GET'
    })
    .then(resp => resp.json())
        .then(date => {
            setTextList(date)
            skipTake.page += 1
    })
    .catch(error => {
        messageError(error)
    })
}

export const textExclude = (id) => {
    const urlWithId = url + `/${id}` 
    fetch(urlWithId, {
        method: 'DELETE'
    })
    .then(resp => {
        resp.status == 204 ?  showMessage("Excluido") : 0
        skipTake.add -= 1
    })
    .catch(error => {
        messageError(error)
    })
}

export const textChange = (obj, id) => {
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

export const textSearchRelationTextWord = (id) => {
    fetch(url + `/${id}`, {
        method: 'POST'
    })
    .then(response => console.log(response))
    .catch(error => {
        messageError(error)
    })
}

export const textGetRelationTextWord = (id) => {
    fetch(url + `/${id}`, {
        method: 'GET'
    })
    .then(resp => resp.json())
        .then(date => {
            setTextWordList(date)
            loadRelationsTextWord()
    })
    .catch(error => {
        messageError(error)
    })
}

const messageError = (error) => {
    apiMessage(error.message, "Erro")
}