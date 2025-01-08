const messageContainer = document.querySelector('[data="messageContainer"]')



export const extractMessage = (string) => {
    const regex = /"([^"]+)"/;
    const message = string.match(regex);
    if(message) {
        return message[0].replace(/"/g, '')
    }
}

export const showMessage = (string) => {
    const message = extractMessage(string)

    if(message) {
        messageContainer.innerHTML = message
    }
}