const closePopupBnt = document.querySelector('[data="BntClosePopup"]')
const popupContainer = document.querySelector('[data="popupContainer"]')
const cardInfos = document.querySelector('[data="cardInfo"]')

closePopupBnt.addEventListener('click', e => {
    popupContainer.classList.add("displayNone")
})

export const openPopup = (string, usePopUpList) => {
    popupContainer.classList.remove("displayNone")
    if(usePopUpList) {
        cardInfos.innerHTML = popUpList[string]
        return
    }
    cardInfos.innerHTML = string
}

export const closePopup = () => {
    popupContainer.classList.add("displayNone")
}

var popUpList = {
    "cardAddWord": `
        <div class="add_word_card_popUp">
            <select name="options" id="options" class="add_word_options" data="addWord-options">
                <option value="Verb">Verbo</option>
                <option value="Noun">Substantivo</option>
                <option value="Adjective">Adjetivo</option>
            </select>
            <form class="add_word_form" data="addWord-form" id="addWord-form">
                <div>
                    <label for="name">Palavra</label>
                    <input type="text" id="name" required data-n>
                </div>
                <div>
                    <label for="meaning">Significado</label>
                    <input type="text" id="meaning" required data-n>
                </div>
                <div>
                    <label for="ThirdPerson">Terceira pessoa</label>
                    <input type="text" id="ThirdPerson" required data-c>
                </div>
                <div>
                    <label for="Preterite">Preterite</label>
                    <input type="text" id="Preterite" required data-c>
                </div>
                <div>
                    <label for="PresentContinuous">Present continuous</label>
                    <input type="text" id="PresentContinuous" required data-c>
                </div>
                <div>
                    <label for="PaticiplePresent">Paticiple present</label>
                    <input type="text" id="PaticiplePresent" required data-c>
                </div>
                <div>
                    <label for="PaticiplePass">Paticiple pass</label>
                    <input type="text" id="PaticiplePass" required data-c>
                </div>
            </form>
            <button type="submit" form="addWord-form">Ok</button>
        </div>
    `
}