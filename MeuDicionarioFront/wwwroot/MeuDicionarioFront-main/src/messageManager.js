const messageContainer = document.querySelector('[data="messageContainer"]')

let lastAnimation = null

export const extractMessage = (string) => {
    const regex = /"([^"]+)"/;
    const message = string.match(regex);
    if(message) {
        return message[0].replace(/"/g, '')
    }
}

export const showMessage = (message, title = "Mensagem") => {
    messageContainer.innerHTML = `
        <h3>${title}</h3>
        <div class="message">
            ${message}
        </div>
    `
    animationMessage()  
}

export const apiMessage = (string, title) => {
    const message = extractMessage(string)
    if(message) {
        showMessage(message, title)
    }
}

const animationMessage = () => {
    messageContainer.classList.add('transformClear')
    messageContainer.classList.add('opacityZero')
    if(lastAnimation) clearTimeout(lastAnimation)
    lastAnimation = setTimeout(() => {
        messageContainer.classList.remove('transformClear')
        messageContainer.classList.remove('opacityZero')
    }, 3000)
}