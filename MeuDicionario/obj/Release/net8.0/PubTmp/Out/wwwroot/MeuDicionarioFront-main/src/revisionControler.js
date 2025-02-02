import { apiMessage } from "./messageManager.js"
import { loadRevision, setRevisionList } from "./revision.js"

const url = "https://localhost:7167/revisao"

export const revisionGetSome = () => {
    fetch(url, {
        method: 'GET'
    })
    .then(resp => resp.json())
        .then(date => {
            setRevisionList(date)
            loadRevision()
    })
    .catch(error => {
        messageError(error)
    })
}

export const revisionDelete = (list) => {
    fetch(url, {
        method: 'DELETE',
        headers: {
            'Content-Type' : 'application/json'
        },
        body: JSON.stringify(list)
    })
    .then(resp => console.log(resp))
    .catch(error => {
        messageError(error)
    })
}

const messageError = (error) => {
    apiMessage(error.message, "Erro")
}