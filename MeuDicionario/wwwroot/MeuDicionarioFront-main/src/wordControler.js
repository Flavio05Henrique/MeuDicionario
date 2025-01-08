import { extractMessage, showMessage } from "./messageManager.js"
import { addOneInView, addSomeInView, setListWord, viewUpdate } from "./viewManager.js"

const url = "https://localhost:7167/palavra"
const skipTake = {
    'skip': 3,
    'take': 3, 
    'page': 0
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
    .then(date => addOneInView(date))
    .catch(error => {
        showMessage(error.message)
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
        showMessage(error.message)
    })
} 

export const findByWord = (word) => {
    const urlWithId = url + `/search?word=${word}`
    fetch(urlWithId, {
        method: 'GET'
    })
    .then(resp => resp.json())
    .then(date => viewUpdate([date]))
    .catch(error => {
        showMessage(error.message)
    })
} 

export const getSome = () => {
    fetch(url + `?skip=${skipTake.skip * skipTake.page}&take=${skipTake.take}`, {
        method: 'GET'
    })
    .then(resp => resp.json())
        .then(date => {
            setListWord(date)
            addSomeInView(date)
            skipTake.page += 1
    })
    .catch(error => {
        showMessage(error.message)
    })
}