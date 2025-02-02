const closePopupBnt = document.querySelector('[data="BntClosePopup"]')
const popupContainer = document.querySelector('[data="popupContainer"]')
const cardInfos = document.querySelector('[data="cardInfo"]')

closePopupBnt.addEventListener('click', e => {
    popupContainer.classList.add("displayNone")
})

export const openPopup = (string) => {
    popupContainer.classList.remove("displayNone")
    cardInfos.innerHTML = string
}

export const closePopup = () => {
    popupContainer.classList.add("displayNone")
}